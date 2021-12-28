using BaseProject;
using UnityEngine;

namespace ModifierSystem
{
    public class GameController : MonoBehaviour
    {
        public Being player;
        public Being enemy;
        //public ModifierPrototypes ModifierPrototypes { get; private set; }

        private float _timer;

        public void Start()
        {
            //ModifierPrototypes = new ModifierPrototypes();

            player = new Being(new BeingProperties(){Id = "Player", DamageData = new DamageData(0, DamageType.Physical, null), Health = 150, UnitType = UnitType.Ally});
            enemy = new Being(new BeingProperties(){Id = "Enemy", DamageData = new DamageData(2, DamageType.Physical, null), Health = 100, UnitType = UnitType.Enemy});

            //player.AddModifier(ModifierPrototypes.GetItem("SpiderPoisonApplier"), AddModifierParameters.NullStartTarget);
            //player.Attack(enemy);
        }

        private void Update()
        {
            _timer+=Time.deltaTime;
            if (_timer >= 1)
            {
                //player.Attack(enemy);
                _timer = 0;
            }

            //player.Update(Time.deltaTime);
            //enemy.Update(Time.deltaTime);
            //Log.Info(_enemy.BaseBeing.Health);
        }
    }
}