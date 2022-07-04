using BaseProject;

namespace ModifierSystem
{
    //TODO Not 100% sure about this, but can't think of any other good way to force Aura to be on init, removable, and refreshable
    public class AuraEffectModifierGenerationProperties : ModifierGenerationProperties
    {
        public const double AuraRemoveTime = 1.1d;

        public AuraEffectModifierGenerationProperties(string id, ModifierInfo info) : base(id, info, LegalTarget.Self)
        {
        }

        public new void AddEffect(EffectComponent effectComponent, DamageData[] damageData = null)
        {
            currentEffectPropertyInfo = new EffectPropertyInfo(effectComponent);
            EffectPropertyInfo.Add(currentEffectPropertyInfo);
            SetEffectOnInit();
            SetRemovable(AuraRemoveTime);
            SetRefreshable();
            if (damageData != null)
                DamageData = damageData;
        }
    }
}