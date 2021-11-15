using System;

namespace ComboSystemComposition
{
    public class StatComponent : EffectComponent
    {
        public StatComponent() : base(() => {/*stats, etc*/})
        {
        }
    }
}