using UnitLibrary;

namespace ModifierLibrary
{
    public sealed class DamageStatComponent : EffectComponent
    {
        private DamageData[] DamageData { get; }

        public DamageStatComponent(DamageData[] damageData, ConditionCheckData conditionCheckData = null, bool isRevertible = false) :
            base(conditionCheckData, isRevertible)
        {
            DamageData = damageData;

            Info = $"DamageStat: {string.Join<DamageData>(", ", damageData)}\n";
        }

        protected override void Effect(Unit receiver, Unit acter)
        {
            receiver.ChangeDamageStat(DamageData);
        }

        protected override void RevertEffect(Unit receiver, Unit acter)
        {
            var negativeDamageStat = new DamageData[DamageData.Length];
            for (int i = 0; i < DamageData.Length; i++)
            {
                var damageData = DamageData[i];
                negativeDamageStat[i] = new DamageData(-damageData.BaseDamage, damageData.DamageType, damageData.ElementData);
            }
            receiver.ChangeDamageStat(negativeDamageStat);
        }
    }
}