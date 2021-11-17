using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseProject;
using BaseProject.Utils;
using ComboSystem;
using JetBrains.Annotations;

namespace ComboSystemComposition
{
    public class ModifierController : IEventCopy<ModifierController>
    {
        private readonly Being _owner;
        private Dictionary<string, Modifier> Modifiers { get; }

        public ModifierController(Being owner)
        {
            Modifiers = new Dictionary<string, Modifier>();
            _owner = owner;
        }

        public void Update(float deltaTime)
        {
            //Log.Info(Modifiers.Count);
            foreach (var valuePair in Modifiers.ToArray())//TODO Making it into an array every frame is uncool, instead mark them as to deleted
            {
                valuePair.Value.Update(deltaTime);
            }
        }

        public void TryAddModifier(Modifier modifier)
        {
            //CheckTarget(modifier, parameters);

            if (ContainsModifier(modifier, out Modifier internalModifier))
            {
                Log.Verbose("HasModifier " + modifier.Id, "modifiers");
                /*switch (modifier.ModifierProperties)
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
                }*/
            }
            else
                AddModifier(modifier);

            //if (parameters.HasFlag(AddModifierParameters.CheckRecipes))
            //{
            //    var comboModifierToAdd = ComboModifierPrototypes.CheckForComboRecipes(Modifiers);
            //    if (comboModifierToAdd.Count > 0)
            //        AddComboModifier(comboModifierToAdd);
            //}
        }

        //private void AddComboModifier(IEnumerable<ComboModifier> modifiers)
        //{
        //    foreach (var modifier in modifiers)
        //    {
        //        TryAddModifier(modifier, AddModifierParameters.OwnerIsTarget);
        //    }
        //    //Check for recipes after adding all modifiers
        //    var comboModifierToAdd = ComboModifierPrototypes.CheckForComboRecipes(Modifiers);
        //    if(comboModifierToAdd.Count > 0)
        //        AddComboModifier(comboModifierToAdd);
        //}

        private void AddModifier(Modifier modifier)
        {
            RegisterModifier(modifier);
            Modifiers.Add(modifier.Id, modifier);
            modifier.Init();
            Log.Verbose("Added modifier " + modifier.GetType().Name +" with target: " + modifier.TargetComponent.Target?.Id, "modifiers");
        }

        public bool RemoveModifier(Modifier modifier)
        {
            bool success = Modifiers.Remove(modifier.Id);
            if(!success)
                Log.Error("Couldn't remove modifier " + modifier.Id, "modifiers");
            return success;
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
            //return Modifiers.All(internalModifier => internalModifier.Id == modifier.Id && internalModifier.GetType() == modifier.GetType());
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

        private void CheckTarget(Modifier modifier, AddModifierParameters parameters)
        {
            if (parameters.HasFlag(AddModifierParameters.OwnerIsTarget))
            {
                if (modifier.TargetComponent.Target == null)
                {
                    modifier.SetTarget(_owner);
                }
                else if (modifier.TargetComponent.Target != _owner)
                {
                    Log.Error("Owner should be the target, but isn't. Target is: " + modifier.TargetComponent.Target +". Reverting to owner", "modifiers");
                    modifier.SetTarget(_owner);
                }
            }
            else
            {
                //Modifier appliers dont need a target at ctor. Extra check, for good measure
                if (modifier.TargetComponent.Target == null && parameters.HasFlag(AddModifierParameters.NullStartTarget) && !typeof(ModifierApplier<ModifierApplierData>).IsSameOrSubclass(modifier.GetType()))
                {
                    Log.Error("Owner isn't the target, and target is null", "modifiers");
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

        [CanBeNull]
        public IEnumerable<Modifier> GetModifierAppliers()
        {
            return Modifiers.Values.Where(m => m.ApplierModifier);
        }
    }
}