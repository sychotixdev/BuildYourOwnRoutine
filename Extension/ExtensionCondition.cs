using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Menu;
using TreeSharp;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension
{
    public abstract class ExtensionCondition : ExtensionComponent
    {
        public ExtensionCondition()
        {
            Owner = "Owner not configured";
            Name = "Name not configured";
        }

        public ExtensionCondition(string owner, string name)
        {
            Owner = owner;
            Name = name;
        }

        public bool Invert { get; set; } = false;
        public string InvertString { get; set; } = "Invert";

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            Invert = Boolean.Parse((string)Parameters[InvertString]);
        }

        public override bool CreateConfigurationMenu(ref Dictionary<string, object> Parameters)
        {
            Invert = ImGuiExtension.Checkbox("Invert", Invert);
            ImGui.SetTooltip("Check this box to invert the returned value of this condition.\nFor Example when enabled, if the condition returns true when in hideout, it would now return true when NOT in hideout.");
            Parameters[InvertString] = Invert.ToString();

            return true;
        }

        public abstract Func<bool> GetCondition(ExtensionParameter extensionParameter);

        
    }
}
