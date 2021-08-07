namespace ComboSystem
{
    public class MovementSpeedDurationModifier : SingleUseDurationModifier<MovementSpeedDurationModifierData>
    {
        public MovementSpeedDurationModifier(string id, MovementSpeedDurationModifierData speedDurationBuffPlayerData, ModifierProperties properties = default) : base(id, properties)
        {
            Data = speedDurationBuffPlayerData;
        }

        protected override bool Apply()
        {
            if (!base.Apply())
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