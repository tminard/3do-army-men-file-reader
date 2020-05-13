using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMMEdit.amm
{
    public class GenericFractionUnit : FractionUnit
    {
        public enum UnitType : byte
        {
            Rifleman = 0,
            Grenader = 1,
            Flamer = 2,
            Bazooka = 3,
            Engineer = 4,
            Morter = 5,
            Miner = 6,
            Special1 = 7
        }

        [Category("Classification"), Description("The generic unit type. Please report if this is empty")]
        public override string UnitTypeLabel { get { return Enum.GetName(typeof(UnitType), UnitTypeID); } }

        public GenericFractionUnit(BinaryReader r) : base(r)
        {
        }

        public GenericFractionUnit(byte unitTypeID, int startPosX, int startPosY, byte rotation, bool autoDeployed, byte numMenInUnit, string unitName) : base(unitTypeID, (byte)UnitClass.GroundUnit, startPosX, startPosY, rotation, autoDeployed, numMenInUnit, unitName)
        {
        }
    }
}
