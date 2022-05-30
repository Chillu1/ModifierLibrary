using System.Collections.Generic;
using System.Linq;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ModifierController : IEventCopy<ModifierController>
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

            if (_secondTimer < 0.2f)
                return;

            CooldownModifierRemover.Update(ComboModifierCooldowns);

            //Log.Info(Modifiers.Count);
            foreach (var modifier in Modifiers.Values)
            {
                modifier.Update(deltaTime, _owner.StatusResistances);
                if (modifier.ToRemove)
                    MainModifierRemover.Add(modifier.Id);
            }

            MainModifierRemover.Update(Modifiers);

            _secondTimer = 0f;
        }

        public bool TryAddModifier(Modifier modifier, AddModifierParameters parameters)
        {
            bool modifierAdded;

            modifier.SetupOwner(_owner);
            HandleTarget(modifier, parameters);

            if (ContainsModifier(modifier, out Modifier internalModifier))
            {
                bool stacked, refreshed;
                //Run stack & refresh in case it has those components
                stacked = internalModifier.Stack();
                refreshed = internalModifier.Refresh();
                //If we didnt stack or refresh, then apply internal modifier effect again? Any issues? We could limit this with a flag/component
                if (!stacked && !refreshed)
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
            RegisterModifier(modifier);
            Modifiers.Add(modifier.Id, modifier);
            modifier.Init();
            modifier.Stack(); //If has stack component, we will trigger it on add
            //Log.Verbose("Added modifier " + modifier.GetType().Name +" with target: " + modifier.TargetComponent.Target?.BaseBeing.Id, "modifiers");
        }

        private void AddComboModifiers(HashSet<ComboModifier> comboModifiers)
        {
            foreach (var modifier in comboModifiers)
            {
                TryAddModifier(modifier, AddModifierParameters.OwnerIsTarget);
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
            else
            {
                //Modifier appliers dont need a target at ctor. Extra check
                if (modifier.TargetComponent.Target == null && parameters.HasFlag(AddModifierParameters.NullStartTarget) &&
                    !modifier.IsApplierModifier)
                {
                    Log.Error("Non-applier modifier doesn't have a target. Owner isn't the target", "modifiers");
                }
            }
        }

        private void RegisterModifier(Modifier modifier)
        {
            //modifier.Removed += modifierEventItem => Log.Verbose(modifierEventItem.Id + " removed", "modifiers");
        }

        public void CopyEvents(ModifierController modifierController)
        {
            //foreach (var modifier in modifierController.Modifiers.Values)
            //    RegisterModifier(Modifiers[modifier.Id]);
        }

        public IEnumerable<Modifier> GetModifierAttackAppliers()
        {
            //Invalid target on appliers with self, so no need for extra checks rn
            return Modifiers.Values.Where(m => m.IsApplierModifier && m.ApplierType == ApplierType.Attack && !m.IsConditionModifier);
        }

        public override string ToString()
        {
            return string.Join(", ", Modifiers);
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