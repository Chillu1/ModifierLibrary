using BaseProject;

namespace ModifierSystem
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

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            ((Unit)receiver).ChangeDamageStat(DamageData);
        }

        protected override void RevertEffect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            var test = new DamageData[DamageData.Length];
            for (int i = 0; i < DamageData.Length; i++)
            {
                var damageData = DamageData[i];
                test[i] = new DamageData(-damageData.BaseDamage, damageData.DamageType, damageData.ElementData);
            }
            ((Unit)receiver).ChangeDamageStat(test);
        }
    }
}