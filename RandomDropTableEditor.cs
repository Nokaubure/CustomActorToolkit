using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CustomActorToolkit
{
    public partial class RandomDropTableEditor : Form
    {

        String OldFileName = "";
        Boolean isrom = false, isztable = false;
        Dictionary<byte,string> SceneNames;
        public int RandomDropTableOffset = 0;
        public DropTable[] DropTables = new DropTable[16];
        ComboBox[] items;
        NumericUpDown[] amounts;
        public string ROMpath = "";
        public int previd = 0;

        public RandomDropTableEditor(int a, string b)
        {
            RandomDropTableOffset = a;
            ROMpath = b;

            InitializeComponent();

            items = new []{ Item0, Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10, Item11, Item12, Item13, Item14, Item15 };
            amounts = new []{ Amount0, Amount1, Amount2, Amount3, Amount4, Amount5, Amount6, Amount7, Amount8, Amount9, Amount10, Amount11, Amount12, Amount13, Amount14, Amount15 };


            InitializeForm();


        }

        private void Close_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void InitializeForm()
        {

            for(int i = 0; i < 16; i++)
            {
                items[i].Items.AddRange(XMLreader.getXMLItems("ItemDrops", "Item"));
            }

            List<byte> ROM = new List<byte>(File.ReadAllBytes(ROMpath));

            int offset = (int)RandomDropTableOffset;
            int cnt = 0;
            while (cnt != 15)
            {
                DropTables[cnt] = new DropTable();
                for (int i = 0; i < 16; i++)
                {
                    DropTables[cnt].ItemVal[i] = ROM[offset + i];
                    DropTables[cnt].ItemAmount[i] = ROM[offset + i + 0xF0];
                }
                offset += 0x10;
                cnt++; 
            }

            ChangeTable(false);
        }


        private void saveROMToolStripMenuItem_Click(object sender, EventArgs e)
        {


                if (IsFileLocked(ROMpath))
                    MessageBox.Show("File is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {


                    BinaryWriter BWS = new BinaryWriter(File.OpenWrite(ROMpath));
                    int offset = (int)RandomDropTableOffset;
                    BWS.Seek(offset, SeekOrigin.Begin);

                    ChangeTable(true);
                    
                    List<Byte> Output = new List<byte>();
                    List<Byte> Output2 = new List<byte>();

                    for (int i = 0; i < 15; i++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            Output.Add(DropTables[i].ItemVal[y]);
                            Output2.Add(DropTables[i].ItemAmount[y]);
                        }
                    }
                    Output.AddRange(Output2);
                    BWS.Write(Output.ToArray());

                    BWS.Close();

                    MessageBox.Show("Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
             }
        }

        private void exportBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.CheckFileExists = false;
            //    saveFileDialog1.FileName = Path.GetFileName(OldFileName) + "_new";
            saveFileDialog1.Filter = "Save file binary (*.bin)|*.bin|All Files (*.*)|*.*";
            saveFileDialog1.CreatePrompt = true;


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (IsFileLocked(saveFileDialog1.FileName))
                    MessageBox.Show("File is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    ChangeTable(true);

                    List<Byte> Output = new List<byte>();
                    List<Byte> Output2 = new List<byte>();

                    for (int i = 0; i < 15; i++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            Output.Add(DropTables[i].ItemVal[y]);
                            Output2.Add(DropTables[i].ItemAmount[y]);
                        }
                    }
                    Output.AddRange(Output2);
                    File.WriteAllBytes(saveFileDialog1.FileName, Output.ToArray());
                    MessageBox.Show("Done! File Size: " + Output.Count.ToString("X") + " bytes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void DropTableID_ValueChanged(object sender, EventArgs e)
        {
            ChangeTable(true);
        }

        private void ChangeTable(bool change = false)
        {
            if (change)
            {

                for (int y = 0; y < 16; y++)
                {
                    DropTables[previd].ItemVal[y] = (byte)Convert.ToByte((items[y].SelectedItem as SongItem).Value);
                    DropTables[previd].ItemAmount[y] = (byte) amounts[y].Value;
                }
                
            }
            for(int i=0; i < 16; i++)
            {
                items[i].SelectedIndex = MainForm.FindSongComboItemValue(items[i].Items, DropTables[(int)DropTableID.Value].ItemVal[i]);
                amounts[i].Value = DropTables[(int) DropTableID.Value].ItemAmount[i];
            }

            previd = (int)DropTableID.Value;
        }

        private bool IsFileLocked(string file)
            {
                FileStream stream = null;
                try
                {
                    stream = File.OpenWrite(file);
                }
                catch (IOException)
                {
                    return true;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
                return false;
            }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        public class DropTable
        {
            public byte[] ItemVal = new byte[16];
            public byte[] ItemAmount = new byte[16];
        }
     


    }
}
