using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.DefaultExtension.Actions
{
    internal class SendKeyActionFactory : ExtensionActionFactory
    {
        public SendKeyActionFactory()
        {
            Owner = "Default";
            Name = "SendKeyActionFactory";
        }

        public override ExtensionAction GetAction()
        {
            return new SendKeyAction(Owner, Name);
        }

        public override List<string> GetFilterTypes()
        {
            return new List<string> { ExtensionComponentFilterType.Input };
        }
    }
}
