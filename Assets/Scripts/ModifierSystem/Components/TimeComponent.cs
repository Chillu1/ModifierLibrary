namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for everything timing related, interval, duration, etc
    /// </summary>
    public class TimeComponent : Component, ITimeComponent
    {
        private IEffectComponent EffectComponent { get; }
        private double Duration { get; set; }
        private bool ResetOnFinished { get; }
        private double _timer;
        private bool _finished;

        public TimeComponent(IEffectComponent effectComponent, double duration, bool resetOnFinished = false)
        {
            EffectComponent = effectComponent;
            Duration = duration;
            ResetOnFinished = resetOnFinished;
        }

        /// <summary>
        ///     Constructor for remove timer
        /// </summary>
        public TimeComponent(RemoveComponent removeComponent, double lingerDuration = 0.5d)
        {
            EffectComponent = removeComponent;
            Duration = lingerDuration;
            ResetOnFinished = false;
        }

        public void Init(ModifierController modifierController)
        {
            if (EffectComponent is RemoveComponent)
            {
                var removeComponent = (RemoveComponent)EffectComponent;
                removeComponent.Init(modifierController);
            }
        }

        public void Update(float deltaTime, double statusResistance = 1d)
        {
            if (_finished)
                return;
            _timer += deltaTime;
            if (_timer >= Duration * statusResistance)
            {
                EffectComponent.Effect();
                _finished = true;

                if(ResetOnFinished)
                {
                    _timer = 0;
                    _finished = false;
                }
            }
        }

        public void RefreshTimer()
        {
            _timer = 0;
        }

        public bool EffectComponentIsOfType<T>(bool checkResetOnFinished = true) where T : IEffectComponent
        {
            if(checkResetOnFinished)
                return ResetOnFinished && EffectComponent.GetType() == typeof(T);

            return EffectComponent.GetType() == typeof(T);
        }
    }
}