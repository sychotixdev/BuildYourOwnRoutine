using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.DefaultBehaviors.Actions;
using TreeRoutine.Menu;
using TreeSharp;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Actions
{
    internal class UseFlaskAction : ExtensionAction
    {
        private int flaskIndex { get; set; } = 1;
        private const String flaskIndexString = "flaskIndex";

        public UseFlaskAction(string owner, string name) : base(owner, name)
        {

        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            flaskIndex = ExtensionComponent.InitialiseParameterInt32(flaskIndexString, flaskIndex, ref Parameters);
        }

        public override bool CreateConfigurationMenu(ExtensionParameter extensionParameter, ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Action Info");
            ImGuiExtension.ToolTip("This action is used to use a specific flask.\nFlask Hotkey will be pulled from plugin settings.");

            flaskIndex = ImGuiExtension.IntSlider("Flask Index", flaskIndex, 1, 5);
            ImGuiExtension.ToolTip("Index for flask to be used (1= farthest left, 5 = farthest right)");
            Parameters[flaskIndexString] = flaskIndex.ToString();
            return true;
        }

        public override Composite GetComposite(ExtensionParameter profileParameter)
        {
            return new UseHotkeyAction(profileParameter.Plugin.KeyboardHelper, x => profileParameter.Plugin.Settings.FlaskSettings[flaskIndex - 1].Hotkey);
        }
    }
}
