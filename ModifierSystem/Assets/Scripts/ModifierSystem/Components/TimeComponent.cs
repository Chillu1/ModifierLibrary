using System.Collections.Generic;
using System.Text;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for everything timing related, interval, duration, etc
    /// </summary>
    public class TimeComponent : Component, ITimeComponent, IRefreshEffectComponent
    {
        public bool IsRemove { get; }
        private ICheckComponent CheckComponent { get; }
        private RemoveComponent RemoveEffectComponent { get; }
        private double Duration { get; }
        private bool ResetOnFinished { get; }
        private double _timer;
        private bool _finished;

        public TimeComponent(ICheckComponent checkComponent, double duration, bool resetOnFinished = false)
        {
            CheckComponent = checkComponent;
            Duration = duration;
            ResetOnFinished = resetOnFinished;
            IsRemove = false;
        }

        /// <summary>
        ///     Constructor for remove timer
        /// </summary>
        public TimeComponent(RemoveComponent removeComponent, double lingerDuration = 0.5d)
        {
            RemoveEffectComponent = removeComponent;
            Duration = lingerDuration;
            ResetOnFinished = false;
            IsRemove = true;
        }

        public void Update(float deltaTime, double statusResistance = 1d)
        {
            if (_finished)
                return;
            _timer += deltaTime / statusResistance;// / timer instead. So it's a dynamic timer, and not making it so players can switch between status res
            if (_timer >= Duration)
            {
                //Log.Info(_timer + "_"+Duration * statusResistance);
                if(IsRemove)
                    RemoveEffectComponent.SimpleEffect();
                else
                    CheckComponent.Effect();

                _finished = true;

                if(ResetOnFinished)
                {
                    _timer = 0;
                    _finished = false;
                }
            }
        }

        public void RefreshEffect(RefreshEffectType refreshEffectType)
        {
            switch (refreshEffectType)
            {
                case RefreshEffectType.RefreshDuration:
                    RefreshTimer();
                    break;
                //case RefreshEffectType.Effect:
                //    EffectComponent.Effect();
                //    break;
                default:
                    Log.Error($"Unsupported refresh effect type {refreshEffectType}");
                    return;
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

        public void DisplayText(StringBuilder builder)
        {
            if (IsRemove)
            {
                builder.Append("Remove in: ");
                builder.Append((Duration - _timer).ToString("F2"));
            }
            else
            {
                builder.Append("Effect in: ");
                builder.Append((Duration - _timer).ToString("F2"));
            }

            builder.AppendLine();
        }

        private bool EffectComponentIsOfType<T>(bool checkResetOnFinished = true) where T : IEffectComponent
        {
            if (IsRemove)
            {
                return checkResetOnFinished
                    ? ResetOnFinished && RemoveEffectComponent.GetType() == typeof(T)
                    : RemoveEffectComponent.GetType() == typeof(T);
            }

            return checkResetOnFinished
                ? ResetOnFinished && CheckComponent.EffectComponentIsOfType<T>()
                : CheckComponent.EffectComponentIsOfType<T>();
        }
    }
}