namespace ComboSystem
{
    public abstract class DurationModifier<TDurationDataType> : Modifier<TDurationDataType> where TDurationDataType : DurationModifierData
    {
        protected float timer;

        protected DurationModifier(string id, ModifierProperties modifierProperties = default) : base(id, modifierProperties)
        {
        }

        public override void Update(float deltaTime)
        {
            timer += deltaTime;
            if (timer >= Data.Duration)
            {
                Remove();
                return;
            }

            base.Update(deltaTime);
        }

        public override void Refresh()
        {
            base.Refresh();
            if (ModifierProperties.HasFlag(ModifierProperties.Refreshable))
            {
                timer = 0f;
                Log.Info("Refreshed timer");
            }
        }
    }
}