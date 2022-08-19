using UnitLibrary;

namespace ModifierLibrary
{
	public class EffectPropertyInfo : IDeepClone<EffectPropertyInfo>
	{
		public EffectComponent EffectComponent { get; }

		public EffectOn EffectOn { get; private set; }

		//Time based

		public bool RepeatOnFinished { get; private set; }

		public double EffectDuration { get; private set; }

		public EffectPropertyInfo(EffectComponent effectComponent)
		{
			EffectComponent = effectComponent;
		}

		public void SetEffectOnInit()
		{
			EffectOn |= EffectOn.Init;
		}

		public void SetEffectOnTime(double duration, bool repeatOnFinished)
		{
			EffectOn |= EffectOn.Time;
			EffectDuration = duration;
			RepeatOnFinished = repeatOnFinished;
		}

		public void SetEffectOnApply()
		{
			EffectOn |= EffectOn.Apply;
		}

		public EffectPropertyInfo DeepClone()
		{
			var clone = new EffectPropertyInfo(EffectComponent.ShallowClone());
			clone.EffectOn = EffectOn;
			clone.RepeatOnFinished = RepeatOnFinished;
			clone.EffectDuration = EffectDuration;
			return clone;
		}
	}
}