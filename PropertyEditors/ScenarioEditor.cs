using AMMEdit.amm;
using AMMEdit.amm.blocks;
using AMMEdit.amm.blocks.subfields;
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

            scenarioList.SelectedIndex = 0;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void scenarioList_SelectedIndexChanged(object sender, EventArgs e)
        {
            listGreenFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[0].m_units;
            listGreenFractions.DisplayMember = "unitName";

            listTanFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[1].m_units;
            listTanFractions.DisplayMember = "unitName";

            listBlueFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[2].m_units;
            listBlueFractions.DisplayMember = "unitName";

            listGreyFractions.DataSource = ((Scenario)scenarioList.SelectedItem).m_fractions[3].m_units;
            listGreyFractions.DisplayMember = "unitName";

            textBox1.DataBindings.Clear();
            textBox1.DataBindings.Add("Text", (Scenario)scenarioList.SelectedItem, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void listGreenFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
        }

        private void listTanFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
        }

        private void listBlueFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
        }

        private void listGreyFractions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridFractionUnit.SelectedObject = ((ListBox)sender).SelectedItem;
        }
    }
}
