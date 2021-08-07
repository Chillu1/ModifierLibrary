namespace ComboSystem
{
    public class MovementSpeedDurationModifier : SingleUseDurationModifier<MovementSpeedDurationModifierData>
    {
        public MovementSpeedDurationModifier(MovementSpeedDurationModifierData speedDurationBuffPlayerData)
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