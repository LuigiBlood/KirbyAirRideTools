using System;
using System.IO;
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

        private void ExportCollisionOBJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "HSD DAT File (*.dat)|*.dat|All files|*.*",
                Title = "Load DAT File...",
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Wavefront OBJ (*.obj)|*.obj|All files|*.*",
                    Title = "Save as OBJ File..."
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter OBJfile = new StreamWriter(sfd.OpenFile());
                    BinaryReader DATfile = new BinaryReader(ofd.OpenFile());
                    Export.InitOBJexport(OBJfile);
                    if ((Export.ExportCollisionOBJ(OBJfile, DATfile) | Export.ExportPathOBJ(OBJfile, DATfile)) == 0)
                        MessageBox.Show("Success");
                    else
                        MessageBox.Show("Error");

                    OBJfile.Close();
                    DATfile.Close();
                }
            }
        }

        private void ExportPartitionOBJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "HSD DAT File (*.dat)|*.dat|All files|*.*",
                Title = "Load DAT File...",
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Wavefront OBJ (*.obj)|*.obj|All files|*.*",
                    Title = "Save as OBJ File..."
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter OBJfile = new StreamWriter(sfd.OpenFile());
                    BinaryReader DATfile = new BinaryReader(ofd.OpenFile());
                    Export.InitOBJexport(OBJfile);
                    if (Export.ExportPartitionOBJ(OBJfile, DATfile) == 0)
                        MessageBox.Show("Success");
                    else
                        MessageBox.Show("Error");

                    OBJfile.Close();
                    DATfile.Close();
                }
            }
        }

        private void ExportCollisionDAEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "HSD DAT File (*.dat)|*.dat|All files|*.*",
                Title = "Load DAT File...",
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Collada DAE (*.dae)|*.dae|All files|*.*",
                    Title = "Save as DAE File..."
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                    {
                        Indent = true,
                        IndentChars = "\t",
                    };

                    XmlWriter DAEfile = XmlWriter.Create(sfd.OpenFile(), xmlWriterSettings);
                    BinaryReader DATfile = new BinaryReader(ofd.OpenFile());
                    if (Export.DAEexport(DAEfile, DATfile) == 0)
                        MessageBox.Show("Success");
                    else
                        MessageBox.Show("Error");

                    DATfile.Close();
                }
            }
        }
    }
}
