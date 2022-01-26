namespace CustomActorToolkit
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SetOverlayPathButton = new System.Windows.Forms.Button();
            this.CompileButton = new System.Windows.Forms.Button();
            this.OverlayLabel = new System.Windows.Forms.Label();
            this.VRAM = new HexNumericUpdown();
            this.vramoffsetlabel = new System.Windows.Forms.Label();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.CompilePage = new System.Windows.Forms.TabPage();
            this.UseZ64hdr = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ActorAllocation = new System.Windows.Forms.NumericUpDown();
            this.DMARowLabel = new System.Windows.Forms.Label();
            this.FindOriginalRowButton = new System.Windows.Forms.Button();
            this.DMARow = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.SendTozzrp = new System.Windows.Forms.Button();
            this.SpecialOverlayLabel = new System.Windows.Forms.Label();
            this.WarningCheckbox = new System.Windows.Forms.CheckBox();
            this.UpdateCRCButton = new System.Windows.Forms.Button();
            this.FindEmptySpaceButton = new System.Windows.Forms.Button();
            this.InjectOffset = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.LaunchButton = new System.Windows.Forms.Button();
            this.ClearDmaButton = new System.Windows.Forms.Button();
            this.InjectButton = new System.Windows.Forms.Button();
            this.FindEmptyActorIDButton = new System.Windows.Forms.Button();
            this.ActorID = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.FindEmptyVRAMButton = new System.Windows.Forms.Button();
            this.DecompilePage = new System.Windows.Forms.TabPage();
            this.SpecialOverlayLabel2 = new System.Windows.Forms.Label();
            this.DecompilingLabel = new System.Windows.Forms.Label();
            this.HexOffsets = new System.Windows.Forms.CheckBox();
            this.JalNames = new System.Windows.Forms.CheckBox();
            this.JalFix = new System.Windows.Forms.CheckBox();
            this.DecompileAtomButton = new System.Windows.Forms.Button();
            this.DecActorID = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ObjectsPage = new System.Windows.Forms.TabPage();
            this.ObjectDMARowLabel = new System.Windows.Forms.Label();
            this.ObjectFindOriginalRowButton = new System.Windows.Forms.Button();
            this.ObjectDMARow = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.ObjectSendTozzrp = new System.Windows.Forms.Button();
            this.FindEmptyObjectID = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ZobjLabel = new System.Windows.Forms.Label();
            this.InjectZobj = new System.Windows.Forms.Button();
            this.FindEmptySpace2 = new System.Windows.Forms.Button();
            this.ObjectInjectOffset = new System.Windows.Forms.NumericUpDown();
            this.ImportZobj = new System.Windows.Forms.Button();
            this.ExportZobj = new System.Windows.Forms.Button();
            this.ObjectID = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.OtherPage = new System.Windows.Forms.TabPage();
            this.ZobjAnimationCopyButton = new System.Windows.Forms.Button();
            this.OpenARomlabel = new System.Windows.Forms.Label();
            this.RandomDropTableEditorButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.loadROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findStartDmaTableAdressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getFilesystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionsToDbgVersion2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.j0FuncsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getZobjInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.correctAndDetailSFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertFileToMipsVariableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertDListToMipsDListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portDebugRomActorTo10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.port10todebugactortoolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadZ64ovlGithubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Downloadz64hdr = new System.Windows.Forms.ToolStripMenuItem();
            this.detectTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugGenerateAtomFuncsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreAllSettingsToDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeMipsCompileFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chanceCCompileFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.VRAM)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.CompilePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActorAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DMARow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActorID)).BeginInit();
            this.DecompilePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DecActorID)).BeginInit();
            this.ObjectsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectDMARow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectInjectOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectID)).BeginInit();
            this.OtherPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetOverlayPathButton
            // 
            this.SetOverlayPathButton.Location = new System.Drawing.Point(33, 16);
            this.SetOverlayPathButton.Name = "SetOverlayPathButton";
            this.SetOverlayPathButton.Size = new System.Drawing.Size(134, 23);
            this.SetOverlayPathButton.TabIndex = 0;
            this.SetOverlayPathButton.Text = "Import Overlay (.S .c)";
            this.SetOverlayPathButton.UseVisualStyleBackColor = true;
            this.SetOverlayPathButton.Click += new System.EventHandler(this.SetOverlayPathButton_Click);
            // 
            // CompileButton
            // 
            this.CompileButton.Location = new System.Drawing.Point(68, 185);
            this.CompileButton.Name = "CompileButton";
            this.CompileButton.Size = new System.Drawing.Size(92, 23);
            this.CompileButton.TabIndex = 1;
            this.CompileButton.Text = "Compile (nOVL)";
            this.CompileButton.UseVisualStyleBackColor = true;
            this.CompileButton.Click += new System.EventHandler(this.CompileButton_Click);
            // 
            // OverlayLabel
            // 
            this.OverlayLabel.AutoSize = true;
            this.OverlayLabel.Location = new System.Drawing.Point(173, 21);
            this.OverlayLabel.Name = "OverlayLabel";
            this.OverlayLabel.Size = new System.Drawing.Size(74, 13);
            this.OverlayLabel.TabIndex = 2;
            this.OverlayLabel.Text = "(Overlay Path)";
            // 
            // VRAM
            // 
            this.VRAM.Hexadecimal = true;
            this.VRAM.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.VRAM.Location = new System.Drawing.Point(33, 146);
            this.VRAM.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.VRAM.Name = "VRAM";
            this.VRAM.Size = new System.Drawing.Size(92, 20);
            this.VRAM.TabIndex = 3;
            this.VRAM.Value = new decimal(new int[] {
            -2139095040,
            0,
            0,
            0});
            // 
            // vramoffsetlabel
            // 
            this.vramoffsetlabel.AutoSize = true;
            this.vramoffsetlabel.Location = new System.Drawing.Point(65, 118);
            this.vramoffsetlabel.Name = "vramoffsetlabel";
            this.vramoffsetlabel.Size = new System.Drawing.Size(153, 13);
            this.vramoffsetlabel.TabIndex = 4;
            this.vramoffsetlabel.Text = "VRAM start offset (OPTIONAL)";
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.CompilePage);
            this.TabControl1.Controls.Add(this.DecompilePage);
            this.TabControl1.Controls.Add(this.ObjectsPage);
            this.TabControl1.Controls.Add(this.OtherPage);
            this.TabControl1.Location = new System.Drawing.Point(12, 31);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(299, 520);
            this.TabControl1.TabIndex = 5;
            // 
            // CompilePage
            // 
            this.CompilePage.Controls.Add(this.UseZ64hdr);
            this.CompilePage.Controls.Add(this.label7);
            this.CompilePage.Controls.Add(this.ActorAllocation);
            this.CompilePage.Controls.Add(this.DMARowLabel);
            this.CompilePage.Controls.Add(this.FindOriginalRowButton);
            this.CompilePage.Controls.Add(this.DMARow);
            this.CompilePage.Controls.Add(this.label2);
            this.CompilePage.Controls.Add(this.SendTozzrp);
            this.CompilePage.Controls.Add(this.SpecialOverlayLabel);
            this.CompilePage.Controls.Add(this.WarningCheckbox);
            this.CompilePage.Controls.Add(this.UpdateCRCButton);
            this.CompilePage.Controls.Add(this.FindEmptySpaceButton);
            this.CompilePage.Controls.Add(this.InjectOffset);
            this.CompilePage.Controls.Add(this.label4);
            this.CompilePage.Controls.Add(this.LaunchButton);
            this.CompilePage.Controls.Add(this.ClearDmaButton);
            this.CompilePage.Controls.Add(this.InjectButton);
            this.CompilePage.Controls.Add(this.FindEmptyActorIDButton);
            this.CompilePage.Controls.Add(this.ActorID);
            this.CompilePage.Controls.Add(this.label3);
            this.CompilePage.Controls.Add(this.FindEmptyVRAMButton);
            this.CompilePage.Controls.Add(this.SetOverlayPathButton);
            this.CompilePage.Controls.Add(this.vramoffsetlabel);
            this.CompilePage.Controls.Add(this.CompileButton);
            this.CompilePage.Controls.Add(this.VRAM);
            this.CompilePage.Controls.Add(this.OverlayLabel);
            this.CompilePage.Location = new System.Drawing.Point(4, 22);
            this.CompilePage.Name = "CompilePage";
            this.CompilePage.Padding = new System.Windows.Forms.Padding(3);
            this.CompilePage.Size = new System.Drawing.Size(291, 494);
            this.CompilePage.TabIndex = 0;
            this.CompilePage.Text = "Compile";
            this.CompilePage.UseVisualStyleBackColor = true;
            // 
            // UseZ64hdr
            // 
            this.UseZ64hdr.AutoSize = true;
            this.UseZ64hdr.Location = new System.Drawing.Point(169, 212);
            this.UseZ64hdr.Name = "UseZ64hdr";
            this.UseZ64hdr.Size = new System.Drawing.Size(80, 17);
            this.UseZ64hdr.TabIndex = 26;
            this.UseZ64hdr.Text = "Use z64hdr";
            this.UseZ64hdr.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Allocation";
            // 
            // ActorAllocation
            // 
            this.ActorAllocation.Hexadecimal = true;
            this.ActorAllocation.Location = new System.Drawing.Point(107, 214);
            this.ActorAllocation.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.ActorAllocation.Name = "ActorAllocation";
            this.ActorAllocation.Size = new System.Drawing.Size(53, 20);
            this.ActorAllocation.TabIndex = 24;
            this.toolTip1.SetToolTip(this.ActorAllocation, "00 performs a \"low to high\" address allocation (high address volatility), unloadi" +
        "ng the overlay if no instances of that actor exist");
            // 
            // DMARowLabel
            // 
            this.DMARowLabel.AutoSize = true;
            this.DMARowLabel.Location = new System.Drawing.Point(47, 328);
            this.DMARowLabel.Name = "DMARowLabel";
            this.DMARowLabel.Size = new System.Drawing.Size(74, 13);
            this.DMARowLabel.TabIndex = 23;
            this.DMARowLabel.Text = "Skip/New File";
            // 
            // FindOriginalRowButton
            // 
            this.FindOriginalRowButton.Enabled = false;
            this.FindOriginalRowButton.Location = new System.Drawing.Point(155, 323);
            this.FindOriginalRowButton.Name = "FindOriginalRowButton";
            this.FindOriginalRowButton.Size = new System.Drawing.Size(104, 23);
            this.FindOriginalRowButton.TabIndex = 22;
            this.FindOriginalRowButton.Text = "Find Original Row";
            this.FindOriginalRowButton.UseVisualStyleBackColor = true;
            this.FindOriginalRowButton.Click += new System.EventHandler(this.FindOriginalRowButton_Click);
            // 
            // DMARow
            // 
            this.DMARow.Location = new System.Drawing.Point(45, 326);
            this.DMARow.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.DMARow.Name = "DMARow";
            this.DMARow.Size = new System.Drawing.Size(92, 20);
            this.DMARow.TabIndex = 21;
            this.DMARow.ValueChanged += new System.EventHandler(this.DMARow_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 300);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "DMA data row";
            // 
            // SendTozzrp
            // 
            this.SendTozzrp.Enabled = false;
            this.SendTozzrp.Location = new System.Drawing.Point(155, 365);
            this.SendTozzrp.Name = "SendTozzrp";
            this.SendTozzrp.Size = new System.Drawing.Size(92, 23);
            this.SendTozzrp.TabIndex = 19;
            this.SendTozzrp.Text = "Send to .zzrp";
            this.SendTozzrp.UseVisualStyleBackColor = true;
            this.SendTozzrp.Click += new System.EventHandler(this.SendTozzrp_Click);
            // 
            // SpecialOverlayLabel
            // 
            this.SpecialOverlayLabel.AutoSize = true;
            this.SpecialOverlayLabel.Location = new System.Drawing.Point(35, 85);
            this.SpecialOverlayLabel.Name = "SpecialOverlayLabel";
            this.SpecialOverlayLabel.Size = new System.Drawing.Size(27, 13);
            this.SpecialOverlayLabel.TabIndex = 18;
            this.SpecialOverlayLabel.Text = "Link";
            this.SpecialOverlayLabel.Visible = false;
            // 
            // WarningCheckbox
            // 
            this.WarningCheckbox.AutoSize = true;
            this.WarningCheckbox.Location = new System.Drawing.Point(169, 189);
            this.WarningCheckbox.Name = "WarningCheckbox";
            this.WarningCheckbox.Size = new System.Drawing.Size(119, 17);
            this.WarningCheckbox.TabIndex = 9;
            this.WarningCheckbox.Text = "Disable C warnings ";
            this.WarningCheckbox.UseVisualStyleBackColor = true;
            this.WarningCheckbox.CheckedChanged += new System.EventHandler(this.WarningCheckbox_CheckedChanged);
            // 
            // UpdateCRCButton
            // 
            this.UpdateCRCButton.Enabled = false;
            this.UpdateCRCButton.Location = new System.Drawing.Point(155, 412);
            this.UpdateCRCButton.Name = "UpdateCRCButton";
            this.UpdateCRCButton.Size = new System.Drawing.Size(92, 23);
            this.UpdateCRCButton.TabIndex = 17;
            this.UpdateCRCButton.Text = "Update CRC";
            this.UpdateCRCButton.UseVisualStyleBackColor = true;
            this.UpdateCRCButton.Click += new System.EventHandler(this.UpdateCRCButton_Click);
            // 
            // FindEmptySpaceButton
            // 
            this.FindEmptySpaceButton.Enabled = false;
            this.FindEmptySpaceButton.Location = new System.Drawing.Point(155, 269);
            this.FindEmptySpaceButton.Name = "FindEmptySpaceButton";
            this.FindEmptySpaceButton.Size = new System.Drawing.Size(104, 23);
            this.FindEmptySpaceButton.TabIndex = 15;
            this.FindEmptySpaceButton.Text = "Find Empty Space";
            this.FindEmptySpaceButton.UseVisualStyleBackColor = true;
            this.FindEmptySpaceButton.Click += new System.EventHandler(this.FindEmptySpaceButton_Click);
            // 
            // InjectOffset
            // 
            this.InjectOffset.Hexadecimal = true;
            this.InjectOffset.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.InjectOffset.Location = new System.Drawing.Point(45, 272);
            this.InjectOffset.Maximum = new decimal(new int[] {
            67108864,
            0,
            0,
            0});
            this.InjectOffset.Name = "InjectOffset";
            this.InjectOffset.Size = new System.Drawing.Size(92, 20);
            this.InjectOffset.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 246);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Inject offset";
            // 
            // LaunchButton
            // 
            this.LaunchButton.Enabled = false;
            this.LaunchButton.Location = new System.Drawing.Point(96, 451);
            this.LaunchButton.Name = "LaunchButton";
            this.LaunchButton.Size = new System.Drawing.Size(92, 23);
            this.LaunchButton.TabIndex = 12;
            this.LaunchButton.Text = "Launch ROM";
            this.LaunchButton.UseVisualStyleBackColor = true;
            this.LaunchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // ClearDmaButton
            // 
            this.ClearDmaButton.Enabled = false;
            this.ClearDmaButton.Location = new System.Drawing.Point(43, 412);
            this.ClearDmaButton.Name = "ClearDmaButton";
            this.ClearDmaButton.Size = new System.Drawing.Size(92, 23);
            this.ClearDmaButton.TabIndex = 11;
            this.ClearDmaButton.Text = "Clear dma table";
            this.ClearDmaButton.UseVisualStyleBackColor = true;
            this.ClearDmaButton.Click += new System.EventHandler(this.ClearDmaButton_Click);
            // 
            // InjectButton
            // 
            this.InjectButton.Enabled = false;
            this.InjectButton.Location = new System.Drawing.Point(43, 365);
            this.InjectButton.Name = "InjectButton";
            this.InjectButton.Size = new System.Drawing.Size(92, 23);
            this.InjectButton.TabIndex = 10;
            this.InjectButton.Text = "Inject to ROM";
            this.InjectButton.UseVisualStyleBackColor = true;
            this.InjectButton.Click += new System.EventHandler(this.InjectButton_Click);
            // 
            // FindEmptyActorIDButton
            // 
            this.FindEmptyActorIDButton.Enabled = false;
            this.FindEmptyActorIDButton.Location = new System.Drawing.Point(143, 80);
            this.FindEmptyActorIDButton.Name = "FindEmptyActorIDButton";
            this.FindEmptyActorIDButton.Size = new System.Drawing.Size(117, 23);
            this.FindEmptyActorIDButton.TabIndex = 8;
            this.FindEmptyActorIDButton.Text = "Find Empty Actor ID";
            this.FindEmptyActorIDButton.UseVisualStyleBackColor = true;
            this.FindEmptyActorIDButton.Click += new System.EventHandler(this.FindEmptyActorIDButton_Click);
            // 
            // ActorID
            // 
            this.ActorID.Hexadecimal = true;
            this.ActorID.Location = new System.Drawing.Point(33, 83);
            this.ActorID.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.ActorID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.ActorID.Name = "ActorID";
            this.ActorID.Size = new System.Drawing.Size(92, 20);
            this.ActorID.TabIndex = 7;
            this.ActorID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ActorID.ValueChanged += new System.EventHandler(this.ActorID_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Actor number / Overlay";
            // 
            // FindEmptyVRAMButton
            // 
            this.FindEmptyVRAMButton.Enabled = false;
            this.FindEmptyVRAMButton.Location = new System.Drawing.Point(143, 143);
            this.FindEmptyVRAMButton.Name = "FindEmptyVRAMButton";
            this.FindEmptyVRAMButton.Size = new System.Drawing.Size(104, 23);
            this.FindEmptyVRAMButton.TabIndex = 5;
            this.FindEmptyVRAMButton.Text = "Find Empty VRAM";
            this.FindEmptyVRAMButton.UseVisualStyleBackColor = true;
            this.FindEmptyVRAMButton.Click += new System.EventHandler(this.FindEmptyVRAMButton_Click);
            // 
            // DecompilePage
            // 
            this.DecompilePage.Controls.Add(this.SpecialOverlayLabel2);
            this.DecompilePage.Controls.Add(this.DecompilingLabel);
            this.DecompilePage.Controls.Add(this.HexOffsets);
            this.DecompilePage.Controls.Add(this.JalNames);
            this.DecompilePage.Controls.Add(this.JalFix);
            this.DecompilePage.Controls.Add(this.DecompileAtomButton);
            this.DecompilePage.Controls.Add(this.DecActorID);
            this.DecompilePage.Controls.Add(this.label1);
            this.DecompilePage.Location = new System.Drawing.Point(4, 22);
            this.DecompilePage.Name = "DecompilePage";
            this.DecompilePage.Padding = new System.Windows.Forms.Padding(3);
            this.DecompilePage.Size = new System.Drawing.Size(291, 494);
            this.DecompilePage.TabIndex = 1;
            this.DecompilePage.Text = "Decompile";
            this.DecompilePage.UseVisualStyleBackColor = true;
            // 
            // SpecialOverlayLabel2
            // 
            this.SpecialOverlayLabel2.AutoSize = true;
            this.SpecialOverlayLabel2.Location = new System.Drawing.Point(102, 18);
            this.SpecialOverlayLabel2.Name = "SpecialOverlayLabel2";
            this.SpecialOverlayLabel2.Size = new System.Drawing.Size(27, 13);
            this.SpecialOverlayLabel2.TabIndex = 19;
            this.SpecialOverlayLabel2.Text = "Link";
            this.SpecialOverlayLabel2.Visible = false;
            // 
            // DecompilingLabel
            // 
            this.DecompilingLabel.AutoSize = true;
            this.DecompilingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.DecompilingLabel.Location = new System.Drawing.Point(18, 139);
            this.DecompilingLabel.Name = "DecompilingLabel";
            this.DecompilingLabel.Size = new System.Drawing.Size(222, 20);
            this.DecompilingLabel.TabIndex = 14;
            this.DecompilingLabel.Text = "Decompiling, please wait...";
            this.DecompilingLabel.Visible = false;
            // 
            // HexOffsets
            // 
            this.HexOffsets.AutoSize = true;
            this.HexOffsets.Checked = true;
            this.HexOffsets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HexOffsets.Location = new System.Drawing.Point(140, 107);
            this.HexOffsets.Name = "HexOffsets";
            this.HexOffsets.Size = new System.Drawing.Size(121, 17);
            this.HexOffsets.TabIndex = 13;
            this.HexOffsets.Text = "Hexadecimal offsets";
            this.HexOffsets.UseVisualStyleBackColor = true;
            this.HexOffsets.Visible = false;
            // 
            // JalNames
            // 
            this.JalNames.AutoSize = true;
            this.JalNames.Checked = true;
            this.JalNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.JalNames.Location = new System.Drawing.Point(140, 84);
            this.JalNames.Name = "JalNames";
            this.JalNames.Size = new System.Drawing.Size(137, 17);
            this.JalNames.TabIndex = 12;
            this.JalNames.Text = "Detailed jal descriptions";
            this.JalNames.UseVisualStyleBackColor = true;
            // 
            // JalFix
            // 
            this.JalFix.AutoSize = true;
            this.JalFix.Checked = true;
            this.JalFix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.JalFix.Location = new System.Drawing.Point(140, 61);
            this.JalFix.Name = "JalFix";
            this.JalFix.Size = new System.Drawing.Size(135, 17);
            this.JalFix.TabIndex = 10;
            this.JalFix.Text = "Fix for easy compilation";
            this.JalFix.UseVisualStyleBackColor = true;
            // 
            // DecompileAtomButton
            // 
            this.DecompileAtomButton.Enabled = false;
            this.DecompileAtomButton.Location = new System.Drawing.Point(18, 57);
            this.DecompileAtomButton.Name = "DecompileAtomButton";
            this.DecompileAtomButton.Size = new System.Drawing.Size(114, 23);
            this.DecompileAtomButton.TabIndex = 11;
            this.DecompileAtomButton.Text = "Decompile (atom)";
            this.DecompileAtomButton.UseVisualStyleBackColor = true;
            this.DecompileAtomButton.Click += new System.EventHandler(this.DecompileAtomButton_Click);
            // 
            // DecActorID
            // 
            this.DecActorID.Hexadecimal = true;
            this.DecActorID.Location = new System.Drawing.Point(100, 16);
            this.DecActorID.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.DecActorID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.DecActorID.Name = "DecActorID";
            this.DecActorID.Size = new System.Drawing.Size(92, 20);
            this.DecActorID.TabIndex = 9;
            this.DecActorID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DecActorID.ValueChanged += new System.EventHandler(this.DecActorID_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Actor number";
            // 
            // ObjectsPage
            // 
            this.ObjectsPage.Controls.Add(this.ObjectDMARowLabel);
            this.ObjectsPage.Controls.Add(this.ObjectFindOriginalRowButton);
            this.ObjectsPage.Controls.Add(this.ObjectDMARow);
            this.ObjectsPage.Controls.Add(this.label8);
            this.ObjectsPage.Controls.Add(this.ObjectSendTozzrp);
            this.ObjectsPage.Controls.Add(this.FindEmptyObjectID);
            this.ObjectsPage.Controls.Add(this.label6);
            this.ObjectsPage.Controls.Add(this.ZobjLabel);
            this.ObjectsPage.Controls.Add(this.InjectZobj);
            this.ObjectsPage.Controls.Add(this.FindEmptySpace2);
            this.ObjectsPage.Controls.Add(this.ObjectInjectOffset);
            this.ObjectsPage.Controls.Add(this.ImportZobj);
            this.ObjectsPage.Controls.Add(this.ExportZobj);
            this.ObjectsPage.Controls.Add(this.ObjectID);
            this.ObjectsPage.Controls.Add(this.label5);
            this.ObjectsPage.Location = new System.Drawing.Point(4, 22);
            this.ObjectsPage.Name = "ObjectsPage";
            this.ObjectsPage.Size = new System.Drawing.Size(291, 494);
            this.ObjectsPage.TabIndex = 2;
            this.ObjectsPage.Text = "Objects";
            this.ObjectsPage.UseVisualStyleBackColor = true;
            // 
            // ObjectDMARowLabel
            // 
            this.ObjectDMARowLabel.AutoSize = true;
            this.ObjectDMARowLabel.Location = new System.Drawing.Point(31, 257);
            this.ObjectDMARowLabel.Name = "ObjectDMARowLabel";
            this.ObjectDMARowLabel.Size = new System.Drawing.Size(74, 13);
            this.ObjectDMARowLabel.TabIndex = 28;
            this.ObjectDMARowLabel.Text = "Skip/New File";
            // 
            // ObjectFindOriginalRowButton
            // 
            this.ObjectFindOriginalRowButton.Enabled = false;
            this.ObjectFindOriginalRowButton.Location = new System.Drawing.Point(139, 252);
            this.ObjectFindOriginalRowButton.Name = "ObjectFindOriginalRowButton";
            this.ObjectFindOriginalRowButton.Size = new System.Drawing.Size(104, 23);
            this.ObjectFindOriginalRowButton.TabIndex = 27;
            this.ObjectFindOriginalRowButton.Text = "Find Original Row";
            this.ObjectFindOriginalRowButton.UseVisualStyleBackColor = true;
            this.ObjectFindOriginalRowButton.Click += new System.EventHandler(this.ObjectFindOriginalRowButton_Click);
            // 
            // ObjectDMARow
            // 
            this.ObjectDMARow.Location = new System.Drawing.Point(29, 255);
            this.ObjectDMARow.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.ObjectDMARow.Name = "ObjectDMARow";
            this.ObjectDMARow.Size = new System.Drawing.Size(92, 20);
            this.ObjectDMARow.TabIndex = 26;
            this.ObjectDMARow.ValueChanged += new System.EventHandler(this.ObjectDMARow_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(97, 227);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "DMA data row";
            // 
            // ObjectSendTozzrp
            // 
            this.ObjectSendTozzrp.Enabled = false;
            this.ObjectSendTozzrp.Location = new System.Drawing.Point(139, 303);
            this.ObjectSendTozzrp.Name = "ObjectSendTozzrp";
            this.ObjectSendTozzrp.Size = new System.Drawing.Size(92, 23);
            this.ObjectSendTozzrp.TabIndex = 24;
            this.ObjectSendTozzrp.Text = "Send to .zzrp";
            this.ObjectSendTozzrp.UseVisualStyleBackColor = true;
            this.ObjectSendTozzrp.Click += new System.EventHandler(this.ObjectSendTozzrp_Click);
            // 
            // FindEmptyObjectID
            // 
            this.FindEmptyObjectID.Enabled = false;
            this.FindEmptyObjectID.Location = new System.Drawing.Point(139, 76);
            this.FindEmptyObjectID.Name = "FindEmptyObjectID";
            this.FindEmptyObjectID.Size = new System.Drawing.Size(117, 23);
            this.FindEmptyObjectID.TabIndex = 21;
            this.FindEmptyObjectID.Text = "Find Empty Object ID";
            this.FindEmptyObjectID.UseVisualStyleBackColor = true;
            this.FindEmptyObjectID.Click += new System.EventHandler(this.FindEmptyObjectID_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(97, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Inject offset";
            // 
            // ZobjLabel
            // 
            this.ZobjLabel.AutoSize = true;
            this.ZobjLabel.Location = new System.Drawing.Point(151, 20);
            this.ZobjLabel.Name = "ZobjLabel";
            this.ZobjLabel.Size = new System.Drawing.Size(69, 13);
            this.ZobjLabel.TabIndex = 19;
            this.ZobjLabel.Text = "(Not Loaded)";
            // 
            // InjectZobj
            // 
            this.InjectZobj.Enabled = false;
            this.InjectZobj.Location = new System.Drawing.Point(29, 303);
            this.InjectZobj.Name = "InjectZobj";
            this.InjectZobj.Size = new System.Drawing.Size(92, 23);
            this.InjectZobj.TabIndex = 18;
            this.InjectZobj.Text = "Inject to ROM";
            this.InjectZobj.UseVisualStyleBackColor = true;
            this.InjectZobj.Click += new System.EventHandler(this.InjectZobj_Click);
            // 
            // FindEmptySpace2
            // 
            this.FindEmptySpace2.Enabled = false;
            this.FindEmptySpace2.Location = new System.Drawing.Point(139, 190);
            this.FindEmptySpace2.Name = "FindEmptySpace2";
            this.FindEmptySpace2.Size = new System.Drawing.Size(104, 23);
            this.FindEmptySpace2.TabIndex = 17;
            this.FindEmptySpace2.Text = "Find Empty Space";
            this.FindEmptySpace2.UseVisualStyleBackColor = true;
            this.FindEmptySpace2.Click += new System.EventHandler(this.FindEmptySpace2_Click);
            // 
            // ObjectInjectOffset
            // 
            this.ObjectInjectOffset.Hexadecimal = true;
            this.ObjectInjectOffset.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.ObjectInjectOffset.Location = new System.Drawing.Point(29, 193);
            this.ObjectInjectOffset.Maximum = new decimal(new int[] {
            67108864,
            0,
            0,
            0});
            this.ObjectInjectOffset.Name = "ObjectInjectOffset";
            this.ObjectInjectOffset.Size = new System.Drawing.Size(92, 20);
            this.ObjectInjectOffset.TabIndex = 16;
            // 
            // ImportZobj
            // 
            this.ImportZobj.Enabled = false;
            this.ImportZobj.Location = new System.Drawing.Point(20, 15);
            this.ImportZobj.Name = "ImportZobj";
            this.ImportZobj.Size = new System.Drawing.Size(114, 23);
            this.ImportZobj.TabIndex = 13;
            this.ImportZobj.Text = "Import Zobj...";
            this.ImportZobj.UseVisualStyleBackColor = true;
            this.ImportZobj.Click += new System.EventHandler(this.ImportZobj_Click);
            // 
            // ExportZobj
            // 
            this.ExportZobj.Enabled = false;
            this.ExportZobj.Location = new System.Drawing.Point(78, 117);
            this.ExportZobj.Name = "ExportZobj";
            this.ExportZobj.Size = new System.Drawing.Size(114, 23);
            this.ExportZobj.TabIndex = 12;
            this.ExportZobj.Text = "Export ROM zobj";
            this.ExportZobj.UseVisualStyleBackColor = true;
            this.ExportZobj.Click += new System.EventHandler(this.ExportZobj_Click);
            // 
            // ObjectID
            // 
            this.ObjectID.Hexadecimal = true;
            this.ObjectID.Location = new System.Drawing.Point(29, 79);
            this.ObjectID.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.ObjectID.Name = "ObjectID";
            this.ObjectID.Size = new System.Drawing.Size(92, 20);
            this.ObjectID.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(97, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Object number";
            // 
            // OtherPage
            // 
            this.OtherPage.Controls.Add(this.ZobjAnimationCopyButton);
            this.OtherPage.Controls.Add(this.OpenARomlabel);
            this.OtherPage.Controls.Add(this.RandomDropTableEditorButton);
            this.OtherPage.Location = new System.Drawing.Point(4, 22);
            this.OtherPage.Name = "OtherPage";
            this.OtherPage.Size = new System.Drawing.Size(291, 494);
            this.OtherPage.TabIndex = 3;
            this.OtherPage.Text = "Other stuff";
            this.OtherPage.UseVisualStyleBackColor = true;
            // 
            // ZobjAnimationCopyButton
            // 
            this.ZobjAnimationCopyButton.Location = new System.Drawing.Point(61, 86);
            this.ZobjAnimationCopyButton.Name = "ZobjAnimationCopyButton";
            this.ZobjAnimationCopyButton.Size = new System.Drawing.Size(141, 23);
            this.ZobjAnimationCopyButton.TabIndex = 21;
            this.ZobjAnimationCopyButton.Text = "ZObj Animation Copy Tool";
            this.ZobjAnimationCopyButton.UseVisualStyleBackColor = true;
            this.ZobjAnimationCopyButton.Click += new System.EventHandler(this.ZobjAnimationCopyButton_Click);
            // 
            // OpenARomlabel
            // 
            this.OpenARomlabel.AutoSize = true;
            this.OpenARomlabel.ForeColor = System.Drawing.Color.Red;
            this.OpenARomlabel.Location = new System.Drawing.Point(86, 27);
            this.OpenARomlabel.Name = "OpenARomlabel";
            this.OpenARomlabel.Size = new System.Drawing.Size(92, 13);
            this.OpenARomlabel.TabIndex = 20;
            this.OpenARomlabel.Text = "Open a ROM first!";
            // 
            // RandomDropTableEditorButton
            // 
            this.RandomDropTableEditorButton.Enabled = false;
            this.RandomDropTableEditorButton.Location = new System.Drawing.Point(61, 43);
            this.RandomDropTableEditorButton.Name = "RandomDropTableEditorButton";
            this.RandomDropTableEditorButton.Size = new System.Drawing.Size(141, 23);
            this.RandomDropTableEditorButton.TabIndex = 19;
            this.RandomDropTableEditorButton.Text = "Random Drop Table Editor";
            this.RandomDropTableEditorButton.UseVisualStyleBackColor = true;
            this.RandomDropTableEditorButton.Click += new System.EventHandler(this.RandomDropTableEditorButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(329, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadROMToolStripMenuItem,
            this.openRecentToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // loadROMToolStripMenuItem
            // 
            this.loadROMToolStripMenuItem.Name = "loadROMToolStripMenuItem";
            this.loadROMToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.loadROMToolStripMenuItem.Text = "Load ROM or zzrp project...";
            this.loadROMToolStripMenuItem.Click += new System.EventHandler(this.loadROMToolStripMenuItem_Click);
            // 
            // openRecentToolStripMenuItem
            // 
            this.openRecentToolStripMenuItem.Name = "openRecentToolStripMenuItem";
            this.openRecentToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.openRecentToolStripMenuItem.Text = "Open recent...";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.testToolStripMenuItem,
            this.findStartDmaTableAdressToolStripMenuItem,
            this.getFilesystemToolStripMenuItem,
            this.functionsToDbgVersion2ToolStripMenuItem,
            this.j0FuncsToolStripMenuItem,
            this.getZobjInfoToolStripMenuItem,
            this.correctAndDetailSFileToolStripMenuItem,
            this.convertFileToMipsVariableToolStripMenuItem,
            this.convertDListToMipsDListToolStripMenuItem,
            this.portDebugRomActorTo10ToolStripMenuItem,
            this.port10todebugactortoolstrip,
            this.downloadZ64ovlGithubToolStripMenuItem,
            this.toolStripMenuItem2,
            this.Downloadz64hdr,
            this.detectTexturesToolStripMenuItem,
            this.debugGenerateAtomFuncsToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton2.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.testToolStripMenuItem.Text = "1.0 functions to dbg (failure)";
            this.testToolStripMenuItem.Visible = false;
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // findStartDmaTableAdressToolStripMenuItem
            // 
            this.findStartDmaTableAdressToolStripMenuItem.Name = "findStartDmaTableAdressToolStripMenuItem";
            this.findStartDmaTableAdressToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.findStartDmaTableAdressToolStripMenuItem.Text = "Find start dma table adress";
            this.findStartDmaTableAdressToolStripMenuItem.Visible = false;
            this.findStartDmaTableAdressToolStripMenuItem.Click += new System.EventHandler(this.findStartDmaTableAdressToolStripMenuItem_Click);
            // 
            // getFilesystemToolStripMenuItem
            // 
            this.getFilesystemToolStripMenuItem.Name = "getFilesystemToolStripMenuItem";
            this.getFilesystemToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.getFilesystemToolStripMenuItem.Text = "Get Filesystem";
            this.getFilesystemToolStripMenuItem.Visible = false;
            this.getFilesystemToolStripMenuItem.Click += new System.EventHandler(this.getFilesystemToolStripMenuItem_Click);
            // 
            // functionsToDbgVersion2ToolStripMenuItem
            // 
            this.functionsToDbgVersion2ToolStripMenuItem.Name = "functionsToDbgVersion2ToolStripMenuItem";
            this.functionsToDbgVersion2ToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.functionsToDbgVersion2ToolStripMenuItem.Text = "1.0 functions to dbg version 2";
            this.functionsToDbgVersion2ToolStripMenuItem.Visible = false;
            this.functionsToDbgVersion2ToolStripMenuItem.Click += new System.EventHandler(this.functionsToDbgVersion2ToolStripMenuItem_Click);
            // 
            // j0FuncsToolStripMenuItem
            // 
            this.j0FuncsToolStripMenuItem.Name = "j0FuncsToolStripMenuItem";
            this.j0FuncsToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.j0FuncsToolStripMenuItem.Text = "J0 funcs";
            this.j0FuncsToolStripMenuItem.Visible = false;
            this.j0FuncsToolStripMenuItem.Click += new System.EventHandler(this.listMMjapfunctions);
            // 
            // getZobjInfoToolStripMenuItem
            // 
            this.getZobjInfoToolStripMenuItem.Name = "getZobjInfoToolStripMenuItem";
            this.getZobjInfoToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.getZobjInfoToolStripMenuItem.Text = "Get Zobj info";
            this.getZobjInfoToolStripMenuItem.Visible = false;
            this.getZobjInfoToolStripMenuItem.Click += new System.EventHandler(this.getZobjInfoToolStripMenuItem_Click);
            // 
            // correctAndDetailSFileToolStripMenuItem
            // 
            this.correctAndDetailSFileToolStripMenuItem.Name = "correctAndDetailSFileToolStripMenuItem";
            this.correctAndDetailSFileToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.correctAndDetailSFileToolStripMenuItem.Text = "Correct and detail .S file";
            this.correctAndDetailSFileToolStripMenuItem.Click += new System.EventHandler(this.correctAndDetailSFileToolStripMenuItem_Click);
            // 
            // convertFileToMipsVariableToolStripMenuItem
            // 
            this.convertFileToMipsVariableToolStripMenuItem.Name = "convertFileToMipsVariableToolStripMenuItem";
            this.convertFileToMipsVariableToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.convertFileToMipsVariableToolStripMenuItem.Text = "Convert file to mips variable";
            this.convertFileToMipsVariableToolStripMenuItem.Click += new System.EventHandler(this.convertFileToMipsVariableToolStripMenuItem_Click);
            // 
            // convertDListToMipsDListToolStripMenuItem
            // 
            this.convertDListToMipsDListToolStripMenuItem.Name = "convertDListToMipsDListToolStripMenuItem";
            this.convertDListToMipsDListToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.convertDListToMipsDListToolStripMenuItem.Text = "Convert DList to mips DList";
            this.convertDListToMipsDListToolStripMenuItem.Visible = false;
            this.convertDListToMipsDListToolStripMenuItem.Click += new System.EventHandler(this.convertDListToMipsDListToolStripMenuItem_Click);
            // 
            // portDebugRomActorTo10ToolStripMenuItem
            // 
            this.portDebugRomActorTo10ToolStripMenuItem.Name = "portDebugRomActorTo10ToolStripMenuItem";
            this.portDebugRomActorTo10ToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.portDebugRomActorTo10ToolStripMenuItem.Text = "Port debug rom actor to 1.0";
            this.portDebugRomActorTo10ToolStripMenuItem.Click += new System.EventHandler(this.portDebugRomActorTo10ToolStripMenuItem_Click);
            // 
            // port10todebugactortoolstrip
            // 
            this.port10todebugactortoolstrip.Name = "port10todebugactortoolstrip";
            this.port10todebugactortoolstrip.Size = new System.Drawing.Size(298, 22);
            this.port10todebugactortoolstrip.Text = "Port 1.0 actor to debug";
            this.port10todebugactortoolstrip.Click += new System.EventHandler(this.port10todebugactortoolstrip_Click);
            // 
            // downloadZ64ovlGithubToolStripMenuItem
            // 
            this.downloadZ64ovlGithubToolStripMenuItem.Name = "downloadZ64ovlGithubToolStripMenuItem";
            this.downloadZ64ovlGithubToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.downloadZ64ovlGithubToolStripMenuItem.Text = "Download old z64ovl github (SO tutorial 6)";
            this.downloadZ64ovlGithubToolStripMenuItem.Click += new System.EventHandler(this.downloadZ64ovlGithubToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(298, 22);
            this.toolStripMenuItem2.Text = "Download new z64ovl github (deprecated)";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // Downloadz64hdr
            // 
            this.Downloadz64hdr.Name = "Downloadz64hdr";
            this.Downloadz64hdr.Size = new System.Drawing.Size(298, 22);
            this.Downloadz64hdr.Text = "Download z64hdr";
            this.Downloadz64hdr.Click += new System.EventHandler(this.Downloadz64hdr_Click);
            // 
            // detectTexturesToolStripMenuItem
            // 
            this.detectTexturesToolStripMenuItem.Name = "detectTexturesToolStripMenuItem";
            this.detectTexturesToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.detectTexturesToolStripMenuItem.Text = "Detect textures";
            this.detectTexturesToolStripMenuItem.Click += new System.EventHandler(this.detectTexturesToolStripMenuItem_Click);
            // 
            // debugGenerateAtomFuncsToolStripMenuItem
            // 
            this.debugGenerateAtomFuncsToolStripMenuItem.Name = "debugGenerateAtomFuncsToolStripMenuItem";
            this.debugGenerateAtomFuncsToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.debugGenerateAtomFuncsToolStripMenuItem.Text = "[Debug] generate atom funcs";
            this.debugGenerateAtomFuncsToolStripMenuItem.Visible = false;
            this.debugGenerateAtomFuncsToolStripMenuItem.Click += new System.EventHandler(this.debugGenerateAtomFuncsToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSettingsToolStripMenuItem,
            this.restoreAllSettingsToDefaultToolStripMenuItem,
            this.changeMipsCompileFlagsToolStripMenuItem,
            this.chanceCCompileFlagsToolStripMenuItem});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(62, 22);
            this.toolStripDropDownButton3.Text = "Settings";
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.saveSettingsToolStripMenuItem.Text = "Save Settings";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsToolStripMenuItem_Click);
            // 
            // restoreAllSettingsToDefaultToolStripMenuItem
            // 
            this.restoreAllSettingsToDefaultToolStripMenuItem.Name = "restoreAllSettingsToDefaultToolStripMenuItem";
            this.restoreAllSettingsToDefaultToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.restoreAllSettingsToDefaultToolStripMenuItem.Text = "Restore all Settings to default";
            this.restoreAllSettingsToDefaultToolStripMenuItem.Click += new System.EventHandler(this.restoreAllSettingsToDefaultToolStripMenuItem_Click);
            // 
            // changeMipsCompileFlagsToolStripMenuItem
            // 
            this.changeMipsCompileFlagsToolStripMenuItem.Name = "changeMipsCompileFlagsToolStripMenuItem";
            this.changeMipsCompileFlagsToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.changeMipsCompileFlagsToolStripMenuItem.Text = "Change Mips compile flags";
            this.changeMipsCompileFlagsToolStripMenuItem.Click += new System.EventHandler(this.changeMipsCompileFlagsToolStripMenuItem_Click);
            // 
            // chanceCCompileFlagsToolStripMenuItem
            // 
            this.chanceCCompileFlagsToolStripMenuItem.Name = "chanceCCompileFlagsToolStripMenuItem";
            this.chanceCCompileFlagsToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.chanceCCompileFlagsToolStripMenuItem.Text = "Change C compile flags";
            this.chanceCCompileFlagsToolStripMenuItem.Click += new System.EventHandler(this.chanceCCompileFlagsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 556);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.TabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "Custom Actor Toolkit 0.54";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.VRAM)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.CompilePage.ResumeLayout(false);
            this.CompilePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActorAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DMARow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActorID)).EndInit();
            this.DecompilePage.ResumeLayout(false);
            this.DecompilePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DecActorID)).EndInit();
            this.ObjectsPage.ResumeLayout(false);
            this.ObjectsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectDMARow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectInjectOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectID)).EndInit();
            this.OtherPage.ResumeLayout(false);
            this.OtherPage.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SetOverlayPathButton;
        private System.Windows.Forms.Button CompileButton;
        private System.Windows.Forms.Label OverlayLabel;
        private HexNumericUpdown VRAM;
        private System.Windows.Forms.Label vramoffsetlabel;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage CompilePage;
        private System.Windows.Forms.TabPage DecompilePage;
        private System.Windows.Forms.Button LaunchButton;
        private System.Windows.Forms.Button ClearDmaButton;
        private System.Windows.Forms.Button InjectButton;
        private System.Windows.Forms.CheckBox WarningCheckbox;
        private System.Windows.Forms.Button FindEmptyActorIDButton;
        private System.Windows.Forms.NumericUpDown ActorID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button FindEmptyVRAMButton;
        private System.Windows.Forms.Button FindEmptySpaceButton;
        private System.Windows.Forms.NumericUpDown InjectOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button UpdateCRCButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem loadROMToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown DecActorID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DecompileAtomButton;
        private System.Windows.Forms.CheckBox JalFix;
        private System.Windows.Forms.ToolStripMenuItem openRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findStartDmaTableAdressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getFilesystemToolStripMenuItem;
        private System.Windows.Forms.TabPage ObjectsPage;
        private System.Windows.Forms.Button InjectZobj;
        private System.Windows.Forms.Button FindEmptySpace2;
        private System.Windows.Forms.NumericUpDown ObjectInjectOffset;
        private System.Windows.Forms.Button ImportZobj;
        private System.Windows.Forms.Button ExportZobj;
        private System.Windows.Forms.NumericUpDown ObjectID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ZobjLabel;
        private System.Windows.Forms.Button FindEmptyObjectID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage OtherPage;
        private System.Windows.Forms.ToolStripMenuItem functionsToDbgVersion2ToolStripMenuItem;
        private System.Windows.Forms.CheckBox JalNames;
        private System.Windows.Forms.CheckBox HexOffsets;
        private System.Windows.Forms.ToolStripMenuItem j0FuncsToolStripMenuItem;
        private System.Windows.Forms.Label OpenARomlabel;
        private System.Windows.Forms.Button RandomDropTableEditorButton;
        private System.Windows.Forms.ToolStripMenuItem getZobjInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem correctAndDetailSFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertFileToMipsVariableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertDListToMipsDListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portDebugRomActorTo10ToolStripMenuItem;
        private System.Windows.Forms.Label DecompilingLabel;
        private System.Windows.Forms.ToolStripMenuItem downloadZ64ovlGithubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detectTexturesToolStripMenuItem;
        private System.Windows.Forms.Label SpecialOverlayLabel;
        private System.Windows.Forms.Label SpecialOverlayLabel2;
        private System.Windows.Forms.Button SendTozzrp;
        private System.Windows.Forms.Button FindOriginalRowButton;
        private System.Windows.Forms.NumericUpDown DMARow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label DMARowLabel;
        private System.Windows.Forms.Label ObjectDMARowLabel;
        private System.Windows.Forms.Button ObjectFindOriginalRowButton;
        private System.Windows.Forms.NumericUpDown ObjectDMARow;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button ObjectSendTozzrp;
        private System.Windows.Forms.ToolStripMenuItem debugGenerateAtomFuncsToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreAllSettingsToDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeMipsCompileFlagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chanceCCompileFlagsToolStripMenuItem;
        private System.Windows.Forms.Button ZobjAnimationCopyButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown ActorAllocation;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem Downloadz64hdr;
        private System.Windows.Forms.ToolStripMenuItem port10todebugactortoolstrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.CheckBox UseZ64hdr;
    }
}

