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
            character.AddModifier(disarmModifierApplier);
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
            character.AddModifier(disarmModifierApplier);
            enemy.AddModifier(modifierPrototypes.Get("SilenceModifierTestApplier"));
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
            character.AddModifier(rootModifierApplier);
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
            character.AddModifier(rootModifierApplier);
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
            character.AddModifier(disarmModifierApplier);
            enemy.AddModifier(modifierPrototypes.GetApplier("SilenceDisarmTwoEffectTest"));

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

        [Test]
        public void TauntStatusEffect()
        {
            var tauntModifierApplier = modifierPrototypes.GetApplier("TauntTest");
            var basicDamageModifierApplier = modifierPrototypes.GetApplier("IceBoltCastTest");
            character.AddModifier(tauntModifierApplier);
            enemy.AddModifier(basicDamageModifierApplier);
            Assert.True(enemy.StatusEffects.LegalActions == LegalAction.All);

            enemy.TargetingSystem.SetAttackTarget(ally);
            character.CastModifier(enemy, "TauntTestApplier");

            Assert.False(enemy.StatusEffects.LegalActions.HasFlag(LegalAction.Cast));
            enemy.Update(1.1f);//Should attack new taunted target, not old ally target

            Assert.False(character.Stats.Health.IsFull);
            Assert.AreEqual(initialHealthAlly, ally.Stats.Health.CurrentHealth, Delta);

            enemy.CastModifier(ally, "IceBoltCastTestApplier"); //Shouldn't do anything, can't cast while taunted
            Assert.AreEqual(initialHealthAlly, ally.Stats.Health.CurrentHealth, Delta);
        }
    }
}