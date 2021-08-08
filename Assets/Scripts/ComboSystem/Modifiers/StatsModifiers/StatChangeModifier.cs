namespace ComboSystem
{
    public class StatChangeModifier : SingleUseModifier<StatChangeModifierData>
    {
        public StatChangeModifier(string id, StatChangeModifierData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override bool Apply()
        {
            if (!ApplyIsValid())
                return false;
            Target!.ChangeStat(Data.StatType, Data.Value);
            return true;
        }

        protected override void Remove()
        {
            Target.ChangeStat(Data.StatType, -Data.Value);
            base.Remove();
        }
    }
}