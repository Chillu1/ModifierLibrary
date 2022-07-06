using System.Linq;
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
            //TODO To array because the list might be modified during iteration, might be smart to have a separate hashset of keys of AttackAppliers,
            foreach (var modifierApplier in ModifierController.GetModifierAttackAppliers().ToArray())
            {
                //Debug.Log("Applying: " + modifierApplier);
                modifierApplier.TryApply(target);
            }
        }
        
        /// <summary>
        ///     Only set sourceBeing when modifier has a taunt effect (not ideal obvs)
        /// </summary>
        public void AddModifier(Modifier modifier, Being sourceBeing = null)
        {
            AddModifier(modifier, modifier.Parameters, sourceBeing);
        }

        public void AddModifierWithParameters(Modifier modifier, AddModifierParameters parameters, Being sourceBeing = null)
        {
            AddModifier(modifier, parameters, sourceBeing);
        }

        private void AddModifier(Modifier modifier, AddModifierParameters parameters, Being sourceBeing = null)
        {
            bool modifierAdded = ModifierController.TryAddModifier(modifier, parameters, sourceBeing);

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