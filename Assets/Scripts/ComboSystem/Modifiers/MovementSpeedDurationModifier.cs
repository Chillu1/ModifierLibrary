namespace ComboSystem
{
    public class MovementSpeedDurationModifier : SingleUseDurationModifier<MovementSpeedDurationModifierData>
    {
        public MovementSpeedDurationModifier(string id, MovementSpeedDurationModifierData data, ModifierProperties properties = default) :
            base(id, data, properties)
        {
        }

        protected override bool Apply()
        {
            if (!ApplyIsValid())
                return false;
            Target!.ChangeStat(StatType.MovementSpeed, Data.MovementSpeed);
            return true;
        }

        protected override void Remove()
        {
            Target.ChangeStat(StatType.MovementSpeed, -Data.MovementSpeed);
            base.Remove();
        }
    }
}