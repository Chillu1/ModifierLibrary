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

    //Maybe not the best design?
    public class BaseModifierData
    {
        public ModifierProperties ModifierProperties { get; protected set; }

        public BaseModifierData(ModifierProperties modifierProperties = default)
        {
            ModifierProperties = modifierProperties;
        }
    }
}