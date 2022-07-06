using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class RemoveComponent : IEffectComponent, IConditionEffectComponent
    {
        private readonly Modifier _modifier;
        [CanBeNull] private readonly CleanUpComponent _cleanUpComponent;

        public string Info { get; }

        public RemoveComponent(Modifier modifier, CleanUpComponent cleanUpComponent = null)
        {
            _modifier = modifier;
            _cleanUpComponent = cleanUpComponent;

            Info = $"Remove: {modifier}\n";
        }

        public void SimpleEffect()
        {
            _cleanUpComponent?.CleanUp();
            _modifier.SetForRemoval();
        }

        public void ConditionEffect(BaseProject.Unit receiver, BaseProject.Unit acter)//Can't make it proper, because Unit and not Unit
        {
            SimpleEffect();
        }
    }
}