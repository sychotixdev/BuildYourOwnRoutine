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
    internal class CanUseFlaskCondition : ExtensionCondition
    {
        private int flaskIndex { get; set; } = 1;
        private const String flaskIndexString = "flaskIndex";

        private int reserveUses { get; set; } = 0;
        private const String reserveUsesString = "reserveUses";


        public CanUseFlaskCondition(string owner, string name) : base(owner, name)
        {

        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            base.Initialise(Parameters);
            flaskIndex = Int32.Parse((String)Parameters[flaskIndexString]);
            reserveUses = Int32.Parse((String)Parameters[reserveUsesString]);

        }

        public override bool CreateConfigurationMenu(ExtensionParameter extensionParameter, ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Condition Info");
            ImGuiExtension.ToolTip("This condition is used to determine if we can use a specific flask.\nIt will ensure that health/hybrid/mana potions are not used when we are at full health/mana.\nThis will also ensure that we do not use up reserved uses.");

            base.CreateConfigurationMenu(extensionParameter, ref Parameters);

            flaskIndex = ImGuiExtension.IntSlider("Flask Index", flaskIndex, 1, 5);
            Parameters[flaskIndexString] = flaskIndex.ToString();
            reserveUses = ImGuiExtension.IntSlider("Reserved Uses", reserveUses, 0, 5);
            Parameters[flaskIndexString] = reserveUses.ToString();
            return true;
        }

        public override Func<bool> GetCondition(ExtensionParameter profileParameter)
        {
            return () => !profileParameter.Plugin.FlaskHelper.canUsePotion(flaskIndex, reserveUses);
        }
    }
}
