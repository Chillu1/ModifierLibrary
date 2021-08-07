namespace ComboSystem
{
    public class MovementSpeedModifierData : BaseModifierData
    {
        public float MovementSpeed { get; protected set; }

        public MovementSpeedModifierData(float movementSpeed, ModifierProperties modifierProperties = ModifierProperties.None) : base(modifierProperties)
        {
            MovementSpeed = movementSpeed;
        }
    }

    public class BaseModifierData
    {
        public ModifierProperties ModifierProperties { get; protected set; }

        public BaseModifierData(ModifierProperties modifierProperties = ModifierProperties.None)
        {
            ModifierProperties = modifierProperties;
        }
    }
}