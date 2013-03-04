
namespace ZohoSync.Config
{
    using System.Configuration;

    /// <summary>
    /// mapping collection
    /// </summary>
    public class MappingCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// mapping element in collection
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>element in array</returns>
        public Mapping this[int index]
        {
            get
            {
                return base.BaseGet(index) as Mapping;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// create new element
        /// </summary>
        /// <returns>mapping element</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new Mapping();
        }

        /// <summary>
        /// create new element
        /// </summary>
        /// <param name="elementName">element name</param>
        /// <returns>mapping element</returns>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new Mapping();
        }

        /// <summary>
        /// get element key
        /// </summary>
        /// <param name="element">element</param>
        /// <returns>zoho table as key</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Mapping)element).ZohoTable;
        }
    }
}
