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
            var damageModifierApplier = modifierPrototypes.GetItem("IceBoltApplier");
            character.AddModifier(damageModifierApplier, AddModifierParameters.NullStartTarget);
            LogAssert.Expect(LogType.Error, new Regex("Target is not valid*"));
            //We can attack our own allies, but we shouldn't apply modifiers that aren't for our allies
            character.Attack(ally);
            Assert.True(ally.BaseBeing.Health.PoolStat.BaseStat.baseValue == 24);
        }
    }
}