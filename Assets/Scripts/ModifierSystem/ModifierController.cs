using System;
using System.Collections.Generic;
using System.Linq;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ModifierController : IEventCopy<ModifierController>
    {
        private readonly Being _owner;
        private Dictionary<string, IModifier> Modifiers { get; }
        private ElementController ElementController { get; }

        public ModifierController(Being owner, ElementController elementController)
        {
            _owner = owner;
            ElementController = elementController;
            Modifiers = new Dictionary<string, IModifier>();
        }

        public void Update(float deltaTime)
        {
            //Log.Info(Modifiers.Count);
            foreach (var valuePair in Modifiers.ToArray())//TODO Making it into an array every frame is uncool, instead mark them as to deleted
            {
                valuePair.Value.Update(deltaTime, _owner.StatusResistances);
            }
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
                    internalModifier.Init(this);//Problem comes here, since the effect might not actually be in Init()

                //Log.Verbose("HasModifier " + modifier.Id, "modifiers");
            }
            else
            {
                AddModifier(modifier);
            }

            if (parameters.HasFlag(AddModifierParameters.CheckRecipes))
            {
                var comboModifierToAdd =  ComboModifierPrototypes.CheckForComboRecipes(new HashSet<string>(Modifiers.Keys), ElementController);
                if (comboModifierToAdd.Count > 0)
                    AddComboModifiers(comboModifierToAdd);
            }
        }

        private void AddModifier(IModifier modifier)
        {
            RegisterModifier(modifier);
            Modifiers.Add(modifier.Id, modifier);
            modifier.Init(this);
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
            var comboModifierToAdd = ComboModifierPrototypes.CheckForComboRecipes(new HashSet<string>(Modifiers.Keys), ElementController);
            if(comboModifierToAdd.Count > 0)//Possible ComboModifier that need comboModifiers, if badly done. Possible infinite loop
                AddComboModifiers(comboModifierToAdd);
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
                //IModifier appliers dont need a target at ctor. Extra check, for good measure
                if (modifier.TargetComponent.Target == null && parameters.HasFlag(AddModifierParameters.NullStartTarget) && !modifier.ApplierModifier)
                {
                    Log.Error("Owner isn't the target, and target is null", "modifiers");
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