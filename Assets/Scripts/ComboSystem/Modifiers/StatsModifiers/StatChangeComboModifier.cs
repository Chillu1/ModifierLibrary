namespace ComboSystem
{
    public class StatChangeComboModifier : SingleUseComboModifier<StatChangeModifierData>
    {
        public StatChangeComboModifier(string id, StatChangeModifierData data, ComboRecipe recipe, ModifierProperties properties = default) :
            base(id, data, recipe, properties)
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