using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Menu;
using TreeSharp;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Conditions
{
    internal class HealthPercentCondition : ExtensionCondition
    {
        private Boolean IsAbove { get; set; }
        private String IsAboveString = "IsAbove";

        private int Percentage { get; set; }
        private String PercentageString = "Percentage";

        public HealthPercentCondition(string owner, string name) : base(owner, name)
        {
            Percentage = 50;
            IsAbove = false;
        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Condition Info");
            ImGui.SetTooltip("This condition will return true if the player's health percentage is above/below the specified amount.");

            base.Initialise(Parameters);

            IsAbove = Boolean.Parse((string)Parameters[IsAboveString]);
            Percentage = Int32.Parse((string)Parameters[PercentageString]);
        }

        public override bool CreateConfigurationMenu(ref Dictionary<String, Object> Parameters)
        {
            base.CreateConfigurationMenu(ref Parameters);

            int radioTarget = IsAbove ? 0 : 1;
            ImGui.RadioButton("Above Percentage", ref radioTarget, 0);
            ImGui.RadioButton("Below Percentage", ref radioTarget, 1);

            IsAbove = radioTarget == 0;
            Parameters[IsAboveString] = IsAbove.ToString();

            Percentage = ImGuiExtension.IntSlider("Health Percentage", Percentage, 1, 100);
            Parameters[PercentageString] = Percentage.ToString();

            return true;
        }

        public override Func<bool> GetCondition(ExtensionParameter profileParameter)
        {
            return () => !profileParameter.Plugin.PlayerHelper.isHealthBelowPercentage(Percentage) == IsAbove;
        }
    }
}
