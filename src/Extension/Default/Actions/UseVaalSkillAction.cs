using ImGuiNET;
using PoeHUD.Controllers;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.RemoteMemoryObjects;
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
    internal class UseVaalSkillAction : ExtensionAction
    {
        private bool useVaalHaste { get; set; } = false;
        private const String useHasteString = "useVaalHaste";

        private bool useVaalGrace { get; set; } = false;
        private const String useDodgeString = "useVaalGrace";

        private bool useVaalClarity { get; set; } = false;
        private const String useNoManaString = "useVaalClarity";

        private bool useVaalReave { get; set; } = false;
        private const String useAoeExtenderString = "useVaalReave";

        private int Key { get; set; }
        private const String keyString = "key";

        public UseVaalSkillAction(string owner, string name) : base(owner, name)
        {

        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            Key = ExtensionComponent.InitialiseParameterInt32(keyString, Key, ref Parameters);
            useVaalHaste = ExtensionComponent.InitialiseParameterBoolean(useHasteString, useVaalHaste, ref Parameters);
            useVaalGrace = ExtensionComponent.InitialiseParameterBoolean(useDodgeString, useVaalGrace, ref Parameters);
            useVaalClarity = ExtensionComponent.InitialiseParameterBoolean(useNoManaString, useVaalClarity, ref Parameters);
            useVaalReave = ExtensionComponent.InitialiseParameterBoolean(useAoeExtenderString, useVaalReave, ref Parameters);
        }

        public override bool CreateConfigurationMenu(ExtensionParameter extensionParameter, ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Vaal Skills");

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            useVaalHaste = ImGuiExtension.Checkbox("Vaal Haste", useVaalHaste);
            Parameters[useHasteString] = useVaalHaste.ToString();
            ImGui.SameLine();

            useVaalGrace = ImGuiExtension.Checkbox("Vaal Grace", useVaalGrace);
            Parameters[useDodgeString] = useVaalGrace.ToString();
            ImGui.SameLine();

            useVaalClarity = ImGuiExtension.Checkbox("Vaal Clarity", useVaalClarity);
            Parameters[useNoManaString] = useVaalClarity.ToString();
            ImGui.SameLine();

            useVaalClarity = ImGuiExtension.Checkbox("Vaal Reave", useVaalReave);
            Parameters[useAoeExtenderString] = useVaalReave.ToString();
            ImGui.SameLine();


            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            ImGuiExtension.ToolTip("This action is used to configure Vaal Skills");
            Key = (int)ImGuiExtension.HotkeySelector("Hotkey", (Keys)Key);
            ImGuiExtension.ToolTip("Hotkey to press for the first Vaal Skill.");
            Parameters[keyString] = Key.ToString();
            return true;
        }

        public override Composite GetComposite(ExtensionParameter profileParameter)
        {
            var playerBuff = GameController.Instance.Game.IngameState.Data.LocalPlayer.GetComponent<Life>();
            var player = profileParameter.Plugin.PlayerHelper;
            var localPlayer = GameController.Instance.Game.IngameState.Data.LocalPlayer;
            var playeraccess = localPlayer.GetComponent<Actor>().ActorSkills;
            var reavePlayerCount = playerBuff.Buffs.Any(x => x.Name == "reave_counter" && x.Charges == 4);

            #region Vaal Haste
            bool VaalHasteUseable = false;
            var VaalHasteUsable = playeraccess.Any(x => x.Name == "VaalHaste" && x.CurrentSouls >= 48);

            if (VaalHasteUsable && !playerBuff.HasBuff("vaal_aura_speed"))
            {
                 VaalHasteUseable = true;
            }
            else
            { 
                 VaalHasteUseable = false;
            }
            #endregion

            #region Vaal Grace
            bool VaalGraceUseable = false;
            var VaalGraceUsable = playeraccess.Any(x => x.Name == "VaalGrace" && x.CurrentSouls >= 48);

            if (VaalGraceUsable && !playerBuff.HasBuff("vaal_aura_dodge"))
            {
                VaalGraceUseable = true;
            }
            else
            {
                VaalGraceUseable = false;
            }
            #endregion

            #region Vaal Clarity
            bool VaalClarityUseable = false;
            var VaalClarityUsable = playeraccess.Any(x => x.Name == "VaalClarity" && x.CurrentSouls >= 32);

            if (VaalClarityUsable && !playerBuff.HasBuff("vaal_aura_no_mana_cost"))
            {
                VaalClarityUseable = true;
            }
            else
            {
                VaalClarityUseable = false;
            }
            #endregion

            #region Vaal Reave
            bool VaalReaveUseable = false;
            var VaalReaveUsable = playeraccess.Any(x => x.Name == "VaalReave" && x.CurrentSouls >= 48);

            if (VaalReaveUsable && reavePlayerCount)
            {
                VaalReaveUseable = true;
            }
            else
            {
                VaalReaveUseable = false;
            }
            #endregion


            return new PrioritySelector(
         new Decorator(x => ((VaalHasteUseable && useVaalHaste) || (VaalGraceUseable && useVaalGrace) || (VaalClarityUseable && useVaalClarity) ||
                             (VaalReaveUseable && useVaalReave)),
             new UseHotkeyAction(profileParameter.Plugin.KeyboardHelper, x => (Keys)Key)
        ));
        }
                
        public override string GetDisplayName(bool isAddingNew)
        {
            string displayName = "Send Key Press";

            if (!isAddingNew)
            {
                displayName += " [";
                displayName += ("Key=" + Key.ToString());
                displayName += "]";

            }

            return displayName;
        }
    }
}
