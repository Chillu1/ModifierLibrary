using System.Collections.Generic;

namespace ComboSystem
{
    public class ComboSystemController
    {
        private readonly Dictionary<string, Modifier> _modifierPrototypes;
        private readonly List<Modifier> _modifiers;

        public ComboSystemController(List<Modifier> modifiers)
        {
            _modifiers = modifiers;
            _modifierPrototypes = new Dictionary<string, Modifier>();
        }

        public void Init()
        {
            SetupModifierPrototypes();
        }

        public void Update(float deltaTime)
        {
            foreach (var modifier in _modifiers)
            {
                modifier.Update(deltaTime);
            }
        }

        private void SetupModifierPrototypes()
        {
            //EffectOverTimeData effectOverTimeData = new EffectOverTimeData(EffectType.StatChange, 1f, 5f);
            //EffectOverTimeModifier<EffectOverTimeData> effectOverTimeModifier = new EffectOverTimeModifier<EffectOverTimeData>(effectOverTimeData);
            DamageOverTimeData slimePoisonData =
                new DamageOverTimeData(new[] {new DamageData() {Damage = 5, DamageType = DamageType.Poison}}, 1f, 5f);
            DamageOverTimeModifier slimePoisonModifier = new DamageOverTimeModifier(slimePoisonData);


        }
    }
}