using System;
using JetBrains.Annotations;

namespace ComboSystemComposition
{
    public class EffectComponent : Component, IEffectComponent
    {
        [NotNull] private event Action OnEffect;

        public EffectComponent(Action onEffect)
        {
            OnEffect = onEffect;
        }

        public void Effect()
        {
            OnEffect.Invoke();
        }
    }
}