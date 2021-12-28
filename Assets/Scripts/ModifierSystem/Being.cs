using System;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public sealed class Being : IBaseBeing
    {
        private ModifierController ModifierController { get; }

        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event Action<Being> ComboEvent;

        #region BaseBeing Members

        private BaseBeing BaseBeing { get; }

        public string Id => BaseBeing.Id;
        public Stats Stats => BaseBeing.Stats;
        public StatusResistances StatusResistances => BaseBeing.StatusResistances;
        public UnitType UnitType => BaseBeing.UnitType;
        public LegalAction LegalActions => BaseBeing.StatusEffects.LegalActions;

        public event BaseBeingEvent AttackEvent
        {
            add => BaseBeing.AttackEvent += value;
            remove => BaseBeing.AttackEvent -= value;
        }
        public event BaseBeingEvent KillEvent
        {
            add => BaseBeing.KillEvent += value;
            remove => BaseBeing.KillEvent -= value;
        }
        public event BaseBeingEvent CastEvent
        {
            add => BaseBeing.CastEvent += value;
            remove => BaseBeing.CastEvent -= value;
        }
        public event BaseBeingEvent HealEvent
        {
            add => BaseBeing.HealEvent += value;
            remove => BaseBeing.HealEvent -= value;
        }
        public event BaseBeingEvent HealedEvent
        {
            add => BaseBeing.HealedEvent += value;
            remove => BaseBeing.HealedEvent -= value;
        }
        public event BaseBeingEvent DeathEvent
        {
            add => BaseBeing.DeathEvent += value;
            remove => BaseBeing.DeathEvent -= value;
        }
        public event BaseBeingEvent HitEvent
        {
            add => BaseBeing.HitEvent += value;
            remove => BaseBeing.HitEvent -= value;
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

            if (!LegalActions.HasFlag(LegalAction.Cast)) //Can't cast
                return false;

            modifier.TryApply(target);

            return true;
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        private void ApplyModifiers(Being target)
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
            BaseBeing.Update(deltaTime);
            ModifierController.Update(deltaTime);
        }

        public void ChangeStatusEffect(StatusEffect effect, float amount)
        {
            BaseBeing.ChangeStatusEffect(effect, amount);
        }

        /// <summary>
        ///     Manual attack, NOT a modifier attack
        /// </summary>
        [CanBeNull]
        public DamageData[] Attack(Being target)
        {
            if (!LegalActions.HasFlag(LegalAction.Act))//Can't attack
                return null;
            
            ApplyModifiers(target);
            var damageData = BaseBeing.Attack(target.BaseBeing);

            //TODO we first Apply mods then attack. That way we add debuffs first, but we dont check for comboModifiers after attacking again, is that a problem?
            ModifierController.CheckForComboRecipes();//Not redundant? Might lead to performance issues in super high combo counts?
            return damageData;
        }

        /// <summary>
        ///     Used for dealing damage with modifiers
        /// </summary>
        public DamageData[] DealDamage(DamageData[] data, Being attacker)
        {
            var damageData = BaseBeing.DealDamage(data, attacker.BaseBeing);
            ModifierController.CheckForComboRecipes();//Elemental, so we check for combos
            return damageData;
        }

        public void Heal(Being target)
        {
            BaseBeing.Heal(target.BaseBeing);
        }
        public void Heal(double value, BaseBeing healer)
        {
            BaseBeing.Heal(value, healer);
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