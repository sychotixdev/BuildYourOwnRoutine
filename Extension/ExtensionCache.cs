using TreeRoutine.Routine.BuildYourOwnRoutine.Extension.DefaultExtension.Actions;
using TreeRoutine.Routine.BuildYourOwnRoutine.Extension.DefaultExtension.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension
{
    public class ExtensionCache
    {
        public ExtensionCache()
        {
            LoadedExtensions = new List<Extension>();
            ConditionList = new List<ExtensionConditionFactory>();
            ActionList = new List<ExtensionActionFactory>();
            CustomExtensionCache = new Dictionary<string, Dictionary<string, object>>();
            ActionFilterList = new HashSet<string>();
            ConditionFilterList = new HashSet<string>();

            loadDefaultActions();
            loadDefaultConditions();
        }

        private void loadDefaultActions()
        {
            ActionList.Add(new UseFlaskActionFactory());
            ActionList.Add(new SendKeyActionFactory());
        }

        private void loadDefaultConditions()
        {
            ConditionList.Add(new ManaPercentConditionFactory());
            ConditionList.Add(new HealthPercentConditionFactory());
            ConditionList.Add(new EnergyShieldPercentConditionFactory());
            ConditionList.Add(new CanUseFlaskConditionFactory());
            ConditionList.Add(new InHideoutConditionFactory());
            ConditionList.Add(new HasCurableAilmentConditionFactory());
        }

        public List<Extension> LoadedExtensions { get; set; }

        public List<ExtensionConditionFactory> ConditionList { get; set; }
        public HashSet<string> ConditionFilterList { get; set; }

        public List<ExtensionActionFactory> ActionList { get; set; }
        public HashSet<string> ActionFilterList { get; set; }

        public Dictionary<string, Dictionary<string, object>> CustomExtensionCache { get; }
    }
}
