using Perpetuum.Common;
using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Perpetuum.Zones.Terrains.Materials.Plants
{
    public class PlantRuleLoader(SettingsLoader settings, IDbRepository<Plantrule> planRuleRepo)
    {
        private List<Plantrule> CachedPlantRules => planRuleRepo.GetMany(
            // It won't take 10 minutes to load the plant rules, but it's a good idea to cache them for a while
            cacheTime: TimeSpan.FromMinutes(10)
        );

        [NotNull]
        public List<PlantRule> LoadPlantRulesWithOverrides(int ruleSetId, double plantAltitudeScale = 1.0)
        {
            var ruleNames = CachedPlantRules
                .Where(x => x.Rulesetid == ruleSetId)
                .Select(r => r.Plantrule1).ToArray();

            var list = new List<PlantRule>(ruleNames.Length);
            foreach (var ruleName in ruleNames)
            {
                var ruleDictionary = LoadRuleByName(ruleName);

                if (ruleDictionary.ContainsKey(k.allowedAltitudeLow))
                {
                    ruleDictionary[k.allowedAltitudeLow] = (int)((int)ruleDictionary[k.allowedAltitudeLow] * plantAltitudeScale);
                }

                if (ruleDictionary.ContainsKey(k.allowedAltitudeHigh))
                {
                    ruleDictionary[k.allowedAltitudeHigh] = (int)((int)ruleDictionary[k.allowedAltitudeHigh] * plantAltitudeScale);
                }

                if (ruleDictionary.ContainsKey(k.allowedWaterLevelLow))
                {
                    ruleDictionary[k.allowedWaterLevelLow] = (int)((int)ruleDictionary[k.allowedWaterLevelLow] * plantAltitudeScale);
                }

                list.Add(new PlantRule(ruleDictionary));
            }

            return list;
        }

        public IDictionary<string, object> LoadRuleByName(string ruleName)
        {
            //this is what we want to load
            var pathToFile = Path.Combine("plantrules",ruleName);

            //the content
            var settingsFromFile = settings.LoadSettingsFromFile(pathToFile);

            if (!settingsFromFile.ContainsKey(k.source))
                return settingsFromFile;

            //is there override defined?
            var sourceRuleName = (string)settingsFromFile[k.source];

            Logger.Info("overwriting " + ruleName + " -> " + sourceRuleName);

            //load the override
            var sourceRuleFromFile = LoadRuleByName(sourceRuleName);

            //no need for this key in the memory
            settingsFromFile.Remove(k.source);

            foreach (var kvp in settingsFromFile)
            {
                //do override
                sourceRuleFromFile[kvp.Key] = kvp.Value;
            }

            settingsFromFile = sourceRuleFromFile;

            return settingsFromFile;
        }
    }
}