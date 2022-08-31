using System;
using UnitLibrary;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ModifierLibrary.Tests
{
	public abstract class ModifierBaseTest
	{
		protected Unit character;
		protected Unit ally;
		protected Unit enemy;
		protected Unit enemyAlly;
		protected Unit[] enemyDummies;

		protected double initialHealthCharacter, initialHealthAlly, initialHealthEnemy, initialHealthEnemyAlly;
		protected double initialDamageCharacter, initialDamageAlly, initialDamageEnemy;
		protected double initialManaCharacter;

		protected const double Delta = 0.01d;

		protected const float
			PermanentComboModifierCooldown = 60; //PermanentMods might be able to be stripped/removed later, does it matter?

		protected ModifierPrototypesTest modifierPrototypes;

		protected ComboModifierPrototypesTest comboModifierPrototypes;
		//protected ComboModifierPrototypesTest comboModifierPrototypes;

		[OneTimeSetUp]
		public void OneTimeInit()
		{
			modifierPrototypes = new ModifierPrototypesTest();
			comboModifierPrototypes = new ComboModifierPrototypesTest();
			ComboModifierPrototypes.SetUnitTestInstance(comboModifierPrototypes);
			//comboModifierPrototypes = new ComboModifierPrototypesTest();
			//comboModifierPrototypes.AddTestModifiers();
		}

		[SetUp]
		public void Init()
		{
			character = new Unit(new UnitProperties
			{
				Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical), AttackSpeed = 1, MovementSpeed = 3,
				Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
			});
			ally = new Unit(new UnitProperties
			{
				Id = "ally", Health = 25, Damage = new DamageData(1, DamageType.Physical), AttackSpeed = 1, MovementSpeed = 3,
				Mana = 50, ManaRegen = 1, UnitType = UnitType.Ally
			});
			enemy = new Unit(new UnitProperties
			{
				Id = "enemy", Health = 30, Damage = new DamageData(1, DamageType.Physical), AttackSpeed = 1, MovementSpeed = 2,
				Mana = 20, ManaRegen = 1, UnitType = UnitType.Enemy
			});
			enemyAlly = new Unit(new UnitProperties
			{
				Id = "enemyAlly", Health = 30, Damage = new DamageData(1, DamageType.Physical), AttackSpeed = 1, MovementSpeed = 2,
				Mana = 20, ManaRegen = 1, UnitType = UnitType.Enemy
			});
			initialHealthCharacter = character.Stats.Health.CurrentHealth;
			initialHealthAlly = ally.Stats.Health.CurrentHealth;
			initialHealthEnemy = enemy.Stats.Health.CurrentHealth;
			initialHealthEnemyAlly = enemyAlly.Stats.Health.CurrentHealth;
			initialDamageCharacter = character.Stats.Damage.DamageSum();
			initialDamageAlly = ally.Stats.Damage.DamageSum();
			initialDamageEnemy = enemy.Stats.Damage.DamageSum();
			initialManaCharacter = character.Stats.Mana.Value;

			enemyDummies = new Unit[5];
			for (int i = 0; i < 5; i++)
			{
				enemyDummies[0] = new Unit(new UnitProperties()
				{
					Id = "enemy", Health = 1, Damage = new DamageData(1, DamageType.Physical), MovementSpeed = 1,
					Mana = 50, ManaRegen = 100, UnitType = UnitType.Enemy
				});
			}
		}

		[TearDown]
		public void CleanUp()
		{
			character = null;
			ally = null;
			enemy = null;
		}

		public class ModifierPrototypesTest : ModifierPrototypes<Modifier>
		{
			public ModifierPrototypesTest() : base(true)
			{
			}

			[CanBeNull]
			public new Modifier Get(string key)
			{
				if (key.Contains("Applier"))
				{
					string subKey = key.Substring(0, key.IndexOf("Applier", StringComparison.Ordinal));
					if (!subKey.EndsWith("Test"))
					{
						Log.Error("Invalid modifier Id, it has to end with 'Test' for unit tests");
						return null;
					}
				}
				else if (!key.EndsWith("Test"))
				{
					Log.Error("Invalid modifier Id, it has to include 'Test' for unit tests");
					return null;
				}

				return base.Get(key);
			}
		}

		public class ComboModifierPrototypesTest : ComboModifierPrototypes
		{
			public ComboModifierPrototypesTest() : base(true)
			{
			}

			public override ComboModifier Get(string key)
			{
				if (!key.EndsWith("Test"))
				{
					Log.Error("Invalid modifier Id, it has to include 'Test' for unit tests");
					return null;
				}

				return base.Get(key);
			}
		}
	}
}