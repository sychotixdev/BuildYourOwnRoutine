using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.DefaultExtension.Conditions
{
    internal class ManaPercentConditionFactory : ExtensionConditionFactory
    {
        public ManaPercentConditionFactory()
        {
            Owner = "Default";
            Name = "ManaPercentConditionFactory";
        }

        public override ExtensionCondition GetCondition()
        {
            return new ManaPercentCondition(Owner, Name);
        }

        public override List<string> GetFilterTypes()
        {
            return new List<string>() { ExtensionComponentFilterType.Player };
        }
    }
}
