using ImGuiNET;
using PoeHUD.Models.Enums;
using PoeHUD.Poe.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeRoutine.Menu;
using TreeSharp;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension.Default.Conditions
{
    internal class NearbyMonstersCondition : ExtensionCondition
    {
        private static Dictionary<String, Tuple<PlayerStats, PlayerStats>> resistanceTypes = new Dictionary<String, Tuple<PlayerStats, PlayerStats>>()
        {
            { "Cold", Tuple.Create(PlayerStats.ColdDamageResistancePct, PlayerStats.MaximumColdDamageResistancePct) },
            { "Fire", Tuple.Create(PlayerStats.FireDamageResistancePct, PlayerStats.MaximumFireDamageResistancePct) },
            { "Lightning",Tuple.Create(PlayerStats.LightningDamageResistancePct, PlayerStats.LightningDamageResistancePct) },
            { "Chaos", Tuple.Create(PlayerStats.ChaosDamageResistancePct, PlayerStats.MaximumChaosDamageResistancePct) }
        };

        private int MinimumMonsterCount { get; set; }
        private readonly String MinimumMonsterCountString = "MinimumMonsterCount";

        private float MaxDistance { get; set; }
        private readonly String MaxDistanceString = "MaxDistance";

        private int ColdResistanceThreshold { get; set; }
        private readonly String ColdResistanceThresholdString = "ColdResistanceThreshold";


        private int FireResistanceThreshold { get; set; }
        private readonly String FireResistanceThresholdString = "FireResistanceThreshold";

        private int LightningResistanceThreshold { get; set; }
        private readonly String LightningResistanceThresholdString = "LightningResistanceThreshold";

        private int ChaosResistanceThreshold { get; set; }
        private readonly String ChaosResistanceThresholdString = "ChaosResistanceThreshold";

        public NearbyMonstersCondition(string owner, string name) : base(owner, name)
        {
            MinimumMonsterCount = 0;
            MaxDistance = 0;
            ColdResistanceThreshold = 0;
            FireResistanceThreshold = 0;
            LightningResistanceThreshold = 0;
            ChaosResistanceThreshold = 0;
        }

        public override void Initialise(Dictionary<String, Object> Parameters)
        {
            base.Initialise(Parameters);

            MinimumMonsterCount = Int32.Parse((string)Parameters[MinimumMonsterCountString]);
            MaxDistance = Single.Parse((string)Parameters[MaxDistanceString]);

            ColdResistanceThreshold = Int32.Parse((string)Parameters[ColdResistanceThresholdString]);
            FireResistanceThreshold = Int32.Parse((string)Parameters[FireResistanceThresholdString]);
            LightningResistanceThreshold = Int32.Parse((string)Parameters[LightningResistanceThresholdString]);
            ChaosResistanceThreshold = Int32.Parse((string)Parameters[ChaosResistanceThresholdString]);
        }

        public override bool CreateConfigurationMenu(ref Dictionary<String, Object> Parameters)
        {
            ImGui.TextDisabled("Condition Info");
            ImGuiExtension.ToolTip("This condition will return true if any of the selected player's resistances\nare reduced by more than or equal to the specified amount.\nReduced max resistance modifiers are taken into effect automatically (e.g. -res map mods).");


            base.CreateConfigurationMenu(ref Parameters);

            MinimumMonsterCount = ImGuiExtension.IntSlider("Minimum Monster Count", MinimumMonsterCount, 1, 50);
            Parameters[MinimumMonsterCountString] = MinimumMonsterCount.ToString();

            MaxDistance = ImGuiExtension.FloatSlider("Maximum Distance", MaxDistance, 1.0f, 100.0f);
            Parameters[MaxDistanceString] = MaxDistance.ToString();

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            ColdResistanceThreshold = ImGuiExtension.IntSlider("Cold Resist Above", ColdResistanceThreshold, 0, 75);
            Parameters[ColdResistanceThresholdString] = ColdResistanceThreshold.ToString();

            FireResistanceThreshold = ImGuiExtension.IntSlider("Fire Resist Above", FireResistanceThreshold, 0, 75);
            Parameters[FireResistanceThresholdString] = FireResistanceThreshold.ToString();

            LightningResistanceThreshold = ImGuiExtension.IntSlider("Lightning Resist Above", LightningResistanceThreshold, 0, 75);
            Parameters[LightningResistanceThresholdString] = LightningResistanceThreshold.ToString();

            ChaosResistanceThreshold = ImGuiExtension.IntSlider("Chaos Resist Above", ChaosResistanceThreshold, 0, 75);
            Parameters[ChaosResistanceThresholdString] = ChaosResistanceThreshold.ToString();

            return true;
        }

        public override Func<bool> GetCondition(ExtensionParameter extensionParameter)
        {
            return () =>
            {
                var mobCount = 0;
                var maxDistanceSquare = MaxDistance * MaxDistance;

                var playerPosition = extensionParameter.Plugin.GameController.Player.GetComponent<Positioned>();

                foreach (var monster in extensionParameter.Plugin.LoadedMonsters)
                {
                    if (!monster.IsValid || !monster.IsAlive)
                        continue;

                    var monsterPosition = monster.GetComponent<Positioned>();
                    var xDiff = playerPosition.GridX - monsterPosition.GridX;
                    var yDiff = playerPosition.GridY - monsterPosition.GridY;
                    var monsterDistance = (xDiff * xDiff + yDiff * yDiff);

                    if (monsterDistance <= maxDistanceSquare)
                    {
                        if (ColdResistanceThreshold > 0 || FireResistanceThreshold > 0 || LightningResistanceThreshold > 0 || ChaosResistanceThreshold > 0)
                        {
                            // We care about resists. Only increment IF we are above the threshold
                            var monsterStats = monster.GetComponent<Stats>();
                            if (ColdResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.ColdDamageResistancePct, out int coldRes) && coldRes >= ColdResistanceThreshold)
                            {
                                mobCount++;
                            }
                            else if (FireResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.FireDamageResistancePct, out int fireRes) && fireRes >= FireResistanceThreshold)
                            {
                                mobCount++;
                            }
                            else if (LightningResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.LightningDamageResistancePct, out int lightningRes) && lightningRes >= LightningResistanceThreshold)
                            {
                                mobCount++;
                            }
                            else if (ChaosResistanceThreshold > 0 && monsterStats.StatDictionary.TryGetValue(PlayerStats.ChaosDamageResistancePct, out int chaosRes) && chaosRes >= ChaosResistanceThreshold)
                            {
                                mobCount++;
                            }
                        } else mobCount++;

                    }

                    if (mobCount >= MinimumMonsterCount)
                        return true;
                }

                return false;
            };
        }
    }
}
