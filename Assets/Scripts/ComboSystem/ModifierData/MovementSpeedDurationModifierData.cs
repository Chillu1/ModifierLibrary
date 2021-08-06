namespace ComboSystem
{
    public class MovementSpeedDurationModifierData : DurationModifierData
    {
        public float MovementSpeed { get; protected set; }

        public MovementSpeedDurationModifierData(float movementSpeed, float duration) : base(duration)
        {
            MovementSpeed = movementSpeed;
        }
    }
}