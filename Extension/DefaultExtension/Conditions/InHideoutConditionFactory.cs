using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.DefaultExtension.Conditions
{
    internal class InHideoutConditionFactory : ExtensionConditionFactory
    {
        public InHideoutConditionFactory()
        {
            Owner = "Default";
            Name = "InHideoutConditionFactory";
        }

        public override ExtensionCondition GetCondition()
        {
            return new SimpleCondition(Owner, Name, x => (() => x.Plugin.Cache.InHideout));
        }

        public override List<string> GetFilterTypes()
        {
            return new List<string>() { ExtensionComponentFilterType.Player };
        }
    }
}
