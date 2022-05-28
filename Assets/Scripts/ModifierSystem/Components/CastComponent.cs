namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for casting, cooldown, mana, etc
    /// </summary>
    public class CastComponent : Component, ICastComponent
    {
        private readonly float _cooldown; //Mby not readonly later on? Still prob better to hold a separate var to control cd reduction
        private float _timer;

        private bool _isCasting;

        public CastComponent(float cooldown)
        {
            _cooldown = cooldown;
        }

        public void Update(float deltaTime)
        {
            if (!_isCasting)
                return;

            _timer += deltaTime;
            if (_timer >= _cooldown)
            {
                _timer -= _cooldown;
                //Automatic cast
                TryCast();
            }
        }

        public bool TryCast()
        {
            return true;
        }

        public void ManualCast()
        {
        }

        public void AutomaticCast()
        {
        }

        public void Cast()
        {
        }
    }
}