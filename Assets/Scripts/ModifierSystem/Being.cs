using BaseProject;
using UnityEngine;

namespace ModifierSystem
{
    public sealed class Being : BaseBeing
    {
        private ModifierController ModifierController { get; }
        private CastingController CastingController { get; }

        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event BaseBeingEvent ComboEvent;

        public Being(BeingProperties beingProperties) : base(beingProperties)
        {
            ModifierController = new ModifierController(this, ElementController);
            CastingController = new CastingController(ModifierController, StatusEffects, TargetingSystem);
        }

        public bool CastModifier(Being target, string modifierId)
        {
            return CastingController.CastModifier(target, modifierId);
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        private void ApplyAttackModifiers(Being target)
        {
            foreach (var modifierApplier in ModifierController.GetModifierAttackAppliers())
            {
                //Debug.Log("Applying: " + modifierApplier);
                modifierApplier.TryApply(target);
            }
        }

        public void AddModifier(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default)
        {
            ModifierController.TryAddModifier(modifier, parameters);
        }

        public bool ContainsModifier(string id)
        {
            return ModifierController.ContainsModifier(id);
        }

        public bool ContainsModifier(Modifier modifier)
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
            CastingController.Update(deltaTime);
        }

        public override DamageData[] Attack(BaseBeing target)
        {
            //Debug.Log("Attack");
            return Attack((Being)target, this);
        }

        /// <summary>
        ///     Manual attack, NOT a modifier attack
        /// </summary>
        public static DamageData[] Attack(Being target, Being attacker)
        {
            if (!attacker.StatusEffects.LegalActions.HasFlag(LegalAction.Act)) //Can't attack
                return null;

            attacker.ApplyAttackModifiers(target);
            var damageData = BaseBeing.Attack(target, attacker);

            //TODO we first Apply mods then attack. That way we add debuffs first, but we dont check for comboModifiers after attacking again, is that a problem?
            attacker.ModifierController
                .CheckForComboRecipes(); //Not redundant? Might lead to performance issues in super high combo counts?
            return damageData;
        }

        /// <summary>
        ///     Used for dealing damage with modifiers
        /// </summary>
        public override DamageData[] DealDamage(DamageData[] data, BaseBeing attacker, AttackType attackType = AttackType.DefaultAttack)
        {
            var damageData = base.DealDamage(data, attacker, attackType);
            ModifierController.CheckForComboRecipes(); //Elemental, so we check for combos
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