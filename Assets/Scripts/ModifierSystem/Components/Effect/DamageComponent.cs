using BaseProject;

namespace ModifierSystem
{
    public class DamageComponent : IEffectComponent
    {
        public DamageData[] Damage { get; private set; }
        private ITargetComponent TargetComponent { get; }

        public DamageComponent(DamageData[] damage, ITargetComponent targetComponent)
        {
            Damage = damage;
            TargetComponent = targetComponent;
        }

        public void Effect()
        {
            //Log.Info(_getTarget().BaseBeing.Id);
            TargetComponent.GetTarget().DealDamage(Damage);
        }

        public void IncreaseDamage(double damage)
        {
            Damage[0].BaseDamage+= damage;
        }
    }
}