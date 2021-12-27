using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for everything timing related, interval, duration, etc
    /// </summary>
    public class TimeComponent : Component, ITimeComponent
    {
        public bool IsRemove { get; }
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
            IsRemove = false;
        }

        /// <summary>
        ///     Constructor for remove timer
        /// </summary>
        public TimeComponent(RemoveComponent removeComponent, double lingerDuration = 0.5d)
        {
            EffectComponent = removeComponent;
            Duration = lingerDuration;
            ResetOnFinished = false;
            IsRemove = true;
        }

        public void Update(float deltaTime, double statusResistance = 1d)
        {
            if (_finished)
                return;
            _timer += deltaTime;
            if (_timer >= Duration * statusResistance)
            {
                //Log.Info(_timer + "_"+Duration * statusResistance);
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

        public HashSet<StatusTag> GetStatusTags()
        {
            HashSet<StatusTag> tempStatusTags = new HashSet<StatusTag>();
            if (EffectComponentIsOfType<RemoveComponent>(false))
                tempStatusTags.Add(new StatusTag(StatusType.Duration));
            if (EffectComponentIsOfType<DamageComponent>(true))
                tempStatusTags.Add(new StatusTag(StatusType.DoT));
            if (EffectComponentIsOfType<StatusComponent>(true))
                tempStatusTags.Add(new StatusTag(StatusType.Stun));
            if (EffectComponentIsOfType<StatusResistanceComponent>(true))
                tempStatusTags.Add(new StatusTag(StatusType.Resistance));//Res? Recursion?
            //if (timeComponent.EffectComponentIsOfType<SlowComponent>(true))
            //    tempStatusTags.Add(new StatusTag(StatusType.Slow));
            return tempStatusTags;
        }

        private bool EffectComponentIsOfType<T>(bool checkResetOnFinished = true) where T : IEffectComponent
        {
            if(checkResetOnFinished)
                return ResetOnFinished && EffectComponent.GetType() == typeof(T);

            return EffectComponent.GetType() == typeof(T);
        }
    }
}