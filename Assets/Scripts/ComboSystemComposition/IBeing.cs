namespace ComboSystemComposition
{
    public interface IBeing
    {
        string Id { get; }
        void DealDamage(double damage);
        void ChangeStat(double stat);
        void AddModifier(Modifier modifier);
    }
}