using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class StatusEffectTest : ModifierBaseTest
    {
        [Test]
        public void ActAttackStatusEffect()
        {
            var disarmModifierApplier = modifierPrototypes.GetItem("DisarmModifierTestApplier");
            character.AddModifier(disarmModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);

            Assert.False(enemy.LegalActions.HasFlag(LegalAction.Act));
            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Cast));

            enemy.Attack(character);//Shouldn't deal any damage, since disarmed
            Assert.AreEqual(initialHealthCharacter, character.Health.CurrentHealth, Delta);
        }

        [Test]
        public void CastSilenceStatusEffect()
        {
            var disarmModifierApplier = modifierPrototypes.GetItem("SilenceModifierTestApplier");
            character.AddModifier(disarmModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.AddModifier(modifierPrototypes.GetItem("SilenceModifierTestApplier"), AddModifierParameters.NullStartTarget);
            character.Attack(enemy);

            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Act));
            Assert.False(enemy.LegalActions.HasFlag(LegalAction.Cast));

            enemy.CastModifier(character, "SilenceModifierTestApplier");//Shouldn't do anything
            Assert.True(character.LegalActions.HasFlag(LegalAction.Act));
            Assert.True(character.LegalActions.HasFlag(LegalAction.Cast));
        }

        [Test]
        public void TimedStatusEffect()
        {
            var rootModifierApplier = modifierPrototypes.GetItem("RootTimedModifierTestApplier");
            character.AddModifier(rootModifierApplier, AddModifierParameters.NullStartTarget);
            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Move));
            enemy.Update(0.01f);
            enemy.Update(0.01f);
            character.Attack(enemy);//Root (init)
            enemy.Update(0.01f);
            Assert.False(enemy.LegalActions.HasFlag(LegalAction.Move));
           // Log.Info(enemy.LegalActions);
            enemy.Update(1.01f);
            //Log.Info(enemy.LegalActions);
            Assert.False(enemy.LegalActions.HasFlag(LegalAction.Move));
            enemy.Update(0.5f);
            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Move));
        }

        [Test]
        public void DelayedStatusEffect()
        {
            var rootModifierApplier = modifierPrototypes.GetItem("DelayedSilenceModifierTestApplier");
            character.AddModifier(rootModifierApplier, AddModifierParameters.NullStartTarget);
            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Cast));
            character.CastModifier(enemy, "DelayedSilenceModifierTestApplier");
            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Cast));
            enemy.Update(0.9f);
            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Cast));
            enemy.Update(0.11f);
            Assert.False(enemy.LegalActions.HasFlag(LegalAction.Cast));
            enemy.Update(1);
            Assert.True(enemy.LegalActions.HasFlag(LegalAction.Cast));
        }
    }
}