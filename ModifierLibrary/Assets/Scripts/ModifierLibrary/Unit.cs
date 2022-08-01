using System.Linq;
using UnitLibrary;
using Force.DeepCloner;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ModifierLibrary
{
    public delegate void UnitEvent(Unit owner, Unit acter);
    
    public class Unit : UnitLibrary.Unit
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

        /// <summary>
        ///     For loading in from a save file.
        /// </summary>
        public Unit(JObject properties) : base(properties)
        {
            ModifierController = new ModifierController(this, ElementController);
            CastingController = new CastingController(ModifierController, StatusEffects, TargetingSystem);
        }

        public bool CastModifier(Modifier modifier)
        {
            return CastingController.CastModifier(modifier);
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

        public void SetGlobalAutomaticCast(bool automaticCast = true)
        {
            ModifierController.SetAutomaticCastAll(automaticCast);
            CastingController.SetGlobalAutomaticCast(automaticCast);
        }

        public Modifier[] GetModifiersUIOrder()
        {
            return ModifierController.GetModifiersUIOrder();
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
        
        [CanBeNull]
        public DamageData[] Attack(Unit target)
        {
            if (!ValidAttack(target))
                return null;

            return InternalAttack(target);
        }

        public override DamageData[] Attack(UnitLibrary.Unit target)
        {
            return Attack((Unit)target);
        }

        /// <summary>
        ///     Manual attack, NOT a modifier attack
        /// </summary>
        private DamageData[] InternalAttack(Unit target)
        {
            ApplyAttackModifiers(target);
            var damageData = base.InternalAttack(target);

            //TODO we first Apply mods then attack. That way we add debuffs first, but we dont check for comboModifiers after attacking again, is that a problem?
            ModifierController
                .CheckForComboRecipes(); //Not redundant? Might lead to performance issues in super high combo counts?
            return damageData;
        }

        /// <summary>
        ///     Used for dealing damage with modifiers
        /// </summary>
        public override DamageData[] DealDamage(DamageData[] data, UnitLibrary.Unit attacker, AttackType attackType = AttackType.DefaultAttack)
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
        
        public void SetGlobalRegenMultiplier(PoolStatType statType, double multiplier)
        {
            Stats.SetGlobalRegenMultiplier(statType, multiplier);
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

        protected override void SaveExtra(JsonTextWriter writer)
        {
            ModifierController.Save(writer);
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