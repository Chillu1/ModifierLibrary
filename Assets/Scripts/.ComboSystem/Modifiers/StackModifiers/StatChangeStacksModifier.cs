namespace ComboSystem
{
    public class StatChangeStacksModifier : EffectOnStacksModifier<StatChangeStacksModifierData>
    {
        public StatChangeStacksModifier(string id, StatChangeStacksModifierData data) : base(id, data)
        {
        }

        protected override void Effect()
        {
            Target!.ChangeStat(Data.StatType, Data.Value);
        }
    }
}