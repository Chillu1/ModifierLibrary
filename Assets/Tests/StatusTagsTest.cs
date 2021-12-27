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
            var spiderPoisonModifier = modifierPrototypes.GetItem("SpiderPoisonTest");

            var tags = spiderPoisonModifier!.StatusTags;
            for (int i = 0; i < tags.Length; i++)
            {
                Assert.True(tags[i].Contains(DamageType.Physical) ||
                            tags[i].Contains(ElementalType.Poison) ||
                            tags[i].Contains(StatusType.Element) ||
                            tags[i].Contains(StatusType.Duration) ||
                            tags[i].Contains(StatusType.DoT));
            }
        }

        [Test]
        public void ManyCorrectTags()
        {
            var allTagsModifier = modifierPrototypes.GetItem("ManyTagsTest");

            var tags = allTagsModifier!.StatusTags;
            for (int i = 0; i < tags.Length; i++)
            {
                Assert.True(tags[i].Contains(DamageType.Physical) ||
                            tags[i].Contains(DamageType.Magical) ||
                            tags[i].Contains(DamageType.Pure) ||
                            tags[i].Contains(ElementalType.Acid) ||
                            tags[i].Contains(ElementalType.Bleed) ||
                            tags[i].Contains(ElementalType.Cold) ||
                            tags[i].Contains(StatusType.Element) ||
                            tags[i].Contains(StatusType.Duration) ||
                            tags[i].Contains(StatusType.DoT));
            }
        }
    }
}