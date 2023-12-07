namespace PawCraft.Utils.Serializer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Specify field order
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FieldOrderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldOrderAttribute"/> class
        /// </summary>
        /// <param name="order">Field order</param>
        public FieldOrderAttribute(int order)
        {
            this.Order = order;
        }

        /// <summary>
        /// Gets or sets field order
        /// </summary>
        public int Order { get; set; }
    }
}
