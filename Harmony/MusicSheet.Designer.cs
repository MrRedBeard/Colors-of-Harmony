namespace Harmony
{
    partial class MusicSheet
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.canvas = new System.Windows.Forms.PictureBox();
            this.scroller = new System.Windows.Forms.Panel();
            this.PnlEditNote = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.TbWL = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TbFreq = new System.Windows.Forms.TextBox();
            this.BtnDelNote = new System.Windows.Forms.Button();
            this.PbIntensity = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.NIntensity = new System.Windows.Forms.NumericUpDown();
            this.TbVal = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TbDur = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TbStart = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.NOctave = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TbNoteName = new System.Windows.Forms.TextBox();
            this.BtnClosePnl = new System.Windows.Forms.Button();
            this.TbBaseNote = new System.Windows.Forms.TextBox();
            this.NTempo = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NBar = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TbTime = new System.Windows.Forms.TextBox();
            this.LbTotalTime = new System.Windows.Forms.Label();
            this.BtnDuplicate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.scroller.SuspendLayout();
            this.PnlEditNote.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NOctave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NTempo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NBar)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.Transparent;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Margin = new System.Windows.Forms.Padding(4);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(1135, 735);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            this.canvas.Resize += new System.EventHandler(this.canvas_Resize);
            // 
            // scroller
            // 
            this.scroller.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scroller.AutoScroll = true;
            this.scroller.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.scroller.Controls.Add(this.canvas);
            this.scroller.Location = new System.Drawing.Point(0, 0);
            this.scroller.Name = "scroller";
            this.scroller.Size = new System.Drawing.Size(1139, 735);
            this.scroller.TabIndex = 1;
            // 
            // PnlEditNote
            // 
            this.PnlEditNote.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PnlEditNote.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.PnlEditNote.Controls.Add(this.BtnDuplicate);
            this.PnlEditNote.Controls.Add(this.label13);
            this.PnlEditNote.Controls.Add(this.TbWL);
            this.PnlEditNote.Controls.Add(this.label12);
            this.PnlEditNote.Controls.Add(this.TbFreq);
            this.PnlEditNote.Controls.Add(this.BtnDelNote);
            this.PnlEditNote.Controls.Add(this.PbIntensity);
            this.PnlEditNote.Controls.Add(this.label11);
            this.PnlEditNote.Controls.Add(this.NIntensity);
            this.PnlEditNote.Controls.Add(this.TbVal);
            this.PnlEditNote.Controls.Add(this.label10);
            this.PnlEditNote.Controls.Add(this.TbDur);
            this.PnlEditNote.Controls.Add(this.label9);
            this.PnlEditNote.Controls.Add(this.TbStart);
            this.PnlEditNote.Controls.Add(this.label8);
            this.PnlEditNote.Controls.Add(this.label7);
            this.PnlEditNote.Controls.Add(this.NOctave);
            this.PnlEditNote.Controls.Add(this.label6);
            this.PnlEditNote.Controls.Add(this.label5);
            this.PnlEditNote.Controls.Add(this.TbNoteName);
            this.PnlEditNote.Controls.Add(this.BtnClosePnl);
            this.PnlEditNote.Location = new System.Drawing.Point(399, 262);
            this.PnlEditNote.Name = "PnlEditNote";
            this.PnlEditNote.Size = new System.Drawing.Size(370, 409);
            this.PnlEditNote.TabIndex = 0;
            this.PnlEditNote.Visible = false;
            this.PnlEditNote.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.PnlEditNote.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.PnlEditNote.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(32, 170);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 17);
            this.label13.TabIndex = 27;
            this.label13.Text = "Wavelength (m)";
            // 
            // TbWL
            // 
            this.TbWL.Location = new System.Drawing.Point(163, 167);
            this.TbWL.Name = "TbWL";
            this.TbWL.Size = new System.Drawing.Size(179, 24);
            this.TbWL.TabIndex = 3;
            this.TbWL.Text = "???";
            this.TbWL.TextChanged += new System.EventHandler(this.TbWL_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(32, 140);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 17);
            this.label12.TabIndex = 25;
            this.label12.Text = "Frequency (Hz)";
            // 
            // TbFreq
            // 
            this.TbFreq.Location = new System.Drawing.Point(163, 137);
            this.TbFreq.Name = "TbFreq";
            this.TbFreq.Size = new System.Drawing.Size(179, 24);
            this.TbFreq.TabIndex = 2;
            this.TbFreq.Text = "???";
            this.TbFreq.TextChanged += new System.EventHandler(this.TbFreq_TextChanged);
            // 
            // BtnDelNote
            // 
            this.BtnDelNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDelNote.BackColor = System.Drawing.Color.Brown;
            this.BtnDelNote.FlatAppearance.BorderSize = 0;
            this.BtnDelNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelNote.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnDelNote.ForeColor = System.Drawing.Color.White;
            this.BtnDelNote.Location = new System.Drawing.Point(35, 356);
            this.BtnDelNote.Margin = new System.Windows.Forms.Padding(4);
            this.BtnDelNote.Name = "BtnDelNote";
            this.BtnDelNote.Size = new System.Drawing.Size(126, 30);
            this.BtnDelNote.TabIndex = 8;
            this.BtnDelNote.Text = "Delete Note";
            this.BtnDelNote.UseVisualStyleBackColor = false;
            this.BtnDelNote.Click += new System.EventHandler(this.BtnDelNote_Click);
            // 
            // PbIntensity
            // 
            this.PbIntensity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(73)))), ((int)(((byte)(5)))));
            this.PbIntensity.Location = new System.Drawing.Point(316, 310);
            this.PbIntensity.Name = "PbIntensity";
            this.PbIntensity.Size = new System.Drawing.Size(26, 24);
            this.PbIntensity.TabIndex = 23;
            this.PbIntensity.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 312);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 17);
            this.label11.TabIndex = 22;
            this.label11.Text = "Intensity";
            this.label11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.label11.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.label11.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // NIntensity
            // 
            this.NIntensity.DecimalPlaces = 3;
            this.NIntensity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NIntensity.Location = new System.Drawing.Point(163, 310);
            this.NIntensity.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NIntensity.Name = "NIntensity";
            this.NIntensity.Size = new System.Drawing.Size(147, 24);
            this.NIntensity.TabIndex = 7;
            this.NIntensity.Value = new decimal(new int[] {
            75,
            0,
            0,
            131072});
            this.NIntensity.ValueChanged += new System.EventHandler(this.NIntensity_ValueChanged);
            this.NIntensity.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NIntensity_KeyUp);
            // 
            // TbVal
            // 
            this.TbVal.Location = new System.Drawing.Point(163, 268);
            this.TbVal.Name = "TbVal";
            this.TbVal.Size = new System.Drawing.Size(179, 24);
            this.TbVal.TabIndex = 6;
            this.TbVal.Text = "1/4";
            this.TbVal.TextChanged += new System.EventHandler(this.TbVal_TextChanged);
            this.TbVal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NIntensity_KeyUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(35, 271);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "Value";
            this.label10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.label10.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.label10.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // TbDur
            // 
            this.TbDur.Location = new System.Drawing.Point(163, 238);
            this.TbDur.Name = "TbDur";
            this.TbDur.Size = new System.Drawing.Size(179, 24);
            this.TbDur.TabIndex = 5;
            this.TbDur.Text = "1.00";
            this.TbDur.TextChanged += new System.EventHandler(this.TbDur_TextChanged);
            this.TbDur.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NIntensity_KeyUp);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(35, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 17);
            this.label9.TabIndex = 17;
            this.label9.Text = "Duration(s)";
            this.label9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.label9.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.label9.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // TbStart
            // 
            this.TbStart.Location = new System.Drawing.Point(163, 209);
            this.TbStart.Name = "TbStart";
            this.TbStart.Size = new System.Drawing.Size(179, 24);
            this.TbStart.TabIndex = 4;
            this.TbStart.Text = "00:00.0000";
            this.TbStart.TextChanged += new System.EventHandler(this.TbStart_TextChanged);
            this.TbStart.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NIntensity_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 212);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Start Time";
            this.label8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.label8.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.label8.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 14F);
            this.label7.Location = new System.Drawing.Point(31, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 23);
            this.label7.TabIndex = 14;
            this.label7.Text = "Edit Note";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.label7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.label7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // NOctave
            // 
            this.NOctave.Location = new System.Drawing.Point(163, 95);
            this.NOctave.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.NOctave.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NOctave.Name = "NOctave";
            this.NOctave.Size = new System.Drawing.Size(179, 24);
            this.NOctave.TabIndex = 1;
            this.NOctave.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NOctave.ValueChanged += new System.EventHandler(this.NOctave_ValueChanged);
            this.NOctave.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NIntensity_KeyUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Octave";
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.label6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.label6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Note Name";
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label11_MouseDown);
            this.label5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label11_MouseMove);
            this.label5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label11_MouseUp);
            // 
            // TbNoteName
            // 
            this.TbNoteName.Location = new System.Drawing.Point(163, 66);
            this.TbNoteName.Name = "TbNoteName";
            this.TbNoteName.Size = new System.Drawing.Size(179, 24);
            this.TbNoteName.TabIndex = 0;
            this.TbNoteName.Text = "C";
            this.TbNoteName.TextChanged += new System.EventHandler(this.TbNoteName_TextChanged);
            this.TbNoteName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NIntensity_KeyUp);
            // 
            // BtnClosePnl
            // 
            this.BtnClosePnl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClosePnl.BackColor = System.Drawing.Color.Transparent;
            this.BtnClosePnl.FlatAppearance.BorderSize = 0;
            this.BtnClosePnl.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.BtnClosePnl.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.BtnClosePnl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClosePnl.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnClosePnl.ForeColor = System.Drawing.Color.White;
            this.BtnClosePnl.Location = new System.Drawing.Point(332, 0);
            this.BtnClosePnl.Margin = new System.Windows.Forms.Padding(4);
            this.BtnClosePnl.Name = "BtnClosePnl";
            this.BtnClosePnl.Size = new System.Drawing.Size(38, 30);
            this.BtnClosePnl.TabIndex = 6;
            this.BtnClosePnl.TabStop = false;
            this.BtnClosePnl.Text = "x";
            this.BtnClosePnl.UseVisualStyleBackColor = false;
            this.BtnClosePnl.Click += new System.EventHandler(this.BtnClosePnl_Click);
            // 
            // TbBaseNote
            // 
            this.TbBaseNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TbBaseNote.Location = new System.Drawing.Point(267, 751);
            this.TbBaseNote.Name = "TbBaseNote";
            this.TbBaseNote.Size = new System.Drawing.Size(100, 24);
            this.TbBaseNote.TabIndex = 1;
            this.TbBaseNote.Text = "1/4";
            this.TbBaseNote.TextChanged += new System.EventHandler(this.TbBaseNote_TextChanged);
            this.TbBaseNote.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MusicSheet_KeyUp);
            // 
            // NTempo
            // 
            this.NTempo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NTempo.DecimalPlaces = 2;
            this.NTempo.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.NTempo.Location = new System.Drawing.Point(79, 752);
            this.NTempo.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.NTempo.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NTempo.Name = "NTempo";
            this.NTempo.Size = new System.Drawing.Size(83, 24);
            this.NTempo.TabIndex = 0;
            this.NTempo.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.NTempo.ValueChanged += new System.EventHandler(this.nTempo_ValueChanged);
            this.NTempo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MusicSheet_KeyUp);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 754);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tempo";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(393, 754);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Notes/Bar";
            // 
            // NBar
            // 
            this.NBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NBar.Location = new System.Drawing.Point(477, 752);
            this.NBar.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.NBar.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NBar.Name = "NBar";
            this.NBar.Size = new System.Drawing.Size(83, 24);
            this.NBar.TabIndex = 2;
            this.NBar.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NBar.ValueChanged += new System.EventHandler(this.nBar_ValueChanged);
            this.NBar.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MusicSheet_KeyUp);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 754);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Base Note";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(862, 754);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Time";
            // 
            // TbTime
            // 
            this.TbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TbTime.Location = new System.Drawing.Point(909, 751);
            this.TbTime.Name = "TbTime";
            this.TbTime.Size = new System.Drawing.Size(100, 24);
            this.TbTime.TabIndex = 3;
            this.TbTime.Text = "00:00.0000";
            this.TbTime.TextChanged += new System.EventHandler(this.TbTime_TextChanged);
            this.TbTime.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MusicSheet_KeyUp);
            // 
            // LbTotalTime
            // 
            this.LbTotalTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LbTotalTime.AutoSize = true;
            this.LbTotalTime.Location = new System.Drawing.Point(1015, 754);
            this.LbTotalTime.Name = "LbTotalTime";
            this.LbTotalTime.Size = new System.Drawing.Size(102, 17);
            this.LbTotalTime.TabIndex = 10;
            this.LbTotalTime.Text = "/ 00:00.0000";
            // 
            // BtnDuplicate
            // 
            this.BtnDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDuplicate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.BtnDuplicate.FlatAppearance.BorderSize = 0;
            this.BtnDuplicate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDuplicate.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnDuplicate.ForeColor = System.Drawing.Color.White;
            this.BtnDuplicate.Location = new System.Drawing.Point(169, 356);
            this.BtnDuplicate.Margin = new System.Windows.Forms.Padding(4);
            this.BtnDuplicate.Name = "BtnDuplicate";
            this.BtnDuplicate.Size = new System.Drawing.Size(126, 30);
            this.BtnDuplicate.TabIndex = 28;
            this.BtnDuplicate.Text = "Duplicate Note";
            this.BtnDuplicate.UseVisualStyleBackColor = false;
            this.BtnDuplicate.Click += new System.EventHandler(this.BtnDuplicate_Click);
            // 
            // MusicSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.Controls.Add(this.LbTotalTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TbTime);
            this.Controls.Add(this.PnlEditNote);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NBar);
            this.Controls.Add(this.NTempo);
            this.Controls.Add(this.TbBaseNote);
            this.Controls.Add(this.scroller);
            this.Font = new System.Drawing.Font("Verdana", 10F);
            this.ForeColor = System.Drawing.Color.GhostWhite;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(240, 0);
            this.Name = "MusicSheet";
            this.Size = new System.Drawing.Size(1144, 790);
            this.Load += new System.EventHandler(this.MusicSheet_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MusicSheet_KeyUp);
            this.Resize += new System.EventHandler(this.MusicSheet_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.scroller.ResumeLayout(false);
            this.PnlEditNote.ResumeLayout(false);
            this.PnlEditNote.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NOctave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NTempo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Panel scroller;
        private System.Windows.Forms.TextBox TbBaseNote;
        private System.Windows.Forms.NumericUpDown NTempo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TbTime;
        private System.Windows.Forms.Label LbTotalTime;
        private System.Windows.Forms.Panel PnlEditNote;
        private System.Windows.Forms.Button BtnClosePnl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TbNoteName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NOctave;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TbStart;
        private System.Windows.Forms.TextBox TbDur;
        private System.Windows.Forms.TextBox TbVal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown NIntensity;
        private System.Windows.Forms.PictureBox PbIntensity;
        private System.Windows.Forms.Button BtnDelNote;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TbFreq;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TbWL;
        private System.Windows.Forms.Button BtnDuplicate;
    }
}
