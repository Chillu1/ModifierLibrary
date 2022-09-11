using JetBrains.Annotations;

namespace ModifierLibraryLite
{
	public class ModifierParameters
	{
		[CanBeNull] public IInitComponent InitComponent { get; private set; }
		[CanBeNull] public ITimeComponent[] TimeComponents { get; private set; }

		public void AddInitComponent(IInitComponent initComponent)
		{
			InitComponent = initComponent;
		}

		public void AddTimeComponents(params ITimeComponent[] timeComponents)
		{
			TimeComponents = timeComponents;
		}
	}
}