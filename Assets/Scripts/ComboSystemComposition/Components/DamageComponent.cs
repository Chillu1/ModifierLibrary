namespace ComboSystemComposition
{
    public class DamageComponent : EffectComponent
    {
        //TODO Damage type to proper, elementalType, etc
        public double Damage { get; private set; }

        public DamageComponent(double damage) : base(delegate {  })
        {
            Damage = damage;
        }
    }
}