using System.Collections.Generic;
using System.Linq;
using BaseProject;
using BaseProject.Utils;
using UnityEngine.Assertions;

namespace ComboSystem
{
    public sealed class ComboModifierPrototypes : ModifierPrototypesBase<ComboModifier>
    {
        public static ComboModifierPrototypes Instance;
        
        public ComboModifierPrototypes()
        {
            Instance = this;
            SetupModifierPrototypes();
        }


        protected override void SetupModifierPrototypes()
        {
            //ComboModifier aspectOfTheCatModifier = new ComboModifier();
            var aspectOfTheCatData = new StatChangeModifierData(StatType.Attack, 10);
            var aspectOfTheCatRecipe = new ComboRecipe(new ComboRecipeProperties()
                { Ids = new[] { "MovementSpeedOfCat", "AttackSpeedOfCat" } });
            StatChangeComboModifier aspectOfTheCat =
                new StatChangeComboModifier("AspectOfTheCat", aspectOfTheCatData, aspectOfTheCatRecipe);
            SetupModifier(aspectOfTheCat);

            Damages explosionData = new Damages(new DamageData() { DamageType = DamageType.Explosion, Damage = 10f });
            var explosionRecipe = new ComboRecipe(new ComboRecipeProperties()
                { DamageTypes = new [] { DamageType.Fire | DamageType.Cold}});
            DamageAttackComboModifier explosion =
                new DamageAttackComboModifier("Explosion", explosionData, explosionRecipe);
            SetupModifier(explosion);
        }

        /// <summary>
        ///     Checks for all possible recipes in collection, and return them back
        /// </summary>
        public static List<ComboModifier> CheckForRecipes(Dictionary<string, Modifier> modifiers)
        {
            var comboModifiersToAdd = new List<ComboModifier>();

            Assert.IsTrue(Instance.prototypes.Count > 0, "0 combo modifiers in collection");
            //Iterate through all combos, check if any match
            foreach (var comboModifier in Instance.prototypes.Values)
            {
                //If comboMod can't stack or refresh, and we have it in the collection, skip it before we check for it.
                if (comboModifier.ModifierProperties == ModifierProperties.None && modifiers.ContainsKey(comboModifier.Id))
                {
                    Log.Info("Skipped combo modifier with Id (in collection): " + comboModifier.Id, "modifiers");
                    continue;
                }

                //First check for ID's
                if (Instance.CheckForIds(comboModifier, modifiers)) //Try to add a combo modifier if controller has all modifiers needed
                {
                    Instance.HandleComboModifierFound(comboModifier, modifiers, comboModifiersToAdd);
                }

                //Check for damageTypes+elementalTypes
                if (Instance.CheckForDamageTypes(comboModifier, modifiers))
                {
                    Instance.HandleComboModifierFound(comboModifier, modifiers, comboModifiersToAdd);
                }

                //Check for Etc
            }

            return comboModifiersToAdd;
        }

        private bool CheckForIds(ComboModifier comboModifier, Dictionary<string, Modifier> modifiers)
        {
            if (comboModifier.Recipe.Ids.IsNullOrEmpty())
                return false;

            bool foundAllIds = true;
            foreach (string id in comboModifier.Recipe.Ids)
            {
                if (!modifiers.ContainsKey(id))
                {
                    foundAllIds = false;
                    //Log.Verbose("Didn't find Id");
                }
            }

            return foundAllIds;
        }

        private bool CheckForDamageTypes(ComboModifier comboModifier, Dictionary<string, Modifier> modifiers)
        {
            if (comboModifier.Recipe.DamageTypes.IsNullOrEmpty())
                return false;

            //We could make this algorithm easier by having a "GetDamageTypes", which takes all damage types in the character collection
            //and compares them to what we need. But that way we have less control over it

            //Finding damageTypes process
            bool foundAllDamageTypes = false;
            //Iterate over every comboRecipe
            DamageType neededDamageType = DamageType.None;
            foreach (var damageType in comboModifier.Recipe.DamageTypes)
            {
                neededDamageType = damageType;
                //Check each modifier in controller for damageTypes
                foreach (var pair in modifiers)
                {
                    var value = pair.Value as Modifier<Damages>;
                    if(value == null)//If it's not a damageData modifier, bail (for now)
                        continue;
                    //Log.Info(value.Data.DamageData[0].DamageType+"_"+neededDamageType);
                    if (value.Data.DamageData.Any(data => (data.DamageType & neededDamageType) != 0))//A flag has been found
                    {
                        var commonFlags = neededDamageType &
                                          value.Data.DamageData.First(data => (data.DamageType & neededDamageType) != 0).DamageType;
                        //Remove common flags that we found
                        neededDamageType &= ~commonFlags;
                    }
                }
            }

            //All damageType were present in the collection
            if (neededDamageType == DamageType.None)
            {
                foundAllDamageTypes = true;
            }

            return foundAllDamageTypes;
        }

        private void HandleComboModifierFound(ComboModifier comboModifier, Dictionary<string, Modifier> modifiers, List<ComboModifier> comboModifiersToAdd)
        {
            if (!CheckForComboModifier(comboModifier, modifiers))//Dont add a comboMod if its already added
            {
                Log.Info("Added combo modifier: " + comboModifier.Id, "modifiers");
                comboModifiersToAdd.Add(comboModifier);
            }
            else
            {
                Log.Info("Already has a combo modifier with Id: " + comboModifier.Id +". Skipping.", "modifiers");
            }
        }

        private bool CheckForComboModifier(ComboModifier comboModifier, Dictionary<string, Modifier> modifiers)
        {
            return modifiers.ContainsKey(comboModifier.Id);
        }
    }
}