using System;
using UnitLibrary;

namespace ModifierLibrary
{
	public sealed class DamageComponent : EffectComponent, IStackEffectComponent
	{
		private DamageData[] Damage { get; }
		private StackEffectType StackType { get; }

		public DamageComponent(DamageData[] damage, StackEffectType stackType = StackEffectType.None,
			ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
		{
			Damage = damage;
			StackType = stackType;

			string textDamage = "";
			foreach (var data in Damage)
			{
				textDamage += $"{data.DamageType}: {data.Damage}";
				if (data.ElementData != null)
					textDamage += $"\n{data.ElementData.ToolTipInfo()}";
			}

			textDamage += "\n";
			Info = "Damage: " + textDamage;
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			//TODO Custom AttackType support
			receiver.DealDamage(Damage, acter, AttackType.Internal);
		}

		public void StackEffect(int stacks, double value)
		{
			foreach (var damageData in Damage)
			{
				if (StackType.HasFlag(StackEffectType.Add))
					damageData.BaseDamage += value;
				if (StackType.HasFlag(StackEffectType.AddStacksBased))
					damageData.BaseDamage += value * stacks;
				if (StackType.HasFlag(StackEffectType.Multiply))
					damageData.Multiplier += value;
				if (StackType.HasFlag(StackEffectType.SetMultiplierStacksBased))
					damageData.Multiplier = stacks;
				if (StackType.HasFlag(StackEffectType.MultiplyStacksBased))
					damageData.Multiplier += value * stacks;
				if (StackType.HasFlag(StackEffectType.OnXStacksAddElemental))
				{
					//TODO
					//damageData.ElementData.?
				}
			}

			//Effect at the end, after all the other possible calcs
			if (StackType.HasFlag(StackEffectType.Effect))
				SimpleEffect();
		}

		[Flags]
		public enum StackEffectType
		{
			None = 0,
			Effect = 1,
			Add = 2, //TODO Add to all damages?
			AddStacksBased = 4,
			Multiply = 8, //TODO Multiply all damages?
			MultiplyStacksBased = 16,
			SetMultiplierStacksBased = 32, //TODO Multiply all damages?

			//DamageComponent specific
			OnXStacksAddElemental = 64,
		}
	}
}