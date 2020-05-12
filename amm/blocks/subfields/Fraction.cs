using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AMMEdit.amm.blocks.subfields.Scenario;

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
        UInt32 numStartAirstrikes;
        UInt32 numStartParas;
        UInt32 numStartAirSupports;
        UInt32 numTotalUnits; // total number of squads in the fraction

        public List<FractionUnit> m_units { get; }

        public string Name { get
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

            this.numStartAirstrikes = r.ReadUInt32();
            this.numStartParas = r.ReadUInt32();
            this.numStartAirSupports = r.ReadUInt32();

            this.numTotalUnits = r.ReadUInt32();

            m_units = new List<FractionUnit>(Convert.ToInt32(numTotalUnits));
            for (int u = 0; u < this.numTotalUnits; u++)
            {
                this.m_units.Add(new FractionUnit(r));
            }
        }

        public byte[] toBytes()
        {
            List<byte> content = new List<byte>();
            Span<byte> buff = stackalloc byte[1024];

            // fraction header
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numCountFields);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.paddingFieldValue);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numRifleSquads);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numGrenaderSquads);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numFlamerSquads);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numBazookaSquads);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numEngineerSquads);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numMorterSquads);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numMinerSquads);
            content.AddRange(buff.Slice(0, 4).ToArray());

            if (numCountFields == 8)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numSpecial2Squads);
                content.AddRange(buff.Slice(0, 4).ToArray());
            }

            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.unknownPadding2);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.unknownPadding3);
            content.AddRange(buff.Slice(0, 4).ToArray());

            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numJeepUnits);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numTankUnits);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numHalfTrackUnits);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numUnknownVehicleUnits);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.unknownField1);

            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numStartAirstrikes);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numStartParas);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteUInt32LittleEndian(buff, this.numStartAirSupports);
            content.AddRange(buff.Slice(0, 4).ToArray());
            BinaryPrimitives.WriteInt32LittleEndian(buff, this.m_units.Count);
            content.AddRange(buff.Slice(0, 4).ToArray());

            // units
            m_units.ForEach(u => content.AddRange(u.ToBytes()));

            return content.ToArray();
        }
    }
}
