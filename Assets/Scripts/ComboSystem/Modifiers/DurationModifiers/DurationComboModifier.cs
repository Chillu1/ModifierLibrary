using BaseProject;

namespace ComboSystem
{
    public abstract class DurationComboModifier<TDurationDataType> : ComboModifier<TDurationDataType> where TDurationDataType : DurationModifierData
    {
        protected float timer;

        protected DurationComboModifier(string id, TDurationDataType data, ComboRecipe recipe, ModifierProperties modifierProperties = default)
            : base(id, data, recipe, modifierProperties)
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
            if (ModifierProperties.HasFlag(ModifierProperties.Refreshable))//Not ideal hardcoding, if we want a duration modifier that doesn't refresh the timer but something else?
            {
                timer = 0f;
                Log.Info("Refreshed timer");
            }

            base.Refresh();
        }
    }
}