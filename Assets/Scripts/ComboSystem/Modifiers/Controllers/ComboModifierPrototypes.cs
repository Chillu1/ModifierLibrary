using System.Collections.Generic;
using UnityEngine.Assertions;

namespace ComboSystem
{
    public sealed class ComboModifierPrototypes : ModifierPrototypesBase<ComboModifier>
    {
        public ComboModifierPrototypes()
        {
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
        }

        /// <summary>
        ///     Checks for all possible recipes in collection, and return them back
        /// </summary>
        public List<ComboModifier> CheckForRecipes(Dictionary<string, Modifier> modifiers)
        {
            var comboModifiersToAdd = new List<ComboModifier>();

            Assert.IsTrue(modifierPrototypes.Count > 0, "0 combo modifiers in collection");
            //Iterate through all combos, check if any match
            foreach (var comboModifier in modifierPrototypes.Values)
            {
                //First check for ID's
                if (CheckForIds(comboModifier, modifiers)) //Try to add a combo modifier if controller has all modifiers needed
                {
                    Log.Info("Found id's for: " + comboModifier.Id);
                    if (!CheckForComboModifier(comboModifier, modifiers))//Dont add a comboMod if its already added
                    {
                        Log.Info("Added combo modifier: " + comboModifier.Id);
                        comboModifiersToAdd.Add(comboModifier);
                    }
                    else
                    {
                        Log.Info("Already has a combo modifier with Id: " + comboModifier.Id +". Skipping.");
                    }
                }

                //Check for damageTypes+elementalTypes

                //Check for Etc
            }

            return comboModifiersToAdd;
        }

        private bool CheckForIds(ComboModifier comboModifier, Dictionary<string, Modifier> modifiers)
        {
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

        private bool CheckForComboModifier(ComboModifier comboModifier, Dictionary<string, Modifier> modifiers)
        {
            return modifiers.ContainsKey(comboModifier.Id);
        }
    }
}