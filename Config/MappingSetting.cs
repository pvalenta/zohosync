
namespace ZohoSync.Config
{
    using System.Configuration;

    /// <summary>
    /// mapping setting
    /// </summary>
    public class MappingSetting : ConfigurationSection
    {
        /// <summary>
        /// get setting
        /// </summary>
        [ConfigurationProperty("setting")]
        public MappingCollection Setting
        {
            get
            {
                return this["setting"] as MappingCollection ?? new MappingCollection();
            }
        }

        /// <summary>
        /// get current setting
        /// </summary>
        public static MappingSetting CurrentSetting
        {
            get
            {
                return ConfigurationManager.GetSection("mappingSetting") as MappingSetting ?? new MappingSetting();
            }
        }
    }
}
