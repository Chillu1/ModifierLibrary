namespace ComboSystem
{
    public class StatChangeComboModifier : InitUseComboModifier<StatChangeModifierData>
    {
        public StatChangeComboModifier(string id, StatChangeModifierData data, ComboRecipe recipe, ModifierProperties properties = default) :
            base(id, data, recipe, properties)
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