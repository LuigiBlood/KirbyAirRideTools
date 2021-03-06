﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace KirbyAirRideTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exportCollisionOBJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "HSD DAT File (*.dat)|*.dat|All files|*.*";
            ofd.Title = "Load DAT File...";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Wavefront OBJ (*.obj)|*.obj|All files|*.*";
                sfd.Title = "Save as OBJ File...";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter OBJfile = new StreamWriter(sfd.OpenFile());
                    BinaryReader DATfile = new BinaryReader(ofd.OpenFile());
                    Export.initOBJexport(OBJfile);
                    if ((Export.exportCollisionOBJ(OBJfile, DATfile) | Export.exportPathOBJ(OBJfile, DATfile)) == 0)
                    {
                        MessageBox.Show("Success");
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                    OBJfile.Close();
                    DATfile.Close();
                }
            }
        }

        private void exportPartitionOBJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "HSD DAT File (*.dat)|*.dat|All files|*.*";
            ofd.Title = "Load DAT File...";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Wavefront OBJ (*.obj)|*.obj|All files|*.*";
                sfd.Title = "Save as OBJ File...";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter OBJfile = new StreamWriter(sfd.OpenFile());
                    BinaryReader DATfile = new BinaryReader(ofd.OpenFile());
                    Export.initOBJexport(OBJfile);
                    if (Export.exportPartitionOBJ(OBJfile, DATfile) == 0)
                    {
                        MessageBox.Show("Success");
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                    OBJfile.Close();
                    DATfile.Close();
                }
            }
        }

        private void exportCollisionDAEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "HSD DAT File (*.dat)|*.dat|All files|*.*";
            ofd.Title = "Load DAT File...";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Collada DAE (*.dae)|*.dae|All files|*.*";
                sfd.Title = "Save as DAE File...";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                    {
                        Indent = true,
                        IndentChars = "\t",
                    };

                    XmlWriter DAEfile = XmlWriter.Create(sfd.OpenFile(), xmlWriterSettings);
                    BinaryReader DATfile = new BinaryReader(ofd.OpenFile());
                    if ((Export.DAEexport(DAEfile, DATfile)) == 0)
                    {
                        MessageBox.Show("Success");
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                    DATfile.Close();
                }
            }
        }
    }
}
