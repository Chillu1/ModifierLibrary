namespace ComboSystem
{
    public class MovementSpeedModifierData
    {
        public float MovementSpeed { get; protected set; }
        public float Duration { get; protected set; }

        public MovementSpeedModifierData(float movementSpeed, float duration)
        {
            MovementSpeed = movementSpeed;
            Duration = duration;
        }
    }
}