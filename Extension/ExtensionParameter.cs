using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension
{
    public class ExtensionParameter
    {
        public ExtensionParameter(BuildYourOwnRoutineCore plugin, BuildYourOwnRoutineSettings settings)
        {
            this.Plugin = plugin;
            this.Settings = settings;
        }

        /// <summary>
        /// Instance of the plugin that you are running in. This should be used to access game information.
        /// </summary>
        public BuildYourOwnRoutineCore Plugin { get; set; }
        /// <summary>
        /// Settings of the plugin that you are running in. Modifying values should be avoided.
        /// </summary>
        public BuildYourOwnRoutineSettings Settings { get; set; }
    }
}
