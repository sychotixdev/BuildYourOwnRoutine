using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeRoutine.DefaultBehaviors.Actions;
using TreeRoutine.Menu;
using TreeSharp;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Actions
{
    internal class SendKeyAction : ExtensionAction
    {
        private int Key { get; set; }
        private const String keyString = "key";

        public SendKeyAction(string owner, string name) : base(owner, name)
        {

        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            Key = Int32.Parse((String)Parameters[keyString]);
        }

        public override bool CreateConfigurationMenu(ExtensionParameter extensionParameter, ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Action Info");
            ImGuiExtension.ToolTip("This action is used to send a hotkey to the game.");
            Key = (int)ImGuiExtension.HotkeySelector("Hotkey", (Keys)Key);
            ImGuiExtension.ToolTip("Hotkey to press for this action.");
            Parameters[keyString] = Key.ToString();
            return true;
        }

        public override Composite GetComposite(ExtensionParameter profileParameter)
        {
            return new UseHotkeyAction(profileParameter.Plugin.KeyboardHelper, x => (Keys)Key);
        }
    }
}
