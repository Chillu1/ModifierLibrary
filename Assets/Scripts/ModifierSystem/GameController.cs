using BaseProject;
using UnityEngine;

namespace ModifierSystem
{
    public class GameController : MonoBehaviour
    {
        private Being _player;
        private Being _enemy;
        public ModifierPrototypes ModifierPrototypes { get; private set; }

        private float _timer;

        public void Start()
        {
            ModifierPrototypes = new ModifierPrototypes();

            _player = new Being(new BeingProperties(){Id = "Player", DamageData = new DamageData(0, DamageType.Physical, null), Health = 150, UnitType = UnitType.Ally});
            _enemy = new Being(new BeingProperties(){Id = "Enemy", DamageData = new DamageData(2, DamageType.Physical, null), Health = 100, UnitType = UnitType.Enemy});

            _player.AddModifier(ModifierPrototypes.GetItem("SpiderPoisonApplier"), AddModifierParameters.NullStartTarget);
            _player.Attack(_enemy);
        }

        private void Update()
        {
            _timer+=Time.deltaTime;
            if (_timer >= 1)
            {
                _player.Attack(_enemy);
                _timer = 0;
            }

            _player.Update(Time.deltaTime);
            _enemy.Update(Time.deltaTime);
            //Log.Info(_enemy.BaseBeing.Health);
        }
    }
}