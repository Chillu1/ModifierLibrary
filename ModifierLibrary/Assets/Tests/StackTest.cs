using UnitLibrary;
using NUnit.Framework;

namespace ModifierLibrary.Tests
{
    public class StackTest : ModifierBaseTest
    {
        [Test]
        public void DoTStackingDamage()
        {
            var doTModifierApplier = modifierPrototypes.Get("DoTStackTestApplier");
            character.AddModifier(doTModifierApplier);
            character.Attack(enemy); //1 phys dmg, 1 poison dmg(init), stack (+2 poison dmg)
            enemy.Update(2.1f); //3 poison damage
            character.Attack(enemy); //1 phys, stack (+2 poison dmg)
            enemy.Update(2.1f); //5 poison dmg

            var doTModifier = modifierPrototypes.Get("DoTStackTest");
            Assert.True(enemy.ContainsModifier(doTModifier));
            Assert.AreEqual(initialHealthEnemy - 1 - 1 - 3 - 1 - 5, enemy.Stats.Health.CurrentHealth, Delta);
        }

        [Test]
        public void SilenceXStacks()
        {
            var silenceModifierApplier = modifierPrototypes.Get("SilenceXStacksTestApplier");
            character.AddModifier(silenceModifierApplier);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            character.Attack(enemy); //1
            enemy.Update(8.1f);
            character.Attack(enemy); //2
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            character.Attack(enemy); //3
            enemy.Update(8.1f);
            character.Attack(enemy); //4
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            var silenceModifier = modifierPrototypes.Get("SilenceXStacksTest");
            Assert.True(enemy.ContainsModifier(silenceModifier));
        }

        [Test]
        public void ApplyStunModifierXStacks()
        {
            var applyStunModifierApplier = modifierPrototypes.Get("ApplyStunModifierXStacksTestApplierApplier");
            var applyStunModifier = modifierPrototypes.Get("ApplyStunModifierXStacksTestApplier");
            var stunModifier = modifierPrototypes.Get("GenericStunModifierTest");
            character.AddModifier(applyStunModifierApplier);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            character.Attack(enemy); //1
            Assert.True(enemy.ContainsModifier(applyStunModifier));
            character.Attack(enemy); //2
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            character.Attack(enemy); //3
            enemy.Update(0.2f);
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.True(enemy.ContainsModifier(stunModifier));
            enemy.Update(3.2f);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.False(enemy.ContainsModifier(stunModifier));
        }
    }
}