using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Special Modifier that is activated on specific conditions (recipes), these can be: specific modifiers (ID), ElementalData or Stats
    /// </summary>
    public sealed class ComboModifier : Modifier
    {
        private ComboRecipes ComboRecipes { get; }

        public float Cooldown { get; }
        public IMultiplier Effect { get; }

        public ComboModifier(string id, ModifierInfo info, ComboRecipes comboRecipes, float cooldown = 5, IMultiplier effect = null) :
            base(id, info, AddModifierParameters.OwnerIsTarget)//TODO Combo always owner is target?
        {
            ComboRecipes = comboRecipes;
            Cooldown = cooldown;
            Effect = effect;
        }

        public override bool ValidatePrototypeSetup()
        {
            bool valid = base.ValidatePrototypeSetup();

            if (!Id.StartsWith("Combo"))
            {
                Log.Error("ComboModifier have to start with Combo", "modifiers");
                valid = false;
            }

            if (Id.Contains("Applier") || IsApplierModifier)
            {
                Log.Error("ComboModifier can't be an applier modifier, right? Maybe?", "modifiers");
                valid = false;
            }

            return valid;
        }
        
        public bool CheckRecipeId(string modifierId)
        {
            foreach (var recipe in ComboRecipes.Recipes)
            {
                if (recipe.Id == null)
                    continue;
                
                foreach (string recipeId in recipe.Id)
                    if (recipeId == modifierId)
                        return true;
            }

            return false;
        }

        public bool CheckRecipes(HashSet<string> modifierIds, ElementController elementController, Stats stats)
        {
            if (modifierIds.Contains(Id))
            {
                //Log.Info($"Already contains {Id}, skipping", "modifiers");
                return
                    false; //Already contains this combo modifier, skip (try refresh & stack? Might be a problem without proper sophisticated cooldowns)
            }

            //Log.Verbose("Checking recipes", "modifiers");
            foreach (var recipe in ComboRecipes.Recipes)
            {
                //Go through all possible recipes
                (bool success, double multiplier) = CheckRecipe(recipe, modifierIds, elementController, stats);
                if (success)
                {
                    if (Effect != null)
                        Effect.SetMultiplier(multiplier);
                    return true; //If we found one, success
                }
            }

            //Log.Verbose("No recipe found", "modifiers");
            return false; //Didn't find a recipe
        }

        private (bool success, double multiplier) CheckRecipe(ComboRecipe recipe, HashSet<string> modifierIds,
            ElementController elementController, Stats stats)
        {
            double multiplier = 1; //For now, we only support dynamic element combos

            //Log.Verbose("Id: " + Id, "modifiers");
            if (recipe.Id != null && recipe.Id.Length != 0)
            {
                if (!CheckForIdConditions(recipe, modifierIds))
                    return (false, 0);
            }

            if (recipe.ElementalRecipe != null && recipe.ElementalRecipe.Length != 0)
            {
                (bool exists, double eleMultiplier) = CheckForElementalConditions(recipe, elementController);
                if (!exists)
                    return (false, 0);
                multiplier = eleMultiplier;
            }

            if (recipe.Stat != null && recipe.Stat.Length != 0)
            {
                if (!CheckForStatConditions(recipe, stats))
                    return (false, 0);
            }

            //Found everything needed, add combo modifier
            return (true, multiplier);
        }

        private bool CheckForIdConditions(ComboRecipe recipe, HashSet<string> ids)
        {
            foreach (string idCondition in recipe.Id!)
            {
                if (!ids.Contains(idCondition))
                    return false;
            }

            return true;
        }

        private (bool exists, double multiplier) CheckForElementalConditions(ComboRecipe recipe, ElementController elementController)
        {
            double minElemental = double.MaxValue;

            foreach (var elementalRecipe in recipe.ElementalRecipe!)
            {
                if (!elementController.HasIntensity(elementalRecipe.elementType, elementalRecipe.Intensity))
                    return (false, 0);

                double intensity = elementController.GetIntensity(elementalRecipe.elementType);
                if (intensity < minElemental)
                    minElemental = intensity;
            }

            return (true, Curves.ComboElementMultiplier.Evaluate(minElemental));
        }

        private bool CheckForStatConditions(ComboRecipe recipe, Stats stats)
        {
            foreach (var stat in recipe.Stat!)
            {
                //TODO What to do with damage stat?
                if (!stats.HasStat(stat.StatType, stat.Value))
                    return false;
            }

            return true;
        }
    }
}