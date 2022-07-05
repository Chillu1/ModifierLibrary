using System.Collections.Generic;
using System.Linq;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ModifierController
    {
        private readonly Being _owner;
        private ElementController ElementController { get; }

        private Dictionary<string, Modifier> Modifiers { get; }
        private ModifierRemover MainModifierRemover { get; }
        private Dictionary<string, float> ComboModifierCooldowns { get; }
        private ModifierRemover CooldownModifierRemover { get; }
        private float _timer;
        private float _secondTimer;

        public ModifierController(Being owner, ElementController elementController)
        {
            _owner = owner;
            ElementController = elementController;
            Modifiers = new Dictionary<string, Modifier>();
            MainModifierRemover = new ModifierRemover();
            ComboModifierCooldowns = new Dictionary<string, float>();
            CooldownModifierRemover = new ModifierRemover();
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;
            _secondTimer += deltaTime;
            if (_timer >= 1)
            {
                //TODO Making it into an array is prob uncool, on the -= _timer line
                foreach (string key in ComboModifierCooldowns.Keys.ToArray())
                {
                    ComboModifierCooldowns[key] -= _timer;
                    if (ComboModifierCooldowns[key] <= 0)
                        CooldownModifierRemover.Add(key);
                }

                _timer = 0;
            }

            if (_secondTimer < 0.2f) //TODO Think about this/check this
                return;

            CooldownModifierRemover.Update(ComboModifierCooldowns);

            //Log.Info(Modifiers.Count);
            foreach (var modifier in Modifiers.Values)
            {
                modifier.Update(_secondTimer, _owner.StatusResistances);
                if (modifier.ToRemove)
                    MainModifierRemover.Add(modifier.Id);
            }

            MainModifierRemover.Update(Modifiers);

            _secondTimer = 0f;
        }

        public bool TryAddModifier(Modifier modifier, AddModifierParameters parameters, Being sourceBeing = null)
        {
            bool modifierAdded;

            //Log.Info("ModifierId: " + modifier.Id + ". BeingId: " + _owner.Id + " Parameters: " + parameters);
            modifier.SetupOwner(_owner);
            modifier.SetupApplierOwner(modifier.IsApplierModifier ? _owner : sourceBeing);
            HandleTarget(modifier, parameters);
            //Log.Info("Owner: "+modifier.TargetComponent.Owner.Id+ ". Target: " + modifier.TargetComponent.Target?.Id, "modifiers");

            if (ContainsModifier(modifier, out Modifier internalModifier))
            {
                bool stacked, refreshed;
                //Run stack & refresh in case it has those components
                stacked = internalModifier.Stack();
                refreshed = internalModifier.Refresh();
                //If we didnt stack or refresh, then apply internal modifier effect again? Any issues? We could limit this with a flag/component
                if (!stacked && !refreshed/* && internalModifier.RepeatInitEffect*/) //Aura effects shouldn't be applied multiple times, if we didn't make a cast applier refreshable, this will trigger again, needs to be fixed//TODO
                    internalModifier.Init(); //Problem comes here, since the effect might not actually be in Init()

                modifierAdded = false;
                //Log.Verbose("HasModifier " + modifier.Id, "modifiers");
            }
            else
            {
                if (modifier is ComboModifier && !ComboModifierCooldowns.ContainsKey(modifier.Id))
                    ComboModifierCooldowns.Add(modifier.Id, ((ComboModifier)modifier).Cooldown);
                AddModifier(modifier);
                modifierAdded = true;
            }

            if (parameters.HasFlag(AddModifierParameters.CheckRecipes))
            {
                CheckForComboRecipes();
            }

            //Log.Info("Added modifier " + modifier + ". Success: "+modifierAdded, "modifiers");
            return modifierAdded;
        }

        public void CheckForComboRecipes()
        {
            var comboModifierToAdd = ComboModifierPrototypes.CheckForComboRecipes(new HashSet<string>(Modifiers.Keys),
                ComboModifierCooldowns, ElementController, _owner.Stats);
            if (comboModifierToAdd.Count > 0)
                AddComboModifiers(comboModifierToAdd);
        }

        private void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier.Id, modifier);
            modifier.Init();
            modifier.Stack(); //If has stack component, we will trigger it on add
            //Log.Verbose("Added modifier " + modifier.GetType().Name +" with target: " + modifier.TargetComponent.Target?.BaseBeing.Id, "modifiers");
        }

        private void AddComboModifiers(HashSet<ComboModifier> comboModifiers)
        {
            foreach (var modifier in comboModifiers)
            {
                //Log.Info("Added combo modifier " + modifier.Id, "modifiers");
                TryAddModifier(modifier, AddModifierParameters.OwnerIsTarget, _owner);
            }

            //Check for recipes after adding all modifiers
            //We're recreating the set because new modifiers have been added
            CheckForComboRecipes(); //Possible ComboModifier that need comboModifiers, if badly done. Possible infinite loop
        }

        public bool RemoveModifier(Modifier modifier)
        {
            return RemoveModifier(modifier.Id);
        }

        public bool RemoveModifier(string id)
        {
            bool success = Modifiers.Remove(id);
            if (!success)
                Log.Error("Couldn't remove modifier " + id, "modifiers");
            return success;
        }

        public bool ContainsModifier(string modifierId)
        {
            return ContainsModifier(modifierId, out _);
        }

        public bool ContainsModifier(string modifierId, out Modifier modifier)
        {
            return Modifiers.TryGetValue(modifierId, out modifier);
        }

        public bool ContainsModifier(Modifier modifier)
        {
            return Modifiers.ContainsKey(modifier.Id);
        }

        /// <summary>
        ///     Used for refreshing, stacking ,etc
        /// </summary>
        public bool ContainsModifier(Modifier modifier, out Modifier internalModifier)
        {
            return Modifiers.TryGetValue(modifier.Id, out internalModifier);
        }

        public void SetAutomaticCast(string modifierId, bool automaticCast = true)
        {
            if (ContainsModifier(modifierId, out var modifier))
                modifier.SetAutomaticCast(automaticCast);
        }

        public void SetAutomaticCastAll(bool automaticCast = true)
        {
            foreach (var modifier in Modifiers.Values.Where(m => m.ApplierType == ApplierType.Cast))
                modifier.SetAutomaticCast(automaticCast);
        }

        public Modifier[] GetModifiersInfo()
        {
            return Modifiers.Values.ToArray();
        }

        public void ListModifiers()
        {
            ListModifiers(Modifiers.Values);
        }

        public void ListModifiers([CanBeNull] IEnumerable<Modifier> modifiers)
        {
            if (modifiers != null)
                Log.Info("OwnerTarget: " + _owner + ". " + string.Join(". ", modifiers) + " Modifiers count: " + Modifiers.Count,
                    "modifiers", true);
        }

        private void HandleTarget(Modifier modifier, AddModifierParameters parameters)
        {
            if (parameters.HasFlag(AddModifierParameters.OwnerIsTarget))
            {
                if (parameters.HasFlag(AddModifierParameters.NullStartTarget))
                    Log.Error("Parameters can't have both OwnerIsTarget and NullStartTarget", "modifiers");

                if (modifier.TargetComponent.Target == null)
                {
                    modifier.TargetComponent.SetTarget(_owner);
                }
                else if (modifier.TargetComponent.Target != _owner)
                {
                    Log.Error($"Owner id:{_owner} should be the target, but isn't. For modifier {modifier.Id} Target is: "
                              + modifier.TargetComponent.Target + ". Reverting to owner", "modifiers");
                    modifier.TargetComponent.SetTarget(_owner);
                }
            }
            else if (parameters.HasFlag(AddModifierParameters.NullStartTarget))
            {
                if (modifier.TargetComponent.Target != null)
                {
                    Log.Error("Start target should be null, but isn't. For modifier " + modifier.Id, "modifiers");
                }

                //Modifier appliers dont need a target at ctor. Extra check
                if (modifier.TargetComponent.Target == null && !modifier.IsApplierModifier)
                {
                    Log.Error("Non-applier modifier doesn't have a target. Owner isn't the target", "modifiers");
                }
            }
        }

        public IEnumerable<Modifier> GetModifierAttackAppliers()
        {
            //Invalid target on appliers with self, so no need for extra checks rn
            return Modifiers.Values.Where(m => m.IsApplierModifier && m.ApplierType == ApplierType.Attack && !m.IsConditionModifier);
        }

        public override string ToString()
        {
            return "Modifiers: " + string.Join(", ", Modifiers.Values);
        }

        private class ModifierRemover
        {
            //Can't think of any setbacks when it comes to not using Modifiers here
            private HashSet<string> ObjectsToRemove { get; }

            public ModifierRemover()
            {
                ObjectsToRemove = new HashSet<string>();
            }

            public void Update<TValue>(IDictionary<string, TValue> collection)
            {
                if (ObjectsToRemove.Count == 0)
                    return;

                foreach (string id in ObjectsToRemove)
                    collection.Remove(id);

                if (ObjectsToRemove.Count > 0)
                    ObjectsToRemove.Clear();
            }

            public void Add(string id)
            {
                ObjectsToRemove.Add(id);
            }
        }
    }
}