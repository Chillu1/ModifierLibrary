using System.Diagnostics.CodeAnalysis;
using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
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
            Assert.True(ally.CurrentHealth == initialHealthAlly-10d);
        }

        [Test]
        public void DoTDamage()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("SpiderPoisonApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);//1 phys dmg, 5 poison dmg
            enemy.Update(2);//5 dmg
            enemy.Update(2);//5 dmg

            var doTModifier = modifierPrototypes.GetItem("SpiderPoison");
            Assert.True(enemy.ContainsModifier(doTModifier));
            Assert.AreEqual(initialHealthEnemy-1-5-5-5, enemy.CurrentHealth, Delta);
        }

        /*[Test]
        public void DoTStackingDamage()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("StackingSpiderPoisonApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            //Log.Info(enemy.CurrentHealth);
            character.Attack(enemy);//1 phys dmg, 5 poison dmg
            character.Attack(enemy);//1 phys, stack
            //Log.Info(enemy.CurrentHealth);
            enemy.Update(2);//7 dmg

            var doTModifier = modifierPrototypes.GetItem("StackingSpiderPoison");
            Assert.True(enemy.ContainsModifier(doTModifier));
            Assert.AreEqual(initialHealthEnemy-1-5-1-7, enemy.CurrentHealth, Delta);
            //We could test for: amount of stacks, simulating the time (then damage taken)
        }*/
    }
}