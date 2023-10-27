namespace PawCraft
{
    using PawCraft.ToolsApi;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Tool property dialog
    /// </summary>
    public partial class ToolPropertyDialog : ContainedForm
    {
        /// <summary>
        /// Height of the line in pixels
        /// </summary>
        private const int LineSize = 26;

        /// <summary>
        /// Current tool instance
        /// </summary>
        private readonly ToolBase toolInstance;

        /// <summary>
        /// Currently active tool dialog
        /// </summary>
        private IToolDialog activeToolDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolPropertyDialog"/> class
        /// </summary>
        public ToolPropertyDialog(ToolBase tool)
        {
            this.toolInstance = tool;
            this.InitializeComponent();
        }

        /// <summary>
        /// Dialog has been closed
        /// </summary>
        /// <param name="sender">Dialog form</param>
        /// <param name="e">Empty event</param>
        private void DialogClosed(object sender, FormClosingEventArgs e)
        {
            if (this.activeToolDialog != null)
            {
                this.activeToolDialog.OnClose();
            }
        }

        /// <summary>
        /// Window is loaded
        /// </summary>
        /// <param name="sender">Dialog form</param>
        /// <param name="e">Empty event</param>
        private void DialogLoad(object sender, EventArgs e)
        {
            ToolDialogAttribute propertiesPanel = this.toolInstance?.GetType().GetCustomAttribute<ToolDialogAttribute>();

            if (propertiesPanel != null)
            {
                UserControl dialog = Activator.CreateInstance(propertiesPanel.Type, new[] { this.toolInstance }) as UserControl;
                this.Controls.Add(dialog);
                this.ClientSize = dialog.Size;
                dialog.Dock = DockStyle.Fill;
                this.activeToolDialog = dialog as IToolDialog;
            }
            else if (this.toolInstance != null)
            {
                int line = 0;

                foreach (PropertyInfo property in this.toolInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    DisplayNameAttribute nameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                    string name = nameAttribute?.DisplayName ?? property.Name;

                    Label label = new Label { Text = name, Location = new Point(5, 8 + (line * ToolPropertyDialog.LineSize)) };
                    this.Controls.Add(label);

                    if (property.PropertyType == typeof(int))
                    {
                        RangeAttribute rangeAttribute = property.GetCustomAttribute<RangeAttribute>();
                        Control created;

                        if (rangeAttribute != null)
                        {
                            NumericUpDown numericUpDown = new NumericUpDown { Location = new Point(150, 5 + (line * ToolPropertyDialog.LineSize)) };
                            numericUpDown.Minimum = (int)rangeAttribute.Minimum;
                            numericUpDown.Maximum = (int)rangeAttribute.Maximum;
                            numericUpDown.Value = (int)property.GetValue(this.toolInstance, null);
                            numericUpDown.ValueChanged += (control, ev) => property.SetValue(this.toolInstance, (int)numericUpDown.Value);
                            created = numericUpDown;
                        }
                        else
                        {
                            TextBox textBox = new TextBox { Location = new Point(150, 5 + (line * ToolPropertyDialog.LineSize)) };
                            textBox.Text = ((int)property.GetValue(this.toolInstance, null)).ToString(CultureInfo.InvariantCulture);
                            textBox.TextChanged += (control, ev) =>
                            {
                                if (int.TryParse(textBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                                {
                                    property.SetValue(this.toolInstance, value);
                                }
                            };

                            created = textBox;
                        }

                        this.Controls.Add(created);
                    }

                    line++;
                }
            }
        }

        /// <summary>
        /// Dialog has been shown
        /// </summary>
        /// <param name="sender">Dialog form</param>
        /// <param name="e">Empty event</param>
        private void DialogShown(object sender, EventArgs e)
        {
            if (this.activeToolDialog != null)
            {
                this.activeToolDialog.OnShown();
            }
        }
    }
}