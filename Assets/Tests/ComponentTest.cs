using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using BaseProject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ModifierSystem.Tests
{
    //TargetComponent
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class ComponentTest : ModifierBaseTest
    {
        [Test]
        public void TargetNotValidAlly()
        {
            var damageModifierApplier = modifierPrototypes.GetItem("IceBoltTestApplier");
            character.AddModifier(damageModifierApplier, AddModifierParameters.NullStartTarget);
            LogAssert.Expect(LogType.Error, new Regex("Target is not valid*"));
            //We can attack our own allies, but we shouldn't apply modifiers that aren't for our allies
            character.Attack(ally);
            Assert.True(ally.Stats.Health.Check(24));
        }

        [Test]
        public void RemoveComponentContains()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            enemy.Update(9.9f);

            var doTModifier = modifierPrototypes.GetItem("SpiderPoisonTest");
            Assert.True(enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void RemoveComponentEffect()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            enemy.Update(10.1f);

            var doTModifier = modifierPrototypes.GetItem("SpiderPoisonTest");
            Assert.False(enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void RemoveComponentLingerEffect()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("IceBoltTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            enemy.Update(0.4f);
            var doTModifier = modifierPrototypes.GetItem("IceBoltTest");
            Assert.True(enemy.ContainsModifier(doTModifier));
            enemy.Update(0.2f);
            Assert.False(enemy.ContainsModifier(doTModifier));
        }
    }
}