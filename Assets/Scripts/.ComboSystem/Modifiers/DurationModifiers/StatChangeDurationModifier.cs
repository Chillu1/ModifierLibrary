using BaseProject;

namespace ComboSystem
{
    public class StatChangeDurationModifier : SingleUseDurationModifier<StatChangeDurationModifierData>
    {
        public StatChangeDurationModifier(string id, StatChangeDurationModifierData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override void Effect()
        {
            Target!.ChangeStat(Data.StatType, Data.Value);
            Log.Info("Stat change condition");
        }

        protected override void Remove()
        {
            Target.ChangeStat(Data.StatType, -Data.Value);
            base.Remove();
        }
    }
}