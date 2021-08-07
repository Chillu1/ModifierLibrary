namespace ComboSystem
{
    public abstract class EffectOverTimeModifier<TEffectOverTimeData> : Modifier<TEffectOverTimeData> where TEffectOverTimeData : EffectOverTimeData
    {
        protected float timer;
        protected float durationTimer;

        public EffectOverTimeModifier(TEffectOverTimeData damageOverTimeData)
        {
            Data = damageOverTimeData;
        }

        protected override bool Apply()
        {
            //Target.ApplyEffect(data.effectType, data.value);
            return base.Apply();
        }

        public override void Update(float deltaTime)
        {
            timer += deltaTime;
            durationTimer += deltaTime;
            base.Update(deltaTime);

            if (durationTimer >= Data.Duration)
            {
                Remove();
                return;
            }

            if (timer >= Data.EveryXSecond)
            {
                Apply();
                timer = 0;
            }
        }
    }
}