using TreeRoutine.Routine.BuildYourOwnRoutine.Profile;
using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using System.Windows.Forms;
using TreeRoutine.Routine.BuildYourOwnRoutine.Flask;

namespace TreeRoutine.Routine.BuildYourOwnRoutine
{
    public class BuildYourOwnRoutineSettings : BaseTreeSettings
    {
        public BuildYourOwnRoutineSettings() : base()
        {

        }

        public FlaskSetting[] FlaskSettings { get; internal set; } = new FlaskSetting[5]
        {
            new FlaskSetting(new HotkeyNode(Keys.D1)),
            new FlaskSetting(new HotkeyNode(Keys.D2)),
            new FlaskSetting(new HotkeyNode(Keys.D3)),
            new FlaskSetting(new HotkeyNode(Keys.D4)),
            new FlaskSetting(new HotkeyNode(Keys.D5))
        };

        public RangeNode<int> FramesBetweenTicks { get; internal set; } = new RangeNode<int>(5, 1, 60);

        public LoadedProfile LoadedProfile { get; set; }
    }
}