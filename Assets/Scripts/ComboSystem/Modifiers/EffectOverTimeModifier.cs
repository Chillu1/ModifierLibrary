namespace ComboSystem
{
    public class EffectOverTimeModifier : Modifier<EffectOverTimeData>
    {
        private float _timer;

        public EffectOverTimeModifier(EffectOverTimeData effectOverTimeData)
        {
            Data = effectOverTimeData;
        }

        protected override void Apply()
        {
            //target.ApplyEffect(data.effectType, data.value);
            //target.DealDamage(data.elementalType/*(damageType too, physical, magical*/, data.damage);
        }

        public override void Update(float deltaTime)
        {
            _timer += deltaTime;

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

            base.Update(deltaTime);
        }
    }
}