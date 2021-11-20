using System;
using BaseProject;

namespace ModifierSystem
{
    public sealed class Being : IBeing
    {
        public BaseBeing BaseBeing { get; }

        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event Action<Being> ComboEvent;
        private ModifierController ModifierController { get; }

        public Being(BeingProperties beingProperties)
        {
            BaseBeing = new BaseBeing(beingProperties);
            ModifierController = new ModifierController(this);
        }

        public void Update(float deltaTime)
        {
            ModifierController.Update(deltaTime);
        }

        public bool CastModifier(Being target, string modifierId)
        {
            if (!ModifierController.ContainsModifier(modifierId, out var modifier))
            {
                Log.Error("Modifier " + modifierId + " not present in collection", "modifiers");
                return false;
            }

            if (!modifier.ApplierModifier)
            {
                //TODO Not sure, about this one, but probably true
                Log.Error("Can't cast a non-applier modifier: "+modifierId, "modifiers");
                return false;
            }

            modifier.TryApply(target);

            return true;
        }

        /// <summary>
        ///     Manual attack, NOT a modifier attack
        /// </summary>
        public void Attack(Being target)
        {
            ApplyModifiers(target);
            BaseBeing.Attack(target.BaseBeing);
        }

        /// <summary>
        ///     Used for dealing damage with modifiers
        /// </summary>
        public DamageData[] DealDamage(DamageData[] data)
        {
            return BaseBeing.DealDamage(data);
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        public void ApplyModifiers(Being target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            if (modifierAppliers == null)
            {
                Log.Verbose(BaseBeing.Id+" has no applier modifiers", "modifiers");
                return;
            }

            foreach (var modifierApplier in modifierAppliers)
                modifierApplier.TryApply(target);
        }

        public void AddModifier(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default)
        {
            ModifierController.TryAddModifier(modifier, parameters);
        }

        public bool ContainsModifier(Modifier modifier)
        {
            return ModifierController.ContainsModifier(modifier);
        }

        public void CopyEvents(Being prototype)
        {
            BaseBeing.CopyEvents(prototype.BaseBeing);
            ComboEvent = prototype.ComboEvent;
            //Copy modifierEvents
            //problem, we copy the event, but the target is wrong modifierController (old)
            ModifierController.CopyEvents(prototype.ModifierController);
        }

        public override string ToString()
        {
            return BaseBeing.ToString() + ModifierController;
        }
    }
}