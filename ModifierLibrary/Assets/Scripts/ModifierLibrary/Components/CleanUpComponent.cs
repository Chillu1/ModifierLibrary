using UnitLibrary.Utils;
using JetBrains.Annotations;

namespace ModifierLibrary
{
	/// <summary>
	///     Responsible for cleaning up (removing) unit event
	/// </summary>
	public class CleanUpComponent : Component, ICleanUpComponent
	{
		[CanBeNull]
		private readonly ConditionalApplyComponent _applyComponent;

		[CanBeNull]
		private readonly EffectComponent[] _effectComponents;

		public CleanUpComponent(ConditionalApplyComponent applyComponent = null, EffectComponent[] effectComponents = null)
		{
			_applyComponent = applyComponent;
			_effectComponents = effectComponents;
		}

		public void CleanUp()
		{
			_applyComponent?.CleanUp();
			foreach (var effectComponent in _effectComponents.EmptyIfNull())
				effectComponent.RevertEffect();
		}
	}
}