namespace ComboSystem
{
    public class EffectOverTimeModifier<TEffectOverTimeData> : Modifier<TEffectOverTimeData> where TEffectOverTimeData : EffectOverTimeData
    {
        private float _timer;

        public EffectOverTimeModifier(TEffectOverTimeData damageOverTimeData)
        {
            Data = damageOverTimeData;
        }

        protected override void Apply()
        {
            //Target.ApplyEffect(data.effectType, data.value);
        }

        public override void Update(float deltaTime)
        {
            _timer += deltaTime;
            base.Update(deltaTime);

            if (_timer >= Data.Duration)
            {
                Remove();
                return;
            }

            if (_timer >= Data.EveryXSecond)
            {
                Apply();
                _timer = 0;
            }
        }
    }
}