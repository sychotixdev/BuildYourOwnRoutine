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
    internal class PlayerMovingCondition : ExtensionCondition
    {
        private int msMoving { get; set; } = 0;
        private const String msMovingString = "msMoving";


        public PlayerMovingCondition(string owner, string name) : base(owner, name)
        {

        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            base.Initialise(Parameters);
            msMoving = Int32.Parse((String)Parameters[msMovingString]);

        }

        public override bool CreateConfigurationMenu(ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Condition Info");
            ImGuiExtension.ToolTip("This condition will return true if the player has been moving longer than the specified time.");

            base.CreateConfigurationMenu(ref Parameters);

            msMoving = ImGuiExtension.IntSlider("Time spent moving (ms)", msMoving, 0, 10000);
            ImGuiExtension.ToolTip("Player must remain moving for this configured number of milliseconds (1000ms = 1 sec) before this condition returns true");
            Parameters[msMovingString] = msMoving.ToString();
            return true;
        }

        public override Func<bool> GetCondition(ExtensionParameter profileParameter)
        {
            return () =>
            {
                if (!profileParameter.Plugin.ExtensionCache.Cache.TryGetValue(Owner, out Dictionary<string, object> myCache))
                {
                    return false;
                }

                if (myCache.TryGetValue(DefaultExtension.CacheStartedMoving, out object o))
                    return false;
                if (o is Int32)
                {
                    return ((int)o) >= msMoving;
                }
                profileParameter.Plugin.LogErr("The cached value " + DefaultExtension.CacheStartedMoving + " is not an int.", 5);
                return false;
            };
        }
    }
}
