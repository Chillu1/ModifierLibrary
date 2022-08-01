using System;
using UnitLibrary;
using NUnit.Framework;

namespace ModifierLibrary.Tests
{
    public class ConditionEventTest : ModifierBaseTest
    {
        [Test]
        public void ConditionApplyOnKill()
        {
            var damageOnKillModifier = modifierPrototypes.Get("DamageOnKillTest");
            character.ChangeDamageStat(new DamageData(29, DamageType.Physical)); //30 dmg per hit
            character.AddModifier(damageOnKillModifier);
            character.Attack(enemy);

            Assert.True(enemy.Stats.Health.IsDead);
            Assert.AreEqual(initialDamageCharacter + 29 + 2, character.Stats.Damage.DamageSum(), Delta);
        }

        [Test]
        public void AddStatDamage()
        {
            var damageOnKillModifier = modifierPrototypes.Get("AddStatDamageTest");
            character.AddModifier(damageOnKillModifier);
            Assert.AreEqual(initialDamageCharacter + 2, character.Stats.Damage.DamageSum(), Delta);
        }

        [Test]
        public void ConditionApplyOnDeathRevenge()
        {
            var damageOnDeathModifier = modifierPrototypes.Get("DamageOnDeathTest");
            enemy.ChangeDamageStat(new DamageData(1000d, DamageType.Physical));
            character.AddModifier(damageOnDeathModifier);
            enemy.Attack(character);

            Assert.True(character.Stats.Health.IsDead);
            Assert.True(enemy.Stats.Health.IsDead);
        }

        [Test]
        public void ConditionTimedDamageOnKill()
        {
            var damageOnKillModifier = modifierPrototypes.Get("TimedDamageOnKillTest");
            character.ChangeDamageStat(new DamageData(29, DamageType.Physical)); //30 dmg per hit
            character.AddModifier(damageOnKillModifier);
            character.Attack(enemy);

            Assert.True(enemy.Stats.Health.IsDead); //Kill
            Assert.AreEqual(initialDamageCharacter + 29 + 2, character.Stats.Damage.DamageSum(), Delta); //Increase in damage

            character.Update(5.1f); //Buff expired
            Assert.AreEqual(initialDamageCharacter + 29, character.Stats.Damage.DamageSum(), Delta); //Expired damage

            character.Attack(enemyDummies[0]); //Kill
            Assert.True(enemyDummies[0].Stats.Health.IsDead);
            Assert.AreEqual(initialDamageCharacter + 29, character.Stats.Damage.DamageSum(), Delta); //No increase in damage, condition expired
        }

        [Test]
        public void ConditionThornsOnHit()
        {
            var thornsOnHitModifier = modifierPrototypes.Get("ThornsOnHitTest");
            character.AddModifier(thornsOnHitModifier);
            enemy.Attack(character);

            Assert.AreEqual(initialHealthEnemy - 5, enemy.Stats.Health.CurrentHealth);
        }

        //[Test] //TODO make into a generated modifier
        public void ConditionApplyHealOnDeath()
        {
            var damageOnDeathModifier = modifierPrototypes.Get("HealOnDeathTest");
            character.ChangeStat(StatType.Heal, 10);
            Assert.AreEqual(initialHealthAlly, ally.Stats.Health.CurrentHealth, Delta);

            enemy.ChangeDamageStat(new DamageData(1000, DamageType.Physical));
            character.AddModifier(damageOnDeathModifier);
            enemy.Attack(character);
            Assert.False(character.Stats.Health.IsDead); //Charge used up
            enemy.Attack(character);
            Assert.True(character.Stats.Health.IsDead);
        }

        [Test]
        public void HealStatBased()
        {
            var healModifier = modifierPrototypes.Get("HealOnHealTest");
            character.ChangeStat(StatType.Heal, 5);
            character.AddModifier(healModifier);
            enemy.ChangeDamageStat(new DamageData(4, DamageType.Physical));
            enemy.Attack(ally);
            enemy.Attack(character);
            Assert.False(ally.Stats.Health.IsFull);
            Assert.False(character.Stats.Health.IsFull);
            character.Heal(ally);
            Assert.True(ally.Stats.Health.IsFull);
            //Log.Info(character.Stats.Health.CurrentHealth);
            Assert.True(character.Stats.Health.IsFull);
        }

