using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.DefaultExtension.Conditions
{
    internal class HasCurableAilmentConditionFactory : ExtensionConditionFactory
    {
        public HasCurableAilmentConditionFactory()
        {
            Owner = "Default";
            Name = "HasCurableAilmentConditionFactory";
        }

        public override ExtensionCondition GetCondition()
        {
            return new HasCurableAilmentCondition(Owner, Name);
        }

        public override List<string> GetFilterTypes()
        {
            return new List<string>() { ExtensionComponentFilterType.Player };
        }
    }
}
