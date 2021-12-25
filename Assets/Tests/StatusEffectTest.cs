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
    }
}