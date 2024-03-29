﻿namespace PawCraft.Entities
{
    using System.ComponentModel;
    using PawCraft.Level;

    /// <summary>
    /// Model properties
    /// </summary>
    public class CrateProperties : BaseEntityProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrateProperties"/> class
        /// </summary>
        /// <param name="entity">Entity data</param>
        public CrateProperties(EntityData entity) : base(entity)
        {
            // Do nothing
        }

        /// <summary>
        /// Gets or sets a value indicating whether crate can spawn bombs
        /// </summary>
        [Category("Can spawn")]
        [Description("Bombs")]
        public bool Bombs
        {
            get
            {
                return (this.Entity.Reserved[0] & 0x02) != 0;
            }

            set
            {
                this.Entity.Reserved[0] = (byte)((this.Entity.Reserved[0] & 0xfd) | (value ? 0x02 : 0x00));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether crate can spawn health
        /// </summary>
        [Category("Can spawn")]
        [DisplayName("Health pack")]
        [Description("Health pack")]
        public bool Health
        {
            get
            {
                return (this.Entity.Reserved[0] & 0x01) != 0;
            }

            set
            {
                this.Entity.Reserved[0] = (byte)((this.Entity.Reserved[0] & 0xfe) | (value ? 0x01 : 0x00));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether crate can spawn mines
        /// </summary>
        [Category("Can spawn")]
        [DisplayName("Mines")]
        [Description("Mines player can place later")]
        public bool Mines
        {
            get
            {
                return (this.Entity.Reserved[0] & 0x04) != 0;
            }

            set
            {
                this.Entity.Reserved[0] = (byte)((this.Entity.Reserved[0] & 0xfb) | (value ? 0x04 : 0x00));
            }
        }

        /// <summary>
        /// Gets or sets respawn time in seconds
        /// </summary>
        [Category("Properties")]
        [DisplayName("Respawn time")]
        [Description("Respawn time in seconds")]
        public byte RespawnTime
        {
            get
            {
                return this.Entity.Reserved[1];
            }

            set
            {
                this.Entity.Reserved[1] = value;
            }
        }
    }
}