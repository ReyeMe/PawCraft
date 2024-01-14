namespace PawCraft.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Specify what dialog to use for the entity settings
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EntityPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPropertyAttribute"/> class
        /// </summary>
        /// <param name="dialog">Dialog to use</param>
        public EntityPropertyAttribute(Type dialog)
        {
            this.Dialog = dialog;
        }

        /// <summary>
        /// Gets or sets dialog type
        /// </summary>
        public Type Dialog { get; set; }
    }
}
