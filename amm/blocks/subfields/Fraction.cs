using AMMEdit.amm.blocks.subfields.units;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using static AMMEdit.amm.blocks.subfields.Scenario;
using static AMMEdit.amm.FractionUnit;

namespace AMMEdit.amm
{
    public class Fraction
    {
        private FractionOrder order;
        UInt32 paddingFieldValue;
        UInt32 numCountFields; // usually 8, sometimes 7
        UInt32 numRifleSquads; // the number of initial rifle squads in the game
        UInt32 numGrenaderSquads;
        UInt32 numFlamerSquads;
        UInt32 numBazookaSquads;
        UInt32 numEngineerSquads;
        UInt32 numMorterSquads;
        UInt32 numMinerSquads; // miners, etc
        UInt32 numSpecial2Squads = 0; // prisoners, etc - can default to 0
        UInt32 unknownPadding2;
        UInt32 unknownPadding3;
        UInt32 numJeepUnits;
        UInt32 numTankUnits;
        UInt32 numHalfTrackUnits;
        UInt32 numUnknownVehicleUnits; // truck?
        UInt32 unknownField1; // no impact on high value. Crash on low. Either padding, or reserved for specific maps. Perhaps for special vehicles (heli?)


        public UInt32 NumStartAirstrikes { get; set; }
        public UInt32 NumStartParas { get; set; }
        public UInt32 NumStartAirSupports { get; set; }

        UInt32 numTotalUnits; // total number of squads in the fraction

        public List<FractionUnit> Units { get; }

        public string Name
        {
            get
            {
                return this.order.ToString();
            }
        }

        public Fraction(FractionOrder order, BinaryReader r)
        {
            this.order = order;

            this.numCountFields = r.ReadUInt32();
            this.paddingFieldValue = r.ReadUInt32();
            this.numRifleSquads = r.ReadUInt32();
            this.numGrenaderSquads = r.ReadUInt32();
            this.numFlamerSquads = r.ReadUInt32();
            this.numBazookaSquads = r.ReadUInt32();
            this.numEngineerSquads = r.ReadUInt32();
            this.numMorterSquads = r.ReadUInt32();
            this.numMinerSquads = r.ReadUInt32();

            if (this.numCountFields == 8)
            {
                this.numSpecial2Squads = r.ReadUInt32();
            }

            this.unknownPadding2 = r.ReadUInt32();
            this.unknownPadding3 = r.ReadUInt32();

            this.numJeepUnits = r.ReadUInt32();
            this.numTankUnits = r.ReadUInt32();
            this.numHalfTrackUnits = r.ReadUInt32();
            this.numUnknownVehicleUnits = r.ReadUInt32();
            this.unknownField1 = r.ReadUInt32();

            this.NumStartAirstrikes = r.ReadUInt32();
            this.NumStartParas = r.ReadUInt32();
            this.NumStartAirSupports = r.ReadUInt32();

            this.numTotalUnits = r.ReadUInt32();

            Units = new List<FractionUnit>(Convert.ToInt32(numTotalUnits));
            for (int u = 0; u < this.numTotalUnits; u++)
            {
                long cp = r.BaseStream.Position;
                r.BaseStream.Seek(1, SeekOrigin.Current);

                byte peekClass = r.ReadByte();
                UnitClass c = (UnitClass)peekClass;

                r.BaseStream.Position = cp;

                switch (c)
                {
                    case UnitClass.VehicleUnit:
                        Units.Add(new VehicleFractionUnit(r));
                        break;
                    default:
                        Units.Add(new GenericFractionUnit(r));
                        break;
                }
            }
        }

        public int GetUnitCountByType(byte unitClass, byte unitType)
        {
            return Units.FindAll(u => u.UnitTypeClass == unitClass && u.UnitTypeID == unitType).Count;
        }

        public void AddUnit(FractionUnit unit)
        {
            Units.Add(unit);

            if (numCountFields == 7 && unit.UnitTypeClass == (byte)UnitClass.GroundUnit && unit.UnitTypeID == (byte)GenericFractionUnit.UnitType.Special1)
            {
                numCountFields = 8;
            }
        }

        public byte[] ToBytes()
        {
            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[1024];

            // fraction header
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numCountFields);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.paddingFieldValue);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Rifleman)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Grenader)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Flamer)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Bazooka)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Engineer)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Morter)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Miner)));
            content.AddRange(buff.Slice(0, 4).ToArray());

            if (numCountFields == 8)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Special1)));
                content.AddRange(buff.Slice(0, 4).ToArray());
            }

            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.unknownPadding2);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.unknownPadding3);
            content.AddRange(buff.Slice(0, 4).ToArray());

            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.Jeep)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.Tank)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.HalfTrack)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, Convert.ToUInt32(GetUnitCountByType((byte)UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.TransportTruck)));
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.unknownField1);

            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.NumStartAirstrikes);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.NumStartParas);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.NumStartAirSupports);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteInt32LittleEndian(buff, this.Units.Count);
            content.AddRange(buff.Slice(0, 4).ToArray());

            // units, reordered to be safe. Note that there is no defined order in the spec.
            List<FractionUnit> genericUnitsOrdered = Units.FindAll(u => u.UnitTypeClass == (byte)UnitClass.GroundUnit);
            genericUnitsOrdered.Sort((a, b) => a.UnitTypeID.CompareTo(b.UnitTypeID));

            List<FractionUnit> vehicleUnitsOrdered = Units.FindAll(u => u.UnitTypeClass == (byte)UnitClass.VehicleUnit);
            vehicleUnitsOrdered.Sort((a, b) => a.UnitTypeID.CompareTo(b.UnitTypeID));

            genericUnitsOrdered.ForEach(u => content.AddRange(u.ToBytes()));
            vehicleUnitsOrdered.ForEach(u => content.AddRange(u.ToBytes()));

            return content.ToArray();
        }
    }
}
