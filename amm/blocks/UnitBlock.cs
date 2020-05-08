using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm
{
    class UnitBlock
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
        byte unitTypeID;
        byte unitTypeClass;
        UInt16 startPosX;
        UInt16 startPosY;
        byte rotation; // 0 = North, counter-clockwise to 255 = N 360d. 64 = 90 degrees, = West. Does not apply to sarge.
        bool autoDeployed;
        byte numMenInUnit; // max 9
        string unitName;
    }
}
