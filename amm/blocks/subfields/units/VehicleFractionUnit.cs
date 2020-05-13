using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm.blocks.subfields.units
{
    public class VehicleFractionUnit : FractionUnit
    {
        public enum VehicleUnitType : byte
        {
            Jeep = 1,
            Tank = 2,
            HalfTrack = 3,
            TransportTruck = 4,
            Sarge = 5
        }

        [Category("Classification"), Description("The vehicle type. Note that Sarge is considered a Vehicle by the game.")]
        public override string UnitTypeLabel { get { return Enum.GetName(typeof(VehicleUnitType), UnitTypeID); } }

        public VehicleFractionUnit(BinaryReader r) : base(r)
        {
            initClassifications();
        }

        public VehicleFractionUnit(byte unitTypeID, int startPosX, int startPosY, byte rotation, bool autoDeployed, byte numMenInUnit, string unitName) : base(unitTypeID, (byte)UnitClass.VehicleUnit, startPosX, startPosY, rotation, autoDeployed, numMenInUnit, unitName)
        {
            initClassifications();
        }

        private void initClassifications()
        {
            if (UnitTypeClass != (byte)GenericFractionUnit.UnitClass.VehicleUnit)
            {
                throw new ArgumentException(string.Format("Given unit is not a vehicle. Expected {0}, got {1}", UnitClass.VehicleUnit.ToString(), UnitTypeClass.ToString()));
            }
        }
    }
}
