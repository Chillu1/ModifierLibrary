using JetBrains.Annotations;
using ModifierLibraryLite.Utilities;

namespace ModifierLibraryLite
{
	public class Modifier : IModifier
	{
		[CanBeNull]
		private readonly IInitComponent _initComponent;
		[CanBeNull]
		private readonly ITimeComponent[] _timeComponents;

		public Modifier(ModifierParameters parameters)
		{
			_initComponent = parameters.InitComponent;
			_timeComponents = parameters.TimeComponents;
		}

		public void Init()
		{
			_initComponent?.Init();
		}
		
		public void Update(float deltaTime)
		{
			foreach (var timeComponent in _timeComponents.EmptyIfNull())
				timeComponent.Update(deltaTime);
		}
	}
}