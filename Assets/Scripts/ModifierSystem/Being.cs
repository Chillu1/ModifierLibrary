using System;
using BaseProject;

namespace ModifierSystem
{
    public sealed class Being
    {
        private ModifierController ModifierController { get; }

        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event Action<Being> ComboEvent;

        #region BaseBeing Members

        private BaseBeing BaseBeing { get; }

        public string Id => BaseBeing.Id;

        public HealthStat Health => BaseBeing.Health;

        public UnitType UnitType => BaseBeing.UnitType;

        public bool IsDead => BaseBeing.IsDead;

        public event Action<BaseBeing, BaseBeing> AttackEvent
        {
            add => BaseBeing.AttackEvent += value;
            remove => BaseBeing.AttackEvent -= value;
        }

        #endregion

        public Being(BeingProperties beingProperties)
        {
            BaseBeing = new BaseBeing(beingProperties);
            ModifierController = new ModifierController(this, BaseBeing.ElementController);
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
                Log.Error("Can't cast a non-applier modifier: " + modifierId, "modifiers");
                return false;
            }

            modifier.TryApply(target);

            return true;
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        public void ApplyModifiers(Being target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            if (modifierAppliers == null)
            {
                Log.Verbose(BaseBeing.Id + " has no applier modifiers", "modifiers");
                return;
            }

            foreach (var modifierApplier in modifierAppliers)
                modifierApplier.TryApply(target);
        }

        public void AddModifier(IModifier modifier, AddModifierParameters parameters = AddModifierParameters.Default)
        {
            ModifierController.TryAddModifier(modifier, parameters);
        }

        public bool ContainsModifier(string id)
        {
            return ModifierController.ContainsModifier(id);
        }

        public bool ContainsModifier(IModifier modifier)
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

        #region BaseBeing Methods

        public void Update(float deltaTime)
        {
            ModifierController.Update(deltaTime);
            BaseBeing.Update(deltaTime);
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

        public bool CheckStat(StatType type, double value)
        {
            return BaseBeing.CheckStat(type, value);
        }

        public void ChangeStat(Stat[] stats)
        {
            BaseBeing.ChangeStat(stats);
        }

        public void ChangeDamageStat(DamageData damageData)
        {
            BaseBeing.ChangeDamageStat(damageData);
        }

        #endregion
    }
}