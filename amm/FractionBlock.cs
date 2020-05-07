using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMEdit.amm
{
    class FractionBlock
    {
        UInt32 numRifleSquads; // the number of initial rifle squads in the game
        UInt32 numGrenaderSquads;
        UInt32 numFlamerSquads;
        UInt32 numBazookaSquads;
        UInt32 numEngineerSquads;
        UInt32 numMorterSquads;
        UInt32 numSpecial1Squads; // miners, zombies, etc
        UInt32 numSpecial2Squads; // prisoners, etc
        UInt32 numJeepUnits;
        UInt32 numTankUnits;
        UInt32 numHalfTrackUnits;
        UInt32 numUnknownVehicle1Units; // truck?
        UInt32 unknownField1; // no impact on high value. Crash on low. Either padding, or reserved for specific maps. Perhaps for special vehicles (heli?)
        UInt32 numTotalUnits; // total number of squads in the fraction
    }
}
