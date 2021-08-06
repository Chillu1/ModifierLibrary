namespace ComboSystem
{
    public class Player : Character
    {
        public Player()
        {
            Name = nameof(Player);
            MaxHealth = 10;
            Health = 10;
            MovementSpeed = 5;
        }
    }
}