using TreeRoutine.DefaultBehaviors.Actions;
using TreeRoutine.DefaultBehaviors.Helpers;
using TreeRoutine.FlaskComponents;
using PoeHUD.Poe.Components;
using System;
using System.Collections.Generic;
using TreeSharp;
using PoeHUD.Models.Enums;
using System.Linq;
using TreeRoutine.Menu;
using ImGuiNET;
using PoeHUD.Framework;
using PoeHUD.Framework.Helpers;
using TreeRoutine.Routine.BuildYourOwnRoutine.UI;
using TreeRoutine.Routine.BuildYourOwnRoutine.Extension;
using TreeRoutine.Routine.BuildYourOwnRoutine.Profile;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using TreeRoutine.Routine.BuildYourOwnRoutine.UI.MenuItem;
using TreeRoutine.Routine.BuildYourOwnRoutine.Flask;

namespace TreeRoutine.Routine.BuildYourOwnRoutine
{
    public class BuildYourOwnRoutineCore : BaseTreeRoutinePlugin<BuildYourOwnRoutineSettings, BaseTreeCache>
    {
        public BuildYourOwnRoutineCore() : base()
        {

        }

        public string ProfileDirectory { get; set; }
        public string ExtensionDirectory { get; set; }
        public ExtensionCache ExtensionCache { get; set; }

        public Composite Tree { get; set; }
        private Coroutine TreeCoroutine { get; set; }

        public KeyboardHelper KeyboardHelper { get; set; } = null;

        private ConfigurationMenu ConfigurationMenu { get; set; }

        public override void Initialise()
        {
            base.Initialise();

            PluginName = "BuildYourOwnRoutineCore";
            KeyboardHelper = new KeyboardHelper(GameController);
            ExtensionCache = new ExtensionCache();

            ProfileDirectory = PluginDirectory + @"/Profile/";
            ExtensionDirectory = PluginDirectory + @"/Extension/";
            ExtensionLoader.LoadAllExtensions(ExtensionCache, ExtensionDirectory);
            ProcessLoadedExtensions();

            ConfigurationMenu = new ConfigurationMenu(this);

            CreateAndStartTreeFromLoadedProfile();

            Settings.FramesBetweenTicks.OnValueChanged += UpdateCoroutineWaitRender;
        }

        private void ProcessLoadedExtensions()
        {
            // For every loaded extension, add its a
            foreach (var extension in ExtensionCache.LoadedExtensions)
            {
                // Load the extension actions
                var allActions = extension.GetActions();
                if (allActions != null && allActions.Count() > 0)
                {
                    ExtensionCache.ActionList.AddRange(allActions);
                }
                // Load th extension conditions
                var allConditions = extension.GetConditions();
                if (allConditions != null && allConditions.Count() > 0)
                {
                    ExtensionCache.ConditionList.AddRange(allConditions);
                }
            }


            // We need to initialize the filter list.
            ExtensionCache.ActionFilterList.Add(ExtensionComponentFilterType.None);
            ExtensionCache.ActionList.ForEach(factory => factory.GetFilterTypes().ForEach(x => ExtensionCache.ActionFilterList.Add(x)));

            ExtensionCache.ConditionFilterList.Add(ExtensionComponentFilterType.None);
            ExtensionCache.ConditionList.ForEach(factory => factory.GetFilterTypes().ForEach(x => ExtensionCache.ConditionFilterList.Add(x)));

        }

        private void UpdateCoroutineWaitRender()
        {
            if (TreeCoroutine != null)
            {
                TreeCoroutine.UpdateCondtion(new WaitRender(Settings.FramesBetweenTicks));
            }
        }

        public bool CreateAndStartTreeFromLoadedProfile()
        {
            if (Settings.LoadedProfile == null)
                return false;

            if (Settings.LoadedProfile.Composite == null)
            {
                LogMessage("Profile " + Settings.LoadedProfile.Name + " was loaded, but it had no composite.", 5);
                return true;
            }

            if (TreeCoroutine != null)
                TreeCoroutine.Done(true);
            var extensionParameter = new ExtensionParameter(this, Settings);
            Tree = new ProfileTreeBuilder(ExtensionCache, extensionParameter).BuildTreeFromTriggerComposite(Settings.LoadedProfile.Composite);

            // Append the cache action to the built tree
            Tree = new Sequence(
                    new TreeSharp.Action(x => ExtensionCache.LoadedExtensions.ForEach(ext => ext.UpdateCache(extensionParameter, ExtensionCache.Cache))),
                    Tree);

            // Add this as a coroutine for this plugin
            TreeCoroutine = (new Coroutine(() => TickTree(Tree)
            , new WaitRender(Settings.FramesBetweenTicks), nameof(BuildYourOwnRoutineCore), "Tree"))
                .AutoRestart(GameController.CoroutineRunner).Run();

            LogMessage("Profile " + Settings.LoadedProfile.Name + " was loaded successfully!", 5);

            return true;
        }

        public override void Render()
        {
            base.Render();
            if (!Settings.Enable.Value) return;

        }

        protected override void RunWindow()
        {
            if (Settings.ShowSettings)
            {
                ImGuiExtension.BeginWindow($"{PluginName} Settings", Settings.LastSettingPos.X, Settings.LastSettingPos.Y, Settings.LastSettingSize.X, Settings.LastSettingSize.Y);
                
                ConfigurationMenu.Render();
                ImGui.EndWindow();
                
                //else Settings.ShowSettings = false;
            }
        }
    }
}