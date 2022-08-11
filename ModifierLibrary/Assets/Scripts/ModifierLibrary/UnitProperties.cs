using System.Linq;
using Newtonsoft.Json.Linq;

namespace ModifierLibrary
{
	public class UnitProperties : UnitLibrary.UnitProperties
	{
		public string[] Modifiers;

		public override void Load(JObject unitData)
		{
			base.Load(unitData);

			JObject[] modifiers = unitData.Value<JArray>("Modifiers")?.Values<JObject>().ToArray();
			if (modifiers == null)
				return;

			Modifiers = new string[modifiers.Length];
			for (int i = 0; i < modifiers.Length; i++)
				Modifiers[i] = modifiers[i].Value<string>(nameof(Modifier.Id));
		}
	}
}