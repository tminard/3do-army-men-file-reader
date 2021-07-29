using AMMEdit.amm;
using AMMEdit.amm.blocks.subfields.units;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static AMMEdit.amm.FractionUnit;

namespace AMMEdit.PropertyEditors.dialogs
{
    public partial class AddUnit : Form
    {
        private Fraction fraction { get; }
        public FractionUnit NewUnit { get; private set; }

        public List<ClassUnitType> UnitTypeSourceData = new List<ClassUnitType> {
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Rifleman, "Rifle", "Unit - Rifleman Squad"),
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Bazooka, "Bazooka", "Unit - Bazooka Squad"),
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Engineer, "Engineer", "Unit - Engineer Squad"),
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Flamer, "Flamer", "Unit - Flamer Squad"),
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Grenader, "Grenader", "Unit - Grenader Squad"),
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Miner, "Miner", "Unit - Miner Squad"),
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Morter, "Morter", "Unit - Morter Squad"),
            new ClassUnitType(UnitClass.GroundUnit, (byte)GenericFractionUnit.UnitType.Special1, "Special", "Unit - Special Unit Squad"),

            new ClassUnitType(UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.Jeep, "Jeep", "Vehicle - Jeep"),
            new ClassUnitType(UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.Tank, "Tank", "Vehicle - Tank"),
            new ClassUnitType(UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.HalfTrack, "Halftrack", "Vehicle - Halftrack"),
            new ClassUnitType(UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.TransportTruck, "Truck", "Vehicle - Truck"),
        };

        public AddUnit(Fraction fraction)
        {
            this.fraction = fraction;

            InitializeComponent();
        }

        private void AddUnit_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("Add Unit - {0}", this.fraction.Name);

            if (fraction.GetUnitCountByType((byte)UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.Sarge) == 0)
            {
                UnitTypeSourceData.Add(new ClassUnitType(UnitClass.VehicleUnit, (byte)VehicleFractionUnit.VehicleUnitType.Sarge, "Sarge", "Other - Sarge"));
            }

            inputTypeClass.DataSource = UnitTypeSourceData;
            inputTypeClass.DisplayMember = "LabelText";
        }

        private void inputTypeClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassUnitType selectedClass = (ClassUnitType)((ComboBox)sender).SelectedItem;
            UnitClass c = selectedClass.UnitClass;

            switch (c)
            {
                case UnitClass.VehicleUnit:
                    NewUnit = new VehicleFractionUnit(selectedClass.UnitType, 0, 0, 0, false, 0, GetNextUnitName(c, selectedClass.UnitType, selectedClass.UnitTypeName));
                    break;
                default:
                    NewUnit = new GenericFractionUnit(selectedClass.UnitType, 0, 0, 0, false, 1, GetNextUnitName(c, selectedClass.UnitType, selectedClass.UnitTypeName));
                    break;
            }

            propertyGridValues.SelectedObject = NewUnit;
        }

        private string GetNextUnitName(UnitClass unitClass, byte type, string typeString)
        {
            int numUnits = fraction.Units.FindAll(u => u.UnitClassField == unitClass && u.UnitTypeID == type).Count;

            return string.Format("U{0}{1}{2}", fraction.Name, typeString, numUnits + 1);
        }
    }
    public class ClassUnitType
    {
        public ClassUnitType(FractionUnit.UnitClass unitClass, byte unitType, string unitTypeName, string labelText)
        {
            UnitClass = unitClass;
            UnitType = unitType;
            UnitTypeName = unitTypeName;
            LabelText = labelText;
        }

        public FractionUnit.UnitClass UnitClass { get; }
        public byte UnitType { get; }

        public string LabelText { get; }

        public string UnitTypeName { get; }
    }
}
