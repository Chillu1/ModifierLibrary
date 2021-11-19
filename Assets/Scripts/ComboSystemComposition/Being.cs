using System;
using BaseProject;

namespace ComboSystemComposition
{
    public sealed class Being : IBeing
    {
        public string Id => BaseBeing.Id;
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

        public void Attack(Being target)
        {
            ApplyModifiers(target);
            BaseBeing.Attack(target.BaseBeing);
        }

        public void DealDamage(DamageData[] data)
        {
            BaseBeing.DealDamage(data);
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        public void ApplyModifiers(Being target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            if (modifierAppliers == null)
            {
                Log.Verbose(Id+" has no applier modifiers", "modifiers");
                return;
            }

            foreach (var modifierApplier in modifierAppliers)
            {
                modifierApplier.TargetComponent.SetTarget(target);
                modifierApplier.Apply();
                //modifierApplier.ApplyModifierToTarget(target);
            }
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