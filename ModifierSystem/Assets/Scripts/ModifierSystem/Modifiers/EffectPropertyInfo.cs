namespace ModifierSystem
{
    public class EffectPropertyInfo
    {
        public IEffectProperties EffectProperties { get; }
        public IBaseEffectProperties BaseProperties { get; }
        public EffectComponent EffectComponent { get; private set; }

        public EffectOn EffectOn { get; private set; }

        //Time based

        public bool RepeatOnFinished { get; private set; }

        public double EffectDuration { get; private set; }

        public EffectPropertyInfo(IEffectProperties effectProperties, IBaseEffectProperties baseProperties = null)
        {
            EffectProperties = effectProperties;
            BaseProperties = baseProperties != null ? baseProperties : new EffectComponent.BaseProperties();
        }

        public EffectPropertyInfo(EffectComponent effectComponent)
        {
            EffectComponent = effectComponent;
        }

        public void SetEffectOnInit()
        {
            EffectOn |= EffectOn.Init;
        }

        public void SetEffectOnTime(double duration, bool repeatOnFinished)
        {
            EffectOn |= EffectOn.Time;
            EffectDuration = duration;
            RepeatOnFinished = repeatOnFinished;
        }

        public void SetEffectOnApply()
        {
            EffectOn |= EffectOn.Apply;
        }
    }
}