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
            var doTModifierApplier = modifierPrototypes.GetItem("IceBoltApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            character.Attack(enemy);
            character.Attack(enemy);

            var doTModifier = modifierPrototypes.GetItem("IceBolt");
            Assert.True(enemy.ContainsModifier(doTModifier));
            Assert.True(enemy.BaseBeing.IsDead);
            //We could test for: amount of stacks, simulating the time (then damage taken)
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