using System.Globalization;
using BaseProject;
using BaseProject.Utils;
using ModifierSystem;
using UnityEditor;
using UnityEngine;


namespace ModifierController.Editor
{
    [CustomEditor(typeof(GameController))]
    public class TestInEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Test script"))
            {
                // BaseBeing being = new BaseBeing(new BeingProperties()
                //     { Id = "TestBeing", Health = 50, DamageData = new DamageData(5, DamageType.Physical, null) });
                // being.ChangeStatusEffect(StatusEffect.Rooted);
                // being.ChangeStatusEffect(StatusEffect.Stunned);
                // Log.Info(being.StatusEffect);
                // being.ChangeStatusEffect(StatusEffect.Rooted, true);
                // Log.Info(being.StatusEffect);

                /*var statusResistances = new StatusResistances();
                var statusTags = new[] { new StatusTag(DamageType.Physical)};
                var statusTagsLong = new[] { new StatusTag(DamageType.Physical), new StatusTag(ElementalType.Acid), new StatusTag(StatusType.DoT)};

                Utilities.TimeTest(delegate
                {
                    statusResistances.GetStatusMultiplier(statusTags);
                }, 100_000,"One: ");

                Utilities.TimeTest(delegate
                {
                    statusResistances.GetStatusMultiplier(statusTagsLong);
                }, 100_000,"Long: ");

                Utilities.TimeTest(delegate
                {
                    statusResistances.GetStatusMultiplier(null);
                }, 100_000,"Null: ");*/

                //Utilities.TimeTest(delegate
                //{
                //    var test = CultureInfo.InvariantCulture;
                //}, 10_000,"Two: ");
            }
        }
    }
}