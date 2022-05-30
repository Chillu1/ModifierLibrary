using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for casting, cooldown, mana, etc
    /// </summary>
    public class CastComponent : Component, ICastComponent
    {
        public bool IsReadyToCast => _timer <= 0;
        public bool IsAutomaticCasting { get; private set; }

        private readonly float _cooldown; //Mby not readonly later on? Still prob better to hold a separate var to control cd reduction
        private float _timer;

        public CastComponent(float cooldown)
        {
            _cooldown = cooldown;
        }

        public void Update(float deltaTime)
        {
            if (_timer <= 0)
                return;

            _timer -= deltaTime;
            if (_timer > 0)
                return;

            if (IsAutomaticCasting)
            {
                CanCast();
            }
        }

        public bool CanCast()
        {
            if (!IsReadyToCast)
            {
                Log.Warning("Tried to cast but not ready", "modifiers");
                return false;
            }

            ResetTimer();
            Cast();

            return true;
        }

        public void Cast()
        {
            //TODO We need to apply the modifier here somehow
        }

        private void ResetTimer()
        {
            _timer = _cooldown;
        }
    }
}