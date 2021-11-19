namespace ComboSystem
{
    public class StatChangeDurationComboModifier : SingleUseDurationComboModifier<StatChangeDurationModifierData>
    {
        public StatChangeDurationComboModifier(string id, StatChangeDurationModifierData data, ComboRecipe recipe, ModifierProperties properties = default) :
            base(id, data, recipe, properties)
        {
        }

        protected override void Effect()
        {
            Target!.ChangeStat(Data.StatType, Data.Value);
        }

        protected override void Remove()
        {
            Target.ChangeStat(Data.StatType, Data.Value);
            base.Remove();
        }
    }
}