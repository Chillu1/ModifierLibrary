using BaseProject;

namespace ModifierSystem
{
    public class StatComponent : EffectComponent
    {
        private readonly Stat[] _stats;
        private readonly ITargetComponent _targetComponent;

        public StatComponent(Stat[] stats, ITargetComponent targetComponent)
        {
            _stats = stats;
            _targetComponent = targetComponent;
        }

        public override void Effect()
        {
            _targetComponent.Target.ChangeStat(_stats);
        }
    }
}