        [Test]
        public void ConditionAttackYourselfOnHit()
        {
            var modifier = modifierPrototypes.Get("AttackYourselfOnHitTest");
            character.AddModifier(modifier);
            enemy.Attack(character);
            Assert.Less(character.Stats.Health.CurrentHealth,
                initialHealthCharacter - initialDamageEnemy); //Player should have hit himself at least once
        }

        [Test]
        public void ConditionAttackYourselfOnHitAndDamaged()
        {
            var modifier = modifierPrototypes.Get("AttackYourselfOnHitTest");
            var secondModifier = modifierPrototypes.Get("AttackYourselfOnDamagedTest");
            character.AddModifier(modifier);
            character.AddModifier(secondModifier);
            enemy.Attack(character);
            Assert.Less(character.Stats.Health.CurrentHealth,
                initialHealthCharacter - initialDamageEnemy); //Player should have hit himself at least once
            Assert.AreEqual(
                initialHealthCharacter - initialDamageEnemy - initialDamageCharacter * 2 * UnitEventController.MaxEventRecursions,
                character.Stats.Health.CurrentHealth, Delta);
            character.Update(0.21f); //Interval
            enemy.Attack(character);
            Assert.AreEqual(
                initialHealthCharacter - initialDamageEnemy * 2 - initialDamageCharacter * 4 * UnitEventController.MaxEventRecursions,
                character.Stats.Health.CurrentHealth, Delta);
        }

        [Test]
        public void ConditionAttackAndHealYourselfOnHit()
        {
            var modifier = modifierPrototypes.Get("AttackYourselfOnHitTest");
            var secondModifier = modifierPrototypes.Get("HealYourselfHitTest");
            character.ChangeDamageStat(new DamageData(3, DamageType.Physical));
            character.ChangeStat(StatType.Heal, 5);
            character.AddModifier(modifier);
            character.AddModifier(secondModifier);
            enemy.Attack(character);
            Assert.True(character.Stats.Health.IsFull);
        }

        [Test]
        public void ConditionReflectDamageOnHit()
        {
            var modifier = modifierPrototypes.Get("ReflectOnDamagedTest");

            enemy.ChangeDamageStat(new DamageData(4, DamageType.Physical));
            character.AddModifier(modifier);

            enemy.Attack(character); //5 damage attack, 20% reflect = 1 damage reflect

            Assert.AreEqual(initialHealthEnemy - 1, enemy.Stats.Health.CurrentHealth, Delta);
        }

        /*[Test]
        public void ConditionApplyHealthOnDeath()
        {
            var modifierApplierApplier = modifierPrototypes.GetItem("DeathHealthTestApplierApplier");
            var modifierApplier = modifierPrototypes.GetItem("DeathHealthTestApplier");
            character.AddModifier(modifierApplierApplier, AddModifierParameters.DefaultOffensive);
            enemy.ChangeDamageStat(new DamageData(1000, DamageType.Physical));
            enemy.Attack(character);
            Assert.False(enemy.ContainsModifier(modifierApplierApplier));
            Assert.True(character.Stats.Health.IsDead);
            Assert.AreEqual(initialHealthEnemy-15, enemy.Stats.Health.CurrentHealth);
            Assert.True(enemy.ContainsModifier(modifierApplier));
        }*/

        [Test]
        public void ConditionRemove()
        {
            var modifier = modifierPrototypes.Get("ConditionRemoveTest");
            character.AddModifier(modifier);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            enemy.Attack(character);
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));

            character.Update(5.1f);
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            enemy.Attack(character);
            //We should have cleaned up the condition after the 5.1s duration
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
        }

        [Test]
        public void ConditionRemoveApplier()
        {
            var modifierApplier = modifierPrototypes.GetApplier("ConditionRemoveApplierTest");
            character.AddModifier(modifierApplier);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            enemy.Attack(character);
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));

            character.Update(5.1f);
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            enemy.Attack(character);
            //We should not have cleaned up the condition after the 5.1s duration
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
        }
    }
}