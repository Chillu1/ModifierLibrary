using System;
using BaseProject;
using BaseProject.Utils;

namespace ComboSystem
{
    /// <summary>
    ///     Base class for all beings that can have modifiers
    /// </summary>
    public sealed class ComboBeing : IEventCopy<ComboBeing>
    {
        public string Id => _baseBeing.Id;
        private BaseBeing _baseBeing;


        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event Action<ComboBeing> ComboEvent;

        private ModifierController ModifierController { get; }

        public ComboBeing(ComboBeingProperties properties)
        {
            _baseBeing = new BaseBeing(properties);
            ModifierController = new ModifierController(this);
            AddModifier(properties.ModifierHolder);
        }

        public void Update(float deltaTime)
        {
            ModifierController.Update(deltaTime);
        }

        public void Attack(ComboBeing target)
        {
            ApplyModifiers(target);
            _baseBeing.Attack(target._baseBeing);
        }

        public void DealDamage(DamageData[] data)
        {
            _baseBeing.DealDamage(data);
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        public void ApplyModifiers(ComboBeing target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            if (modifierAppliers == null)
            {
                Log.Verbose(_baseBeing.Id+" has no applier modifiers", "modifiers");
                return;
            }

            foreach (var modifierApplier in modifierAppliers)
                modifierApplier.ApplyModifierToTarget(target);
        }

        public void AddModifier(ModifierHolder modifierHolder)
        {
            if (modifierHolder == null)
            {
                Log.Verbose("ModifierHolder is null on: "+ _baseBeing.Id, "modifiers");
                return;
            }

            foreach (var modifierParams in modifierHolder.modifiers)
            {
                AddModifier(modifierParams.modifier, modifierParams.addModifierProperties, modifierParams.activateCondition);
            }
            //ModifierController.ListModifiers();
        }

        public void AddModifier(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default, ActivationCondition condition = default)
        {
            if (typeof(ModifierApplier<ModifierApplierData>).IsSameOrSubclass(modifier.GetType()))
            {
                //A special applier modifier
                if (!parameters.HasFlag(AddModifierParameters.NullStartTarget) || parameters == AddModifierParameters.Default)//Wrong? Can owner be target in applier mods?
                {
                    Log.Error("Wrong parameters for adding a modifier applier: "+parameters, "modifiers");
                    ModifierController.TryAddModifier(modifier, AddModifierParameters.DefaultOffensive, condition);
                    return;
                }
            }

            ModifierController.TryAddModifier(modifier, parameters, condition);
        }

        public void AddModifierApplier(ModifierApplier<ModifierApplierData> modifier,
            AddModifierParameters parameters = AddModifierParameters.DefaultOffensive, ActivationCondition condition = default)
        {
            AddModifier(modifier, parameters, condition);
        }

        public bool ContainsModifier(Modifier modifier)
        {
            return ModifierController.ContainsModifier(modifier);
        }

        public void ListModifiers()
        {
            ModifierController.ListModifiers();
        }

        public bool IsValidTarget(Modifier modifier)
        {
            return true;
        }

        public void CopyEvents(ComboBeing prototype)
        {
            _baseBeing.CopyEvents(prototype._baseBeing);
            ComboEvent = prototype.ComboEvent;
            //Copy modifierEvents
            //problem, we copy the event, but the target is wrong modifierController (old)
            ModifierController.CopyEvents(prototype.ModifierController);
        }

        public override string ToString()
        {
            return _baseBeing.ToString() + ModifierController;
        }
    }
}