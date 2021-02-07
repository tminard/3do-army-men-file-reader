using AMMEdit.amm;
using AMMEdit.amm.blocks;
using AMMEdit.amm.blocks.subfields;
using AMMEdit.PropertyEditors.dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit.PropertyEditors
{
    public partial class ScenarioEditor : Form
    {
        private List<Scenario> m_scenarios;

        public ScenarioEditor(List<Scenario> scenarios)
        {
            m_scenarios = scenarios;
            InitializeComponent();
        }

        private void ScenarioEditor_Load(object sender, EventArgs e)
        {
            scenarioList.DataSource = m_scenarios;
            scenarioList.DisplayMember = "Name";
            scenarioList.ValueMember = "FieldID";

            if (scenarioList.Items.Count > 0)
            {
                scenarioList.SelectedIndex = 0;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void scenarioList_SelectedIndexChanged(object sender, EventArgs e)
        {
            listGreenFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[0].Units;
            listGreenFractions.DisplayMember = "unitName";

            listTanFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[1].Units;
            listTanFractions.DisplayMember = "unitName";

            listBlueFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[2].Units;
            listBlueFractions.DisplayMember = "unitName";

            listGrayFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[3].Units;
            listGrayFractions.DisplayMember = "unitName";

            textBox1.DataBindings.Clear();
            textBox1.DataBindings.Add("Text", (Scenario)scenarioList.SelectedItem, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void listGreenFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
            buttonRemoveGreenUnit.Enabled = true;
        }

        private void listTanFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
            buttonRemoveTanUnit.Enabled = true;
        }

        private void listBlueFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
            buttonRemoveBlueUnit.Enabled = true;
        }

        private void listGrayFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
            buttonRemoveGrayUnit.Enabled = true;
        }

        private void AddUnitDialog(int fractionIndex, ListBox fractionList)
        {
            AddUnit addUnitDialog = new AddUnit(((Scenario)scenarioList.SelectedItem).m_fractions[fractionIndex]);

            if (addUnitDialog.ShowDialog(this) == DialogResult.OK)
            {
                // TODO: handle validations
                ((Scenario)scenarioList.SelectedItem).m_fractions[fractionIndex].AddUnit(addUnitDialog.NewUnit);

                fractionList.DataSource = null;
                fractionList.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[fractionIndex].Units;
                fractionList.DisplayMember = "unitName";
            }
        }

        private void RemoveUnitDialog(int fractionIndex, ListBox fractionList, string fractionName)
        {
            int targetIndx = fractionList.SelectedIndex;
            FractionUnit target = (FractionUnit)(fractionList).SelectedItem;

            if (MessageBox.Show(this, string.Format("Are you sure you wish to remove {0} from the {1} fraction? This can break existing scripts. Please make a backup of your map!", target.UnitName, fractionName), "Remove unit", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ((Scenario)scenarioList.SelectedItem).m_fractions[fractionIndex].Units.RemoveAt(targetIndx);

                fractionList.DataSource = null;
                fractionList.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[fractionIndex].Units;
                fractionList.DisplayMember = "unitName";
            }
        }

        private void buttonAddGreenUnit_Click(object sender, EventArgs e)
        {
            AddUnitDialog(0, listGreenFractions);
        }

        private void buttonRemoveGreenUnit_Click(object sender, EventArgs e)
        {
            RemoveUnitDialog(0, listGreenFractions, "Green");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddUnitDialog(1, listTanFractions);
        }

        private void buttonRemoveTanUnit_Click(object sender, EventArgs e)
        {
            RemoveUnitDialog(1, listTanFractions, "Tan");
        }

        private void buttonAddBlueUnit_Click(object sender, EventArgs e)
        {
            AddUnitDialog(2, listBlueFractions);
        }

        private void buttonAddGrayUnit_Click(object sender, EventArgs e)
        {
            AddUnitDialog(3, listGrayFractions);
        }

        private void buttonRemoveBlueUnit_Click(object sender, EventArgs e)
        {
            RemoveUnitDialog(2, listBlueFractions, "Blue");
        }

        private void buttonRemoveGrayUnit_Click(object sender, EventArgs e)
        {
            RemoveUnitDialog(3, listGrayFractions, "Gray");
        }
    }
}
