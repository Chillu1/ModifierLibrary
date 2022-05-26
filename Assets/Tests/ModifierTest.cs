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
            var damageModifierApplier = modifierPrototypes.Get("IceBoltTestApplier");
            character.AddModifier(damageModifierApplier, AddModifierParameters.NullStartTarget);
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
            character.AddModifier(healModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.ChangeDamageStat(new DamageData(9, DamageType.Physical, null)); //10 dmg
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
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
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
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            var doTModifier = modifierPrototypes.Get("DoTRefreshTest");
            character.Attack(enemy);
            Assert.True(enemy.ContainsModifier(doTModifier));
            enemy.Update(8); //2 seconds left
            character.Attack(enemy); //Refresh
            enemy.Update(6); //4 seconds left
            Assert.True(enemy.ContainsModifier(doTModifier));
        }
    }
}