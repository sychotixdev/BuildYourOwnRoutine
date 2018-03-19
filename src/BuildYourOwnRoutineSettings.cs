using TreeRoutine.Routine.BuildYourOwnRoutine.Profile;
using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using System.Windows.Forms;
using TreeRoutine.Routine.BuildYourOwnRoutine.Flask;
using ImGuiVector2 = System.Numerics.Vector2;
using ImGuiVector4 = System.Numerics.Vector4;

namespace TreeRoutine.Routine.BuildYourOwnRoutine
{
    public class BuildYourOwnRoutineSettings : BaseTreeSettings
    {
        public BuildYourOwnRoutineSettings() : base()
        {
            var centerPos = BasePlugin.API.GameController.Window.GetWindowRectangle().Center;
            LastSettingSize = new ImGuiVector2(620, 376);
            LastSettingPos = new ImGuiVector2(centerPos.X - LastSettingSize.X / 2, centerPos.Y - LastSettingSize.Y / 2);
            ShowSettings = false;
        }

        public FlaskSetting[] FlaskSettings { get; internal set; } = new FlaskSetting[5]
        {
            new FlaskSetting(new HotkeyNode(Keys.D1)),
            new FlaskSetting(new HotkeyNode(Keys.D2)),
            new FlaskSetting(new HotkeyNode(Keys.D3)),
            new FlaskSetting(new HotkeyNode(Keys.D4)),
            new FlaskSetting(new HotkeyNode(Keys.D5))
        };

        public RangeNode<int> TicksPerSecond { get; internal set; } = new RangeNode<int>(10, 1, 30);

        public LoadedProfile LoadedProfile { get; set; }

        public ImGuiVector2 LastSettingPos { get; set; }
        public ImGuiVector2 LastSettingSize { get; set; }

        public ToggleNode ShowSettings { get; set; }
    }
}