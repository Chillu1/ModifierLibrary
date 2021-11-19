namespace ComboSystem
{
    public abstract class SingleUseComboModifier<TDataType> : InitUseComboModifier<TDataType>
    {
        private float _timer;
        private const float LingerTime = 0.5f;

        protected SingleUseComboModifier(string id, TDataType data, ComboRecipe recipe, ModifierProperties modifierProperties = default) :
            base(id, data, recipe, modifierProperties)
        {
        }

        public override void Update(float deltaTime)
        {
            //We need to let the modifier linger, so combos can be registered  before it gets removed
            _timer+=deltaTime;
            if (_timer > LingerTime)
                Remove();
        }
    }
}