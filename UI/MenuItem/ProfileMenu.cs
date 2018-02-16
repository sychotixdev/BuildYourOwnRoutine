using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Routine.BuildYourOwnRoutine.Profile;
using TreeRoutine.Routine.BuildYourOwnRoutine.Extension;
using System.IO;
using TreeRoutine.Menu;
using TreeRoutine.Routine.BuildYourOwnRoutine.Trigger;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.UI.MenuItem
{
    public class ProfileMenu
    {
        public ProfileMenu(BuildYourOwnRoutineCore plugin)
        {
            this.Plugin = plugin;
        }

        private BuildYourOwnRoutineCore Plugin { get; set; }

        private const string TriggerMenuLabel = "TriggerMenu";
        private const string OkMenuLabel = "OkMenu";

        private TriggerMenu NewTriggerMenu = null;
        private int currentlySelectedProfile = -1;
        private string currentFileName = "";

        public void Render()
        {
            ImGuiExtension.BeginWindow("Profile Menu", Plugin.Settings.LastSettingPos.X, Plugin.Settings.LastSettingPos.Y, Plugin.Settings.LastSettingSize.X, Plugin.Settings.LastSettingSize.Y);

            if (ImGui.Button("New profile") || Plugin.Settings.LoadedProfile == null)
            {
                Plugin.Settings.LoadedProfile = new Profile.LoadedProfile();
            }

            ImGui.SameLine();


            if (ImGui.Button("Load profile")) ImGui.OpenPopup("Load profile Menu");
            if (ImGui.BeginPopupModal("Load profile Menu", WindowFlags.AlwaysAutoResize))
            {
                string[] files = Directory.GetFiles(Plugin.ProfileDirectory);

                if (files == null || files.Length == 0)
                {
                    ImGui.Text("No profiles in profile directory.");
                    currentlySelectedProfile = -1;
                }
                else
                {
                    ImGui.Combo("Files", ref currentlySelectedProfile, files.Select(x => Path.GetFileName(x)).ToArray());
                }

                if (currentlySelectedProfile >= 0 && ImGui.Button("Load selected profile"))
                {
                    Profile.LoadedProfile loadedProfile = BuildYourOwnRoutineCore.LoadSettingFile<Profile.LoadedProfile>(files[currentlySelectedProfile]);
                    if (loadedProfile == null || loadedProfile.Name == null)
                    {
                        StartNewOKMenu("Profile did not load properly");
                    }
                    else
                    {
                        Plugin.Settings.LoadedProfile = loadedProfile;
                        Plugin.CreateAndStartTreeFromLoadedProfile();

                        currentlySelectedProfile = -1;
                        ImGui.CloseCurrentPopup();
                    }

                }
                // Render the menu from loading profile
                RenderOkMenu();

                if (ImGui.Button("Cancel Load Profile"))
                {
                    currentlySelectedProfile = -1;
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
            }

            ImGui.SameLine();

            if (ImGui.Button("Save profile")) ImGui.OpenPopup("Save profile Menu");
            if (ImGui.BeginPopupModal("Save profile Menu", WindowFlags.AlwaysAutoResize))
            {
                
                currentFileName = ImGuiExtension.InputText("File Name", currentFileName, 100, InputTextFlags.AlwaysInsertMode);
                if (currentFileName != null && currentFileName.Length > 0 && ImGui.Button("Save profile to file"))
                {
                    BuildYourOwnRoutineCore.SaveSettingFile<Profile.LoadedProfile>(Plugin.ProfileDirectory + currentFileName, Plugin.Settings.LoadedProfile);

                    currentFileName = "";
                    ImGui.CloseCurrentPopup();
                }
                ImGui.SameLine();
                if (ImGui.Button("Cancel save profile"))
                {
                    currentFileName = "";
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }

            if (ImGui.Button("Reload Tree"))
            {
                // Validate current tree
                var root = Plugin.Settings.LoadedProfile.Composite;
                if (root == null)
                {
                    // Warn the user that there is no profile to reload
                    StartNewOKMenu("No profile to reload.");
                }
                else
                {
                    var error = ValidateTree(Plugin.Settings.LoadedProfile.Composite);
                    if (error != null)
                    {
                        StartNewOKMenu(error);
                    }
                    else
                    {
                        // Everything seems good. Load the tree
                        Plugin.CreateAndStartTreeFromLoadedProfile();
                    }
                }
            }

            RenderOkMenu();

            if (ImGui.TreeNodeEx("Profile Tree", TreeNodeFlags.OpenOnArrow | TreeNodeFlags.DefaultOpen))
            {
                if (Plugin.Settings.LoadedProfile.Composite == null)
                {
                    if (ImGui.Button("+"))
                    {
                        ImGui.OpenPopup(TriggerMenuLabel);
                        NewTriggerMenu = new TriggerMenu(Plugin.ExtensionCache, null);
                    }
                    ImGuiExtension.ToolTip("Add root");


                    // If start profile is clicked, trigger menu is rendered
                    RenderTriggerMenu();
                }
                else
                {
                    if (!CreateTreeForComposite(null, Plugin.Settings.LoadedProfile.Composite, 0))
                    {
                        // Was asked to remove the root node.
                        Plugin.Settings.LoadedProfile.Composite = null;
                    }
                }
                ImGui.TreePop();
            }
            
            ImGui.EndWindow();
        }

        private Boolean CreateTreeForComposite(TriggerComposite parent, TriggerComposite composite, int depth)
        {
            String typeTag = "";
            if (composite.Type == TriggerType.Action)
                typeTag = "[A]";
            else if (composite.Type == TriggerType.Decorator)
                typeTag = "[D]";
            else if (composite.Type == TriggerType.PrioritySelector)
                typeTag = "[P]";
            else if (composite.Type == TriggerType.Sequence)
                typeTag = "[S]";

            string label = depth + ":" + typeTag + composite.Name;
            if (ImGui.TreeNodeEx(label, TreeNodeFlags.OpenOnArrow))
            {
                //ImGui.Text(label);

                ImGui.SameLine();

                if (ImGui.Button("X"))
                {
                    // Simply return false to remove the child
                    return false;
                }
                ImGuiExtension.ToolTip("Remove");

                ImGui.SameLine();

                if (ImGui.Button("^"))
                {
                    ImGui.OpenPopup(TriggerMenuLabel);
                    NewTriggerMenu = new TriggerMenu(Plugin.ExtensionCache, parent, composite);
                }

                ImGuiExtension.ToolTip("Edit");

                RenderTriggerMenu();

                if (composite.Type != TriggerType.Action)
                {
                    // Decorators can only have one child
                    if (composite.Type != TriggerType.Decorator || (composite.Children == null || composite.Children.Count == 0))
                    {
                        if (ImGui.Button("+"))
                        {
                            ImGui.OpenPopup(TriggerMenuLabel);
                            NewTriggerMenu = new TriggerMenu(Plugin.ExtensionCache, composite);
                        }
                        ImGuiExtension.ToolTip("Add Child");
                    }

                    if (composite.Children != null && composite.Children.Any())
                    {
                        List<TriggerComposite> childrenToRemove = new List<TriggerComposite>();
                        foreach (var child in composite.Children)
                        {
                            if (!CreateTreeForComposite(composite, child, depth + 1))
                                childrenToRemove.Add(child);
                        }
                        // Remove all children who were requested deletion
                        childrenToRemove.ForEach(x => composite.Children.Remove(x));
                    }
                }
                ImGui.TreePop();
            }

            return true;
        }

        private void RenderTriggerMenu()
        {
            if (ImGui.BeginPopupModal(TriggerMenuLabel, WindowFlags.AlwaysAutoResize))
            {
                if (!NewTriggerMenu.Render())
                {
                    if (NewTriggerMenu.TriggerComposite != null)
                    {
                        // We saved, not canceled.
                        if (NewTriggerMenu.Parent == null)
                        {
                            // We are saving the root
                            Plugin.Settings.LoadedProfile.Composite = NewTriggerMenu.TriggerComposite;
                        }
                        else
                        {
                            // Add the saved trigger to the parent
                            NewTriggerMenu.Parent.Children.Add(NewTriggerMenu.TriggerComposite);
                        }
                    }

                    NewTriggerMenu = null;
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }
        }

        private string ValidateTree(TriggerComposite composite)
        {
            if (composite == null)
            {
                return "No profile loaded";
            }
            if (composite.Type != TriggerType.Action && (composite.Children == null || composite.Children.Count == 0))
            {
                return "Composite must have at least one child. Name: " + composite.Name;
            }

            if (composite.Children != null && composite.Children.Count > 0)
            {
                foreach(var child in composite.Children)
                {
                    string retVal = ValidateTree(child);
                    if (retVal != null)
                        return retVal;
                }
            }

            return null;
        }

        #region OKMenu
        private string OKMessage { get; set; }
        private void StartNewOKMenu(string message)
        {
            OKMessage = message;
            ImGui.OpenPopup(OkMenuLabel);
        }

        private void RenderOkMenu()
        {
            if (ImGui.BeginPopupModal(OkMenuLabel, (WindowFlags)35))
            {
                ImGui.TextDisabled(OKMessage);
                if (ImGui.Button("OK"))
                {
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }
        }
        #endregion

    }
}
