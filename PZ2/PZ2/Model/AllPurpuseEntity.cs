using System;
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
        private PowerEntity entity; // moze ovaj tip da bude zbog nasledjivanje
        private EntityType typeE;
        private Brush colorBrush;
        private LineEntity lineEntity;

        public AllPurpuseEntity(PowerEntity entity, EntityType typeE, Brush colorBrush)
        {
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
    }
}
