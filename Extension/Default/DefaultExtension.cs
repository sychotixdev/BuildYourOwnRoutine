using PoeHUD.Poe.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Actions;
using TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Conditions;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default
{
    internal class DefaultExtension : Extension
    {
        public const string CacheStartedMoving = "StartedMoving";
        private Stopwatch MovingStopwatch { get; set; } = new Stopwatch();


        public DefaultExtension()
        {
            this.Name = "Default";
        }

        public override List<ExtensionActionFactory> GetActions()
        {
            List<ExtensionActionFactory> list = new List<ExtensionActionFactory>();

            list.Add(new UseFlaskActionFactory(Name));
            list.Add(new SendKeyActionFactory(Name));

            return list;
        }

        public override List<ExtensionConditionFactory> GetConditions()
        {
            List<ExtensionConditionFactory> list = new List<ExtensionConditionFactory>();

            list.Add(new ManaPercentConditionFactory(Name));
            list.Add(new HealthPercentConditionFactory(Name));
            list.Add(new EnergyShieldPercentConditionFactory(Name));
            list.Add(new CanUseFlaskConditionFactory(Name));
            list.Add(new InHideoutConditionFactory(Name));
            list.Add(new HasCurableAilmentConditionFactory(Name));

            return list;
        }

        public override void UpdateCache(ExtensionParameter extensionParameter, Dictionary<string, Dictionary<string, object>> cache)
        {
            // First, get the dictionary out
            Dictionary<string, object> myCache;
            if (!cache.TryGetValue(Name, out myCache))
            {
                myCache = new Dictionary<string, object>();
                cache.Add(Name, myCache);
            }
            cache.Add(CacheStartedMoving, myCache);


            // Add cache values

            long elapsedMovingTime = 0;
            var player = extensionParameter.Plugin.GameController.Player.GetComponent<Actor>();
            if (player != null && player.Address != 0 && player.isMoving)
            {
                if (!MovingStopwatch.IsRunning)
                    MovingStopwatch.Start();
                elapsedMovingTime = MovingStopwatch.ElapsedMilliseconds;
            }
            else
            {
                MovingStopwatch.Stop();
            }

            myCache.Add(CacheStartedMoving, elapsedMovingTime);


        }

    }
}
