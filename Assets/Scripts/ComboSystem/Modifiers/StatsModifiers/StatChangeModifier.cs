namespace ComboSystem
{
    public class StatChangeModifier : InitUseModifier<StatChangeModifierData>
    {
        public StatChangeModifier(string id, StatChangeModifierData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override void Effect()
        {
            Target!.ChangeStat(Data.StatType, Data.Value);
        }

        protected override void Remove()
        {
            Target.ChangeStat(Data.StatType, -Data.Value);
            base.Remove();
        }
    }
}