using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm
{
    public class FractionUnit
    {
        enum GroundUnitType : byte
        {
            Rifle = 0,
            Grenader = 1,
            Flamer = 2,
            Bazooka = 3,
            Engineer = 4,
            Morter = 5,
            Miner = 6,
            Special1 = 7
        }
        enum VehicleUnitType : byte
        {
            Jeep = 1,
            Tank = 2,
            HalfTrack = 3,
            TransportTruck = 4
        }
        enum UnitClass : byte
        {
            GroundUnit = 0,
            VehicleUnit = 80
        }
        private byte unitTypeID;
        private byte unitTypeClass;
        private UInt16 padding1;
        private UInt16 startPosX;
        private UInt16 padding2;
        private UInt16 startPosY;
        private UInt16 padding3;
        private byte rotation; // 0 = North, counter-clockwise to 255 = N 360d. 64 = 90 degrees, = West. Does not apply to sarge.
        private bool autoDeployed;
        private byte numMenInUnit; // max 9
        private byte lenName;
        public string unitName { get; }

        private string unitNameCString;


        public FractionUnit(byte unitTypeID, byte unitTypeClass, ushort startPosX, ushort startPosY, byte rotation, bool autoDeployed, byte numMenInUnit, string unitName)
        {
            if (unitName.Length + 1 > Byte.MaxValue) { throw new ArgumentException("Unit name cannot exceed byte length"); }
            if (numMenInUnit > 9) { throw new ArgumentException("Num units cannot exceed 9"); };

            this.unitTypeID = unitTypeID;
            this.unitTypeClass = unitTypeClass;
            this.padding1 = 0;
            this.startPosX = startPosX;
            this.padding2 = 0;
            this.startPosY = startPosY;
            this.padding3 = 0;
            this.rotation = rotation;
            this.autoDeployed = autoDeployed;
            this.numMenInUnit = numMenInUnit;
            this.unitName = unitName;
            this.unitNameCString = unitName + "\0";
            this.lenName = Convert.ToByte(unitNameCString.Length);
        }

        public FractionUnit(BinaryReader r)
        {
            unitTypeID = r.ReadByte();
            unitTypeClass = r.ReadByte();
            padding1 = r.ReadUInt16();
            startPosX = r.ReadUInt16();
            padding2 = r.ReadUInt16();
            startPosY = r.ReadUInt16();
            padding3 = r.ReadUInt16();
            rotation = r.ReadByte();
            autoDeployed = Convert.ToBoolean(r.ReadByte());
            numMenInUnit = r.ReadByte();
            lenName = r.ReadByte();
            unitNameCString = new string(r.ReadChars(Convert.ToInt16(this.lenName)));

            unitName = this.unitNameCString.Substring(0, this.unitNameCString.Length - 1); // get outta here with that nil
        }

        public byte[] ToBytes()
        {
            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[1024];

            content.Add(unitTypeID);
            content.Add(unitTypeClass);
            content.AddRange(ShortToBytes(padding1));
            content.AddRange(ShortToBytes(startPosX));
            content.AddRange(ShortToBytes(padding2));
            content.AddRange(ShortToBytes(startPosY));
            content.AddRange(ShortToBytes(padding3));

            content.Add(rotation);
            content.Add(Convert.ToByte(autoDeployed));
            content.Add(numMenInUnit);
            content.Add(lenName);
            content.AddRange(ASCIIEncoding.ASCII.GetBytes(unitNameCString.ToArray()));

            return content.ToArray();
        }

        private byte[] ShortToBytes(ushort val)
        {
            Span<byte> buff = stackalloc byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(buff, val);

            return buff.Slice(0, 2).ToArray();
        }
    }
}
