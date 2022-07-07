using BaseProject;
using UnityEngine;

namespace ModifierSystem
{
    public class GameController : MonoBehaviour
    {
        public Unit player;
        public Unit enemy;
        public ModifierPrototypes<Modifier> ModifierPrototypes { get; private set; }

        private float _timer;
        private Modifier _test;

        public void Start()
        {
            ModifierPrototypes = new ModifierPrototypes<Modifier>(true);

            player = new Unit(new UnitProperties()
                { Id = "Player", Damage = new DamageData(0, DamageType.Physical, null), Health = 150, UnitType = UnitType.Ally, Mana = 100, ManaRegen = 10});


            _test = ModifierPrototypes.GetApplier("DoTStackTest");

            //for (int i = 0; i < 100; i++) modifier.TryApply(player);

            //enemy = new Unit(new UnitProperties(){Id = "Enemy", DamageData = new DamageData(2, DamageType.Physical, null), Health = 100, UnitType = UnitType.Enemy});

            //player.AddModifier(ModifierPrototypes.GetItem("SpiderPoisonApplier"), AddModifierParameters.NullStartTarget);
            //player.Attack(enemy);
        }

        private void Update()
        {
            for (int i = 0; i < 100; i++)
            {
                var modifier = (Modifier)_test.Clone();
            }

            return;
            _timer += Time.deltaTime;
            if (_timer >= 1)
            {
                //player.Attack(enemy);
                _timer = 0;
            }

            //player.Update(Time.deltaTime);
            //enemy.Update(Time.deltaTime);
            //Log.Info(_enemy.Unit.Health);
        }
    }
}