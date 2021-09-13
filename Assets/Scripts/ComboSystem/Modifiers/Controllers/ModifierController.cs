using System;
using System.Collections.Generic;
using System.Linq;
using BaseProject;
using BaseProject.Utils;
using JetBrains.Annotations;

namespace ComboSystem
{
    public class ModifierController
    {
        private readonly Being _ownerTarget;
        private Dictionary<string, Modifier> Modifiers { get; }

        public ModifierController(Being ownerTarget)
        {
            Modifiers = new Dictionary<string, Modifier>();
            _ownerTarget = ownerTarget;
        }

        public void Update(float deltaTime)
        {
            //Log.Info(Modifiers.Count);
            foreach (var valuePair in Modifiers.ToArray())//TODO Making it into an array every frame is uncool
            {
                valuePair.Value.Update(deltaTime);
            }
        }

        public void TryAddModifier(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default,
            ActivationCondition condition = default)
        {
            CheckTarget(modifier, parameters);

            if (ContainsModifier(modifier, out Modifier internalModifier))
            {
                Log.Verbose("HasModifier " + modifier.Id, "modifiers");
                switch (modifier.ModifierProperties)
                {
                    case ModifierProperties.None:
                        return;
                    case ModifierProperties.Stackable:
                        internalModifier.Stack();
                        break;
                    case ModifierProperties.Refreshable:
                        internalModifier.Refresh();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
                AddModifier(modifier, condition);

            if (parameters.HasFlag(AddModifierParameters.CheckRecipes))
            {
                var comboModifierToAdd = ComboModifierPrototypes.CheckForRecipes(Modifiers);
                if (comboModifierToAdd.Count > 0)
                    AddComboModifier(comboModifierToAdd);
                //Log.Verbose(comboModifierToAdd.Count);
            }
        }

        private void AddComboModifier(IEnumerable<ComboModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                TryAddModifier(modifier, AddModifierParameters.OwnerIsTarget);
            }
            //Check for recipes after adding all modifiers
            var comboModifierToAdd = ComboModifierPrototypes.CheckForRecipes(Modifiers);
            if(comboModifierToAdd.Count > 0)
                AddComboModifier(comboModifierToAdd);
        }

        private void AddModifier(Modifier modifier, ActivationCondition condition = default)
        {
            RegisterModifier(modifier);
            Modifiers.Add(modifier.Id, modifier);
            modifier.Init();
            modifier.SetActivationCondition(condition);
            Log.Verbose("Added modifier " + modifier.GetType().Name +" with target: " + modifier.Target?.Id, "modifiers");
        }

        public bool RemoveModifier(Modifier modifier)
        {
            return Modifiers.Remove(modifier.Id);
        }

        public bool ContainsModifier(Modifier modifier)
        {
            return Modifiers.ContainsKey(modifier.Id);
            //return Modifiers.All(internalModifier => internalModifier.Id == modifier.Id && internalModifier.GetType() == modifier.GetType());
        }

        /// <summary>
        ///     Used for refreshing, stacking ,etc
        /// </summary>
        public bool ContainsModifier(Modifier modifier, out Modifier internalModifier)
        {
            return Modifiers.TryGetValue(modifier.Id, out internalModifier);
            //return Modifiers.All(internalModifier => internalModifier.Id == modifier.Id && internalModifier.GetType() == modifier.GetType());
        }

        [CanBeNull]
        public IEnumerable<ModifierApplier<ModifierApplierData>> GetModifierAppliers()
        {
            //Log.Verbose(_ownerTarget?.Id, "modifiers");
            return Modifiers.Values.Where(mod => typeof(ModifierApplier<ModifierApplierData>).IsSameOrSubclass(mod.GetType()))
                .Cast<ModifierApplier<ModifierApplierData>>();
        }

        public void ListModifiers()
        {
            ListModifiers(Modifiers.Values);
        }
        public void ListModifiers([CanBeNull] IEnumerable<Modifier> modifiers)
        {
            if (modifiers != null)
                Log.Info(string.Join(". ", modifiers) + ". Modifiers count: " + Modifiers.Count, "modifiers", true);
        }

        private void CheckTarget(Modifier modifier, AddModifierParameters parameters)
        {
            if (parameters.HasFlag(AddModifierParameters.OwnerIsTarget))
            {
                if (modifier.Target == null)
                {
                    modifier.SetTarget(_ownerTarget);
                }
                else if (modifier.Target != _ownerTarget)
                {
                    Log.Error("Owner should be the target, but isn't. Target is: " + modifier.Target +". Reverting to owner", "modifiers");
                    modifier.SetTarget(_ownerTarget);
                }
            }
            else
            {
                //Modifier appliers dont need a target at ctor. Extra check, for good measure
                if (modifier.Target == null && parameters.HasFlag(AddModifierParameters.NullStartTarget) && !typeof(ModifierApplier<ModifierApplierData>).IsSameOrSubclass(modifier.GetType()))
                {
                    Log.Error("Owner isn't the target, and target is null", "modifiers");
                }
            }
        }

        private void RegisterModifier(Modifier modifier)
        {
            modifier.Removed += modifierEventItem => RemoveModifier(modifierEventItem);
            modifier.Removed += modifierEventItem => Log.Verbose(modifierEventItem.Id + " removed", "modifiers");
        }

        public override string ToString()
        {
            return "Modifiers: ";//TODO List all modifiers
        }
    }
}