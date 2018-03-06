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
using PoeHUD.Models;
using System.Collections.Concurrent;

namespace TreeRoutine.Routine.BuildYourOwnRoutine
{
    public class BuildYourOwnRoutineCore : BaseTreeRoutinePlugin<BuildYourOwnRoutineSettings, BaseTreeCache>
    {
        public BuildYourOwnRoutineCore() : base()
        {

        }

        public string ProfileDirectory { get; protected set; }
        public string ExtensionDirectory { get; protected set; }
        public ExtensionCache ExtensionCache { get; protected set; }

        public Composite Tree { get; protected set; }
        private Coroutine TreeCoroutine { get; set; }

        public KeyboardHelper KeyboardHelper { get; protected set; } = null;

        private ConfigurationMenu ConfigurationMenu { get; set; }

        public ConcurrentBag<EntityWrapper> LoadedMonsters { get; protected set; } = new ConcurrentBag<EntityWrapper>();

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

            Settings.TicksPerSecond.OnValueChanged += UpdateCoroutineWaitRender;
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
                TreeCoroutine.UpdateCondtion(new WaitTime(1000 / Settings.TicksPerSecond));
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
            var extensionParameter = new ExtensionParameter(this);
            Tree = new ProfileTreeBuilder(ExtensionCache, extensionParameter).BuildTreeFromTriggerComposite(Settings.LoadedProfile.Composite);

            // Append the cache action to the built tree
            Tree = new Sequence(
                    new TreeSharp.Action(x => ExtensionCache.LoadedExtensions.ForEach(ext => ext.UpdateCache(extensionParameter, ExtensionCache.Cache))),
                    Tree);

            // Add this as a coroutine for this plugin
            TreeCoroutine = (new Coroutine(() => TickTree(Tree)
            , new WaitTime(1000 / Settings.TicksPerSecond), nameof(BuildYourOwnRoutineCore), "Tree"))
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

                // Storing window Position and Size changed by the user
                if (ImGui.GetWindowHeight() > 21)
                {
                    Settings.LastSettingPos = ImGui.GetWindowPosition();
                    Settings.LastSettingSize = ImGui.GetWindowSize();
                }

                ImGui.EndWindow();

            }
        }

        public override void EntityAdded(EntityWrapper entityWrapper)
        {
            if (entityWrapper.HasComponent<Monster>() && entityWrapper.IsValid && entityWrapper.IsAlive)
            {
                //This will Cache the Positioned Component.
                var k = entityWrapper.GetComponent<Positioned>();
                var p = entityWrapper.GetComponent<ObjectMagicProperties>();
                LoadedMonsters.Add(entityWrapper);
            }
        }
        public override void EntityRemoved(EntityWrapper entityWrapper)
        {
            if (LoadedMonsters.TryPeek(out entityWrapper))
            {
                if (!LoadedMonsters.TryTake(out entityWrapper))
                {
                    LogError("Failed to remove an entity from the monster cache! Report this error as actually being possible.", 5);
                }
            }
        }
    }
}