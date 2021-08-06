namespace ComboSystem
{
    public class MovementSpeedModifier : SingleUseModifier<MovementSpeedModifierData>
    {
        public MovementSpeedModifier(MovementSpeedModifierData speedBuffPlayerData)
        {
            Data = speedBuffPlayerData;
        }

        protected override void Apply()
        {
            Target.ChangeStat(StatType.MovementSpeed, Data.MovementSpeed);
        }

        protected override void Remove()
        {
            Target.ChangeStat(StatType.MovementSpeed, -Data.MovementSpeed);
        }
    }
}