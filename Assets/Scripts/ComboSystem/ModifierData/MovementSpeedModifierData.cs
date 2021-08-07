namespace ComboSystem
{
    public class MovementSpeedModifierData
    {
        public float MovementSpeed { get; protected set; }

        public MovementSpeedModifierData(float movementSpeed)
        {
            MovementSpeed = movementSpeed;
        }
    }
}