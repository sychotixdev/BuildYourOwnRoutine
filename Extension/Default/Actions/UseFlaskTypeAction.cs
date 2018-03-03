using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.DefaultBehaviors.Actions;
using TreeRoutine.FlaskComponents;
using TreeRoutine.Menu;
using TreeSharp;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Actions
{
    internal class UseFlaskTypeAction : ExtensionAction
    {
        private bool useLife { get; set; } = false;
        private const String useLifeString = "useLife";

        private bool useMana { get; set; } = false;
        private const String useManaString = "useMana";

        private bool useHybrid { get; set; } = false;
        private const String useHybridString = "useHybrid";

        private bool useInstant { get; set; } = false;
        private const String useInstantString = "useInstant";

        private bool useDefense { get; set; } = false;
        private const String useDefenseString = "useDefense";

        private bool useUtility { get; set; } = false;
        private const String useUtilityString = "useUtility";

        private bool useSpeedrun { get; set; } = false;
        private const String useSpeedrunString = "useSpeedrun";

        private bool useOffense { get; set; } = false;
        private const String useOffenseString = "useOffense";

        private bool usePoison { get; set; } = false;
        private const String usePoisonString = "usePoison";

        private bool useFreeze { get; set; } = false;
        private const String useFreezeString = "useFreeze";

        private bool useIgnite { get; set; } = false;
        private const String useIgniteString = "useIgnite";

        private bool useShock { get; set; } = false;
        private const String useShockString = "useShock";

        private bool useBleed { get; set; } = false;
        private const String useBleedString = "useBleed";

        private bool useCurse { get; set; } = false;
        private const String useCurseString = "useCurse";

        private bool useUnique { get; set; } = false;
        private const String useUniqueString = "useUnique";

        private bool useOffenseAndSpeedrun { get; set; } = false;
        private const String useOffenseAndSpeedrunString = "useOffenseAndSpeedrun";


        public UseFlaskTypeAction(string owner, string name) : base(owner, name)
        {

        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            useLife = Boolean.Parse((String)Parameters[useLifeString]);
            useMana = Boolean.Parse((String)Parameters[useManaString]);
            useHybrid = Boolean.Parse((String)Parameters[useHybridString]);
            useInstant = Boolean.Parse((String)Parameters[useInstantString]);
            useDefense = Boolean.Parse((String)Parameters[useDefenseString]);
            useUtility = Boolean.Parse((String)Parameters[useUtilityString]);
            useSpeedrun = Boolean.Parse((String)Parameters[useSpeedrunString]);
            useOffense = Boolean.Parse((String)Parameters[useOffenseString]);
            usePoison = Boolean.Parse((String)Parameters[usePoisonString]);
            useFreeze = Boolean.Parse((String)Parameters[useFreezeString]);
            useIgnite = Boolean.Parse((String)Parameters[useIgniteString]);
            useShock = Boolean.Parse((String)Parameters[useShockString]);
            useBleed = Boolean.Parse((String)Parameters[useBleedString]);
            useCurse = Boolean.Parse((String)Parameters[useCurseString]);
            useUnique = Boolean.Parse((String)Parameters[useUniqueString]);
            useOffenseAndSpeedrun = Boolean.Parse((String)Parameters[useOffenseAndSpeedrunString]);
        }

        public override bool CreateConfigurationMenu(ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Action Info");
            ImGuiExtension.ToolTip("This action is used to use a specific type(s) of flask.\nFlask Hotkey will be pulled from plugin settings.\nFlask types will be pulled from the file /config/flaskinfo.json");

            useLife = ImGuiExtension.Checkbox("Life", useLife);
            Parameters[useLifeString] = useLife.ToString();

            ImGui.SameLine();
            useMana = ImGuiExtension.Checkbox("Mana", useMana);
            Parameters[useManaString] = useMana.ToString();

            ImGui.SameLine();
            useHybrid = ImGuiExtension.Checkbox("Hybrid", useHybrid);
            Parameters[useHybridString] = useHybrid.ToString();

            useInstant = ImGuiExtension.Checkbox("Use Instant", useInstant);
            ImGuiExtension.ToolTip("This only makes sense to use with life/mana/hybrid flasks");

            Parameters[useInstantString] = useInstant.ToString();

            useDefense = ImGuiExtension.Checkbox("Defense", useDefense);
            Parameters[useDefenseString] = useDefense.ToString();


            ImGui.SameLine();
            useOffense = ImGuiExtension.Checkbox("Offense", useOffense);
            Parameters[useOffenseString] = useOffense.ToString();

            useUtility = ImGuiExtension.Checkbox("Utility", useUtility);
            Parameters[useUtilityString] = useUtility.ToString();

            ImGui.SameLine();
            useSpeedrun = ImGuiExtension.Checkbox("Speedrun", useSpeedrun);
            Parameters[useSpeedrunString] = useSpeedrun.ToString();


            usePoison = ImGuiExtension.Checkbox("Poison", usePoison);
            Parameters[usePoisonString] = usePoison.ToString();

            ImGui.SameLine();
            useFreeze = ImGuiExtension.Checkbox("Freeze", useFreeze);
            Parameters[useFreezeString] = useFreeze.ToString();

            useIgnite = ImGuiExtension.Checkbox("Ignite", useIgnite);
            Parameters[useIgniteString] = useIgnite.ToString();

            ImGui.SameLine();
            useShock = ImGuiExtension.Checkbox("Shock", useShock);
            Parameters[useShockString] = useShock.ToString();

            useBleed = ImGuiExtension.Checkbox("Bleed", useBleed);
            Parameters[useBleedString] = useBleed.ToString();

            ImGui.SameLine();
            useCurse = ImGuiExtension.Checkbox("Curse", useCurse);
            Parameters[useCurseString] = useCurse.ToString();

            useUnique = ImGuiExtension.Checkbox("Unique", useUnique);
            Parameters[useUniqueString] = useUnique.ToString();

            useOffenseAndSpeedrun = ImGuiExtension.Checkbox("Offense and Speedrun", useOffenseAndSpeedrun);
            Parameters[useOffenseAndSpeedrunString] = useOffenseAndSpeedrun.ToString();
            return true;
        }

        public override Composite GetComposite(ExtensionParameter extensionParameter)
        {
            List<FlaskActions> actions = new List<FlaskActions>();
            if (useLife) actions.Add(FlaskActions.Life);
            if (useMana) actions.Add(FlaskActions.Mana);
            if (useHybrid) actions.Add(FlaskActions.Hybrid);
            if (useDefense) actions.Add(FlaskActions.Defense);
            if (useUtility) actions.Add(FlaskActions.Utility);
            if (useSpeedrun) actions.Add(FlaskActions.Speedrun);
            if (useOffense) actions.Add(FlaskActions.Offense);
            if (usePoison) actions.Add(FlaskActions.PoisonImmune);
            if (useFreeze) actions.Add(FlaskActions.FreezeImmune);
            if (useIgnite) actions.Add(FlaskActions.IgniteImmune);
            if (useShock) actions.Add(FlaskActions.ShockImmune);
            if (useBleed) actions.Add(FlaskActions.BleedImmune);
            if (useCurse) actions.Add(FlaskActions.CurseImmune);
            if (useUnique) actions.Add(FlaskActions.UniqueFlask);
            if (useOffenseAndSpeedrun) actions.Add(FlaskActions.OFFENSE_AND_SPEEDRUN);

            if(actions.Count == 0)
            {
                extensionParameter.Plugin.Log("No actions selected.", 5);
                return new TreeSharp.Action();
            }

            return createUseFlaskAction(extensionParameter, actions, useInstant, null);
        }

        private Composite createUseFlaskAction(ExtensionParameter extensionParameter, List<FlaskActions> flaskActions, Boolean instant, Func<List<FlaskActions>> ignoreFlasksWithAction = null)
        {
            return new UseHotkeyAction(extensionParameter.Plugin.KeyboardHelper, x =>
            {
                extensionParameter.Plugin.Log("Searching for flask.", 5);

                var foundFlask = findFlaskMatchingAnyAction(extensionParameter, flaskActions, instant, ignoreFlasksWithAction);

                if (foundFlask == null)
                {
                    extensionParameter.Plugin.Log("No flask found.", 5);
                    return null;
                }

                extensionParameter.Plugin.Log("Found a flask! Index: " + foundFlask.Index, 5);


                return extensionParameter.Settings.FlaskSettings[foundFlask.Index].Hotkey;
            });
        }

        private PlayerFlask findFlaskMatchingAnyAction(ExtensionParameter extensionParameter, List<FlaskActions> flaskActions, Boolean? instant = null, Func<List<FlaskActions>> ignoreFlasksWithAction = null)
        {
            var allFlasks = extensionParameter.Plugin.FlaskHelper.getAllFlaskInfo();

            // We have no flasks or settings for flasks?
            if (allFlasks == null || extensionParameter.Settings.FlaskSettings == null)
            {
                extensionParameter.Plugin.Log("No flasks or no settings.", 5);
                return null;
            }


            List<FlaskActions> ignoreFlaskActions = ignoreFlasksWithAction == null ? null : ignoreFlasksWithAction();

            var flaskList = allFlasks
                    .Where(x =>
                    // Below are cheap operations and should be done first
                    (instant == null || instant.GetValueOrDefault() == x.Instant) // Only search for flasks matching the requested instant value
                    && (flaskActions.Contains(x.Action1) || flaskActions.Contains(x.Action2)) // Find any flask that matches the actions sent in
                    && (ignoreFlaskActions == null || !ignoreFlasksWithAction().Contains(x.Action1) && !ignoreFlasksWithAction().Contains(x.Action2)) // Do not choose ignored flask types
                    && extensionParameter.Plugin.FlaskHelper.canUsePotion(x) // Do not return flasks we can't use
                    // Below are more expensive operations and should be done last
                    && (x.Instant || (!extensionParameter.Plugin.PlayerHelper.playerHasBuffs(new List<string> { x.BuffString1 }) || !extensionParameter.Plugin.PlayerHelper.playerHasBuffs(new List<string> { x.BuffString2 }))) // If the flask is not instant, ensure we are missing at least one of the flask buffs
                    ).OrderByDescending(x => x.TotalUses).ToList();


            if (flaskList == null || !flaskList.Any())
            {
                extensionParameter.Plugin.Log("Flask list was empty", 5);
                return null;
            }

            return flaskList.FirstOrDefault();
        }
    }
}
