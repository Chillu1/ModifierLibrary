using System.Text;
using BaseProject;

namespace ModifierSystem
{
    public class CooldownComponent : ICooldownComponent
    {
        //public bool IsReady => _timer <= 0;

        private readonly float _cooldown; //Mby not readonly later on? Still prob better to hold a separate var to control cd reduction
        private float _timer;

        public CooldownComponent(float cooldown)
        {
            _cooldown = cooldown;
        }

        public void Update(float deltaTime) //TODO Cooldown reductions based on tag
        {
            if (_timer <= 0)
                return;

            _timer -= deltaTime;
            if(_timer < 0)
                _timer = 0;
        }

        public bool IsReady()
        {
            if (_timer <= 0)
                return true;

            //Log.Info("Cooldown not ready: " + _timer+"/"+_cooldown);
            return false;
        }

        public void ResetTimer()
        {
            _timer = _cooldown;
        }

        public void DisplayText(StringBuilder builder)
        {
            builder.Append("Cooldown: ");
            builder.Append((_timer).ToString("F2"));
            builder.Append("/");
            builder.Append(_cooldown.ToString("F2"));
            builder.AppendLine();
        }
    }
}