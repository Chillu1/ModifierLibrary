using System.Text.RegularExpressions;
using UnitLibrary;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ModifierLibrary.Tests
{
	public class StatusTagsTest : ModifierBaseTest
	{
		[Test]
		public void TryOverwriteTags()
		{
			var modifier = modifierPrototypes.Get("SpiderPoisonTest");
			LogAssert.Expect(LogType.Error, new Regex("overwriting*"));
			modifier!.FinishSetup();
		}

		[Test]
		public void CorrectTagsPhysicalPoisonDot()
		{
			var spiderPoisonModifier = modifierPrototypes.Get("SpiderPoisonTest");

			var tags = spiderPoisonModifier!.StatusTags;
			for (int i = 0; i < tags.Length; i++)
			{
				Assert.True(tags[i].Contains(DamageType.Physical) ||
				            tags[i].Contains(ElementType.Poison) ||
				            tags[i].Contains(StatusType.Element) ||
				            tags[i].Contains(StatusType.Duration) ||
				            tags[i].Contains(StatusType.DoT));
			}
		}

		[Test]
		public void ManyCorrectTags()
		{
			var allTagsModifier = modifierPrototypes.Get("ManyTagsTest");

			var tags = allTagsModifier!.StatusTags;
			for (int i = 0; i < tags.Length; i++)
			{
				Assert.True(tags[i].Contains(DamageType.Physical) ||
				            tags[i].Contains(DamageType.Magical) ||
				            tags[i].Contains(DamageType.Pure) ||
				            tags[i].Contains(ElementType.Acid) ||
				            tags[i].Contains(ElementType.Bleed) ||
				            tags[i].Contains(ElementType.Cold) ||
				            tags[i].Contains(StatusType.Element) ||
				            tags[i].Contains(StatusType.Duration) ||
				            tags[i].Contains(StatusType.DoT));
			}
		}
	}
}