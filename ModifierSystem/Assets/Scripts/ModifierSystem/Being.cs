using BaseProject;
using Force.DeepCloner;
using UnityEngine;

namespace ModifierSystem
{
    public class Being : BaseBeing
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
            bool modifierAdded = ModifierController.TryAddModifier(modifier, parameters);

            if (modifierAdded && modifier.ApplierType.HasFlag(ApplierType.Cast))
                CastingController.AddCastModifier(modifier);
        }

        public bool ContainsModifier(string id)
        {
            return ModifierController.ContainsModifier(id);
        }

        public bool ContainsModifier(Modifier modifier)
        {
            return ModifierController.ContainsModifier(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            ModifierController.RemoveModifier(modifier);
            if (modifier.ApplierType.HasFlag(ApplierType.Cast))
                CastingController.RemoveCastModifier(modifier);
        }

        public void SetAutomaticCastAll(bool automaticCast = true)
        {
            ModifierController.SetAutomaticCastAll(automaticCast);
        }

        public void CopyEvents(Being prototype)
        {
            base.CopyEvents(prototype);
            ComboEvent = prototype.ComboEvent;
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

        public override object Clone()
        {
            var clone = this.DeepClone();
            //clone.CopyEvents(this);//Doesn't work? + DeepClone does it instead?

            return clone;
        }

        #endregion
    }
}