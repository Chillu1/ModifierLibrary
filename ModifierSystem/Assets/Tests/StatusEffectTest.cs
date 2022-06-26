using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class StatusEffectTest : ModifierBaseTest
    {
        [Test]
        public void ActAttackStatusEffect()
        {
            var disarmModifierApplier = modifierPrototypes.Get("DisarmModifierTestApplier");
            character.AddModifier(disarmModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);

            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));

            enemy.Attack(character); //Shouldn't deal any damage, since disarmed
            Assert.AreEqual(initialHealthCharacter, character.Stats.Health.CurrentHealth, Delta);
        }

        [Test]
        public void CastSilenceStatusEffect()
        {
            var disarmModifierApplier = modifierPrototypes.Get("SilenceModifierTestApplier");
            character.AddModifier(disarmModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.AddModifier(modifierPrototypes.Get("SilenceModifierTestApplier"), AddModifierParameters.NullStartTarget);
            character.CastModifier(enemy, "SilenceModifierTestApplier");

            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));

            enemy.CastModifier(character, "SilenceModifierTestApplier"); //Shouldn't do anything
            Assert.True(character.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.True(character.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
        }

        [Test]
        public void TimedStatusEffect()
        {
            var rootModifierApplier = modifierPrototypes.Get("RootTimedModifierTestApplier");
            character.AddModifier(rootModifierApplier, AddModifierParameters.NullStartTarget);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Move));
            enemy.Update(0.01f);
            enemy.Update(0.01f);
            character.Attack(enemy); //Root (init)
            enemy.Update(0.01f);
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Move));
            // Log.Info(enemy.StatusEffects.LegalActions);
            enemy.Update(1.01f);
            //Log.Info(enemy.StatusEffects.LegalActions);
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Move));
            enemy.Update(0.5f);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Move));
        }

        [Test]
        public void DelayedStatusEffect()
        {
            var rootModifierApplier = modifierPrototypes.Get("DelayedSilenceModifierTestApplier");
            character.AddModifier(rootModifierApplier, AddModifierParameters.NullStartTarget);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            character.CastModifier(enemy, "DelayedSilenceModifierTestApplier");
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            enemy.Update(0.9f);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            enemy.Update(0.2f);
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            enemy.Update(1f);
            Assert.True(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
        }

        [Test]
        public void TwoEffectsSilenceDisarm()
        {
            var disarmModifierApplier = modifierPrototypes.GetApplier("SilenceDisarmTwoEffectTest");
            character.AddModifier(disarmModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.AddModifier(modifierPrototypes.GetApplier("SilenceDisarmTwoEffectTest"), AddModifierParameters.NullStartTarget);

            Assert.True(enemy.StatusEffects.LegalActions == LegalAction.All);

            character.CastModifier(enemy, "SilenceDisarmTwoEffectTestApplier");

            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));

            enemy.Attack(character); //Shouldn't do anything (disarmed)
            Assert.True(character.Stats.Health.IsFull);

            enemy.CastModifier(character, "SilenceDisarmTwoEffectTestApplier"); //Shouldn't do anything (silenced)
            Assert.True(character.StatusEffects.LegalActions.HasFlag(LegalAction.Act));
            Assert.True(character.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));

            enemy.Update(1.2f);

            enemy.Attack(character);
            Assert.False(character.Stats.Health.IsFull);
        }
    }
}