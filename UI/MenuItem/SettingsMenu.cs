using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Menu;
using TreeRoutine.Routine.BuildYourOwnRoutine.Flask;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.UI.MenuItem
{
    internal class SettingsMenu
    {
        public SettingsMenu(BuildYourOwnRoutineCore plugin)
        {
            this.Plugin = plugin;
        }

        private BuildYourOwnRoutineCore Plugin { get; set; }

        public void Render()
        {
            if (ImGui.TreeNodeEx("Individual Flask Settings", TreeNodeFlags.DefaultOpen))
            {
                for (int i = 0; i < 5; i++)
                {
                    FlaskSetting currentFlask = Plugin.Settings.FlaskSettings[i];
                    if (ImGui.TreeNodeEx("Flask " + (i + 1) + " Settings", TreeNodeFlags.DefaultOpen))
                    {
                        currentFlask.Hotkey.Value = ImGuiExtension.HotkeySelector("Hotkey", currentFlask.Hotkey);
                        ImGui.TreePop();
                    }
                }

                ImGui.TreePop();
            }
        }
    }
}
