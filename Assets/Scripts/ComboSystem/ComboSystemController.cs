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
            for (int index = 0; index < _modifiers.Count; index++)
            {
                var modifier = _modifiers[index];
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
            SetupModifier("SlimePoison", slimePoisonModifier);

            var speedBuffPlayerData = new MovementSpeedModifierData(2f, 10f);
            var speedBuffPlayer = new MovementSpeedModifier(speedBuffPlayerData);
            SetupModifier("PlayerSpeedBuff", speedBuffPlayer);

            void SetupModifier(string modifierName, Modifier modifier)
            {
                modifier.Removed += modifierEventItem => _modifiers.Remove(modifierEventItem);
                _modifierPrototypes.Add(modifierName, modifier);
            }
        }
    }
}