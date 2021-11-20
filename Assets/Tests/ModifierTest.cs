using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    //DamageDealt (modifier)
    //TimeComponent (normal, interval, etc)

    //ModifierApplier
    //Stack
    //Refresh
    public class ModifierTest : ModifierBaseTest
    {
        [Test]
        public void DamageDealt()
        {
            var damageModifierApplier = modifierPrototypes.GetItem("IceBoltApplier");
            character.AddModifier(damageModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            character.Attack(enemy);
            character.Attack(enemy);

            var damageModifier = modifierPrototypes.GetItem("IceBolt");
            Assert.True(enemy.ContainsModifier(damageModifier));
            Assert.True(enemy.BaseBeing.IsDead);
            //We could test for: amount of stacks, simulating the time (then damage taken)
        }

        [Test]
        public void AllyHeal()
        {
            double initialHealth = ally.BaseBeing.Health.PoolStat.BaseStat.baseValue;
            var healModifierApplier = modifierPrototypes.GetItem("AllyHealApplier");
            character.AddModifier(healModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.BaseBeing.ChangeDamageStat(new DamageData(9, DamageType.Physical));//10 dmg
            enemy.Attack(ally);
            character.CastModifier(ally, "AllyHealApplier");
            var healModifier = modifierPrototypes.GetItem("AllyHeal");
            Assert.True(ally.ContainsModifier(healModifier));
            Assert.True(ally.BaseBeing.Health.PoolStat.IsFull);

            enemy.Attack(ally);
            enemy.Attack(ally);
            character.CastModifier(ally, "AllyHealApplier");
            Assert.True(ally.BaseBeing.Health.PoolStat.BaseStat.baseValue == initialHealth-10d);
        }

        [Test]
        public void DoTStackingDamage()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("SpiderPoisonApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);

            var doTModifier = modifierPrototypes.GetItem("SpiderPoison");
            Assert.True(enemy.ContainsModifier(doTModifier));
            //We could test for: amount of stacks, simulating the time (then damage taken)
        }
    }
}