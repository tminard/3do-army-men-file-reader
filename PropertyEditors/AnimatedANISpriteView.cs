﻿using AMMEdit.ani;
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
    public partial class AnimatedANISpriteView : Form
    {
        public AnimatedANISpriteView(AniFile aniFile)
        {
            InitializeComponent();
            AniFile = aniFile;
        }

        public AniFile AniFile { get; }

        private void AnimatedANISpriteView_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = AniFile;
            listBox1.DataSource = AniFile.Sprites;
            listBox1.DisplayMember = "Name";
            pictureBox1.BackColor = Color.AntiqueWhite;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AniFile.SpriteData selectedSpriteData = (AniFile.SpriteData)((ListBox)sender).SelectedItem;
            propertyGrid1.SelectedObject = selectedSpriteData;
            pictureBox1.Image = selectedSpriteData.Image;
        }
    }
}
