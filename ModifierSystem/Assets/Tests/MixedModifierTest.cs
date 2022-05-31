using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class MixedModifierTest : ModifierBaseTest
    {
        [Test]
        public void DamagePerStackFlag()
        {
            var modifierApplier = modifierPrototypes.Get("DamagePerStackTestApplier");
            character.AddModifier(modifierApplier);
            //Log.Info(enemy.Stats.Health.CurrentHealth);
            character.Attack(enemy); //1+2 damage

            enemy.Update(6);
            Assert.AreEqual(initialHealthEnemy - 1 - 2, enemy.Stats.Health.CurrentHealth, Delta);
            Assert.True(enemy.ContainsModifier("DamagePerStackTest"));
            character.Attack(enemy); //1+2*2 damage, refresh
            Assert.AreEqual(initialHealthEnemy - 1 - 2 - 1 - 2 * 2, enemy.Stats.Health.CurrentHealth, Delta);

            enemy.Update(6);
            character.Update(1); //For _eventController
            Assert.True(enemy.ContainsModifier("DamagePerStackTest"));

            enemy.Update(5);
            Assert.False(enemy.ContainsModifier("DamagePerStackTest"));

            character.Attack(enemy); //1+2 damage
            Assert.True(enemy.ContainsModifier("DamagePerStackTest"));
            Assert.AreEqual(initialHealthEnemy - 1 - 2 - 1 - 2 * 2 - 1 - 2, enemy.Stats.Health.CurrentHealth, Delta);
        }
    }
}