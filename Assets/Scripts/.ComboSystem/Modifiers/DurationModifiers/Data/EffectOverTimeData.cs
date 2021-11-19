namespace ComboSystem
{
    public class EffectOverTimeData
    {
        public EffectType EffectType { get; protected set; }
        public float EveryXSecond { get; protected set; }
        public float Duration { get; protected set; }

        public EffectOverTimeData(EffectType effectType, float everyXSecond, float duration)
        {
            EffectType = effectType;
            EveryXSecond = everyXSecond;
            Duration = duration;
        }
    }
}