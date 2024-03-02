namespace PawCraft.Entities
{
    using PawCraft.Level;
    using PawCraft.Rendering;
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Model properties
    /// </summary>
    public class ModelProperties : BaseEntityProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProperties"/> class
        /// </summary>
        /// <param name="entity">Entity data</param>
        public ModelProperties(EntityData entity) : base(entity)
        {
            this.HasCollisions = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether model entity can be collided with
        /// </summary>
        [Category("Properties")]
        [Description("Can be collided with")]
        public bool HasCollisions
        {
            get
            {
                return this.Entity.Reserved[0] != 0;
            }

            set
            {
                this.Entity.Reserved[0] = (byte)(value ? 1 : 0);
            }
        }

        /// <summary>
        /// Gets or sets model file
        /// </summary>
        [Browsable(true)]
        [TypeConverter(typeof(ModelSelector))]
        [Category("Properties")]
        [Description("Name of the model to use")]
        public string Model
        {
            get
            {
                return EntityModelHandler.GetModelName(this.Entity.Reserved[1]);
            }

            set
            {
                int index = EntityModelHandler.GetNames().ToList().IndexOf(value);
                this.Entity.Reserved[1] = (byte)Math.Max(Math.Min(index, byte.MaxValue), 0);
            }
        }

        /// <summary>
        /// Model selector
        /// </summary>
        public class ModelSelector : StringConverter
        {
            /// <summary>
            /// Gets combobox values
            /// </summary>
            /// <param name="context">Type context</param>
            /// <returns>Combobox values</returns>
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(EntityModelHandler.GetNames().ToArray());
            }

            /// <summary>
            /// Combobox is of a list type
            /// </summary>
            /// <param name="context">Type context</param>
            /// <returns>True to show combobox as list</returns>
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                // True will limit to list. false will show the list, but allow free-form entry
                return true;
            }

            /// <summary>
            /// Show combobox
            /// </summary>
            /// <param name="context">Type context</param>
            /// <returns>True to show combobox</returns>
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                // True means show a combobox
                return true;
            }
        }
    }
}