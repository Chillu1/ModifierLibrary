namespace ComboSystemComposition
{
    public class TimeComponent : Component, ITimeComponent
    {
        public double Duration { get; protected set; }
        public bool ResetOnFinished { get;  }
        public IEffectComponent EffectComponent { get; }
        private double _timer;

        public TimeComponent(double duration, bool resetOnFinished, IEffectComponent effectComponent)
        {
            Duration = duration;
            ResetOnFinished = resetOnFinished;
            EffectComponent = effectComponent;
        }

        public TimeComponent(double duration, RemoveComponent removeComponent)
        {
            Duration = duration;
            EffectComponent = removeComponent;
        }

        public virtual void Update(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer >= Duration)
            {
                //Remove, Effect, whatever
                //Elapsed();
                EffectComponent.Effect();
                if(ResetOnFinished)
                    _timer = 0;
            }
        }

        protected virtual void Elapsed()
        {
            EffectComponent.Effect();
        }
    }
}