using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using UnitLibrary;

namespace ModifierLibrary.Tests
{
	//DamageDealt (modifier)
	//TimeComponent (normal, interval, etc)

	//ModifierApplier
	//Stack
	//Refresh
	[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
	public class ModifierTest : ModifierBaseTest
	{
		[Test]
		public void DamageDealt()
		{
			var damageModifierApplier = modifierPrototypes.Get("IceBoltTestApplier");
			character.AddModifier(damageModifierApplier);
			character.Attack(enemy);
			character.Attack(enemy);
			character.Attack(enemy);

			var damageModifier = modifierPrototypes.Get("IceBoltTest");
			Assert.True(enemy.ContainsModifier(damageModifier));
			Assert.True(enemy.Stats.Health.IsDead);
			//We could test for: amount of stacks, simulating the time (then damage taken)
		}

		[Test]
		public void AllyHeal()
		{
			var healModifierApplier = modifierPrototypes.Get("AllyHealTestApplier");
			character.AddModifier(healModifierApplier);
			enemy.ChangeDamageStat(new DamageData(9, DamageType.Physical)); //10 dmg
			enemy.Attack(ally);
			character.CastModifier(ally, "AllyHealTestApplier");
			var healModifier = modifierPrototypes.Get("AllyHealTest");
			Assert.True(ally.ContainsModifier(healModifier));
			Assert.True(ally.Stats.Health.IsFull);

			enemy.Attack(ally);
			enemy.Attack(ally);
			character.CastModifier(ally, "AllyHealTestApplier");
			Assert.True(ally.Stats.Health.CurrentHealth == initialHealthAlly - 10d);
		}

		[Test]
		public void DoTDamage()
		{
			var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
			character.AddModifier(doTModifierApplier);
			character.Attack(enemy); //1 phys dmg, 5 poison dmg
			enemy.Update(2); //5 dmg
			enemy.Update(2); //5 dmg

			var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
			Assert.True(enemy.ContainsModifier(doTModifier));
			Assert.AreEqual(initialHealthEnemy - 1 - 5 - 5 - 5, enemy.Stats.Health.CurrentHealth, Delta);
		}

		[Test]
		public void DoTRefresh()
		{
			var doTModifierApplier = modifierPrototypes.Get("DoTRefreshTestApplier");
			character.AddModifier(doTModifierApplier);
			var doTModifier = modifierPrototypes.Get("DoTRefreshTest");
			character.Attack(enemy);
			Assert.True(enemy.ContainsModifier(doTModifier));
			enemy.Update(8); //2 seconds left
			character.Attack(enemy); //Refresh
			enemy.Update(6); //4 seconds left
			Assert.True(enemy.ContainsModifier(doTModifier));
		}

		[Test]
		public void AutomaticCasting()
		{
			var modifier = modifierPrototypes.GetApplier("IceBoltAutomaticCastTest");
			character.AddModifier(modifier);

			character.TargetingSystem.SetTarget(TargetType.Cast, enemy);

			character.Update(0.1f);
			Assert.AreEqual(initialHealthEnemy - 2, enemy.Stats.Health.CurrentHealth, Delta);

			character.Update(0.5f); //CD
			Assert.AreEqual(initialHealthEnemy - 2, enemy.Stats.Health.CurrentHealth, Delta);

			character.Update(1f); //CD is over
			Assert.AreEqual(initialHealthEnemy - 2 - 2, enemy.Stats.Health.CurrentHealth, Delta);

			character.Update(0.5f); //CD Again
			Assert.AreEqual(initialHealthEnemy - 2 - 2, enemy.Stats.Health.CurrentHealth, Delta);
		}

		[Test]
		public void AutomaticCastingNoTarget()
		{
			var modifier = modifierPrototypes.GetApplier("IceBoltAutomaticCastTest");
			character.AddModifier(modifier);

			character.Update(0.5f);

			character.Update(1f);
		}

		[TestCase("IceBoltTest")]
		[TestCase("DisarmModifierTest")]
		[TestCase("DamagePerStackTest")]
		[TestCase("ApplyStunModifierXStacksTest")]
		[TestCase("ApplyStunModifierXStacksTestApplier")]
		public void IsModifierAChildOfApplier(string id)
		{
			Assert.True(modifierPrototypes.IsModifierAChild(id));
		}

		[TestCase("IceBoltTestApplier")]
		[TestCase("ApplyStunModifierXStacksTestApplierApplier")]
		[TestCase("DamageOnLowHealthTest")]
		[TestCase("ReflectOnDamagedTest")]
		public void IsModifierNotAChildOfApplier(string id)
		{
			Assert.False(modifierPrototypes.IsModifierAChild(id));
		}

		[Test]
		public void MultiTargetCastResistanceRevert()
		{
			character.AddModifier(modifierPrototypes.GetApplier("TemporaryAllDamageResistanceBuffTest"));

			Assert.True(enemy.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));
			Assert.True(enemyAlly.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));

			character.CastModifier(enemy, "TemporaryAllDamageResistanceBuffTestApplier");
			character.CastModifier(enemyAlly, "TemporaryAllDamageResistanceBuffTestApplier");

			Assert.True(enemy.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));
			Assert.True(enemyAlly.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));

			enemyAlly.Update(2.1f);

			Assert.True(enemy.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));
			Assert.True(enemyAlly.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));

			enemy.Update(1.9f);
			character.CastModifier(enemy, "TemporaryAllDamageResistanceBuffTestApplier");
			character.CastModifier(enemyAlly, "TemporaryAllDamageResistanceBuffTestApplier");
			enemy.Update(1f);

			Assert.True(enemy.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));
			Assert.True(enemyAlly.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 100));
		}
	}
}