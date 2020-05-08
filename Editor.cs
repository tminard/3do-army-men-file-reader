﻿using AMMEdit.amm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMMEdit
{
    public partial class Editor : Form
    {
        private MapFile currentMap;

        public Editor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openAMMFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentAMMFile.Text = openAMMFileDialog.FileName;

                // TODO: load async. It is fast enough for now.
                currentMap = new MapFileLoader(openAMMFileDialog.FileName).read();

                listBox1.DataSource = currentMap.GetGenericFields();
                listBox1.DisplayMember = "DisplayFieldName";
                listBox1.ValueMember = "FieldID";
            }
        }
    }
}
