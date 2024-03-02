namespace PawCraft
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using PawCraft.Entities;
    using PawCraft.Rendering;

    /// <summary>
    /// Entity editor window
    /// </summary>
    public partial class EntityEditorWindow : ContainedForm
    {
        /// <summary>
        /// Entity data
        /// </summary>
        private readonly Entity entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityEditorWindow"/> class
        /// </summary>
        /// <param name="entity">Entity data</param>
        internal EntityEditorWindow(Entity entity)
        {
            this.entity = entity;
            this.InitializeComponent();
            this.ReloadEntityParameters();
        }

        /// <summary>
        /// Load up editor dialog
        /// </summary>
        /// <param name="sender">This window</param>
        /// <param name="e">Empty event</param>
        private void EntityEditorDialogLoad(object sender, EventArgs e)
        {
            foreach (object option in Enum.GetValues(typeof(Level.EntityData.EntityType)))
            {
                if ((Level.EntityData.EntityType)option != Level.EntityData.EntityType.Empty)
                {
                    DisplayAttribute display = option.GetType().GetField(option.ToString()).GetCustomAttribute<DisplayAttribute>();

                    if (display == null)
                    {
                        this.currentEntity.Items.Add(option.ToString());
                    }
                    else
                    {
                        this.currentEntity.Items.Add(display.Name);
                    }
                }
            }

            this.currentEntity.SelectedIndex = (int)(this.entity.Data.Type - 1);

            this.currentEntity.SelectedIndexChanged += (senderx, ex) =>
            {
                Level.EntityData.EntityType newType = (Level.EntityData.EntityType)(this.currentEntity.SelectedIndex + 1);

                if (newType != this.entity.Data.Type)
                {
                    this.entity.Data.ResetReservedValues();
                }

                this.entity.Data.Type = newType;
                this.ReloadEntityParameters();
            };
        }

        /// <summary>
        /// Reload entity parameters
        /// </summary>
        private void ReloadEntityParameters()
        {
            Entities.EntityPropertyAttribute properties = this.entity.Data.Type.GetType().GetField(this.entity.Data.Type.ToString()).GetCustomAttribute<Entities.EntityPropertyAttribute>();

            if (properties != null)
            {
                this.properties.SelectedObject = Activator.CreateInstance(properties.Dialog, new[] { this.entity.Data });
            }
            else
            {
                this.properties.SelectedObject = new BaseEntityProperties(this.entity.Data);
            }
        }
    }
}