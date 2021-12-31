using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class StackTest : ModifierBaseTest
    {
        [Test]
        public void DoTStackingDamage()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("DoTStackTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);//1 phys dmg, 1 poison dmg(init), stack (+2 poison dmg)
            enemy.Update(2.1f);//3 poison damage
            character.Attack(enemy);//1 phys, stack (+2 poison dmg)
            enemy.Update(2.1f);//5 poison dmg

            var doTModifier = modifierPrototypes.GetItem("DoTStackTest");
            Assert.True(enemy.ContainsModifier(doTModifier));
            Assert.AreEqual(initialHealthEnemy-1-1-3-1-5, enemy.Stats.Health.CurrentHealth, Delta);
        }

        [Test]
        public void SilenceXStacks()
        {
            var silenceModifierApplier = modifierPrototypes.GetItem("SilenceXStacksTestApplier");
            character.AddModifier(silenceModifierApplier, AddModifierParameters.NullStartTarget);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            character.Attack(enemy);//1
            enemy.Update(8.1f);
            character.Attack(enemy);//2
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            character.Attack(enemy);//3
            enemy.Update(8.1f);
            character.Attack(enemy);//4
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            var silenceModifier = modifierPrototypes.GetItem("SilenceXStacksTest");
            Assert.True(enemy.ContainsModifier(silenceModifier));
        }

        [Test]
        public void ApplyStunModifierXStacks()
        {
            var applyStunModifierApplier = modifierPrototypes.GetItem("ApplyStunModifierXStacksTestApplierApplier");
            var applyStunModifier = modifierPrototypes.GetItem("ApplyStunModifierXStacksTestApplier");
            var stunModifier = modifierPrototypes.GetItem("GenericStunModifierTest");
            character.AddModifier(applyStunModifierApplier, AddModifierParameters.NullStartTarget);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            character.Attack(enemy);//1
            Assert.True(enemy.ContainsModifier(applyStunModifier));
            character.Attack(enemy);//2
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            character.Attack(enemy);//3
            enemy.Update(0.2f);
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.True(enemy.ContainsModifier(stunModifier));
            enemy.Update(3.2f);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.False(enemy.ContainsModifier(stunModifier));
        }
    }
}