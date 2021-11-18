namespace ComboSystem
{
    /// <summary>
    ///     Applies on init & gets removed after linger time, one time use
    /// </summary>
    /// <example>Can be used: on damage someone (special debuff) etc.</example>
    /// <typeparam name="TDataType"></typeparam>
    public abstract class SingleUseInitModifier<TDataType> : InitUseModifier<TDataType>
    {
        private float _timer;
        //TODO Linger time can be based on attackspeed-10%, or castspeed-10%, unless it's above 0.5s, then 0.5s instead±.
        //Should lingerTime be based on the modifier, and not const for all SingleUseModifiers?
        private const float LingerTime = 0.5f;
        
        protected SingleUseInitModifier(string id, TDataType data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        public override void Update(float deltaTime)
        {
            //We need to let the modifier linger, so combos can be registered before it gets removed
            _timer+=deltaTime;
            if (_timer > LingerTime) 
                Remove();
        }
    }
}