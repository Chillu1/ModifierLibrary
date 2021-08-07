namespace ComboSystem
{
    public abstract class EffectOverTimeModifier<TEffectOverTimeData> : Modifier<TEffectOverTimeData> where TEffectOverTimeData : EffectOverTimeData
    {
        protected float timer;
        protected float durationTimer;

        public EffectOverTimeModifier(string id, TEffectOverTimeData damageOverTimeData, ModifierProperties properties = default) : base(id, properties)
        {
            Data = damageOverTimeData;
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