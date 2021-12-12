using System;
using System.Collections.Generic;
using System.Linq;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     ComboModifier does everything that normal modifier can, but is activated on specific conditions
    /// </summary>
    public class ComboModifier : IModifier, IEventCopy<ComboModifier>
    {
        public string Id => Modifier.Id;

        private Modifier Modifier { get; }
        private ComboRecipes ComboRecipes { get; }
        private double Cooldown { get; }

        public TargetComponent TargetComponent => Modifier.TargetComponent;
        public bool ApplierModifier => Modifier.ApplierModifier;

        public ComboModifier(Modifier modifier, ComboRecipes comboRecipes, double cooldown)
        {
            Modifier = modifier;
            ComboRecipes = comboRecipes;
            Cooldown = cooldown;
        }

        public bool ValidatePrototypeSetup()
        {
            bool success = Modifier.ValidatePrototypeSetup();

            if (Id.Contains("Applier") || Modifier.ApplierModifier)
            {
                Log.Error("ComboModifier can't be an applier modifier, right?", "modifiers");
                success = false;
            }

            return success;
        }

        public bool CheckRecipes(HashSet<string> modifierIds, ElementController elementController)
        {
            if (modifierIds.Contains(Id))
                return false;//Already contains this combo modifier, skip (try refresh & stack? Might be a problem without proper sophisticated cooldowns)

            foreach (var recipe in ComboRecipes.Recipes)
            {
                //Go through all possible recipes
                if (CheckRecipe(recipe, modifierIds, elementController))
                    return true;//If we found one, success
            }

            return false;//Didn't find a recipe
        }

        private bool CheckRecipe(ComboRecipe recipe, HashSet<string> modifierIds, ElementController elementController)
        {
            if (recipe.Id != null && recipe.Id.Length != 0)
            {
                if (!CheckForIdConditions(recipe, modifierIds))
                    return false;
            }

            if (recipe.ElementalRecipe != null && recipe.ElementalRecipe.Length != 0)
            {
                if (!CheckForElementalConditions(recipe, elementController))
                    return false;
            }

            //Found everything needed, add combo modifier
            return true;
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
        private bool CheckForElementalConditions(ComboRecipe recipe, ElementController elementController)
        {
            foreach (var elementalRecipe in recipe.ElementalRecipe!)
            {
                if (!elementController.HasIntensity(elementalRecipe.ElementalType, elementalRecipe.Intensity))
                    return false;
            }

            return true;
        }

        public void CopyEvents(ComboModifier prototype)
        {
            Modifier.CopyEvents(prototype.Modifier);
        }


        public void Init() => Modifier.Init();
        public void TryApply(Being target) => Modifier.TryApply(target);

        public void Update(float deltaTime) => Modifier.Update(deltaTime);

        public bool Stack() => Modifier.Stack();

        public bool Refresh() => Modifier.Refresh();

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
    }

    public class ComboRecipes
    {
        /// <summary>
        ///     Possible recipes
        /// </summary>
        [NotNull] public ComboRecipe[] Recipes;

        public ComboRecipes([NotNull] ComboRecipe recipe)
        {
            Recipes = new[] { recipe };
        }

        public ComboRecipes([NotNull] ComboRecipe[] recipes)
        {
            Recipes = recipes;
        }
    }

    public class ComboRecipe
    {
        [CanBeNull]
        public string[] Id;
        [CanBeNull]
        public ElementalRecipe[] ElementalRecipe;

        public ComboRecipe()
        {
        }
        public ComboRecipe(string[] id)
        {
            Id = id;
        }
        public ComboRecipe(ElementalRecipe[] elementalRecipe)
        {
            ElementalRecipe = elementalRecipe;
        }
        public ComboRecipe(string[] id,ElementalRecipe[] elementalRecipe)
        {
            Id = id;
            ElementalRecipe = elementalRecipe;
        }
    }

    public class ElementalRecipe
    {
        public ElementalType ElementalType;
        public double Intensity;

        public ElementalRecipe(ElementalType elementalType, double intensity)
        {
            ElementalType = elementalType;
            Intensity = intensity;
        }
    }
}