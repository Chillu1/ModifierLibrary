using System;
using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ConditionEventTest : ModifierBaseTest
    {
        [Test]
        public void ConditionApplyOnKillEffect()
        {
            var damageOnKillModifier = modifierPrototypes.GetItem("DamageOnKillTest");
            character.ChangeDamageStat(new DamageData(29, DamageType.Physical));//30 dmg per hit
            character.AddModifier(damageOnKillModifier);
            character.Attack(enemy);

            Assert.True(enemy.Stats.Health.IsDead);
            Assert.AreEqual(initialDamageCharacter+29+2, character.Stats.Damage.DamageSum(), Delta);
        }

        [Test]
        public void AddStatDamageEffect()
        {
            var damageOnKillModifier = modifierPrototypes.GetItem("AddStatDamageTest");
            character.AddModifier(damageOnKillModifier);
            Assert.AreEqual(initialDamageCharacter+2, character.Stats.Damage.DamageSum(), Delta);
        }

        [Test]
        public void ConditionApplyOnDeathRevengeEffect()
        {
            var damageOnDeathModifier = modifierPrototypes.GetItem("DamageOnDeathTest");
            enemy.ChangeDamageStat(new DamageData(1000d, DamageType.Physical));
            character.AddModifier(damageOnDeathModifier);
            enemy.Attack(character);

            Assert.True(character.Stats.Health.IsDead);
            Assert.True(enemy.Stats.Health.IsDead);
        }

        [Test]
        public void ConditionTimedDamageOnKillEffect()
        {
            var damageOnKillModifier = modifierPrototypes.GetItem("TimedDamageOnKillTest");
            character.ChangeDamageStat(new DamageData(29, DamageType.Physical));//30 dmg per hit
            character.AddModifier(damageOnKillModifier);
            character.Attack(enemy);

            Assert.True(enemy.Stats.Health.IsDead);//Kill
            Assert.AreEqual(initialDamageCharacter+29+2, character.Stats.Damage.DamageSum(), Delta);//Increase in damage

            character.Update(5.1f);//Buff expired

            character.Attack(enemyDummies[0]);//Kill
            Assert.True(enemyDummies[0].Stats.Health.IsDead);
            Assert.AreEqual(initialDamageCharacter+29+2, character.Stats.Damage.DamageSum(), Delta);//No increase in damage
        }

        [Test]
        public void ConditionThornsOnHitEffect()
        {
            var thornsOnHitModifier = modifierPrototypes.GetItem("ThornsOnHitTest");
            character.AddModifier(thornsOnHitModifier);
            enemy.Attack(character);

            Assert.AreEqual(initialHealthEnemy-5, enemy.Stats.Health.CurrentHealth);
        }

        [Test]
        public void ConditionApplyHealOnDeathEffect()
        {
            var damageOnDeathModifier = modifierPrototypes.GetItem("HealOnDeathTest");
            character.ChangeStat(new Stat(StatType.Heal, 10));
            Assert.AreEqual(initialHealthAlly, ally.Stats.Health.CurrentHealth, Delta);

            enemy.ChangeDamageStat(new DamageData(1000, DamageType.Physical));
            character.AddModifier(damageOnDeathModifier);
            enemy.Attack(character);
            Assert.False(character.Stats.Health.IsDead);//Charge used up
            enemy.Attack(character);
            Assert.True(character.Stats.Health.IsDead);
        }

        [Test]
        public void HealStatBasedEffect()
        {
            var healModifier = modifierPrototypes.GetItem("HealOnHealTest");
            character.ChangeStat(StatType.Heal, 5);
            character.AddModifier(healModifier);
            enemy.ChangeDamageStat(new DamageData(4, DamageType.Physical));
            enemy.Attack(ally);
            enemy.Attack(character);
            Assert.False(ally.Stats.Health.PoolStat.IsFull);
            Assert.False(character.Stats.Health.PoolStat.IsFull);
            character.Heal(ally);
            Assert.True(ally.Stats.Health.PoolStat.IsFull);
            //Log.Info(character.Stats.Health.CurrentHealth);
            Assert.True(character.Stats.Health.PoolStat.IsFull);
        }
    }
}