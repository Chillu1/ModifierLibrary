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

        private Dictionary<string, IModifier> Modifiers { get; }
        private HashSet<string> ModifiersToRemove { get; }//Can't think of any setbacks when it comes to not using IModifiers here
        private Dictionary<string, float> ComboModifierCooldowns { get; }
        private float _timer;

        public ModifierController(Being owner, ElementController elementController)
        {
            _owner = owner;
            ElementController = elementController;
            Modifiers = new Dictionary<string, IModifier>();
            ModifiersToRemove = new HashSet<string>();
            ComboModifierCooldowns = new Dictionary<string, float>();
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer >= 1)
            {
                foreach (string key in ComboModifierCooldowns.Keys.ToArray())//TODO Making it into an array every is prob uncool
                {
                    ComboModifierCooldowns[key] -= _timer;
                    if (ComboModifierCooldowns[key] <= 0)
                        ComboModifierCooldowns.Remove(key);
                }
                _timer = 0;
            }

            //Log.Info(Modifiers.Count);
            foreach (var modifier in Modifiers.Values)
            {
                modifier.Update(deltaTime, _owner.StatusResistances);
                if(modifier.ToRemove)
                    ModifiersToRemove.Add(modifier.Id);
            }

            foreach (string id in ModifiersToRemove)
                Modifiers.Remove(id);

            if(ModifiersToRemove.Count > 0)
                ModifiersToRemove.Clear();
        }

        public void TryAddModifier(IModifier modifier, AddModifierParameters parameters)
        {
            modifier.TargetComponent.SetupOwner(_owner);
            HandleTarget(modifier, parameters);

            if (ContainsModifier(modifier, out IModifier internalModifier))
            {
                bool stacked, refreshed;
                //Run stack & refresh in case it has those components
                stacked = internalModifier.Stack();
                refreshed = internalModifier.Refresh();
                //If we didnt stack or refresh, then apply internal modifier effect again? Any issues? We could limit this with a flag/component
                if(!stacked && !refreshed)
                    internalModifier.Init();//Problem comes here, since the effect might not actually be in Init()

                //Log.Verbose("HasModifier " + modifier.Id, "modifiers");
            }
            else
            {
                if (modifier.GetType() == typeof(ComboModifier) && !ComboModifierCooldowns.ContainsKey(modifier.Id))
                    ComboModifierCooldowns.Add(modifier.Id, ((ComboModifier)modifier).Cooldown);
                AddModifier(modifier);
            }

            if (parameters.HasFlag(AddModifierParameters.CheckRecipes))
            {
                CheckForComboRecipes();
            }
        }

        public void CheckForComboRecipes()
        {
            var comboModifierToAdd = ComboModifierPrototypes.CheckForComboRecipes(new HashSet<string>(Modifiers.Keys),
                ComboModifierCooldowns, ElementController, _owner.Stats);
            if (comboModifierToAdd.Count > 0)
                AddComboModifiers(comboModifierToAdd);
        }

        private void AddModifier(IModifier modifier)
        {
            RegisterModifier(modifier);
            Modifiers.Add(modifier.Id, modifier);
            modifier.Init();
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
            CheckForComboRecipes();//Possible ComboModifier that need comboModifiers, if badly done. Possible infinite loop
        }

        public bool RemoveModifier(IModifier modifier)
        {
            bool success = Modifiers.Remove(modifier.Id);
            if(!success)
                Log.Error("Couldn't remove modifier " + modifier.Id, "modifiers");
            return success;
        }

        public bool ContainsModifier(string modifierId)
        {
            return ContainsModifier(modifierId, out _);
        }

        public bool ContainsModifier(string modifierId, out IModifier modifier)
        {
            return Modifiers.TryGetValue(modifierId, out modifier);
        }

        public bool ContainsModifier(IModifier modifier)
        {
            return Modifiers.ContainsKey(modifier.Id);
        }

        /// <summary>
        ///     Used for refreshing, stacking ,etc
        /// </summary>
        public bool ContainsModifier(IModifier modifier, out IModifier internalModifier)
        {
            return Modifiers.TryGetValue(modifier.Id, out internalModifier);
            //return Modifiers.All(internalModifier => internalModifier.Id == modifier.Id && internalModifier.GetType() == modifier.GetType());
        }

        public void ListModifiers()
        {
            ListModifiers(Modifiers.Values);
        }
        public void ListModifiers([CanBeNull] IEnumerable<IModifier> modifiers)
        {
            if (modifiers != null)
                Log.Info("OwnerTarget: " + _owner + ". " + string.Join(". ", modifiers) + " Modifiers count: " + Modifiers.Count,
                    "modifiers", true);
        }

        private void HandleTarget(IModifier modifier, AddModifierParameters parameters)
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
                //IModifier appliers dont need a target at ctor. Extra check
                if (modifier.TargetComponent.Target == null && parameters.HasFlag(AddModifierParameters.NullStartTarget) && !modifier.ApplierModifier)
                {
                    Log.Error("Non-applier modifier doesn't have a target. Owner isn't the target", "modifiers");
                }
            }
        }

        private void RegisterModifier(IModifier modifier)
        {
            //modifier.Removed += modifierEventItem => Log.Verbose(modifierEventItem.Id + " removed", "modifiers");
        }

        public void CopyEvents(ModifierController modifierController)
        {
            //foreach (var modifier in modifierController.Modifiers.Values)
            //    RegisterModifier(Modifiers[modifier.Id]);
        }

        [CanBeNull]
        public IEnumerable<IModifier> GetModifierAppliers()
        {
            return Modifiers.Values.Where(m => m.ApplierModifier);
        }

        public override string ToString()
        {
            return string.Join(", ",Modifiers);
        }
    }
}