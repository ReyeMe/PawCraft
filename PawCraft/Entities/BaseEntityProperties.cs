namespace PawCraft.Entities
{
    using PawCraft.Rendering;
    using PawCraft.Utils;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    /// <summary>
    /// Base entity properties
    /// </summary>
    public class BaseEntityProperties
    {
        /// <summary>
        /// Gets entity data
        /// </summary>
        protected Entity Entity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityProperties"/> class
        /// </summary>
        /// <param name="entity">Entity data</param>
        public BaseEntityProperties(Entity entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Gets or sets entity location
        /// </summary>
        [Category("Transformation")]
        [Description("Tile coordinates")]
        public Point Location
        {
            get
            {
                return new Point(this.Entity.Data.X, this.Entity.Data.Y);
            }

            set
            {
                this.Entity.Data.X = (ushort)Math.Max(Math.Min(value.X, Level.LevelData.MapDimensionSize - 1), 0);
                this.Entity.Data.Y = (ushort)Math.Max(Math.Min(value.Y, Level.LevelData.MapDimensionSize - 1), 0);
            }
        }

        /// <summary>
        /// Gets or sets rotation of the entity in radians
        /// </summary>
        [Category("Transformation")]
        [Description("Rotation in degrees")]
        public double Rotation
        {
            get
            {
                return Math.Round(this.Entity.Data.Direction.FromFixed().FromRadians(), 2);
            }

            set
            {
                this.Entity.Data.Direction = Math.Round(value, 2).ToRadians().ToFixed();
            }
        }
    }
}
