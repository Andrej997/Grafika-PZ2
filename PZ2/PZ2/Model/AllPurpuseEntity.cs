﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PZ2.Model
{
    public enum EntityType
    {
        Empty = 0, // ako se ne stavlja nista, tj. prazan objekat
        Substation,
        Node,
        Switch,
        Line
    }
    /// <summary>
    /// This entity can hold any entity
    /// </summary>
    public class AllPurpuseEntity
    {
        private long id;
        private int x;
        private int y;
        private PowerEntity entity; // moze ovaj tip da bude zbog nasledjivanje
        private EntityType typeE;
        private Brush colorBrush;
        private LineEntity lineEntity;

        public AllPurpuseEntity(long id, PowerEntity entity, EntityType typeE, Brush colorBrush)
        {
            Id = id;
            Entity = entity;
            TypeE = typeE;
            ColorBrush = colorBrush;
        }

        public AllPurpuseEntity(LineEntity lineEntity, EntityType typeE, Brush colorBrush)
        {
            LineEntity = lineEntity;
            TypeE = typeE;
            ColorBrush = colorBrush;
        }

        /// <summary>
        /// Constuctor for line
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public AllPurpuseEntity(int x, int y)
        {
            X = x;
            Y = y;
            TypeE = EntityType.Line;
            ColorBrush = Brushes.Black;
        }

        /// <summary>
        /// Another constructor for lines
        /// </summary>
        /// <param name="lineEntity"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public AllPurpuseEntity(LineEntity lineEntity, int x, int y)
        {
            LineEntity = lineEntity;
            X = x;
            Y = y;
            TypeE = EntityType.Line;
            ColorBrush = Brushes.Black;
        }

        public PowerEntity Entity
        {
            get => entity;
            set => entity = value;
        }

        public EntityType TypeE
        {
            get => typeE;
            set => typeE = value;
        }

        public Brush ColorBrush
        {
            get => colorBrush;
            set => colorBrush = value;
        }

        public LineEntity LineEntity
        {
            get => lineEntity;
            set => lineEntity = value;
        }
        public long Id { get => id; set => id = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }
}
