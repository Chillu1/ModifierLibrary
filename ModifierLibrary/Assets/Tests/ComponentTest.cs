using System.Diagnostics.CodeAnalysis;
using UnitLibrary;
using NUnit.Framework;

namespace ModifierLibrary.Tests
{
	//TargetComponent
	[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
	public class ComponentTest : ModifierBaseTest
	{
		// [Test]
		// public void TargetNotValidAlly()
		// {
		//     var damageModifierApplier = modifierPrototypes.GetItem("IceBoltTestApplier");
		//     character.AddModifier(damageModifierApplier);
		//     //We can attack our own allies, but we shouldn't apply modifiers that aren't for our allies
		//     character.Attack(ally);
		//     Assert.AreEqual(initialHealthAlly - initialDamageAlly, ally.Stats.Health.CurrentHealth, Delta);
		// }

		[Test]
		public void RemoveComponentContains()
		{
			var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
			character.AddModifier(doTModifierApplier);
			character.Attack(enemy);
			enemy.Update(9.9f);

			var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
			Assert.True(enemy.ContainsModifier(doTModifier));
		}

		[Test]
		public void RemoveComponentEffect()
		{
			var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
			character.AddModifier(doTModifierApplier);
			character.Attack(enemy);
			enemy.Update(10.1f);

			var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
			Assert.False(enemy.ContainsModifier(doTModifier));
		}

		[Test]
		public void RemoveComponentLingerEffect()
		{
			var doTModifierApplier = modifierPrototypes.Get("IceBoltTestApplier");
			character.AddModifier(doTModifierApplier);
			character.Attack(enemy);
			enemy.Update(0.4f);
			var doTModifier = modifierPrototypes.Get("IceBoltTest");
			Assert.True(enemy.ContainsModifier(doTModifier));
			enemy.Update(0.2f);
			Assert.False(enemy.ContainsModifier(doTModifier));
		}

		[Test]
		public void HealStatBasedEffect()
		{
			Assert.True(ally.Stats.Health.IsFull);
			var healModifier = modifierPrototypes.Get("HealStatBasedTest");
			character.ChangeStat(StatType.Heal, 5);
			character.AddModifier(healModifier);
			enemy.ChangeDamageStat(new DamageData(4, DamageType.Physical));
			enemy.Attack(ally);
			character.Heal(ally);

			Assert.True(ally.Stats.Health.IsFull);
		}

		[Test]
		public void HealthCost()
		{
			//On attack, we should try to apply the modifier, check for cost, then apply, then take the cost.
			var healthCostModifier = modifierPrototypes.Get("IceBoltHealthCostTestApplier");
			character.AddModifier(healthCostModifier);

			Assert.True(character.Stats.Health.IsFull);
			character.Attack(enemy);

			Assert.AreEqual(initialHealthCharacter - 10, character.Stats.Health.CurrentHealth, Delta); //cost component took away 10 health
			Assert.AreNotEqual(initialHealthEnemy, enemy.Stats.Health.CurrentHealth); //enemy took damage
		}

		[Test]
		public void HealthCostNotLethal()
		{
			var healthCostModifier = modifierPrototypes.Get("IceBoltHealthCostTestApplier");
			character.AddModifier(healthCostModifier);

			Assert.True(character.Stats.Health.IsFull);
			for (int i = 0; i < 7; i++)
			{
				character.Attack(enemy);
			}

			Assert.False(character.Stats.Health.IsDead);
		}

		[Test]
		public void ManaCost()
		{
			var healthCostModifier = modifierPrototypes.GetApplier("IceBoltCastManaCostTest");
			character.AddModifier(healthCostModifier);

			Assert.True(character.Stats.Mana.IsFull);
			character.CastModifier(enemy, "IceBoltCastManaCostTestApplier");

			Assert.AreEqual(initialManaCharacter - 10, character.Stats.Mana.Value, Delta); //cost component used up 10 mana
			Assert.False(character.Stats.Mana.IsFull);
		}

		[Test]
		public void ManaCostAttack()
		{
			var healthCostModifier = modifierPrototypes.GetApplier("IceBoltAttackManaCostTest");
			character.AddModifier(healthCostModifier);

			Assert.True(character.Stats.Mana.IsFull);
			character.Attack(enemy);

			Assert.AreEqual(initialManaCharacter - 10, character.Stats.Mana.Value, Delta); //cost component used up 10 mana
			Assert.False(character.Stats.Mana.IsFull);
		}

		[Test]
		public void Cooldown()
		{
			double dmg = initialDamageCharacter;

			var healthCostModifier = modifierPrototypes.GetApplier("IceBoltCooldownTest");
			character.AddModifier(healthCostModifier);

			Assert.True(enemy.Stats.Health.IsFull);

			character.Attack(enemy); //CD is on
			Assert.AreEqual(initialHealthEnemy - dmg - 2, enemy.Stats.Health.CurrentHealth, Delta);

			character.Attack(enemy); //CD is still active
			Assert.AreEqual(initialHealthEnemy - dmg - 2 - dmg, enemy.Stats.Health.CurrentHealth, Delta);

			character.Update(4); //CD is still active
			character.Attack(enemy);
			Assert.AreEqual(initialHealthEnemy - dmg - 2 - dmg - dmg, enemy.Stats.Health.CurrentHealth, Delta);

			character.Update(2); //CD is over
			character.Attack(enemy);
			Assert.AreEqual(initialHealthEnemy - dmg - 2 - dmg - dmg - dmg - 2, enemy.Stats.Health.CurrentHealth, Delta);

			character.Attack(enemy); //CD active again
			Assert.AreEqual(initialHealthEnemy - dmg - 2 - dmg - dmg - dmg - 2 - dmg, enemy.Stats.Health.CurrentHealth, Delta);
		}

		[Test]
		public void TwoEffects()
		{
			var fireBallModifier = modifierPrototypes.GetApplier("FireBallTwoEffectTest");
			character.AddModifier(fireBallModifier);

			character.Attack(enemy); //1 auto attack, 10 init dmg

			Assert.AreEqual(initialHealthEnemy - 11, enemy.Stats.Health.CurrentHealth, Delta);

			enemy.Update(1.1f);

			Assert.AreEqual(initialHealthEnemy - 11 - 3, enemy.Stats.Health.CurrentHealth, Delta);
		}

		[Test]
		public void TwoEffectsInitDot()
		{
			var fireBallModifier = modifierPrototypes.GetApplier("FireBallTwoEffectInitTest");
			character.AddModifier(fireBallModifier);

			character.Attack(enemy); //1 auto attack, 10 init dmg, 3 dot

			Assert.AreEqual(initialHealthEnemy - 14, enemy.Stats.Health.CurrentHealth, Delta);

			enemy.Update(1.1f);

			Assert.AreEqual(initialHealthEnemy - 14 - 3, enemy.Stats.Health.CurrentHealth, Delta);
		}

		[Test]
		public void TemporaryDamageResistance()
		{
			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));

			var modifier = modifierPrototypes.Get("TemporaryPhysicalResistanceTest");
			character.AddModifier(modifier);

			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));

			character.Update(1.1f);

			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));
		}

		[Test]
		public void TemporaryDamageBuff()
		{
			Assert.AreEqual(initialDamageCharacter, character.Stats.Damage.DamageSum(), Delta);

			var modifier = modifierPrototypes.Get("TemporaryDamageBuffTest");
			character.AddModifier(modifier);

			Assert.AreEqual(initialDamageCharacter + 10, character.Stats.Damage.DamageSum(), Delta);

			character.Update(11f);

			Assert.AreEqual(initialDamageCharacter, character.Stats.Damage.DamageSum(), Delta);
		}

		[Test]
		public void RevertibleDamageResistanceBuff()
		{
			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));

			var modifierApplier = modifierPrototypes.GetApplier("TemporaryAlliedDamageResistanceBuffTest");
			ally.AddModifier(modifierApplier);

			ally.CastModifier(character, "TemporaryAlliedDamageResistanceBuffTestApplier");
			ally.CastModifier(character, "TemporaryAlliedDamageResistanceBuffTestApplier");

			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));

			character.Update(11f);

			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));

			ally.CastModifier(character, "TemporaryAlliedDamageResistanceBuffTestApplier");
			character.Update(8f);

			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));
		}
	}
}