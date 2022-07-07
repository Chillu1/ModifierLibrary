using System.Linq;
using BaseProject;
using Force.DeepCloner;
using UnityEngine;

namespace ModifierSystem
{
    public class Unit : BaseProject.Unit
    {
        private ModifierController ModifierController { get; }
        private CastingController CastingController { get; }

        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event UnitEvent ComboEvent;

        public Unit(UnitProperties unitProperties) : base(unitProperties)
        {
            ModifierController = new ModifierController(this, ElementController);
            CastingController = new CastingController(ModifierController, StatusEffects, TargetingSystem);
        }

        public bool CastModifier(Unit target, string modifierId)
        {
            return CastingController.CastModifier(target, modifierId);
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        private void ApplyAttackModifiers(Unit target)
        {
            //TODO To array because the list might be modified during iteration, might be smart to have a separate hashset of keys of AttackAppliers,
            foreach (var modifierApplier in ModifierController.GetModifierAttackAppliers().ToArray())
            {
                //Debug.Log("Applying: " + modifierApplier);
                modifierApplier.TryApply(target);
            }
        }
        
        /// <summary>
        ///     Only set sourceUnit when modifier has a taunt effect (not ideal obvs)
        /// </summary>
        public void AddModifier(Modifier modifier, Unit sourceUnit = null)
        {
            AddModifier(modifier, modifier.Parameters, sourceUnit);
        }

        public void AddModifierWithParameters(Modifier modifier, AddModifierParameters parameters, Unit sourceUnit = null)
        {
            AddModifier(modifier, parameters, sourceUnit);
        }

        private void AddModifier(Modifier modifier, AddModifierParameters parameters, Unit sourceUnit = null)
        {
            bool modifierAdded = ModifierController.TryAddModifier(modifier, parameters, sourceUnit);

            if (modifierAdded && (modifier.ApplierType.HasFlag(ApplierType.Cast) || modifier.ApplierType.HasFlag(ApplierType.Aura)))
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
            bool modifierRemoved = ModifierController.RemoveModifier(modifier);
            if (modifierRemoved && (modifier.ApplierType.HasFlag(ApplierType.Cast) || modifier.ApplierType.HasFlag(ApplierType.Aura)))
                CastingController.RemoveCastModifier(modifier);
        }

        public void SetAutomaticCastAll(bool automaticCast = true)
        {
            ModifierController.SetAutomaticCastAll(automaticCast);
            CastingController.SetAutomaticCastAll(automaticCast);
        }

        public Modifier[] GetModifiersInfo()
        {
            return ModifierController.GetModifiersInfo();
        }

        public void CopyEvents(Unit prototype)
        {
            base.CopyEvents(prototype);
            ComboEvent = prototype.ComboEvent;
        }

        public override string ToString()
        {
            return base.ToString() + ModifierController;
        }

        #region Unit Methods

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            ModifierController.Update(deltaTime);
            CastingController.Update(deltaTime);
        }

        public override DamageData[] Attack(BaseProject.Unit target)
        {
            //Debug.Log("Attack");
            return Attack((Unit)target, this);
        }

        /// <summary>
        ///     Manual attack, NOT a modifier attack
        /// </summary>
        public static DamageData[] Attack(Unit target, Unit attacker)
        {
            if (!attacker.StatusEffects.LegalActions.HasFlag(LegalAction.Act)) //Can't attack
                return null;

            attacker.ApplyAttackModifiers(target);
            var damageData = BaseProject.Unit.Attack(target, attacker);

            //TODO we first Apply mods then attack. That way we add debuffs first, but we dont check for comboModifiers after attacking again, is that a problem?
            attacker.ModifierController
                .CheckForComboRecipes(); //Not redundant? Might lead to performance issues in super high combo counts?
            return damageData;
        }

        /// <summary>
        ///     Used for dealing damage with modifiers
        /// </summary>
        public override DamageData[] DealDamage(DamageData[] data, BaseProject.Unit attacker, AttackType attackType = AttackType.DefaultAttack)
        {
            var damageData = base.DealDamage(data, attacker, attackType);
            ModifierController.CheckForComboRecipes(); //Elemental, so we check for combos
            return damageData;
        }
        
        public void ChangeStat((StatType type, double value)[] stats)
        {
            Stats.ChangeStat(stats);
            ModifierController.CheckForComboRecipes();
        }

        public void ChangeStat(StatType statType, double value)
        {
            Stats.ChangeStat(statType, value);
            ModifierController.CheckForComboRecipes();
        }
        
        public void ChangeStatMultiplier((StatType type, double multiplier)[] stats)
        {
            Stats.ChangeStatMultiplier(stats);
            ModifierController.CheckForComboRecipes();
        }

        public void ChangeStatMultiplier(StatType statType, double multiplier)
        {
            Stats.ChangeStatMultiplier(statType, multiplier);
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