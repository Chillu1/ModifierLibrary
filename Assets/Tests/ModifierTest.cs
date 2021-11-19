using NUnit.Framework;

namespace ModifierSystem.Tests
{
    //ModifierApplier
    //Stack
    //Refresh
    public class ModifierTest : ModifierBaseTest
    {
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