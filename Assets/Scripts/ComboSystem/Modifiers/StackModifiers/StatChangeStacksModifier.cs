namespace ComboSystem
{
    public class StatChangeStacksModifier : EffectOnStacksModifier<StatChangeStacksModifierData>
    {
        public StatChangeStacksModifier(string id, StatChangeStacksModifierData data) : base(id, data)
        {
        }

        protected override bool Apply()
        {
            if (!ApplyIsValid())
                return false;
            Target!.ChangeStat(Data.StatType, Data.Value);
            return true;
        }
    }
}