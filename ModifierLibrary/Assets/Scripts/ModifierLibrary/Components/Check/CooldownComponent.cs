using UnitLibrary;

namespace ModifierLibrary
{
	public class CooldownComponent : ICooldownComponent
	{
		//public bool IsReady => _timer <= 0;

		private readonly float _cooldown; //Mby not readonly later on? Still prob better to hold a separate var to control cd reduction
		private float _timer;

		private const string Info = "Cooldown: ";

		public CooldownComponent(float cooldown)
		{
			_cooldown = cooldown;
		}

		public void Update(float deltaTime) //TODO Cooldown reductions based on tag
		{
			//Log.Info(_timer+"/"+_cooldown);
			if (_timer <= 0)
				return;

			_timer -= deltaTime;
			if (_timer < 0)
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

		public string GetBasicInfo()
		{
			return $"{Info}{_cooldown:F2}\n";
		}

		public string GetBattleInfo()
		{
			return $"{Info}{_timer:F2}/{_cooldown:F2}\n";
		}

		public override string ToString()
		{
			return GetBattleInfo();
		}
	}
}