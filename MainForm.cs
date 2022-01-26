using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using Ionic.Zip;
using Microsoft.Win32.SafeHandles;
using RedCell.Diagnostics.Update;
using Microsoft.VisualBasic;


namespace CustomActorToolkit
{

    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle,  uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            uint lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            uint hTemplateFile);

        private const int MY_CODE_PAGE = 437;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint GENERIC_READ = 0x80000000;
        private const uint FILE_SHARE_WRITE = 0x2;
        private const uint OPEN_EXISTING = 0x3;

       // public List<byte> ROM;
        public string ObjectPath = "";
        public string OverlayPath = "";
        public string RomPath = "";
        public uint initvars;
        public uint bss;
        public bool IgnoreDmaTable = true;
        public int OverlayTableOffset = 0x00B8D440;
        public int OverlayTableOffsetEnd = 0x00B90F20;
        public int ObjectTableOffset = 0;
        public int RandomDropTable = 0;
        public int LinkOffset = 0;
        public int ActorDmaTableStart = 0;
        public int ActorDmaTableEnd = 0;
        public int DmaTableStart = 0;
        public int DmaTableEnd = 0;
        public string LinkFuncsInCode = "";
        public int PauseMenuOffset = 0;
        public string PauseMenuFuncsInCode = "";
        public string romgame = "";
        public string romprefix = "";
        public string originaltext = "";
        public static readonly string[] DamageEffects = { "None", "Fire", "Ice", "Electric", "Knockback", "???"};
        public bool IgnoreWarnings = false;
        public static Settings settings = new Settings();
        public bool globalzzrpmode = false;
        public string zzrpglobalpath = "";
        public string Z64hdrLinkerScript = "oot_mq_debug";

        public MainForm()
        {
            InitializeComponent();
            List<byte> ROM = new List<byte>();
            OverlayPath = "";
            RomPath = "";
            initvars = 0;
            originaltext = Text;
           // ConsoleWindow console = new ConsoleWindow();
            CreateConsole();
            UpdateWindow();
            OverlayLabel.Text = "Open an overlay file";

            Log.Console = false;
            Log.Debug = false;
            Log.Prefix = "[Update] ";
        
            Updater updater = new Updater();

            updater.StartMonitoring();

            /*

            if (Directory.Exists(@"gcc\mips64\include\z64ovl-master"))
            {
                if (Directory.Exists(@"gcc\mips64\include\z64ovl")) Directory.Delete(@"gcc\mips64\include\z64ovl", true);
                Directory.Move(@"gcc\mips64\include\z64ovl-master", @"gcc\mips64\include\z64ovl");
            }

            */

            if (File.Exists("Settings.xml"))
            {
                settings = IO.Import<Settings>("Settings.xml");
            }
            else
            {
                IO.Export<Settings>(settings, "Settings.xml");
                
            }
            

            WarningCheckbox.Checked = settings.DisableCWarnings;
            UseZ64hdr.Checked = settings.Usez64hdr;
        }

        private void loadROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Rom files (*.z64;*.rom)|*.z64;*.rom|zzromtool/zzrpl project file (*.zzrp;*.zzrpl)|*.zzrp;*.zzrpl|All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadRom(openFileDialog1.FileName);
                
            }
        }

        private void LoadRom(string Filename)
        {

            string extension = Path.GetExtension(Filename);
            if (extension == ".zzrp" || extension == ".zzrpl")
            {
                romgame = "OOT";
                romprefix = "DBGMQ";
                globalzzrpmode = true;
                RomPath = "";
                zzrpglobalpath = openFileDialog1.FileName;
                Text = originaltext + " - " + extension + " project";

            }
            else
            {
                globalzzrpmode = false;

                List<byte> ROM = new List<byte>(File.ReadAllBytes(Filename));

                RomPath = Filename;

                InjectOffset.Maximum = ROM.Count;

                InjectOffset.Value = 0x3000000;

                ObjectInjectOffset.Value = 0x3000000;




                //zelda@srd022j 03-02-21 00:16:31

                bool found = false;

                var bytes = new List<Byte>();

                for (int i = 0; i < 0x50000; i++)
                {
                    bytes.Add(ROM[i]);
                    bytes.Add(ROM[i + 1]);
                    bytes.Add(ROM[i + 2]);
                    bytes.Add(ROM[i + 3]);
                    bytes.Add(ROM[i + 4]);
                    bytes.Add(ROM[i + 5]);
                    if (!bytes.Contains(0x00) && System.Text.Encoding.ASCII.GetString(bytes.ToArray()) == "zelda@")
                    {
                        bytes.Clear();
                        for (int ii = 0; ii < 50; ii++)
                        {
                            for (int i3 = 0; i3 < 17; i3++)
                            {
                                bytes.Add(ROM[i + ii + i3]);
                            }

                            if (!bytes.Contains(0x00) && Regex.Replace(System.Text.Encoding.ASCII.GetString(bytes.ToArray()), @"\d", "X") == "XX-XX-XX XX:XX:XX")
                            {
                                string builddate = System.Text.Encoding.ASCII.GetString(bytes.ToArray());
                                // Console.WriteLine(builddate);
                                XmlDocument doc = new XmlDocument();
                                var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/Roms.xml");
                                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                                doc.Load(fs);
                                XmlNodeList nodes = doc.SelectNodes("Table/ROM");
                                String[] output = { "0", "" };
                                if (nodes != null)
                                    foreach (XmlNode node in nodes)
                                    {
                                        XmlAttributeCollection nodeAtt = node.Attributes;
                                        if (nodeAtt["Build"] != null && nodeAtt["Build"].Value == builddate)
                                        {
                                            romgame = nodeAtt["Game"].Value;
                                            romprefix = nodeAtt["Prefix"].Value;
                                            OverlayTableOffset = Convert.ToInt32(nodeAtt["OverlayTable"].Value, 16);
                                            OverlayTableOffsetEnd = Convert.ToInt32(nodeAtt["OverlayTableEnd"].Value, 16);
                                            ObjectTableOffset = Convert.ToInt32(nodeAtt["ObjectTable"].Value, 16);
                                            IgnoreDmaTable = nodeAtt["IgnoreDmaTable"] != null;
                                            RandomDropTable = Convert.ToInt32(nodeAtt["RandomDropTable"].Value, 16);
                                            LinkOffset = Convert.ToInt32(nodeAtt["LinkOffset"].Value, 16);
                                            LinkFuncsInCode = nodeAtt["LinkFuncsInCode"].Value;
                                            PauseMenuOffset = Convert.ToInt32(nodeAtt["PauseMenuOffset"].Value, 16);
                                            PauseMenuFuncsInCode = nodeAtt["PauseMenuFuncsInCode"].Value;
                                            Text = originaltext + " - " + node.InnerText;
                                            ActorDmaTableStart = Convert.ToInt32(nodeAtt["ActorDmaTableStart"].Value, 16);
                                            ActorDmaTableEnd = Convert.ToInt32(nodeAtt["ActorDmaTableEnd"].Value, 16);
                                            DmaTableStart = Convert.ToInt32(nodeAtt["DmaTableStart"].Value, 16);
                                            DmaTableEnd = Convert.ToInt32(nodeAtt["DmaTableEnd"].Value, 16);
                                            Z64hdrLinkerScript = nodeAtt["z64hdr"].Value;
                                            found = true;
                                            break;
                                        }
                                    }
                                if (!found) break;

                            }
                            bytes.Clear();
                        }
                        if (!found)
                        {
                            MessageBox.Show("Only OoT MQ debug, OoT 1.0 U, MM 1.0 J , MM 1.0 U roms are supported", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    bytes.Clear();
                }
            }


            UpdateWindow();
        }

        private void UpdateWindow()
        {
            UpdateCRCButton.Enabled = (RomPath != "");
            FindEmptyVRAMButton.Enabled = (RomPath != "" && OverlayPath != "");
            FindEmptyActorIDButton.Enabled = (RomPath != "");
            FindEmptySpaceButton.Enabled = (RomPath != "" && OverlayPath != "");
            ClearDmaButton.Enabled = (RomPath != "");
            UpdateCRCButton.Enabled = (RomPath != "");
            InjectButton.Enabled = (RomPath != "" && OverlayPath != "");
            LaunchButton.Enabled = (RomPath != "");
            CompileButton.Enabled = (OverlayPath != "");
            OverlayLabel.Text = Path.GetFileName(OverlayPath);
            ZobjLabel.Text = Path.GetFileName(ObjectPath);
            DecompileAtomButton.Enabled = (RomPath != "");
            ExportZobj.Enabled = (RomPath != "");
            ImportZobj.Enabled = (RomPath != "" || globalzzrpmode);
            InjectZobj.Enabled = (RomPath != "") && ObjectPath != "";
            FindEmptyObjectID.Enabled = (RomPath != "");
            FindEmptySpace2.Enabled = (RomPath != "" && ObjectPath != "");
            OpenARomlabel.Visible = (RomPath == "");
            RandomDropTableEditorButton.Enabled = (RomPath != "" && romgame == "OOT");
            FindOriginalRowButton.Enabled = (RomPath != "");
            ObjectFindOriginalRowButton.Enabled = (RomPath != "");
            SendTozzrp.Enabled = ((RomPath != "" || globalzzrpmode) && OverlayPath != "");
            ObjectSendTozzrp.Enabled = ((RomPath != "" || globalzzrpmode) && ObjectPath != "");
            //    ClearDmaButton.Text = (romprefix == "DBGMQ") ? "Clear DMA Table" : "Rebuild DMA Table";

            openRecentToolStripMenuItem.DropDownItems.Clear();
            //openRecentToolStripMenuItem.DropDownItems.Add(new System.Windows.Forms.ToolStripMenuItem() { Name = "nothing", Text = "<nothing>" });

            XmlDocument doc = new XmlDocument();
            File.Delete("XML/RecentFilestmp.xml");

            System.IO.File.Copy("XML/RecentFiles.xml", "XML/RecentFilestmp.xml");
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/RecentFilestmp.xml");
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            doc.Load(fs);
            XmlNodeList nodes = doc.SelectNodes("Table/ROM");


            if (RomPath != "" || globalzzrpmode)
            {
                XmlNode deletenode = doc.SelectSingleNode("//ROM[text()='" + (globalzzrpmode ? zzrpglobalpath : RomPath) + "']");
                if (deletenode != null)
                {
                    deletenode.ParentNode.RemoveChild(deletenode);
                }

                XmlNode newnode = doc.CreateElement("ROM");
                newnode.InnerText = (globalzzrpmode ? zzrpglobalpath : RomPath);
                XmlNode Table = doc.SelectSingleNode("//Table");
                Table.AppendChild(newnode);
                doc.Save("XML/RecentFiles.xml");

            }

            if (nodes != null)
            {
                openRecentToolStripMenuItem.DropDownItems.Clear();
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    XmlNode node = nodes[i];

                    ToolStripMenuItem MenuItem = new System.Windows.Forms.ToolStripMenuItem() { Name = node.InnerText, Text = node.InnerText };
                    MenuItem.Click += new System.EventHandler(this.nothingToolStripMenuItem_Click);

                    openRecentToolStripMenuItem.DropDownItems.Add(MenuItem);
                };
            }

            fs.Close();
            File.Delete("XML/RecentFilestmp.xml");
            openRecentToolStripMenuItem.Enabled = openRecentToolStripMenuItem.DropDownItems.Count > 0;


        }

        private void SetOverlayPathButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                OverlayPath = openFileDialog1.FileName;

                UpdateWindow();

            }

        }

        private void ClearDmaButton_Click(object sender, EventArgs e)
        {

            if (IsFileLocked(RomPath))
                MessageBox.Show("ROM is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
             

                    BinaryWriter BWS = new BinaryWriter(File.OpenWrite(RomPath));
                    int StartOffset = ActorDmaTableStart;
                    int EndOffset = ActorDmaTableEnd;  
                    BWS.Seek(StartOffset, SeekOrigin.Begin);

                    byte[] Output = new byte[EndOffset - StartOffset];

                    BWS.Write(Output.ToArray());

                    BWS.Close();

                    MessageBox.Show("DMA table cleared!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            
        }

        private void UpdateCRCButton_Click(object sender, EventArgs e)
        {
            if (IsFileLocked(RomPath))
                MessageBox.Show("ROM is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Stream sw = File.Open(RomPath, FileMode.Open, FileAccess.ReadWrite);

                uint[] crc = new uint[2];
                byte[] data = new byte[0x00101000];

                uint d, r, t1, t2, t3, t4, t5, t6 = 0xDF26F436;

                t1 = t2 = t3 = t4 = t5 = t6;

                sw.Position = 0;
                sw.Read(data, 0, 0x00101000);

                for (int i = 0x00001000; i < 0x00101000; i += 4)
                {
                    d = (uint)((data[i] << 24) | (data[i + 1] << 16) | (data[i + 2] << 8) | data[i + 3]);
                    if ((t6 + d) < t6) t4++;
                    t6 += d;
                    t3 ^= d;
                    r = (d << (int)(d & 0x1F)) | (d >> (32 - (int)(d & 0x1F)));
                    t5 += r;
                    if (t2 > d) t2 ^= r;
                    else t2 ^= t6 ^ d;
                    t1 += (uint)((data[0x00000750 + (i & 0xFF)] << 24) | (data[0x00000751 + (i & 0xFF)] << 16) |
                          (data[0x00000752 + (i & 0xFF)] << 8) | data[0x00000753 + (i & 0xFF)]) ^ d;
                }
                crc[0] = t6 ^ t4 ^ t3;
                crc[1] = t5 ^ t2 ^ t1;

                if (BitConverter.IsLittleEndian)
                {
                    crc[0] = (crc[0] >> 24) | ((crc[0] >> 8) & 0xFF00) | ((crc[0] << 8) & 0xFF0000) | ((crc[0] << 24) & 0xFF000000);
                    crc[1] = (crc[1] >> 24) | ((crc[1] >> 8) & 0xFF00) | ((crc[1] << 8) & 0xFF0000) | ((crc[1] << 24) & 0xFF000000);
                }

                //Seek to 0x10 from rom start
                sw.Position = 0x10;
                BinaryWriter br = new BinaryWriter(sw);
                br.Write(crc[0]);
                br.Write(crc[1]);

                br.Close();
                sw.Close();

                Console.WriteLine("CRC \n" + crc[0].ToString("X") + "\n" + crc[1].ToString("X"));


                MessageBox.Show("CRC updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static bool IsFileLocked(string file)
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            "Custom Actor Toolkit" + " - Zelda OoT Set of tools to create custom actors" + Environment.NewLine + Environment.NewLine +
            "Hi, Im Nokaubure and I made this tool in 2018 to help newcomers creating custom actors. Ofcourse you need MIPS knowledge to create them" + Environment.NewLine + Environment.NewLine +
            "Thanks to: " + Environment.NewLine +
            "ZZT32 for making nOvl" + Environment.NewLine +
            "mzxrules for making Atom" + Environment.NewLine +
            "z64me and CrookedPoe for making z64ovl" + Environment.NewLine,
            "About" + Environment.NewLine, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists("assemble.bat"))
            {
                File.Create("assemble.bat").Close(); 
               
            }
            else if (IsFileLocked("assemble.bat"))
                MessageBox.Show("assemble.bat is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {


                

                StreamWriter sw = new StreamWriter("assemble.bat");

               // string Filename = OverlayPath.Split('.')[OverlayPath.Split('.').Length-2];

                string Filename = OverlayPath.Substring(0, LastIndexOf(OverlayPath,"."));

                string ext = OverlayPath.Substring(LastIndexOf(OverlayPath,"."));

                string ShortFilename = Filename.Substring(OverlayPath.LastIndexOf("\\") + 1);

    

                if (File.Exists(Filename + ".ovl"))
                {
                    File.Delete(Filename + ".ovl");
                }
                if (File.Exists(Path.GetDirectoryName(Filename) + @"\gcc\bin\" + Filename + ".o"))
                {
                    File.Delete(Path.GetDirectoryName(Filename) + @"\gcc\bin\" + Filename + ".o");
                }
                if (File.Exists("output.txt"))
                {
                    File.Delete("output.txt");
                }



                Filename = "\"" + Filename;


                Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                string flags = "";
                if (ext.Contains("c"))
                {
                    flags = settings.CCompileFlags + " " + ((IgnoreWarnings) ? "-w " : "");
                }
                else
                {
                    flags = settings.MipsCompileFlags +  " " + ((IgnoreWarnings) ? "-w " : "");
                }
                string objdump = "";
                objdump = @".\gcc\bin\mips64-objdump -t " + "\".\\gcc\\bin\\" + ShortFilename + ".o\"" + " | findstr /I \"constructor destructor init update main dest draw init_vars initvars\" >> " + "output.txt" + Environment.NewLine;

                string data = "";

                if (!UseZ64hdr.Checked)
                {

                    data = "@echo off" + Environment.NewLine +
                    @"cd .\gcc\bin\" + Environment.NewLine +
                    @"mips64-gcc " + flags + Filename + ext + "\"" + Environment.NewLine +
                    @"mips64-ld -o " + Filename + ".elf" + "\" \"" + ShortFilename + ".o\"" + @" -T z64-ovl.ld --emit-relocs" + Environment.NewLine +
                    @"cd ..\..\" + Environment.NewLine +
                    objdump +
                    @"nOVL\novl -vv -c -A 0x" + ((ulong)(VRAM.Value)).ToString("X8") + " -o " + Filename + ".ovl" + "\" " + Filename + ".elf" + "\" " + Environment.NewLine +
                    //  "del " + ShortFilename + ".o" + Environment.NewLine +
                    "del " + Filename + ".elf" + "\"" + Environment.NewLine +
                    "del \".\\gcc\\bin\\" + ShortFilename + ".o" + "\"" + Environment.NewLine +
                    "echo done, output " + Filename + ".ovl" + "\"";
                }
                else
                {
                    data = "@echo off" + Environment.NewLine +
                    @"cd .\gcc\bin\" + Environment.NewLine +
                    @"mips64-gcc -I ""../mips64/include/z64hdr"" -I ""../mips64/include/z64hdr/include"" " + flags + Filename + ext + "\"" + Environment.NewLine +
                    @"cd ..\mips64\include\z64hdr\" + Environment.NewLine +
                    @"copy ..\..\..\bin\conf.ld entry.ld" + Environment.NewLine +
                    @"..\..\..\bin\mips64-ld -L """ + Directory.GetCurrentDirectory() + @"\gcc\mips64\include\z64hdr\common"" -L """ + Directory.GetCurrentDirectory() + @"\gcc\mips64\include\z64hdr\" + Z64hdrLinkerScript  + @""" -T """ + Z64hdrLinkerScript + @"/z64hdr.ld"" --emit-relocs -o " + Filename + ".elf" + "\" \"../../../bin/" + ShortFilename + ".o\"" + @" " + Environment.NewLine +
                    @"cd ..\..\..\..\" + Environment.NewLine +
                    objdump +
                    @"nOVL\novl -vv -c -A 0x" + ((ulong)(VRAM.Value)).ToString("X8") + " -o " + Filename + ".ovl" + "\" " + Filename + ".elf" + "\" " + Environment.NewLine +
                    //  "del " + ShortFilename + ".o" + Environment.NewLine +
                    "del " + Filename + ".elf" + "\"" + Environment.NewLine +
                    "del \".\\gcc\\bin\\" + ShortFilename + ".o" + "\"" + Environment.NewLine +
                    "echo done, output " + Filename + ".ovl" + "\"";
                }

                sw.Write(data);
                sw.Flush();
                sw.Close();



                String pdetail = @"/c assemble.bat";
                ProcessStartInfo pcmd = new ProcessStartInfo("cmd.exe");
                pcmd.Arguments = pdetail;
                pcmd.UseShellExecute = false;
                pcmd.RedirectStandardOutput = true;
                pcmd.RedirectStandardError = true;
                Process cmd = Process.Start(pcmd);

                string output = cmd.StandardError.ReadToEnd();
                Console.WriteLine(output);
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());

                string tmp;

                //cmd.WaitForExit();

                if (output.IndexOf(".data") == -1)
                {
                    if (!UseZ64hdr.Checked)
                        MessageBox.Show("Something went wrong, better fix it before proceding (enable z64hdr if the actor is supposed to use it)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        MessageBox.Show("Something went wrong, better fix it before proceding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                   tmp = output.Substring(output.IndexOf(".data"));
                   tmp = tmp.Substring(tmp.IndexOf("to 0x")+5,8);
                   initvars = Convert.ToUInt32(tmp, 16);

                    string[] textlines = File.ReadLines(OverlayPath).ToArray();
                    bool countingbytes = false;
                    bss = 0;
                    for (int i = 0; i < textlines.Count(); i++)
                    {
                        if (textlines[i].Contains(".bss"))
                            countingbytes = true;
                        if (textlines[i].Contains("0x") && countingbytes)
                        {
                            Console.WriteLine(textlines[i]);
                            bss += Convert.ToUInt32(textlines[i].Substring(textlines[i].IndexOf("0x") + 2), 16);
                        }
                            
                        
                    }
                    if (bss != 0) bss = (uint) (Math.Ceiling((double)bss / 0x10) * 0x10);

                    initvars -= 0x80800000;
                    initvars += (uint)(VRAM.Value);

                    // Console.WriteLine(((long)VRAM.Value).ToString("X8"));
                    Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt") + " Success! click inject button");
                   // Console.WriteLine("init vars: " + initvars.ToString("X"));
                    //  Console.WriteLine("bss: " + bss.ToString("X"));
                }

                UpdateWindow();

                
            }
        }

        public int LastIndexOf(string str, string find)
        {
            int ret = str.LastIndexOf(find);
            if (ret == -1) ret = str.Length;
            return ret;
        }

        private void InjectButton_Click(object sender, EventArgs e)
        {
            if (IsFileLocked(RomPath))
                MessageBox.Show("ROM is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
            //    string Filename = OverlayPath.Split('.')[OverlayPath.Split('.').Length-2] + ".ovl";

                string Filename = OverlayPath.Substring(0, LastIndexOf(OverlayPath,".")) + ".ovl";
                

                if (File.Exists(Filename))
                {
                    BinaryWriter BWS = new BinaryWriter(File.OpenWrite(RomPath));
                    BWS.Seek((int)InjectOffset.Value, SeekOrigin.Begin);

                    var data = new List<byte>(File.ReadAllBytes(Filename));
                    var tmp = new List<byte>();
                    uint VRAMstart = (uint)VRAM.Value;
                    uint VRAMend = (uint)(VRAM.Value + data.Count + bss);

                    BWS.Write(data.ToArray());

                    List<Byte> ActorRow = new List<byte>();

                    if (ActorID.Value > 0)
                    {

                        // Find Initialization Variables
                        for (int i = 0; i < (data.Count - 0x20); i += 4)
                        {
                           // ushort init_aid = Helpers.Read16(data, i + 0x00);
                            ushort init_oid = Helpers.Read16(data, i + 0x08);
                            ushort init_pad = Helpers.Read16(data, i + 0x0A);
                            ushort instancesize = Helpers.Read16(data, i + 0x0E);
                            uint init_func_init = Helpers.Read32(data, i + 0x10);
                            uint init_func_dest = Helpers.Read32(data, i + 0x14);
                            uint init_func_main = Helpers.Read32(data, i + 0x18);
                            uint init_func_draw = Helpers.Read32(data, i + 0x1C);
                            if (
                                ((init_oid > 0x0000) && (init_oid < 0x0300)) &&
                                (init_pad == 0x0000 || init_pad == 0xBEEF) &&
                                (instancesize >= 0x013C) &&
                                (((init_func_init >= VRAMstart) && init_func_init <= VRAMend) &&
                                ((init_func_dest >= VRAMstart && init_func_dest <= VRAMend) || init_func_dest == 0x00000000) &&
                                ((init_func_main >= VRAMstart && init_func_main <= VRAMend) || init_func_main == 0x00000000) &&
                                ((init_func_draw >= VRAMstart && init_func_draw <= VRAMend) || init_func_draw == 0x00000000)))
                            {
                                initvars = (uint)i + VRAMstart;
                               
                                break;
                            }
                        }

                        Helpers.Append32(ref ActorRow, (uint)InjectOffset.Value);
                        Helpers.Append32(ref ActorRow, (uint)(InjectOffset.Value + data.Count));
                        Helpers.Append32(ref ActorRow, VRAMstart);
                        Helpers.Append32(ref ActorRow, VRAMend);
                        Helpers.Append32(ref ActorRow, 00000000);
                        Helpers.Append32(ref ActorRow, initvars);
                        Helpers.Append32(ref ActorRow, 00000000); // no filename
                        Helpers.Append16(ref ActorRow, (ushort)ActorAllocation.Value);
                        ActorRow.Add(00);
                        ActorRow.Add(00);

                        BWS.Seek((int)(OverlayTableOffset + (ActorID.Value * 0x20)), SeekOrigin.Begin);

                    }
                    else if (ActorID.Value == 0)
                    {
                        Helpers.Append32(ref ActorRow, (uint)InjectOffset.Value);
                        Helpers.Append32(ref ActorRow, (uint)(InjectOffset.Value + data.Count));
                        Helpers.Append32(ref ActorRow, VRAMstart);
                        Helpers.Append32(ref ActorRow, VRAMend);

                        if (LinkOffset == 0 || LinkFuncsInCode == "")
                        {
                            MessageBox.Show("Link injection not supported in this version", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            BWS.Close();
                            return;
                        }
                        string[] offsets = LinkFuncsInCode.Split(',');
                     //   int[] offsets = new[] { 0x00B0D5B8, 0x00B0D5C0, 0x00B0D5C8, 0x00B0D5D4, 0x00B0D5DC, 0x00B0D5E8, 0x00B0D5F0, 0x00B0D5FC };

                        if (Helpers.Read16(data, 0) > 0x8000)
                        {
                            ushort val,val2 = 0;
                            for (int i = 0; i < offsets.Length; i++)
                            {
                                tmp.Clear();
                                val = Helpers.Read16(data, 0 + (i * 2));
                                val2 = Helpers.Read16(data, 2 + (i * 2));
                                if (i % 2 == 0 && val2 > 0x7FFF)
                                    val += 1;
                                Helpers.Append16(ref tmp, val);
                                BWS.Seek(Convert.ToInt32(offsets[i],16) + 2, SeekOrigin.Begin);
                                BWS.Write(tmp.ToArray());
                            }
                        }
                        else //its in C
                        {
                            if (File.Exists("output.txt"))
                            {
                                //   Console.WriteLine("output.txt");

                                string[] textlines = File.ReadLines("output.txt").ToArray();

                                uint init = 0, dest = 0, update = 0, draw = 0;
                                // constructor destructor init update dest draw init_vars initvars
                                for (int i = 0; i < textlines.Count(); i++)
                                {

                                    if ((textlines[i].Contains(" constructor", 1) || textlines[i].Contains(" init", 1) || textlines[i].Contains(" func_constructor", 1) || textlines[i].Contains(" func_init",1)) && textlines[i].Contains(".text"))
                                    {
                                        init = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                        init += (uint)VRAM.Value;
                                    }
                                    else if ((textlines[i].Contains(" destructor", 1) || textlines[i].Contains(" dest", 1) || textlines[i].Contains(" func_destructor", 1) || textlines[i].Contains(" func_dest",1)) && textlines[i].Contains(".text"))
                                    {
                                        dest = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                        dest += (uint)VRAM.Value;
                                    }
                                    else if ((textlines[i].Contains(" update", 1) || textlines[i].Contains(" func_update", 1) || textlines[i].Contains(" main", 1) || textlines[i].Contains(" func_main", 1)) && textlines[i].Contains(".text"))
                                    {
                                        update = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                        update += (uint)VRAM.Value;
                                    }
                                    else if ((textlines[i].Contains(" draw", 1) || textlines[i].Contains(" func_draw", 1)) && textlines[i].Contains(".text"))
                                    {
                                        draw = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                        draw += (uint)VRAM.Value;
                                    }


                                }

                                if (init != 0)
                                {
                                    List<byte> fixcompilation = new List<byte>();
                                    Helpers.Append32(ref fixcompilation, init);
                                    Helpers.Append32(ref fixcompilation, dest);
                                    Helpers.Append32(ref fixcompilation, update);
                                    Helpers.Append32(ref fixcompilation, draw);
                                    Helpers.Append32(ref fixcompilation,0);
                                    ushort val, val2 = 0;
                                    for (int i = 0; i < offsets.Length; i++)
                                    {
                                        tmp.Clear();
                                        val = Helpers.Read16(fixcompilation, 0 + (i * 2));
                                        val2 = Helpers.Read16(fixcompilation, 2 + (i * 2));
                                        if (i % 2 == 0 && val2 > 0x7FFF)
                                            val += 1;
                                        Helpers.Append16(ref tmp, val);
                                        BWS.Seek(Convert.ToInt32(offsets[i], 16) + 2, SeekOrigin.Begin);
                                        BWS.Write(tmp.ToArray());
                                    }
                                }



                            }
                        }
                            //         Console.WriteLine("Func 1: " + Helpers.Read32(data,0).ToString("X8"));
                            //       Console.WriteLine("Func 2: " + Helpers.Read32(data,4).ToString("X8"));
                            //        Console.WriteLine("Func 3: " + Helpers.Read32(data,0).ToString("X8"));
                            //       Console.WriteLine("Func 4: " + Helpers.Read32(data,0xC).ToString("X8"));

                            BWS.Seek((int)(LinkOffset), SeekOrigin.Begin);
                    }
                    else if (ActorID.Value == -1)
                    {
                        Helpers.Append32(ref ActorRow, (uint)InjectOffset.Value);
                        Helpers.Append32(ref ActorRow, (uint)(InjectOffset.Value + data.Count));
                        Helpers.Append32(ref ActorRow, VRAMstart);
                        Helpers.Append32(ref ActorRow, VRAMend);

                        if (PauseMenuOffset == 0 || PauseMenuFuncsInCode == "")
                        {
                            MessageBox.Show("Pause Menu injection not supported in this version", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            BWS.Close();
                            return;
                        }
                        string[] offsets = PauseMenuFuncsInCode.Split(',');

                        if (Helpers.Read16(data, 0) > 0x8000)
                        {
                            ushort val, val2 = 0;
                            for (int i = 0; i < offsets.Length; i++)
                            {
                                tmp.Clear();
                                val = Helpers.Read16(data, 0 + (i * 2));
                                val2 = Helpers.Read16(data, 2 + (i * 2));
                                if (i % 2 == 0 && val2 > 0x7FFF)
                                    val += 1;
                                Helpers.Append16(ref tmp, val);
                                BWS.Seek(Convert.ToInt32(offsets[i], 16) + 2, SeekOrigin.Begin);
                                BWS.Write(tmp.ToArray());
                            }
                        }
                        else //its in C
                        {
                            if (File.Exists("output.txt"))
                            {
                                //   Console.WriteLine("output.txt");

                                string[] textlines = File.ReadLines("output.txt").ToArray();

                                uint init = 0, dest = 0, update = 0, draw = 0;
                                // constructor destructor init update dest draw init_vars initvars
                                for (int i = 0; i < textlines.Count(); i++)
                                {

                                    if ((textlines[i].Contains(" constructor", 1) || textlines[i].Contains(" init", 1) || textlines[i].Contains(" func_constructor", 1) || textlines[i].Contains(" func_init", 1)) && textlines[i].Contains(".text"))
                                    {
                                        init = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                        init += (uint)VRAM.Value;
                                    }
                                    else if ((textlines[i].Contains(" draw", 1) || textlines[i].Contains(" func_draw", 1)) && textlines[i].Contains(".text"))
                                    {
                                        draw = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                        draw += (uint)VRAM.Value;
                                    }


                                }

                                if (init != 0)
                                {
                                    List<byte> fixcompilation = new List<byte>();
                                    Helpers.Append32(ref fixcompilation, init);
                                    Helpers.Append32(ref fixcompilation, draw);
                                    Helpers.Append32(ref fixcompilation, 0);
                                    ushort val, val2 = 0;
                                    for (int i = 0; i < offsets.Length; i++)
                                    {
                                        tmp.Clear();
                                        val = Helpers.Read16(fixcompilation, 0 + (i * 2));
                                        val2 = Helpers.Read16(fixcompilation, 2 + (i * 2));
                                        if (i % 2 == 0 && val2 > 0x7FFF)
                                            val += 1;
                                        Helpers.Append16(ref tmp, val);
                                        BWS.Seek(Convert.ToInt32(offsets[i], 16) + 2, SeekOrigin.Begin);
                                        BWS.Write(tmp.ToArray());
                                    }
                                }



                            }
                        }


                        BWS.Seek((int)(PauseMenuOffset), SeekOrigin.Begin);
                    }

                    BWS.Write(ActorRow.ToArray());

                    if (DMARow.Value != 0)
                    {
                        List<byte> dmatmp = new List<byte>();
                        BWS.Seek((int) (DmaTableStart + (DMARow.Value * 0x10)), SeekOrigin.Begin);
                        Helpers.Append32(ref dmatmp, (uint)InjectOffset.Value);
                        Helpers.Append32(ref dmatmp, (uint)(InjectOffset.Value + data.Count));
                        Helpers.Append32(ref dmatmp, (uint)InjectOffset.Value);
                        Helpers.Append32(ref dmatmp, 0);
                        BWS.Write(dmatmp.ToArray());
                        Console.WriteLine("Row " + DMARow.Value + " of DMA table updated");
                    }

                    BWS.Close();

                    MessageBox.Show("Injection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("You have to compile first before injecting anything!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(RomPath))
                System.Diagnostics.Process.Start(RomPath);
            else
                MessageBox.Show("There's no ROM to launch.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void FindEmptyActorIDButton_Click(object sender, EventArgs e)
        {
            int offset = OverlayTableOffset;
            int incr = 0;
            List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));
            for (int i = offset; i < OverlayTableOffsetEnd; i+= 0x20)
            {
                if (Helpers.Read64(ROM,i) == 0 && Helpers.Read64(ROM, i+8) == 0 && Helpers.Read64(ROM, i + 16) == 0 && Helpers.Read64(ROM, i + 24) == 0)
                {
                    ActorID.Value = incr;
                    break;
                }
                incr++;
            }
        }

        private void FindEmptyVRAMButton_Click(object sender, EventArgs e)
        {

        //    string Filename = OverlayPath.Split('.')[OverlayPath.Split('.').Length-2] + ".ovl";

            string Filename = OverlayPath.Substring(0, LastIndexOf(OverlayPath,".")) + ".ovl";

            if (ActorID.Value == 0)
            {
                List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));
                VRAM.Value = Helpers.Read32(ROM, LinkOffset + 8);
                return;
            }
            else if (ActorID.Value == -1)
            {
                List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));
                VRAM.Value = Helpers.Read32(ROM, PauseMenuOffset + 8);
                return;
            }

            if (File.Exists(Filename))
            {
            var data = new List<byte>(File.ReadAllBytes(Filename));
            var VRamChunkList = new List<VRamChunk>();
            int offset = OverlayTableOffset;
            uint VRAMoffset = 0x80800000;
                List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));

            if ((Helpers.Read32(ROM, offset + (0x20 * (int)ActorID.Value) + 8) != 0 && Helpers.Read32(ROM, offset + (0x20 * (int)ActorID.Value) + 12) != 0) && Helpers.Read32(ROM, offset + (0x20 * (int)ActorID.Value) + 12) - Helpers.Read32(ROM, offset + (0x20 * (int)ActorID.Value) + 8) >= data.Count)
            {
                    //new actor fits in the RAM of the old actor
                    VRAM.Value = Helpers.Read32(ROM, offset + (0x20 * (int)ActorID.Value) + 8);
                    return;
            }


            for (int i = offset; i < 0x00B90F20; i += 0x20)
            {
                if (Helpers.Read32(ROM, i + 8) != 0 && Helpers.Read32(ROM, i + 12) != 0)
                {
                    VRamChunkList.Add(new VRamChunk(Helpers.Read32(ROM, i + 8), Helpers.Read32(ROM, i + 12)));
                }

            }

            VRamChunkList.OrderBy(x => x.startoffset);

            for (int i = 0; i < VRamChunkList.Count; i++)
            {
                    if (VRAMoffset + data.Count <= VRamChunkList[i].startoffset) break;
                    else VRAMoffset = VRamChunkList[i].endoffset;
            }

            //new actor is allocated in a new ram offset
            VRAM.Value = VRAMoffset;

            }
            else
            {
                MessageBox.Show("You have to compile first before injecting anything!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class VRamChunk
        {
            public uint startoffset = 0;
            public uint endoffset = 0;

            public VRamChunk(uint a, uint b)
            {
                startoffset = a;
                endoffset = b;
                
            }
        }

        private void FindEmptySpaceButton_Click(object sender, EventArgs e)
        {
          //  string Filename = OverlayPath.Split('.')[OverlayPath.Split('.').Length-2] + ".ovl";

            string Filename = OverlayPath.Substring(0, LastIndexOf(OverlayPath,".")) + ".ovl";
            

            bool notfound = false;
            if (File.Exists(Filename))
            {
                var data = new List<byte>(File.ReadAllBytes(Filename));
                int offset = (int) InjectOffset.Value;
                offset = (offset + 0xF) & -0x10;
                List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));

                if ((int)ActorID.Value == -1 && (Helpers.Read32(ROM, PauseMenuOffset) != 0 && Helpers.Read32(ROM, PauseMenuOffset + 4) != 0) && Helpers.Read32(ROM, PauseMenuOffset + 4) - Helpers.Read32(ROM, PauseMenuOffset) >= data.Count) 
                {
                    InjectOffset.Value = Helpers.Read32(ROM, PauseMenuOffset);
                }
                else if ((Helpers.Read32(ROM, OverlayTableOffset + (0x20 * (int)ActorID.Value)) != 0 && Helpers.Read32(ROM, OverlayTableOffset + (0x20 * (int)ActorID.Value) + 4) != 0) && Helpers.Read32(ROM, OverlayTableOffset + (0x20 * (int)ActorID.Value) + 4) - Helpers.Read32(ROM, OverlayTableOffset + (0x20 * (int)ActorID.Value)) >= data.Count)
                {
                    //new actor fits in the ROM of the old actor
                    InjectOffset.Value = Helpers.Read32(ROM, OverlayTableOffset + (0x20 * (int)ActorID.Value));
                    return;
                }

                for (int i = offset; i < ROM.Count-data.Count; i += 0x10)
                {
                    notfound = false;
                    for (int ii = 0; ii < data.Count && !notfound; ii += 4)
                    {
                        if (Helpers.Read32(ROM, i+ii) != 0) notfound = true;
                        //Console.WriteLine(ii);
                    }
                    if (!notfound)
                    {
                        InjectOffset.Value = i;
                        break;
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("You have to compile first before injecting anything!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DecompileAtomButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists("Atom/rompath.xml"))
            {
                MessageBox.Show("rompath.xml not found in atom folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
        
                int offset = 0;
                int overlayid = (int)DecActorID.Value;
                if ((int)DecActorID.Value > 0)
                    offset = OverlayTableOffset;
                else if ((int)DecActorID.Value == 0)
                    offset = LinkOffset;
                else if ((int)DecActorID.Value == -1)
                {
                    offset = PauseMenuOffset;
                    DecActorID.Value = 0;
                }
                List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));
                uint startactoroffset = (Helpers.Read32(ROM, offset + (0x20 * (int)DecActorID.Value) + 0));
                uint endactoroffset = (Helpers.Read32(ROM, offset + (0x20 * (int)DecActorID.Value) + 4));

                DecompilingLabel.Visible = true;

                if (startactoroffset != 0 && endactoroffset != 0)
                {

                XmlDocument doc = new XmlDocument();
                File.Delete("Atom/rompath_old.xml");
                File.Delete("atom/base/" + romgame + "/Actors.json_back");
                File.Delete("Atom/base/" + romgame + "/" + romprefix + "/dmadata.json_back");

                System.IO.File.Move("Atom/rompath.xml", "Atom/rompath_old.xml");
                var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"Atom/rompath_old.xml");
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                doc.Load(fs);

                XmlNodeList deletenodes = doc.SelectNodes("//Rom[@version='" + romprefix + "']");
                XmlNode Roms = doc.SelectSingleNode("//Roms");
                for (int i = deletenodes.Count - 1; i >= 0; i--)
                {
                    deletenodes[i].ParentNode.RemoveChild(deletenodes[i]);
                }

                string verbosegame = (romgame == "OOT") ? "OcarinaOfTime" : "MajorasMask";

                XmlNode newnode = doc.CreateElement("Rom");
                XmlAttributeCollection nodeAtt = newnode.Attributes;
                XmlAttribute attribute = doc.CreateAttribute("key");
                attribute.Value = verbosegame+"." + romprefix;
                nodeAtt.Append(attribute);
                attribute = doc.CreateAttribute("game");
                attribute.Value = verbosegame;
                nodeAtt.Append(attribute);
                attribute = doc.CreateAttribute("version");
                attribute.Value = romprefix;
                nodeAtt.Append(attribute);
                newnode.InnerText = RomPath;
                Roms.AppendChild(newnode);

                doc.Save("Atom/rompath.xml");

                fs.Close();

                File.Delete("Atom/rompath_old.xml");

                string chain = "[{\"Id\":\""+ ((int)DecActorID.Value).ToString("X4") + "\",\"Name\":\"run\"}]";

                string chain2 = "[{\"Filename\":\"run\",\"VRomStart\":\"" + startactoroffset + "\",\"VRomEnd\":\"" + endactoroffset + "\"}]";

                File.Move("Atom/base/" + romgame + "/Actors.json", "Atom/base/" + romgame + "/Actors.json_back");

                StreamWriter sw = new StreamWriter("Atom/base/" + romgame + "/Actors.json");
                sw.Write(chain);
                sw.Flush();
                sw.Close();

                File.Move("Atom/base/" + romgame + "/" + romprefix + "/dmadata.json", "Atom/base/" + romgame + "/" + romprefix + "/dmadata.json_back");

                sw = new StreamWriter("Atom/base/" + romgame + "/" + romprefix + "/dmadata.json");
                sw.Write(chain2);
                sw.Flush();
                sw.Close();
                String pdetail = "";
                if (romgame == "OOT")
                    { pdetail = @"/c cd atom && Atom.exe df " + romgame + " " + romprefix + " run";}
                else
                    { pdetail = @"/c cd atom && Atom.exe all " + romgame + " " + romprefix;}
                ProcessStartInfo pcmd = new ProcessStartInfo("cmd.exe");
                pcmd.Arguments = pdetail;
                Process cmd = Process.Start(pcmd);

                cmd.WaitForExit();

                File.Delete("atom/base/" + romgame + "/Actors.json");
                File.Move("atom/base/" + romgame + "/Actors.json_back", "atom/base/" + romgame + "/Actors.json");

                File.Delete("Atom/base/" + romgame + "/" + romprefix + "/dmadata.json");
                File.Move("Atom/base/" + romgame + "/" + romprefix + "/dmadata.json_back", "Atom/base/" + romgame + "/" + romprefix + "/dmadata.json");

                string atomfile = "";
                if ((overlayid == 0))
                    atomfile = "Link" + romprefix + ".S";
                else if ((overlayid == -1))
                    atomfile = "PauseMenu" + romprefix + ".S";
                else
                    atomfile = "Actor" + romprefix + "_" + ((int)(DecActorID.Value)).ToString("X4") + ".S";

                if ((File.Exists("Atom/__run.txt") && romgame == "OOT") || (File.Exists("Atom/M/" + romprefix + "/run.txt") && romgame == "MM"))
                {
                    File.Delete(atomfile);
                    if (romgame == "OOT")
                        File.Move("Atom/__run.txt", atomfile);
                    else
                        File.Move("Atom/M/" + romprefix +"/run.txt", atomfile);
                }

                CorrectSource(atomfile, overlayid);

                if (overlayid == 0)
                {
                    string text = File.ReadAllText(atomfile);
                    if (romprefix == "N0")
                        text = text.Replace("lwl     t9, 0x0044(t7)", "lwl     t9, 0x0044(t7)\r\nvar_8084A3DB:\r\n");
                    else if (romprefix == "DBGMQ")
                        text = text.Replace("var_8084C51B", "var_808544F8");

                        

                    string header = ".section .text\r\n## used to update the function references in code file\r\nFixCompilation: ";
                    string[] offsets = LinkFuncsInCode.Split(',');
                    string[] specialfncstr = new[] { "_constructor", "_destructor", "_update", "_draw" };
                    for (int i = 0; i < offsets.Length; i+=2)
                    {
                            //Console.WriteLine(Convert.ToInt32(offsets[i],16) + 2);
                            int firsthalf = Helpers.Read16(ROM, Convert.ToInt32(offsets[i], 16) + 2);
                            int secondhalf = Helpers.Read16(ROM, Convert.ToInt32(offsets[i+1], 16) + 2);
                            if (secondhalf > 0x7FFF) firsthalf--;
                            string funcoff = firsthalf.ToString("X4") + secondhalf.ToString("X4");
                            header += ".word func" + specialfncstr[i/2] + "\r\n";
                            text = text.Replace("func_" + funcoff, "func" + specialfncstr[i / 2]);
                        }
                    header += "\r\n";

                    text = text.Replace(".section .text", header);
                    File.WriteAllText(atomfile, text);

                    
                }
                else if (overlayid == -1)
                {
                    string text = File.ReadAllText(atomfile);


                    string header = ".section .text\r\n## used to update the function references in code file\r\nFixCompilation: ";
                    string[] offsets = PauseMenuFuncsInCode.Split(',');
                    string[] specialfncstr = new[] { "_constructor", "_draw" };
                    for (int i = 0; i < offsets.Length; i += 2)
                    {
                        //Console.WriteLine(Convert.ToInt32(offsets[i],16) + 2);
                        int firsthalf = Helpers.Read16(ROM, Convert.ToInt32(offsets[i], 16) + 2);
                        int secondhalf = Helpers.Read16(ROM, Convert.ToInt32(offsets[i + 1], 16) + 2);
                        if (secondhalf > 0x7FFF) firsthalf--;
                        string funcoff = firsthalf.ToString("X4") + secondhalf.ToString("X4");
                        header += ".word func" + specialfncstr[i/2] + "\r\n";
                        text = text.Replace("func_" + funcoff, "func" + specialfncstr[i / 2]);
                    }
                    header += "\r\n";

                    text = text.Replace(".section .text", header);
                        // text = text.Replace(".bss", "##.bss");
                    text = text + "\nvar_fckpj64: .space 0x10 ## Without this line pj64 crashes for some reason";
                    File.WriteAllText(atomfile, text);


                }
                    DecActorID.Value = overlayid;
                    DecompilingLabel.Visible = false;

                }
                else
                {
                    MessageBox.Show("This actor file is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        public void CorrectSource(string atomfile, int ID = -99)
        {
            if (JalNames.Checked)
            {
                string text = File.ReadAllText(atomfile);
                text = Regex.Replace(text, @"## 0000[A-F0-9]{4}", "");
                text = text.Replace(" .data\r\n\r\n.word ", " .data\r\n\r\nvar_something: .word ");
                text = text.Replace(": .word 0", ": .word \\\r\n0");
                text = text.Replace(": .byte 0", ": .byte \\\r\n0");
                File.WriteAllText(atomfile, text);

                string[] textlines = File.ReadLines(atomfile).ToArray();
                text = File.ReadAllText(atomfile);
                XmlDocument funcdoc = new XmlDocument();
                var funcfileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), (romgame == "OOT") ? @"XML/ActorFunctionsDbgN0.xml" : @"XML/ActorFunctionsMMU0J0.xml");
                FileStream funcfs = new FileStream(funcfileName, FileMode.Open, FileAccess.Read);
                funcdoc.Load(funcfs);
                XmlNodeList nodes = funcdoc.SelectNodes("Table/Function");
                for (int i = 0; i < textlines.Count(); i++)
                {
                    if (textlines[i].Contains("jal     func_8"))
                    {
                        string funcid = textlines[i].Substring(textlines[i].IndexOf("f"), 13).Replace("func_", "");
                        if (Convert.ToUInt32(funcid, 16) >= 0x80800000)
                        {
                            string newstr = textlines[i] + "\t\t## " + "Local Function";
                            text = ReplaceFirst(text, textlines[i] + "\r\n", newstr + "\r\n");
                            File.WriteAllText(atomfile, text);
                        }
                        else
                        {
                            foreach (XmlNode node in nodes)
                            {
                                if (node.Attributes["RAM" + romprefix].Value == funcid)
                                {
                                    string strchain = node.Attributes["Description"].Value + " " + node.Attributes["Args"].Value + " " + ((node.Attributes["Description2"] != null) ? node.Attributes["Description2"].Value : "");
                                    if (strchain.Replace(" ", "") == "") strchain = node.Attributes["Source"].Value;
                                    if (strchain.Replace(" ", "") == "") strchain = "Undocumented/Unported";
                                    string newstr = textlines[i] + "\t\t## " + strchain;
                                    newstr = newstr.Replace("\r\n", " | ").Replace("\r", " | ").Replace("\n", " | ").Replace(System.Environment.NewLine, " | ");
                                    text = ReplaceFirst(text, textlines[i] + "\r\n", newstr + "\r\n");
                                    File.WriteAllText(atomfile, text);
                                }
                            }
                        }

                    }
                }
            }

            if (JalFix.Checked)
            {
                int specialfnc = 0;
                if (ID < 1) specialfnc = 4;
                string[] specialfncstr = new[] { "_constructor", "_destructor", "_update", "_draw" };
                string funcname;
                string text = File.ReadAllText(atomfile);
                string[] textlines = File.ReadLines(atomfile).ToArray();
                int bytecount = 0;
                bool countingbytes = false;
                bool secondround = false;
                FindJal:
                for (int i = 0; i < textlines.Count(); i++)
                {
                    // this renames local functions as localfunc_... and global functions as 0x0...
                    if (textlines[i].Contains("jal     func_8"))
                    {
                        funcname = textlines[i].Substring(textlines[i].IndexOf("f"), 13);
                        if (!text.Contains(funcname + ":"))
                        {
                            text = text.Replace("jal     " + funcname, "jal     0x0" + funcname.Substring(6));
                            File.WriteAllText(atomfile, text);

                        }
                        else
                        {
                            text = text.Replace(funcname, "local" + funcname);
                            File.WriteAllText(atomfile, text);
                            textlines = File.ReadLines(atomfile).ToArray();
                        }
                    }
                    if (countingbytes)
                    {
                        bytecount += Regex.Matches(textlines[i], "0x").Count;
                    }
                    if (textlines[i].Contains(".word \\") && specialfnc == 0)
                    {
                        bytecount = 0;
                        countingbytes = true;
                    }
                    // this detects the constructor, destructor, update and draw functions
                    if ((textlines[i].Contains(".word func_8") || specialfnc > 0) && specialfnc < 4 && (bytecount == 0x4 || (secondround && bytecount >= 0x4)))
                    {
                        countingbytes = false;
                        if (textlines[i].Contains(".word func_8"))
                        {
                            funcname = textlines[i].Substring(textlines[i].IndexOf("f"), 13);
                            if (text.Contains(funcname + ":"))
                            {
                                text = text.Replace(funcname, "func" + specialfncstr[specialfnc]);
                                File.WriteAllText(atomfile, text);
                            }
                        }
                        if (!textlines[i].Contains(".byte")) specialfnc++;
                    }
                }
                if (specialfnc == 0 && !secondround)
                {
                    countingbytes = false;
                    bytecount = 0;
                    secondround = true;
                    goto FindJal;
                }





            }
        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static void CreateConsole()
        {
            AllocConsole();

            IntPtr stdHandle = CreateFile(
                "CONOUT$",
                GENERIC_WRITE | GENERIC_READ,
                FILE_SHARE_WRITE,
                0, OPEN_EXISTING, 0, 0
            );

            SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
            StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

          // if (GetConsoleMode(stdHandle, out var cMode))
          //      SetConsoleMode(stdHandle, cMode | 0x0200);

        }

        private void nothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            LoadRom(((ToolStripMenuItem)sender).Text);
            
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Function> OOTfuncs = new List<Function>();
            List<Function> DBGfuncs = new List<Function>();
            List<Function> OOTfuncs_simplified = new List<Function>();
            List<Function> DBGfuncs_simplified = new List<Function>();
            string codeOOT = File.ReadAllText("code1.0.S");
            string[] OOTlines = File.ReadLines("code1.0.S").ToArray();
            string codeDBG = File.ReadAllText("codeDBG.S");
            string[] DBGlines = File.ReadLines("codeDBG.S").ToArray();

            OOTfuncs = GetFunctions(OOTlines, false);
            DBGfuncs = GetFunctions(DBGlines, false);
            OOTfuncs_simplified = GetFunctions(OOTlines,true);
            DBGfuncs_simplified = GetFunctions(DBGlines, true);

            float matches = 0;
            float duplicatematches = 0;

            int index = 0;
            int lastindex = 0;

            for (int i = 0; i < OOTfuncs.Count; i++)
            {
                index = DBGfuncs.FindIndex(x => ((Function)x).lines.SequenceEqual(OOTfuncs[i].lines) && !x.taken);
                lastindex = DBGfuncs.FindLastIndex(x => ((Function)x).lines.SequenceEqual(OOTfuncs[i].lines) && !x.taken);
                if (index != -1)
                {
                    Console.WriteLine(OOTfuncs[i].name + " -> " + DBGfuncs[i].name);
                    DBGfuncs[i].taken = true;
                    DBGfuncs_simplified[i].taken = true;
                    matches++;
                    if (index != lastindex) duplicatematches++;
                }
                else
                {
                    index = DBGfuncs_simplified.FindIndex(x => ((Function)x).lines.SequenceEqual(OOTfuncs_simplified[i].lines) && !x.taken);
                    lastindex = DBGfuncs_simplified.FindLastIndex(x => ((Function)x).lines.SequenceEqual(OOTfuncs_simplified[i].lines) && !x.taken);
                    if (index != -1)
                    {
                        Console.WriteLine(OOTfuncs[i].name + " -> " + DBGfuncs[i].name);
                        DBGfuncs[i].taken = true;
                        DBGfuncs_simplified[i].taken = true;
                        matches++;
                        if (index != lastindex) duplicatematches++;
                    }
                    //  Console.WriteLine(OOTfuncs[i].name + " -> not found");
                }
            }
            Console.WriteLine("Done! Converted the address of " + ((matches/(float)OOTfuncs.Count)*100f) + "% of OOT1.0 functions to OOTDBG");
            Console.WriteLine("Duplicate matches " + ((duplicatematches / (float)OOTfuncs.Count) * 100f) + "%");



        }

        public List<Function> GetFunctions(string[] lines, bool simplified)
        {
            List<Function> ret = new List<Function>();
            for (int i = 0; i < lines.Count(); i++)
            {
                // this renames local functions as localfunc_... and global functions as 0x0...
                if (lines[i].Contains("func_8") && lines[i].Contains(":"))
                {
                    List<string> Lines = new List<string>();
                    string name = lines[i].Replace(":", "");
                    i++;
                    while (i < lines.Count() && lines[i].Length > 2)
                    {
                        if (!lines[i].Contains("func_") && !lines[i].Contains("lbl_") && (!simplified || (simplified && !lines[i].Contains("nop"))))
                        {
                            if (!simplified)
                            {
                                string trueline = lines[i].Replace("\t", "");
                                trueline = trueline.Replace(" ", "");
                                if (trueline.Contains("#")) trueline = trueline.Substring(0, trueline.IndexOf('#'));
                                if (trueline != "") Lines.Add(trueline);
                            }
                            else
                            {
                                string trueline = lines[i].Replace("\t", " ");
                                if (trueline[0] == ' ') trueline = trueline.Substring(1);
                                if (trueline.Contains(" ")) trueline = trueline.Substring(0, trueline.IndexOf(" "));
                                if (trueline.Contains("#")) trueline = trueline.Substring(0, trueline.IndexOf('#'));
                                if (trueline != "") Lines.Add(trueline);
                            }
                        }
                        else if (lines[i].Contains("func_") || lines[i].Contains("lbl_"))
                        {
                            if (!simplified) Lines.Add("DUMMY");
                        }
                        i++;
                    }
                    Lines.Sort();
                    ret.Add(new Function(name, Lines.ToArray()));
                    

                }
            }
            Console.WriteLine("Amount of functions found: " + ret.Count);
            return ret;
        }




        private void findStartDmaTableAdressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Rom files (*.z64;*.rom)|*.z64;*.rom|All Files (*.*)|*.*";
            uint data_romstart = 0;
            uint data_zeroes = 0;
            uint data_endoffset = 0;
            uint data_trueoffset = 0;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<byte> ROM = new List<byte>(File.ReadAllBytes(openFileDialog1.FileName));
                for (int i = 0; i < 0x50000; i++)
                {
                    data_romstart = Helpers.Read32(ROM, i);
                    data_zeroes = Helpers.Read16(ROM, i + 4);
                    data_endoffset = Helpers.Read32(ROM, i + 4);
                    {
                        if (data_romstart == 0 && data_zeroes == 0 && data_endoffset != 0)
                        {
                            data_trueoffset = Helpers.Read32(ROM, i + 0x10);
                            if (data_trueoffset == data_endoffset)
                            {
                                Console.WriteLine("DMA table found at: " + (i + 1).ToString("X8"));
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void getFilesystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Rom files (*.z64;*.rom)|*.z64;*.rom|All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<byte> ROM = new List<byte>(File.ReadAllBytes(openFileDialog1.FileName));
                string[] filenames = File.ReadLines("filenames.txt").ToArray();
                int counter = 0;
                for (int i = 0x0001C110; i < 0x000221F0; i+=0x10)
                //for (int i = 0x0001AE90; i < 0x00020F60; i+=0x10) //gcn usa
                {
                    int startoffset = (int) Helpers.Read32(ROM, i);
                    int endoffset = (int)Helpers.Read32(ROM, i+4);
                    List<byte> data = new List<byte>();
                    if (startoffset == 0) { counter++; continue; }
                    if (filenames[counter] == "[?]") { counter++; continue;}
                    for (int ii = startoffset; ii < endoffset; ii++)
                    {
                        data.Add(ROM[ii]);
                    }
                    //File.Create("extracted/" + filenames[counter]);
                    BinaryWriter sw = new BinaryWriter(File.OpenWrite("extracted/" + filenames[counter]));
                    sw.Write(data.ToArray());
                    sw.Flush();
                    sw.Close();
                    //Console.WriteLine("" + counter + "\t" + Helpers.Read32(ROM,i).ToString("X8") + "\t" + Helpers.Read32(ROM, i+4).ToString("X8") + "\t" + (Helpers.Read32(ROM, i+4)-Helpers.Read32(ROM, i)).ToString("X8"));
                    counter++;
                }
            }
        }

        private void ExportZobj_Click(object sender, EventArgs e)
        {
            string filename = "./Object" + romprefix + "_" + ((int)ObjectID.Value).ToString("X4") + ".zobj";
            
                if (IsFileLocked(filename))
                    MessageBox.Show("File is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));

                    List<Byte> ObjectData = new List<byte>();

                    int offset = (int) (ObjectTableOffset + 0x08 + (ObjectID.Value * 0x08));

                    uint startoffset = Helpers.Read32(ROM, offset);
                    uint endoffset = Helpers.Read32(ROM, offset+4);

                    for (int i = (int)startoffset; i < endoffset; i ++)
                    {
                       ObjectData.Add(ROM[i]);
                    }

                    File.WriteAllBytes(filename, ObjectData.ToArray());
                    MessageBox.Show("Done! File Size: " + ObjectData.Count.ToString("X") + " bytes" + " start offset " + startoffset.ToString("X8"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            

            

        }

        private void ImportZobj_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "ZObj Files (*.zobj*)|*.zobj*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                ObjectPath = openFileDialog1.FileName;

                UpdateWindow();

            }
        }

        private void InjectZobj_Click(object sender, EventArgs e)
        {
            if (IsFileLocked(ObjectPath))
                MessageBox.Show("Zobj is in use... try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (File.Exists(ObjectPath))
                {
                    BinaryWriter BWS = new BinaryWriter(File.OpenWrite(RomPath));
                    BWS.Seek((int)ObjectInjectOffset.Value, SeekOrigin.Begin);

                    var data = new List<byte>(File.ReadAllBytes(ObjectPath));

                    BWS.Write(data.ToArray());

                    List<Byte> ObjectRow = new List<byte>();

                    Helpers.Append32(ref ObjectRow, (uint)ObjectInjectOffset.Value);
                    Helpers.Append32(ref ObjectRow, (uint)(ObjectInjectOffset.Value + data.Count));

                    BWS.Seek((int)(ObjectTableOffset+ 0x08 + (ObjectID.Value * 0x08)), SeekOrigin.Begin);

                    BWS.Write(ObjectRow.ToArray());

                    if (ObjectDMARow.Value != 0)
                    {
                        List<byte> dmatmp = new List<byte>();
                        BWS.Seek((int)(DmaTableStart + (ObjectDMARow.Value * 0x10)), SeekOrigin.Begin);
                        Helpers.Append32(ref dmatmp, (uint)ObjectInjectOffset.Value);
                        Helpers.Append32(ref dmatmp, (uint)(ObjectInjectOffset.Value + data.Count));
                        Helpers.Append32(ref dmatmp, (uint)ObjectInjectOffset.Value);
                        Helpers.Append32(ref dmatmp, 0);
                        BWS.Write(dmatmp.ToArray());
                        Console.WriteLine("Row " + ObjectDMARow.Value + " of DMA table updated");
                    }

                    BWS.Close();

                    MessageBox.Show("Injection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Zobj no longer exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void FindEmptyObjectID_Click(object sender, EventArgs e)
        {
            int offset = ObjectTableOffset + 0x08;
            int incr = 1;
            List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));
            for (int i = offset + 0x08; i < ObjectTableOffset+3208; i += 0x08)
            {
                if (Helpers.Read32(ROM, i) == 0 && Helpers.Read32(ROM, i + 4) == 0)
                {
                    ObjectID.Value = incr;
                    break;
                }
                incr++;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SaveHitbox_Click(object sender, EventArgs e)
        {

        }

        private void functionsToDbgVersion2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<String> DbgFiles = Directory.GetFiles("./Overlays/U0/", "*.S").Select(Path.GetFileName).ToList(); 
            List<String> N0Files = Directory.GetFiles("./Overlays/J0/", "*.S").Select(Path.GetFileName).ToList(); 
            Dictionary<uint,uint> Port = new Dictionary<uint, uint>();

            for (int i = 0; i < N0Files.Count; i++)
            {
                if (!DbgFiles.Contains(N0Files[i])) continue;
                string[] N0lines = File.ReadLines("./Overlays/J0/" + N0Files[i]).ToArray();
                string[] Dbglines = File.ReadLines("./Overlays/U0/" + DbgFiles.Find(x => x == N0Files[i])).ToArray();
                List<Function> N0funcs = GetFunctions(N0lines);
                List<Function> Dbgfuncs = GetFunctions(Dbglines);

                if (N0funcs.Count == Dbgfuncs.Count)
                {
                    for(int w = 0; w < N0funcs.Count; w++)
                    {
                        if (N0funcs[w].funcs.Count != Dbgfuncs[w].funcs.Count) continue;

                        for (int y = 0; y < N0funcs[w].funcs.Count(); y++)
                        {
                            if (!Port.ContainsKey(N0funcs[w].funcs[y]))
                                Port.Add(N0funcs[w].funcs[y], Dbgfuncs[w].funcs[y]);
                        }
                    }
                }


                //  List<uint> N0funcs = new List<uint>();
                //  List<uint> Dbgfuncs = new List<uint>();

                /*

                for (int y = 0; y < N0lines.Count(); y++)
                {
                    if (N0lines[y].Contains("func_8"))
                    {
                        uint funcid = Convert.ToUInt32(N0lines[y].Substring(N0lines[y].IndexOf("f"), 13).Replace("func_", ""), 16);
                        if (funcid < 0x80800000) N0funcs.Add(funcid);
                    }
                }
                for (int y = 0; y < Dbglines.Count(); y++)
                {
                    if (Dbglines[y].Contains("func_8"))
                    {
                        uint funcid = Convert.ToUInt32(Dbglines[y].Substring(Dbglines[y].IndexOf("f"), 13).Replace("func_", ""), 16);
                        if (funcid < 0x80800000) Dbgfuncs.Add(funcid);
                    }
                }

                if (Dbgfuncs.Count == N0funcs.Count)
                {
                    for (int y = 0; y < N0funcs.Count(); y++)
                    {
                        if (!Port.ContainsKey(N0funcs[y]))
                             Port.Add(N0funcs[y], Dbgfuncs[y]);
                    }
                }*/

                /*
                for (int y = 0; y < N0lines.Count() && y < Dbglines.Count(); y++)
                {
                    if (N0lines[y].Contains("func_8"))
                    {
                        uint funcid = Convert.ToUInt32(N0lines[y].Substring(N0lines[y].IndexOf("f"), 13).Replace("func_", ""),16);
                        if (funcid < 0x80800000 && !Port.ContainsKey(funcid) && Dbglines[y].Contains("func_8"))
                        {
                            uint funcid2 = Convert.ToUInt32(Dbglines[y].Substring(Dbglines[y].IndexOf("f"), 13).Replace("func_", ""), 16);
                            if (funcid2 > 0x80800000 || funcid2 < funcid || Port.ContainsValue(funcid2)) continue;

                            Port.Add(funcid,funcid2);
                        }
                    }
                }
                */
            }


            XmlDocument doc = new XmlDocument();
            XmlDocument doc2 = new XmlDocument();
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/ActorFunctionsMMJ0.xml");
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            doc.Load(fs);
            var fileName2 = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/FunctionsDbg.xml");
            FileStream fs2 = new FileStream(fileName2, FileMode.Open, FileAccess.Read);
            doc2.Load(fs2);
            XmlNode doc2table = doc2.SelectSingleNode("Table");
            XmlNodeList nodes = doc.SelectNodes("Table/Function");
            foreach (XmlNode node in nodes)
            {
                XmlAttributeCollection nodeAtt = node.Attributes;
                if (Port.ContainsKey(Convert.ToUInt32(nodeAtt["RAMJ0"].Value,16)))
                {
                    XmlAttribute newAttr = doc.CreateAttribute("RAMU0");
                    newAttr.Value = Port[Convert.ToUInt32(nodeAtt["RAMJ0"].Value, 16)].ToString("X");
                    node.Attributes.InsertBefore(newAttr, nodeAtt["RAMJ0"]);
                  //  nodeAtt["RAMdebug"].Value = Port[Convert.ToUInt32(nodeAtt["RAMdebug"].Value,16)].ToString("X");
                    XmlNode importNode = doc2.ImportNode(node, true);
                    doc2table.AppendChild(importNode);
                }
            }
            doc2.Save("XML/FunctionsU02.xml");
            Console.WriteLine("Number of functions converted: " + Port.Count + " of " + nodes.Count);


        }



        private void listMMjapfunctions(object sender, EventArgs e)
        {
            List<String> J0Files = Directory.GetFiles("./Overlays/J0/", "*.S").Select(Path.GetFileName).ToList();
            List<uint> FuncList = new List<uint>();

            for (int i = 0; i < J0Files.Count; i++)
            {
                string[] J0lines = File.ReadLines("./Overlays/J0/" + J0Files[i]).ToArray();
                List<Function> J0funcs = GetFunctions(J0lines);

                for (int w = 0; w < J0funcs.Count; w++)
                {
                    for (int y = 0; y < J0funcs[w].funcs.Count(); y++)
                    {
                        if (!FuncList.Contains(J0funcs[w].funcs[y]))
                            FuncList.Add(J0funcs[w].funcs[y]);
                    }
                }
                

                
            }


            XmlDocument doc = new XmlDocument();
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/ActorFunctionsMMJ0.xml");
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            doc.Load(fs);
        //    XmlNodeList nodes = doc.SelectNodes("Table/Function");
            foreach(uint func in FuncList)
            {
                XmlNode newnode = doc.CreateElement("Function");
                XmlNode Table = doc.SelectSingleNode("//Table");
                XmlAttribute newAttr = doc.CreateAttribute("RAMJ0");
                newAttr.Value = func.ToString("X8");
                newnode.Attributes.Append(newAttr);

                newAttr = doc.CreateAttribute("Source");
                newAttr.Value = "";
                newnode.Attributes.Append(newAttr);

                newAttr = doc.CreateAttribute("ShortName");
                newAttr.Value = "";
                newnode.Attributes.Append(newAttr);

                newAttr = doc.CreateAttribute("Description");
                newAttr.Value = "";
                newnode.Attributes.Append(newAttr);

                newAttr = doc.CreateAttribute("Args");
                newAttr.Value = "";
                newnode.Attributes.Append(newAttr);

                Table.AppendChild(newnode);

            }
            doc.Save("XML/ActorFunctionsMMJ0-2.xml");
            Console.WriteLine("Number of functions added: " + FuncList.Count);


        }


        public List<Function> GetFunctions(string[] lines)
        {
            List<Function> ret = new List<Function>();
            for (int i = 0; i < lines.Count(); i++)
            {
                // this renames local functions as localfunc_... and global functions as 0x0...
                if (lines[i].Contains("func_8") && lines[i].Contains(":"))
                {
                    List<uint> Funcs = new List<uint>();
                    string name = lines[i].Replace(":", "");
                    i++;
                    while (i < lines.Count() && lines[i].Length > 2)
                    {
                        if (lines[i].Contains("func_8"))
                        {
                            uint funcid = Convert.ToUInt32(lines[i].Substring(lines[i].IndexOf("f"), 13).Replace("func_", ""), 16);
                            if (funcid < 0x80800000) Funcs.Add(funcid);
                        }
                        i++;
                    }
                    
                    ret.Add(new Function(name, Funcs));


                }
            }
          //  Console.WriteLine("Amount of functions found: " + ret.Count);
            return ret;
        }

        private void FindEmptySpace2_Click(object sender, EventArgs e)
        {
            bool notfound = false;
            if (File.Exists(ObjectPath))
            {
                var data = new List<byte>(File.ReadAllBytes(ObjectPath));
                int offset = (int)ObjectInjectOffset.Value;
                offset = 16 * (int)Math.Round(offset / 16.0);
                List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));

                if ((Helpers.Read32(ROM, (int) (ObjectTableOffset + 0x08 + (ObjectID.Value * 0x08))) != 0 && Helpers.Read32(ROM, (int) (ObjectTableOffset + 0x08 + (ObjectID.Value * 0x08) + 0x04)) != 0 && Helpers.Read32(ROM, (int)(ObjectTableOffset + 0x08 + (ObjectID.Value * 0x08) + 0x04)) - Helpers.Read32(ROM, (int)(ObjectTableOffset + 0x08 + (ObjectID.Value * 0x08))) >= data.Count))
                {
                    //new object fits in the ROM of the old object
                    ObjectInjectOffset.Value = Helpers.Read32(ROM, (int) (ObjectTableOffset + 0x08 + (ObjectID.Value * 0x08)));
                    return;
                }


                

                for (int i = offset; i < ROM.Count - data.Count; i += 0x10)
                {
                    notfound = false;
                    for (int ii = 0; ii < data.Count && !notfound; ii += 4)
                    {
                        if (Helpers.Read32(ROM, i + ii) != 0) notfound = true;
                        //Console.WriteLine(ii);
                    }
                    if (!notfound)
                    {
                        ObjectInjectOffset.Value = i;
                        break;
                    }
                }
            }
        }

        public static int FindSongComboItemValue(ComboBox.ObjectCollection items, uint marker)
        {
            foreach (SongItem item in items)
            {
                if (Convert.ToUInt32(item.Value.ToString()) == marker) return items.IndexOf(item);
            }
            return 0;
        }

        private void RandomDropTableEditorButton_Click(object sender, EventArgs e)
        {
            RandomDropTableEditor randomdroptableeditor = new RandomDropTableEditor(RandomDropTable, RomPath);
            if (!randomdroptableeditor.IsDisposed)
                randomdroptableeditor.ShowDialog();
        }

        private void getZobjInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
       
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "ZObj Files (*.zobj*)|*.zobj*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                int bank = 0x06;
                byte[] zobj = File.ReadAllBytes(openFileDialog1.FileName);
                List<byte> zobjlist = new List<byte>(zobj);
                uint limb_count = 0;
                int hirearchyfound = 0;

                for (int i = 0; i < zobj.Length; i += 0x4)
                {
                    if (zobj[i] == bank && zobj[i + 5] == 0x00 && zobj[i + 6] == 0x00 && zobj[i + 7] == 0x00)
                    {
                        int check = Convert.ToInt32(string.Format("{0:X2}{1:X2}{2:X2}", zobj[i + 1], zobj[i + 2], zobj[i + 3]), 16);
                        int check_prev = Convert.ToInt32(string.Format("{0:X2}{1:X2}{2:X2}", zobj[i - 3], zobj[i - 2], zobj[i - 1]), 16);
                        // if (key == 0xDD) Console.WriteLine("Likelike: " + (check - check_prev).ToString("X"));
                        if (check - check_prev == 0x0C || check - check_prev == 0x10)
                        {
                            int hirearchyoffset = (int)Helpers.Read24S(zobjlist, i + 1);
                            Console.WriteLine("Hirearchy #" + hirearchyfound + ": " + hirearchyoffset.ToString("X8"));
                            hirearchyfound++;
                                
                        limb_count = zobjlist[i + 4];
                        int animrotvaloffset = 0x00;
                        int animrotindexoffset = 0x00;


                        for (int ii = 0; ii < zobj.Length; ii += 4)
                        {
                            if (!(ii + 4 > zobj.GetUpperBound(0)))
                            {
                                if (zobj[ii + 2] == 0x00 && zobj[ii + 3] == 0x00 && zobj[ii + 4] == bank && zobj[ii + 8] == bank && zobj[ii + 14] == 0x00 && zobj[ii + 15] == 0x00)
                                {

                                //    Console.WriteLine(string.Format("06{1:X6}", ii));
                                    animrotvaloffset = Helpers.Read24S(zobjlist, ii + 5);
                                    animrotindexoffset = Helpers.Read24S(zobjlist, ii + 9);
                                    break;

                                }
                            }
                        }
                        //  Console.WriteLine("Actor: " + key.ToString("X"));
                        //  Console.WriteLine("Hierarchy Header: 0x{0}", (i).ToString("X6"));
                        Console.WriteLine("{0:D2} limbs!", limb_count);
                        int counter = 0;
                        short[] prevlimb = new short[] { 0, 0, 0 };
   

                        }

                    }
                }
                
            }
        }

        private void correctAndDetailSFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    CorrectSource(openFileDialog1.FileName);
                    MessageBox.Show("Corrected " + openFileDialog1.FileName + " source", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Something failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(exception);
                }


            ;
            }
        }

        private void convertFileToMipsVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<byte> file = new List<byte>(File.ReadAllBytes(openFileDialog1.FileName));

                string variable = "var_NAME: .word \\\n";

                int column = 0;

                for (int i = 0; i < file.Count-1; i+=4)
                {
                    variable += "0x" + Helpers.Read32(file, i).ToString("X8") + ", ";
                    column++;
                    if (column == 4)
                    {
                        variable += "\\\n";
                        column = 0;
                    }
                }

                if (!variable.Contains(","))
                {
                    MessageBox.Show("wut? file too small", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    variable = variable.Substring(0, variable.LastIndexOf(","));

                    Clipboard.SetText(variable);

                    MessageBox.Show("Variable size copied to clipboard! Size:" + file.Count, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }




            }
        }

        private void convertDListToMipsDListToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "DList File (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<byte> file = new List<byte>(File.ReadAllBytes(openFileDialog1.FileName));

                uint opcode = 0;

                for (int i = 0; i < file.Count - 1; i += 4)
                {
                    opcode = Helpers.Read32(file, i);

                }




            }

        }

        private void portDebugRomActorTo10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Source file (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // List<byte> file = new List<byte>(File.ReadAllBytes(openFileDialog1.FileName));

                string file = openFileDialog1.FileName;


                string[] textlines = File.ReadLines(openFileDialog1.FileName).ToArray();
                string text = File.ReadAllText(openFileDialog1.FileName);
                XmlDocument funcdoc = new XmlDocument();
                var funcfileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),  @"XML/ActorFunctionsDbgN0.xml" );
                FileStream funcfs = new FileStream(funcfileName, FileMode.Open, FileAccess.Read);
                funcdoc.Load(funcfs);
                XmlNodeList nodes = funcdoc.SelectNodes("Table/Function");
                for (int i = 0; i < textlines.Count(); i++)
                {
                    if (textlines[i].Contains("jal") && (textlines[i].Contains("0x0")))
                    {
                        string orgfuncid = textlines[i].Substring(textlines[i].IndexOf("0x")+3, 7);
                        string funcid = "8" + orgfuncid;
                        Console.WriteLine("funcid " + funcid);

                        if (Convert.ToUInt32(funcid, 16) < 0x80800000)
                        {
                            foreach (XmlNode node in nodes)
                            {
                                if (node.Attributes["RAMDBGMQ"].Value == funcid.ToUpper())
                                {
                                    string newstr = textlines[i];
                                    string n0func = "TEMP4TEMP4TEMP" + node.Attributes["RAMN0"].Value.Substring(1);
                                    newstr = newstr.Replace("0" + orgfuncid, n0func);
                                    text = ReplaceFirst(text, textlines[i] + "\r\n", newstr + "\r\n");
                                    File.WriteAllText(file, text);
                                }
                            }
                        }

                    }
                }
                text = text.Replace("TEMP4TEMP4TEMP", "0");
                File.WriteAllText(file, text);

            }
        }

        private void downloadZ64ovlGithubToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var client = new WebClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                client.DownloadFile("https://github.com/z64me/z64ovl_archived/archive/master.zip", "gcc\\mips64\\include\\z64ovl.zip");
                if (Directory.Exists(@"gcc\mips64\include\z64ovl_archived-master"))
                {
                    //Directory.Delete(@"gcc\mips64\include\z64ovl-master", true);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C rmdir /Q /S gcc\\mips64\\include\\z64ovl_archived-master";
                    process.StartInfo = startInfo;
                    process.Start();
                }
                if (Directory.Exists(@"gcc\mips64\include\z64ovl"))
                {
                    //Directory.Delete(@"gcc\mips64\include\z64ovl-master", true);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C rmdir /Q /S gcc\\mips64\\include\\z64ovl";
                    process.StartInfo = startInfo;
                    process.Start();
                }

                while (Directory.Exists(@"gcc\mips64\include\z64ovl-master") || Directory.Exists(@"gcc\mips64\include\z64ovl"))
                {
                    // do nothing
                }

                using (var zip = ZipFile.Read("gcc\\mips64\\include\\z64ovl.zip"))
                    zip.ExtractAll("gcc\\mips64\\include\\", ExtractExistingFileAction.Throw);
                

            }
            //File.Delete("gcc\\mips64\\include\\z64ovl.zip");
            //  Directory.Delete(@"gcc\mips64\include\z64ovl", true);

            //  Directory.Move(@"gcc\mips64\include\z64ovl-master", @"gcc\mips64\include\z64ovl");

            Directory.Move(@"gcc\mips64\include\z64ovl_archived-master", @"gcc\mips64\include\z64ovl");

            MessageBox.Show("z64ovl Updated!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
           // downloadZ64ovlGithubToolStripMenuItem.Enabled = false;
        }

        private void WarningCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            IgnoreWarnings = WarningCheckbox.Checked;
        }

        private void detectTexturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "ZObj Files (*.zobj*)|*.zobj*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

               List<byte> data = new List<byte>(File.ReadAllBytes(openFileDialog1.FileName));
               List<Texture> textures = new List<Texture>();

                uint paletteoffset = 0;
                uint textureoffset = 0;
                byte fmt = 0;
                byte siz = 0;
                ushort width = 0;
                ushort height = 0;
                int filesize = 0;

                for (int i = 0; i < data.Count - 8; i += 4)
                {
                  //  uint vertexoffset = (uint)Helpers.Read24S(data, i + 5);

                    //if (data[i] == 0x01 && (data[i + 1] & 0xF0) == 0x00 && (data[i + 2] & 0x0F) == 0x00 && data[i + 4] == 0x06 && vertexoffset < data.Count && vertexoffset != 0)
                    if (data[i] == 0xD7 && Helpers.Read32(data,i) == 0xD7000002)
                    {
                        //D7 command detected

                        i += 8;

                        for (; i < data.Count; i += 8)
                        {
                            if (data[i] == 0xFD && data[i+4] > 0x00) //code special texture
                            {
                                if (textureoffset != 0)
                                {
                                    if (textures.FindIndex(x => x.offset == textureoffset) == -1)
                                        textures.Add(new Texture(textureoffset, fmt, siz, width, height, paletteoffset));
                                    paletteoffset = 0;
                                }
                                // textureoffset = (uint)Helpers.Read24S(data, i + 5); UNCOMMENT
                                textureoffset = (uint)Helpers.Read32(data, i + 4);
                            }
                            else if (data[i] == 0xF5)
                            {
                                fmt = (byte)( ((data[i + 1]) & 0b11100000) >> 5);
                                siz = (byte)(((data[i + 1]) & 0b00011000) >> 3);
                            }
                            else if (data[i] == 0xF3)
                            {
                                filesize = (int) ((Helpers.Read32(data, i + 4) & 0x00FFF000) >> 12);
                            }
                            else if (data[i] == 0xF0)
                            {
                                paletteoffset = textureoffset;
                                textureoffset = 0;
                                width = 0;
                                height = 0;
                            }
                            else if (data[i] == 0xF2)
                            {
                                width = (ushort) ((Helpers.Read32(data, i + 4) & 0x00FFF000) >> 12);
                                height = (ushort)((Helpers.Read32(data, i + 4) & 0x00000FFF));
                                width = (ushort) ((width + 4) / 4);
                                height = (ushort)((height + 4) / 4);
                            }
                            else if (data[i] == 0xDF)
                            {
                                if (textureoffset != 0)
                                {
                                    if (textures.FindIndex(x => x.offset == textureoffset) == -1)
                                        textures.Add(new Texture(textureoffset, fmt, siz, width, height, paletteoffset));
                                    textureoffset = 0;
                                    paletteoffset = 0;
                                }
                            }

                        }



                    }

                    /*
                   uint textureoffset = (uint) Helpers.Read24S(data, i + 5);
                   if(data[i] == 0xFD && data[i+2] == 0x00 && data[i + 3] == 0x00 && data[i+4] == 0x06 && textureoffset < data.Count)
                   {
                       Texture texture = new Texture();
                     //  byte fmt = (byte)( ((data[i + 1]) & 0b11100000) >> 5);
                     //  byte siz = (byte)(((data[i + 1]) & 0b00011000) >> 3);

                       if (fmt < 5 && siz < 4 && textures.FindIndex(x => x.offset == textureoffset) == -1)
                       {
                           textures.Add(new Texture(textureoffset,fmt,siz));
                       }

                      
                }
                 */
                }

                textures = textures.OrderBy(x => x.offset).ToList();

                Console.WriteLine("Textures found in " + Path.GetFileName(openFileDialog1.FileName) + ": " + textures.Count);

                foreach(Texture texture in textures)
                {
                    string[] fmtt = new[] { "RGBA", "YUV", "CI", "IA", "I" };
                    string[] sizz = new[] { "4", "8", "16", "32" };
                    Console.WriteLine("Offset: " + texture.offset.ToString("X8") + " Format: " + fmtt[texture.fmt] + sizz[texture.siz] + " " + texture.width + "x" + texture.height + ((texture.paletteoffset != 0) ? " Palette: " + texture.paletteoffset.ToString("X8") : ""));
                }
       

            }
        }

        private void ActorID_ValueChanged(object sender, EventArgs e)
        {
            if (ActorID.Value < 1)
            {
                vramoffsetlabel.Text = "VRAM start offset (Click find empty VRAM!)";
                SpecialOverlayLabel.Visible = true;
                if (ActorID.Value == 0)
                {
                    SpecialOverlayLabel.Text = "Link";
                }
                else if (ActorID.Value == -1)
                {
                    SpecialOverlayLabel.Text = "Pause Menu  ";
                }
            }
            else
            {
                vramoffsetlabel.Text = "VRAM start offset (OPTIONAL)";
                SpecialOverlayLabel.Visible = false;
            }
        }

        private void DecActorID_ValueChanged(object sender, EventArgs e)
        {
            if (DecActorID.Value < 1)
            {
                SpecialOverlayLabel2.Visible = true;
                if (DecActorID.Value == 0)
                {
                    SpecialOverlayLabel2.Text = "Link";
                }
                else if (DecActorID.Value == -1)
                {
                    SpecialOverlayLabel2.Text = "Pause Menu  ";
                }
            }
            else
            {
                SpecialOverlayLabel2.Visible = false;
            }
        }

        private void SendTozzrp_Click(object sender, EventArgs e)
        {
            if (ActorID.Value < -1)
            {
                MessageBox.Show("This overlay is not supported in zzromtool yet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "zzromtool/zzrpl project file (*.zzrp;*.zzrpl)|*.zzrp;*.zzrpl|All Files (*.*)|*.*";

            if ((!globalzzrpmode && openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) || globalzzrpmode)
            {
                if (globalzzrpmode) openFileDialog1.FileName = zzrpglobalpath;

                string Filepath = OverlayPath.Substring(0, LastIndexOf(OverlayPath, ".")) + ".ovl";

                string zzrppath = Path.GetDirectoryName(openFileDialog1.FileName);

                bool iszzrpl = false;

                string ext = openFileDialog1.FileName.Substring(LastIndexOf(openFileDialog1.FileName, "."));
                if (ext.Contains("zzrpl")) iszzrpl = true;

                string newdir = "";

                StreamWriter sw;

                if (ActorID.Value > 0)
                {

                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(zzrppath + "/actor/");
                    DirectoryInfo[] dirsInDir = hdDirectoryInWhichToSearch.GetDirectories("" + ActorID.Value + "*.*");
                    foreach (DirectoryInfo dir in dirsInDir)
                    {
                        string test = dir.Name;
                        if (test.Contains(" ")) test = test.Substring(0, test.IndexOf(" "));

                        int tmp = 0;
                        if (Int32.TryParse(test, out tmp))
                        {
                            if (tmp == ActorID.Value)
                            {
                                Directory.Delete(dir.FullName, true);
                                Console.WriteLine("directory " + dir.FullName + " deleted");
                            }
                        }

                    }

                    newdir = zzrppath + "/actor/" + ActorID.Value + " - 0x" + ((int)ActorID.Value).ToString("X4") + " " + Path.GetFileNameWithoutExtension(OverlayPath);

                    Directory.CreateDirectory(newdir);
                    Console.WriteLine("directory " + newdir + " created");
                    File.Copy(Filepath, newdir + "/actor.zovl");


                }
                else if (ActorID.Value == 0)
                {
                    newdir = zzrppath + "/system";
                    if (iszzrpl)
                    {
                        newdir = newdir + "/overlay/ovl_player_actor";
                        
                    }
                    else
                    {
                        if (File.Exists(newdir + "/ovl_player_actor.zovl"))
                            File.Delete(newdir + "/ovl_player_actor.zovl");
                    }

                    File.Copy(Filepath, newdir + "/ovl_player_actor.zovl");
                }
                else if (ActorID.Value == -1)
                {
                    newdir = zzrppath + "/system";
                    if (iszzrpl)
                    {
                        newdir = newdir + "/ovl_kaleido_scope";
                    }
                    else
                    {
                        if (File.Exists(newdir + "/ovl_kaleido_scope.zovl"))
                            File.Delete(newdir + "/ovl_kaleido_scope.zovl");
                    }

                    File.Copy(Filepath, newdir + "/ovl_kaleido_scope.zovl");
                }

                bool initvarsfound = false;


                if (File.Exists("output.txt"))
                {
                 //   Console.WriteLine("output.txt");

                    string[] textlines = File.ReadLines("output.txt").ToArray();

                    if (ActorID.Value > 0)
                    {

                        for (int i = 0; i < textlines.Count(); i++)
                        {
                            if ((textlines[i].Contains(" init_vars", 1) || textlines[i].Contains(" initvars", 1)) && textlines[i].Contains(".text"))
                            {
                                uint initvars = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                initvars += (uint)VRAM.Value;

                                File.Create(newdir + "/conf.txt").Close();
                                sw = new StreamWriter(newdir + "/conf.txt");
                                sw.WriteLine("Allocation    0x0000\nVRAM    0x" + ((uint)(VRAM.Value)).ToString("X8") + "\nivar    0x" + ((uint)(initvars)).ToString("X8"));
                                sw.Close();

                                initvarsfound = true;


                                // Console.WriteLine("Initvars found in output.txt at " + ((uint)(initvars)).ToString("X8"));

                                break;
                            }


                        }

                    }

                    else if (ActorID.Value == 0) // LINK
                    {

                        uint init = 0, dest = 0, update = 0, draw = 0;
                        // constructor destructor init update dest draw init_vars initvars
                        for (int i = 0; i < textlines.Count(); i++)
                        {

                            if ((textlines[i].Contains(" constructor", 1) || textlines[i].Contains(" init", 1) || textlines[i].Contains(" func_constructor", 1) || textlines[i].Contains(" func_init", 1)) && textlines[i].Contains(".text"))
                            {
                                init = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                init += (uint)VRAM.Value;




                            }
                            else if ((textlines[i].Contains(" destructor", 1) || textlines[i].Contains(" dest", 1) || textlines[i].Contains(" func_destructor", 1) || textlines[i].Contains(" func_dest", 1)) && textlines[i].Contains(".text"))
                            {
                                dest = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                dest += (uint)VRAM.Value;


                            }
                            else if ((textlines[i].Contains(" update", 1) || textlines[i].Contains(" func_update", 1) || textlines[i].Contains(" main", 1) || textlines[i].Contains(" func_main", 1)) && textlines[i].Contains(".text"))
                            {
                                update = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                update += (uint)VRAM.Value;


                            }
                            else if ((textlines[i].Contains(" draw", 1) || textlines[i].Contains(" func_draw", 1)) && textlines[i].Contains(".text"))
                            {
                                draw = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                draw += (uint)VRAM.Value;


                            }


                        }

                        if (init != 0)
                        {
                            if (iszzrpl)
                            {
                                File.Create(newdir + "/conf.txt").Close();
                                sw = new StreamWriter(newdir + "/conf.txt");
                            }
                            else
                            {
                                File.Create(newdir + "/ovl_player_actor.txt").Close();
                                sw = new StreamWriter(newdir + "/ovl_player_actor.txt");
                            }

                            sw.WriteLine("init    0x" + ((uint)(init)).ToString("X8") + "\n" + "dest    0x" + ((uint)(dest)).ToString("X8") + "\n" + "main    0x" + ((uint)(update)).ToString("X8") + "\n" + "draw    0x" + ((uint)(draw)).ToString("X8") + "\n");
                            sw.Close();

                        }
                        else
                        {
                            MessageBox.Show("Main functions not found, make sure you have atleast init, dest, update, draw", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (ActorID.Value == -1)
                    {

                        uint init = 0, dest = 0, update = 0, draw = 0;
                        // constructor destructor init update dest draw init_vars initvars
                        for (int i = 0; i < textlines.Count(); i++)
                        {

                            if ((textlines[i].Contains(" constructor", 1) || textlines[i].Contains(" init", 1) || textlines[i].Contains(" func_constructor", 1) || textlines[i].Contains(" func_init", 1)) && textlines[i].Contains(".text"))
                            {
                                init = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                init += (uint)VRAM.Value;




                            }
                            else if ((textlines[i].Contains(" draw", 1) || textlines[i].Contains(" func_draw", 1)) && textlines[i].Contains(".text"))
                            {
                                draw = Convert.ToUInt32(textlines[i].Substring(0, 8), 16);

                                draw += (uint)VRAM.Value;


                            }


                        }

                        if (init != 0 && draw != 0)
                        {
                            if (iszzrpl)
                            {
                                File.Create(newdir + "/conf.txt").Close();
                                sw = new StreamWriter(newdir + "/conf.txt");
                            }
                            else
                            {
                                File.Create(newdir + "/ovl_kaleido_scope.txt").Close();
                                sw = new StreamWriter(newdir + "/ovl_kaleido_scope.txt");
                            }
                            sw.WriteLine("init    0x" + ((uint)(init)).ToString("X8") + "\n" + "draw    0x" + ((uint)(draw)).ToString("X8") + "\n");
                            sw.Close();

                        }
                        else
                        {
                            MessageBox.Show("Main functions not found, make sure you have atleast init and draw", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                  
                    
                   // initvars -= 0x80800000;
                   // initvars += (uint)(VRAM.Value);
                }
                if (ActorID.Value > 0 && !initvarsfound)
                {
                    var data = new List<byte>(File.ReadAllBytes(Filepath));
                    uint VRAMstart = (uint)VRAM.Value;
                    uint VRAMend = (uint)(VRAM.Value + data.Count + bss);

                    uint initvars = 999999;
                    // Find Initialization Variables
                    for (int i = 0; i < (data.Count - 0x20); i += 4)
                    {
                        // ushort init_aid = Helpers.Read16(data, i + 0x00);
                        ushort init_oid = Helpers.Read16(data, i + 0x08);
                        ushort init_pad = Helpers.Read16(data, i + 0x0A);
                        ushort instancesize = Helpers.Read16(data, i + 0x0E);
                        uint init_func_init = Helpers.Read32(data, i + 0x10);
                        uint init_func_dest = Helpers.Read32(data, i + 0x14);
                        uint init_func_main = Helpers.Read32(data, i + 0x18);
                        uint init_func_draw = Helpers.Read32(data, i + 0x1C);
                        if (
                            ((init_oid > 0x0000) && (init_oid < 0x0300)) &&
                            (init_pad == 0x0000 || init_pad == 0xBEEF) &&
                            (instancesize >= 0x013C) &&
                            (((init_func_init >= VRAMstart) && init_func_init <= VRAMend) &&
                            ((init_func_dest >= VRAMstart && init_func_dest <= VRAMend) || init_func_dest == 0x00000000) &&
                            ((init_func_main >= VRAMstart && init_func_main <= VRAMend) || init_func_main == 0x00000000) &&
                            ((init_func_draw >= VRAMstart && init_func_draw <= VRAMend) || init_func_draw == 0x00000000)))
                        {

                            initvars = (uint)i + VRAMstart;

       


                            File.Create(newdir + "/conf.txt").Close();
                            sw = new StreamWriter(newdir + "/conf.txt");
                            sw.WriteLine("Allocation    0x0000" + ((uint)(ActorAllocation.Value)).ToString("X4") + "\nVRAM    0x" + ((uint)(VRAM.Value)).ToString("X8") + "\nivar    0x" +((uint)(initvars)).ToString("X8"));
                            sw.Close();

                            break;
                        }
                    }


                }

                MessageBox.Show("File sent to " + ((iszzrpl) ? "zzrpl" : "zzrp") + " project successfully! \n" + newdir, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void FindOriginalRowButton_Click(object sender, EventArgs e)
        {
            int offset = (int) (OverlayTableOffset + (0x20 * ActorID.Value));
            int incr = 0;
            List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));

            uint startoffset = Helpers.Read32(ROM, offset);
            uint endoffset = Helpers.Read32(ROM, offset+4);

            offset = DmaTableStart;

            DMARow.Value = 0;
            DMARowLabel.Visible = true;

            for (int i = offset; i < DmaTableEnd; i += 0x10)
            {
                if (Helpers.Read32(ROM, i) == startoffset && Helpers.Read32(ROM, i + 4) == endoffset && Helpers.Read32(ROM, i + 8) == startoffset && Helpers.Read32(ROM, i + 12) == 0)
                {
                    DMARow.Value = incr;
                    DMARowLabel.Visible = false;
                    break;
                }
                incr++;
            }
        }

        private void DMARow_ValueChanged(object sender, EventArgs e)
        {
                DMARowLabel.Visible = (DMARow.Value == 0);
        }

        private void ObjectDMARow_ValueChanged(object sender, EventArgs e)
        {
            ObjectDMARowLabel.Visible = (ObjectDMARow.Value == 0);
        }

        private void ObjectFindOriginalRowButton_Click(object sender, EventArgs e)
        {
            int offset = (int)(ObjectTableOffset + (0x8 * ObjectID.Value));
            int incr = 0;
            List<byte> ROM = new List<byte>(File.ReadAllBytes(RomPath));

            uint startoffset = Helpers.Read32(ROM, offset);
            uint endoffset = Helpers.Read32(ROM, offset + 4);

            offset = DmaTableStart;

            ObjectDMARow.Value = 0;
            ObjectDMARowLabel.Visible = true;

            for (int i = offset; i < DmaTableEnd; i += 0x10)
            {
                if (Helpers.Read32(ROM, i) == startoffset && Helpers.Read32(ROM, i + 4) == endoffset && Helpers.Read32(ROM, i + 8) == startoffset && Helpers.Read32(ROM, i + 12) == 0)
                {
                    ObjectDMARow.Value = incr;
                    ObjectDMARowLabel.Visible = false;
                    break;
                }
                incr++;
            }
        }

        private void ObjectSendTozzrp_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "zzromtool/zzrpl project file (*.zzrp;*.zzrpl)|*.zzrp;*.zzrpl|All Files (*.*)|*.*";

            if ((!globalzzrpmode && openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) || globalzzrpmode)
            {
                if (globalzzrpmode) openFileDialog1.FileName = zzrpglobalpath;

                string Filepath = openFileDialog1.FileName;

                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Path.GetDirectoryName(Filepath) + "/object/");
                DirectoryInfo[] dirsInDir = hdDirectoryInWhichToSearch.GetDirectories("" + ObjectID.Value + "*.*");
                foreach (DirectoryInfo dir in dirsInDir)
                {
                    string test = dir.Name;
                    if (test.Contains(" ")) test = test.Substring(0, test.IndexOf(" "));

                    int tmp = 0;
                    if (Int32.TryParse(test, out tmp))
                    {
                        if (tmp == ObjectID.Value)
                        {
                            Directory.Delete(dir.FullName, true);
                            Console.WriteLine("directory " + dir.FullName + " deleted");
                        }
                    }

                }

                string newdir = Path.GetDirectoryName(Filepath) + "/object/" + ObjectID.Value + " - 0x" + ((int)ObjectID.Value).ToString("X4") + " " + Path.GetFileNameWithoutExtension(ObjectPath);

                Directory.CreateDirectory(newdir);
                Console.WriteLine("directory " + newdir + " created");
                File.Copy(ObjectPath, newdir + "/zobj.zobj");

            }
        }

        private void debugGenerateAtomFuncsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Create("atomfuncs.txt").Close();
            StreamWriter sw = new StreamWriter("atomfuncs.txt");

            sw.WriteLine("[\n");
            XmlDocument funcdoc = new XmlDocument();
            var funcfileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), (romgame == "OOT") ? @"XML/ActorFunctionsDbgN0.xml" : @"XML/ActorFunctionsMMU0J0.xml");
            FileStream funcfs = new FileStream(funcfileName, FileMode.Open, FileAccess.Read);
            funcdoc.Load(funcfs);
            XmlNodeList nodes = funcdoc.SelectNodes("Table/Function");

            foreach (XmlNode node in nodes)
            {
                string name = (node.Attributes["ShortName"].Value == "") ? "null" : "\"" + node.Attributes["ShortName"].Value + "\"";
                string description = (node.Attributes["Description"].Value == "") ? "null" : "\"" + node.Attributes["Description"].Value + "\"";
                string description2 = "null";
                string args = (node.Attributes["Args"].Value == "") ? "null" : "\"" + node.Attributes["Args"].Value + "\"";

                sw.WriteLine("  {\r\n    \"Addr\": \"" + node.Attributes["RAMDBGMQ"].Value + "\",\r\n    \"Name\": " + name + ",\r\n    \"Desc\": " + description + ",\r\n    \"Desc2\": " + description2 + ",\r\n    \"Args\": " + args + "\r\n  },");


            }
            sw.WriteLine("\n]");
            sw.Close();

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //IO.Export<Settings>(settings, "Settings.xml");
        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.Usez64hdr = UseZ64hdr.Checked;
            settings.DisableCWarnings = WarningCheckbox.Checked;
            IO.Export<Settings>(settings, "Settings.xml");
        }

        private void restoreAllSettingsToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings = new Settings();
            IO.Export<Settings>(settings, "Settings.xml");
        }

        private void changeMipsCompileFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string val = Interaction.InputBox("Enter MIPS compile flags", "MIPS Compile flags", settings.MipsCompileFlags);
            if (val != "")
                settings.MipsCompileFlags = val;

        }

        private void chanceCCompileFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string val = Interaction.InputBox("Enter C compile flags", "C Compile flags", settings.CCompileFlags);
            if (val != "")
                settings.CCompileFlags = val;
        }

        private void ZobjAnimationCopyButton_Click(object sender, EventArgs e)
        {
            CopyAnimationsForm copyanimation = new CopyAnimationsForm();
            if (!copyanimation.IsDisposed)
                copyanimation.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                client.DownloadFile("https://github.com/z64me/z64ovl/archive/master.zip", "gcc\\mips64\\include\\z64ovl.zip");
                if (Directory.Exists(@"gcc\mips64\include\z64ovl-master"))
                {
                    //Directory.Delete(@"gcc\mips64\include\z64ovl-master", true);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C rmdir /Q /S gcc\\mips64\\include\\z64ovl-master";
                    process.StartInfo = startInfo;
                    process.Start();
                }
                if (Directory.Exists(@"gcc\mips64\include\z64ovl"))
                {
                    //Directory.Delete(@"gcc\mips64\include\z64ovl-master", true);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C rmdir /Q /S gcc\\mips64\\include\\z64ovl";
                    process.StartInfo = startInfo;
                    process.Start();
                }

                while(Directory.Exists(@"gcc\mips64\include\z64ovl-master") || Directory.Exists(@"gcc\mips64\include\z64ovl"))
                {
                    // do nothing
                }

                using (var zip = ZipFile.Read("gcc\\mips64\\include\\z64ovl.zip"))
                    zip.ExtractAll("gcc\\mips64\\include\\", ExtractExistingFileAction.Throw);


            }
          //  File.Delete("gcc\\mips64\\include\\z64ovl.zip");
            //  Directory.Delete(@"gcc\mips64\include\z64ovl", true);

            //  Directory.Move(@"gcc\mips64\include\z64ovl-master", @"gcc\mips64\include\z64ovl");

            Directory.Move(@"gcc\mips64\include\z64ovl-master", @"gcc\mips64\include\z64ovl");

            MessageBox.Show("z64ovl Updated!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // downloadZ64ovlGithubToolStripMenuItem.Enabled = false;
        }

        private void port10todebugactortoolstrip_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Source file (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // List<byte> file = new List<byte>(File.ReadAllBytes(openFileDialog1.FileName));

                string file = openFileDialog1.FileName;


                string[] textlines = File.ReadLines(openFileDialog1.FileName).ToArray();
                string text = File.ReadAllText(openFileDialog1.FileName);
                XmlDocument funcdoc = new XmlDocument();
                var funcfileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"XML/ActorFunctionsDbgN0.xml");
                FileStream funcfs = new FileStream(funcfileName, FileMode.Open, FileAccess.Read);
                funcdoc.Load(funcfs);
                XmlNodeList nodes = funcdoc.SelectNodes("Table/Function");
                for (int i = 0; i < textlines.Count(); i++)
                {
                    if (textlines[i].Contains("jal") && (textlines[i].Contains("0x0")))
                    {
                        string orgfuncid = textlines[i].Substring(textlines[i].IndexOf("0x") + 3, 7);
                        string funcid = "8" + orgfuncid;
                        Console.WriteLine("funcid " + funcid);

                        if (Convert.ToUInt32(funcid, 16) < 0x80800000)
                        {
                            foreach (XmlNode node in nodes)
                            {
                                if (node.Attributes["RAMN0"].Value == funcid.ToUpper())
                                {
                                    
                                    string newstr = textlines[i];
                                    string n0func = "TEMP4TEMP4TEMP" + node.Attributes["RAMDBGMQ"].Value.Substring(1);
                                    newstr = newstr.Replace("0" + orgfuncid, n0func);
                                    text = ReplaceFirst(text, textlines[i] + "\r\n", newstr + "\r\n");
                                    File.WriteAllText(file, text);
                                }
                            }
                        }

                    }
                }
                text = text.Replace("TEMP4TEMP4TEMP", "0");
                File.WriteAllText(file, text);

            }
        }

        private void Downloadz64hdr_Click(object sender, EventArgs e)
        {

            using (var client = new WebClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                client.DownloadFile("https://github.com/turpaan64/z64hdr/archive/refs/heads/main.zip", "gcc\\mips64\\include\\z64hdr.zip");
                if (Directory.Exists(@"gcc\mips64\include\z64hdr-main"))
                {
                    //Directory.Delete(@"gcc\mips64\include\z64ovl-master", true);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C rmdir /Q /S gcc\\mips64\\include\\z64hdr-main";
                    process.StartInfo = startInfo;
                    process.Start();
                }
                if (Directory.Exists(@"gcc\mips64\include\z64hdr"))
                {
                    //Directory.Delete(@"gcc\mips64\include\z64ovl-master", true);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C rmdir /Q /S gcc\\mips64\\include\\z64hdr";
                    process.StartInfo = startInfo;
                    process.Start();
                }

                while (Directory.Exists(@"gcc\mips64\include\z64hdr-main") || Directory.Exists(@"gcc\mips64\include\z64hdr"))
                {
                    // do nothing
                }

                using (var zip = ZipFile.Read("gcc\\mips64\\include\\z64hdr.zip"))
                    zip.ExtractAll("gcc\\mips64\\include\\", ExtractExistingFileAction.Throw);


            }


            Directory.Move(@"gcc\mips64\include\z64hdr-main", @"gcc\mips64\include\z64hdr");

            MessageBox.Show("z64hdr Updated!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
  
        }
    }

    public class Function
    {
        public string name;
        public string[] lines;
        public bool taken = false;
        public List<uint> funcs = new List<uint>();
        public Function(string _name, string[] _lines)
        {
            name = _name;
            lines = _lines;
            
        }
        public Function(string _name, List<uint> _funcs)
        {
            name = _name;
            funcs = _funcs;

        }
    }

    public class Hitbox
    {
        public byte effect, damage, size, hookshotable, unknownA, unknownB, unknownC, unknownD;
        public int direction1, direction2;

    }

    public class SongItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }


    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, int comp)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    public class Texture
    {
        public uint offset;
        public byte fmt;
        public byte siz;
        public ushort width;
        public ushort height;
        public uint paletteoffset;

        public Texture()
        {
        }

        public Texture(uint _offset, byte _fmt, byte _siz, ushort _width, ushort _height, uint _paletteoffset)
        {
            offset = _offset;
            fmt = _fmt;
            siz = _siz;
            width = _width;
            height = _height;
            paletteoffset = _paletteoffset;
        }

        /*
        G_IM_FMT_RGBA = 0
        G_IM_FMT_YUV = 1
        G_IM_FMT_CI = 2
        G_IM_FMT_IA = 3
        G_IM_FMT_I = 4


        G_IM_SIZ_4b = 0
        G_IM_SIZ_8b = 1
        G_IM_SIZ_16b = 2
        G_IM_SIZ_32b = 3
        */

            }

    public class Settings
    {
        public string CCompileFlags = "-G 0 -O1 -fno-reorder-blocks -std=gnu99 -mtune=vr4300 -march=vr4300 -mabi=32 -c -mips3 -mno-explicit-relocs -mno-memcpy -mno-check-zero-division";
        public string MipsCompileFlags = "-G 0 -Os -std=gnu99 -mtune=vr4300 -march=vr4300 -mabi=32 -c -mips3";
        public bool DisableCWarnings = false;
        public bool Usez64hdr = false;

    }




}
