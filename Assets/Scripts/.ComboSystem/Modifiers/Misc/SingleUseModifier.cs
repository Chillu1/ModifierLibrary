using BaseProject;

namespace ComboSystem
{
    /// <summary>
    ///     Applies & gets removed after linger time, one time use
    /// </summary>
    /// <example>Can be used: on attacked, procs fire shield, etc.</example>
    /// <typeparam name="TDataType"></typeparam>
    public abstract class SingleUseModifier<TDataType> : Modifier<TDataType>
    {
        protected bool IsOn;
        private float _timer;
        //TODO Linger time can be based on attackspeed-10%, or castspeed-10%, unless it's above 0.5s, then 0.5s instead±.
        //Should lingerTime be based on the modifier, and not const for all SingleUseModifiers?
        private const float LingerTime = 0.5f;

        protected SingleUseModifier(string id, TDataType data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        public override void Update(float deltaTime)
        {
            Log.Info(IsOn);
            if (!IsOn)
                return;

            //We need to let the modifier linger, so combos can be registered before it gets removed
            _timer+=deltaTime;
            if (_timer > LingerTime)
                Remove();
        }

        protected override void Effect()
        {
            Log.Info("Effect");
            IsOn = true;
        }
    }
}