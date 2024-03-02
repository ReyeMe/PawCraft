namespace PawCraft.Entities
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using PawCraft.Level;
    using PawCraft.Rendering;
    using PawCraft.Utils;

    /// <summary>
    /// Base entity properties
    /// </summary>
    public class BaseEntityProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityProperties"/> class
        /// </summary>
        /// <param name="entity">Entity data</param>
        public BaseEntityProperties(EntityData entity)
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
                return new Point(this.Entity.X, this.Entity.Y);
            }

            set
            {
                this.Entity.X = (ushort)Math.Max(Math.Min(value.X, Level.LevelData.MapDimensionSize - 1), 0);
                this.Entity.Y = (ushort)Math.Max(Math.Min(value.Y, Level.LevelData.MapDimensionSize - 1), 0);
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
                return Math.Round(this.Entity.Direction.FromFixed().FromRadians(), 2);
            }

            set
            {
                this.Entity.Direction = Math.Round(value, 2).ToRadians().ToFixed();
            }
        }

        /// <summary>
        /// Gets entity data
        /// </summary>
        protected EntityData Entity { get; }
    }
}