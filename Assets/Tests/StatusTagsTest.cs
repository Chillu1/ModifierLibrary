using System.Text.RegularExpressions;
using BaseProject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ModifierSystem.Tests
{
    public class StatusTagsTest : ModifierBaseTest
    {
        [Test]
        public void TryOverwriteTags()
        {
            var modifier = modifierPrototypes.GetItem("SpiderPoisonTest");
            LogAssert.Expect(LogType.Error, new Regex("overwriting*"));
            modifier!.FinishSetup();
        }

        [Test]
        public void CorrectTagsPhysicalPoisonDot()
        {
            var spiderPoisonModifier = new Modifier("SpiderPoisonTest");
            var target = new TargetComponent();
            var damageData = new[] { new DamageData(5, DamageType.Physical, new ElementData(ElementalType.Poison, 10, 20)) };
            var effect = new DamageComponent(damageData, target);
            var apply = new ApplyComponent(effect, target);
            spiderPoisonModifier.AddComponent(new InitComponent(apply));
            spiderPoisonModifier.AddComponent(target);
            spiderPoisonModifier.AddComponent(new TimeComponent(effect, 2, true));
            spiderPoisonModifier.AddComponent(new TimeComponent(new RemoveComponent(spiderPoisonModifier), 10));
            var tags = spiderPoisonModifier.FinishSetup(damageData);

            for (int i = 0; i < tags.Length; i++)
            {
                Assert.True(tags[i].Contains(DamageType.Physical) ||
                            tags[i].Contains(ElementalType.Poison) ||
                            tags[i].Contains(StatusType.Element) ||
                            tags[i].Contains(StatusType.DoT));
            }
        }
    }
}