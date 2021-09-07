namespace ComboSystem
{
    /// <summary>
    ///     Applies & gets removed after linger time, one time use
    /// </summary>
    /// <typeparam name="TDataType"></typeparam>
    public abstract class SingleUseModifier<TDataType> : InitUseModifier<TDataType>
    {
        private float _timer;
        private const float LingerTime = 0.5f;
        
        protected SingleUseModifier(string id, TDataType data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
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