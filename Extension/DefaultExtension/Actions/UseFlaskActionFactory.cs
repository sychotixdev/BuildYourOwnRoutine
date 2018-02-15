using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.DefaultExtension.Actions
{
    internal class UseFlaskActionFactory : ExtensionActionFactory
    {
        public UseFlaskActionFactory()
        {
            Owner = "Default";
            Name = "UseFlaskActionFactory";
        }

        public override ExtensionAction GetAction()
        {
            return new UseFlaskAction(Owner, Name);
        }

        public override List<string> GetFilterTypes()
        {
            return new List<string>() { ExtensionComponentFilterType.Flask };
        }
    }
}
