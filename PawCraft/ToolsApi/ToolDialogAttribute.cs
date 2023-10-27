namespace PawCraft.ToolsApi
{
    using System;

    /// <summary>
    /// Tool dialog control type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ToolDialogAttribute : Attribute
    {
        /// <summary>
        /// Tool dialog control type
        /// </summary>
        private Type type;

        /// <summary>
        /// Gets or sets tool dialog control type
        /// </summary>
        public Type Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
            }
        }
    }
}