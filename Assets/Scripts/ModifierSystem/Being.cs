using BaseProject;

namespace ModifierSystem
{
    public sealed class Being : BaseBeing
    {
        private ModifierController ModifierController { get; }

        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event BaseBeingEvent ComboEvent;

        public Being(BeingProperties beingProperties) : base(beingProperties)
        {
            ModifierController = new ModifierController(this, ElementController);
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

            if (!StatusEffects.LegalActions.HasFlag(LegalAction.Cast)) //Can't cast
                return false;

            modifier.TryApply(target);

            return true;
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        private void ApplyModifiers(Being target)
        {
            foreach (var modifierApplier in ModifierController.GetModifierAppliers())
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
            base.CopyEvents(prototype);
            ComboEvent = prototype.ComboEvent;
            //Copy modifierEvents
            //problem, we copy the event, but the target is wrong modifierController (old)
            ModifierController.CopyEvents(prototype.ModifierController);
        }

        public override string ToString()
        {
            return base.ToString() + ModifierController;
        }

        #region BaseBeing Methods

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            ModifierController.Update(deltaTime);
        }

        public override DamageData[] Attack(BaseBeing target)
        {
            return Attack(target, this);
        }

        /// <summary>
        ///     Manual attack, NOT a modifier attack
        /// </summary>
        public override DamageData[] Attack(BaseBeing target, BaseBeing attacker)
        {
            if (!attacker.StatusEffects.LegalActions.HasFlag(LegalAction.Act))//Can't attack
                return null;
            
            ApplyModifiers((Being)target);
            var damageData = base.Attack(target, attacker);

            //TODO we first Apply mods then attack. That way we add debuffs first, but we dont check for comboModifiers after attacking again, is that a problem?
            ((Being)attacker).ModifierController.CheckForComboRecipes();//Not redundant? Might lead to performance issues in super high combo counts?
            return damageData;
        }

        /// <summary>
        ///     Used for dealing damage with modifiers
        /// </summary>
        public override DamageData[] DealDamage(DamageData[] data, BaseBeing attacker, AttackType attackType = AttackType.DefaultAttack)
        {
            var damageData = base.DealDamage(data, attacker, attackType);
            ModifierController.CheckForComboRecipes();//Elemental, so we check for combos
            return damageData;
        }

        public void ChangeStat(Stat[] stats)
        {
            Stats.ChangeStat(stats);
            ModifierController.CheckForComboRecipes();
        }

        public void ChangeStat(Stat stat)
        {
            Stats.ChangeStat(stat);
            ModifierController.CheckForComboRecipes();
        }

        public void ChangeStat(StatType statType, double value)
        {
            Stats.ChangeStat(statType, value);
            ModifierController.CheckForComboRecipes();
        }

        public void ChangeDamageStat(DamageData[] damageData)
        {
            Stats.ChangeDamageStat(damageData);
            ModifierController.CheckForComboRecipes();
        }

        public void ChangeDamageStat(DamageData damageData)
        {
            Stats.ChangeDamageStat(damageData);
            ModifierController.CheckForComboRecipes();
        }

        #endregion
    }
}