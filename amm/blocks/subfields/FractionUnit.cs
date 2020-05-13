using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace AMMEdit.amm
{
    public abstract class FractionUnit
    {
        public enum UnitClass : byte
        {
            GroundUnit = 0,
            VehicleUnit = 128
        }

        [Description("Appears to be empty padding. Report if this contains data")]
        public UInt16 Padding { get; }

        [Category("Placement"), Description("The default X position for this unit. Note maps are 256 tiles wide, with each tile being 16 pixels. The stratmap is 6 segments. Each segment is about 42 pixels.")]
        public Int32 StartPosX { get; set; }

        [Category("Placement"), Description("The default Y position for this unit. Note maps are 256 tiles tall, with each tile being 16 pixels. The stratmap is 6 segments. Each segment is about 42 pixels.")]
        public Int32 StartPosY { get; set; }

        [Category("Placement"), Description("The starting rotation for the unit. Starts at 0 = North, and goes counter-clockwise up to the max value of a byte (255) as a complete 360 degrees. I.e. 64 = 90 degrees = West.")]
        public byte Rotation { get; set; } // 0 = North, counter-clockwise to 255 = N 360d. 64 = 90 degrees, = West. Does not apply to sarge.

        [Category("Classification")]
        public byte UnitTypeID { get; }

        [Category("Classification")]
        public byte UnitTypeClass { get; }

        [Category("Classification")]
        public UnitClass UnitClassField { get { return (UnitClass)Convert.ToInt32(UnitTypeClass); } }

        [Category("Deployment"), Description("If false, unit is automatically deployed at the starting position when the Scenario begins. Otherwise, unit must be deployed via DEPLOY in scripts. Exception: For Multi-Player, setting this to True allows user to manually place units. If AI, the unit will be placed at the starting positions.")]
        public bool NotDeployed { get; set; }

        [Category("Deployment"), Description("Number of men in the unit. 0 is the same as 1. No more than 9 is allowed for AM1.")]
        public byte NumMenInUnit { get; set; } // max 9

        [Category("Scripting"), Description("The name of the unit for scripting. The name is used to reference in unit in scripts.")]
        public string UnitName { get; set; }

        abstract public string UnitTypeLabel { get; }

        private string unitNameCString;
        private byte lenName;

        protected FractionUnit(byte unitTypeID, byte unitTypeClass, int startPosX, int startPosY, byte rotation, bool autoDeployed, byte numMenInUnit, string unitName)
        {
            if (unitName.Length + 1 > Byte.MaxValue) { throw new ArgumentException("Unit name cannot exceed byte length"); }
            if (numMenInUnit > 9) { throw new ArgumentException("Num units cannot exceed 9"); };

            this.UnitTypeID = unitTypeID;
            this.UnitTypeClass = unitTypeClass;
            this.Padding = 0;
            this.StartPosX = startPosX;
            this.StartPosY = startPosY;
            this.Rotation = rotation;
            this.NotDeployed = autoDeployed;
            this.NumMenInUnit = numMenInUnit;
            this.UnitName = unitName;
            this.unitNameCString = unitName + "\0";
            this.lenName = Convert.ToByte(unitNameCString.Length);
        }

        protected FractionUnit(BinaryReader r)
        {
            UnitTypeID = r.ReadByte();
            UnitTypeClass = r.ReadByte();
            Padding = r.ReadUInt16();
            StartPosX = r.ReadInt32();
            StartPosY = r.ReadInt32();
            Rotation = r.ReadByte();
            NotDeployed = Convert.ToBoolean(r.ReadByte());
            NumMenInUnit = r.ReadByte();
            lenName = r.ReadByte();
            unitNameCString = new string(r.ReadChars(Convert.ToInt16(this.lenName)));

            UnitName = this.unitNameCString.Substring(0, this.unitNameCString.Length - 1); // get outta here with that nil
        }

        public byte[] ToBytes()
        {
            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[1024];

            content.Add(UnitTypeID);
            content.Add(UnitTypeClass);
            content.AddRange(ShortToBytes(Padding));
            content.AddRange(IntToBytes(StartPosX));
            content.AddRange(IntToBytes(StartPosY));

            content.Add(Rotation);
            content.Add(Convert.ToByte(NotDeployed));
            content.Add(NumMenInUnit);

            unitNameCString = UnitName + "\0";

            content.Add(Convert.ToByte(unitNameCString.Length));
            content.AddRange(ASCIIEncoding.ASCII.GetBytes(unitNameCString.ToArray()));

            return content.ToArray();
        }

        private byte[] ShortToBytes(ushort val)
        {
            Span<byte> buff = stackalloc byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(buff, val);

            return buff.Slice(0, 2).ToArray();
        }

        private byte[] IntToBytes(Int32 val)
        {
            Span<byte> buff = stackalloc byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(buff, val);

            return buff.Slice(0, 4).ToArray();
        }
    }
}