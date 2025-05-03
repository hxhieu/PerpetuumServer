using Newtonsoft.Json;
using System.ComponentModel;

namespace Perpetuum
{
    public class RatesConfiguration
    {
        public int Ep { get; set; } = 1;
        public int Credit { get; set; } = 1;
        public int PlayerDamage { get; set; } = 1;
        public int MissionReward { get; set; } = 1;
        public int Loot { get; set; } = 1;
        public int Capacity { get; set; } = 1;
    }

    public class GlobalConfiguration
    {
        public static GlobalConfiguration Instance { get; private set; }

        public string ListenerIP { get; set; }
        public int ListenerPort { get; set; }

        public string GameRoot { get; set; }
        public string WebServiceIP { get; set; }
        public string PersonalConfig { get; set; }
        public string ConnectionString { get; set; }
        public string RelayName => "relay";

        public bool EnableUpnp { get; set; }

        public int SteamAppID { get; set; }
        public byte[] SteamKey { get; set; }

        public string ResourceServerURL { get; set; }

        public bool EnableDev { get; set; }

        public CorporationConfiguration Corporation { get; set; }

        public RatesConfiguration Rates { get; set; }

        public bool StartServerInAdminOnlyMode { get; set; }

        // Default NIC value for new player.
        [DefaultValue(500000), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int StartCredit { get; set; }

        // Default NIC per level value for new player.
        [DefaultValue(125000), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int LevelCredit { get; set; }

        // Default EP value for new player.
        [DefaultValue(40000), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int StartEP { get; set; }

        // Default camouflage bonus value.
        [DefaultValue(5), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int CamouflageBonus { get; set; }

        public GlobalConfiguration() { }
        public GlobalConfiguration(string settingJson)
        {
            Instance = JsonConvert.DeserializeObject<GlobalConfiguration>(settingJson);
        }
    }
}
