using UnitLibrary;

namespace ModifierLibrary
{
	public class CostComponent : Component, ICostComponent
	{
		private PoolStatType Type { get; }
		private double Amount { get; }

		private Unit _owner;

		private const string Info = "Cost: ";

		public CostComponent(PoolStatType type, double amount)
		{
			Type = type;
			Amount = amount;
		}

		public void SetupOwner(Unit owner)
		{
			_owner = owner;
		}

		public bool ContainsCost()
		{
			switch (Type)
			{
				case PoolStatType.Mana:
					return _owner.Stats.HasStat(StatType.Mana, Amount);
				case PoolStatType.Health:
					bool success = _owner.Stats.HasStat(StatType.Health, Amount, ComparisonCheck.Greater); //Not lethal
					//Log.Verbose("We have enough health stat: " + success, "modifiers");
					return success;
				default:
					Log.Error($"CostComponent: {Type} is not a valid CostType", "modifiers");
					break;
			}

			return false;
		}

		public void ApplyCost()
		{
			switch (Type)
			{
				case PoolStatType.Mana:
					_owner.Stats.Mana.Use(Amount);
					break;
				case PoolStatType.Health:
					//TODO TEMP DamageData
					//Log.Verbose("Applying health cost: " + Amount, "modifiers");
					_owner.DealDamage(new[] { new DamageData(Amount, DamageType.Physical) }, _owner, AttackType.Reflect);
					break;
				default:
					Log.Error($"CostComponent: {Type} is not a valid CostType", "modifiers");
					break;
			}
		}

		public string GetBasicInfo()
		{
			return Info + $"{Amount} {Type}\n";
		}
	}
}