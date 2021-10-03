using BaseProject;

namespace ComboSystem
{
    public abstract class EffectOverTimeModifier<TEffectOverTimeData> : Modifier<TEffectOverTimeData> where TEffectOverTimeData : EffectOverTimeData
    {
        protected float timer;
        protected float durationTimer;

        public EffectOverTimeModifier(string id, TEffectOverTimeData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        public override void Update(float deltaTime)
        {
            timer += deltaTime;
            durationTimer += deltaTime;
            base.Update(deltaTime);

            if (timer >= Data.EveryXSecond)
            {
                //Log.Verbose($"Applying {Id}, timer: {timer}", "modifiers");
                Apply();
                timer = 0;
            }

            if (durationTimer >= Data.Duration)
            {
                //Log.Verbose($"Removing {Id}, timer: {timer}", "modifiers");
                Remove();
                return;
            }
        }
    }
}