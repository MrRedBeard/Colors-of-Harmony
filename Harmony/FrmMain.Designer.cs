namespace Harmony
{
    partial class FrmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.BtnOpen = new System.Windows.Forms.Button();
            this.BtnPlay = new System.Windows.Forms.Button();
            this.TmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnConvert = new System.Windows.Forms.Button();
            this.LbTip = new System.Windows.Forms.Label();
            this.BtnSynth = new System.Windows.Forms.Button();
            this.TmrRedraw = new System.Windows.Forms.Timer(this.components);
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.sheet = new Harmony.MusicSheet();
            this.SuspendLayout();
            // 
            // BtnOpen
            // 
            this.BtnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.BtnOpen.FlatAppearance.BorderSize = 0;
            this.BtnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOpen.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnOpen.ForeColor = System.Drawing.Color.White;
            this.BtnOpen.Location = new System.Drawing.Point(16, 16);
            this.BtnOpen.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(100, 36);
            this.BtnOpen.TabIndex = 0;
            this.BtnOpen.TabStop = false;
            this.BtnOpen.Text = "Open";
            this.tt.SetToolTip(this.BtnOpen, "Open audio/video file (Ctrl + O)");
            this.BtnOpen.UseVisualStyleBackColor = false;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // BtnPlay
            // 
            this.BtnPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.BtnPlay.FlatAppearance.BorderSize = 0;
            this.BtnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPlay.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnPlay.ForeColor = System.Drawing.Color.White;
            this.BtnPlay.Location = new System.Drawing.Point(124, 16);
            this.BtnPlay.Margin = new System.Windows.Forms.Padding(4);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.Size = new System.Drawing.Size(100, 36);
            this.BtnPlay.TabIndex = 1;
            this.BtnPlay.TabStop = false;
            this.BtnPlay.Text = "Play";
            this.tt.SetToolTip(this.BtnPlay, "Play/pause music (Space)");
            this.BtnPlay.UseVisualStyleBackColor = false;
            this.BtnPlay.Visible = false;
            this.BtnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // TmrUpdate
            // 
            this.TmrUpdate.Tick += new System.EventHandler(this.TmrUpdate_Tick);
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.BtnSave.FlatAppearance.BorderSize = 0;
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnSave.ForeColor = System.Drawing.Color.White;
            this.BtnSave.Location = new System.Drawing.Point(908, 16);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(100, 36);
            this.BtnSave.TabIndex = 5;
            this.BtnSave.TabStop = false;
            this.BtnSave.Text = "Export";
            this.tt.SetToolTip(this.BtnSave, "Export the sheet music as an image (Ctrl + E)");
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Visible = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnConvert
            // 
            this.BtnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.BtnConvert.FlatAppearance.BorderSize = 0;
            this.BtnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConvert.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnConvert.ForeColor = System.Drawing.Color.White;
            this.BtnConvert.Location = new System.Drawing.Point(800, 16);
            this.BtnConvert.Margin = new System.Windows.Forms.Padding(4);
            this.BtnConvert.Name = "BtnConvert";
            this.BtnConvert.Size = new System.Drawing.Size(100, 36);
            this.BtnConvert.TabIndex = 6;
            this.BtnConvert.TabStop = false;
            this.BtnConvert.Text = "Convert";
            this.tt.SetToolTip(this.BtnConvert, "Convert the current audio file to another audio format (Ctrl + T)");
            this.BtnConvert.UseVisualStyleBackColor = false;
            this.BtnConvert.Visible = false;
            this.BtnConvert.Click += new System.EventHandler(this.BtnConvert_Click);
            // 
            // LbTip
            // 
            this.LbTip.Location = new System.Drawing.Point(362, 16);
            this.LbTip.Name = "LbTip";
            this.LbTip.Size = new System.Drawing.Size(358, 36);
            this.LbTip.TabIndex = 7;
            this.LbTip.Text = "Right click near the left end of a note to edit it. \r\nCtrl+N to add a note.";
            this.LbTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LbTip.Visible = false;
            // 
            // BtnSynth
            // 
            this.BtnSynth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.BtnSynth.FlatAppearance.BorderSize = 0;
            this.BtnSynth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSynth.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.BtnSynth.ForeColor = System.Drawing.Color.White;
            this.BtnSynth.Location = new System.Drawing.Point(232, 16);
            this.BtnSynth.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSynth.Name = "BtnSynth";
            this.BtnSynth.Size = new System.Drawing.Size(100, 36);
            this.BtnSynth.TabIndex = 8;
            this.BtnSynth.TabStop = false;
            this.BtnSynth.Text = "Synth";
            this.tt.SetToolTip(this.BtnSynth, "Play synthesized notes (Ctrl + S)");
            this.BtnSynth.UseVisualStyleBackColor = false;
            this.BtnSynth.Visible = false;
            this.BtnSynth.Click += new System.EventHandler(this.BtnSynth_Click);
            // 
            // TmrRedraw
            // 
            this.TmrRedraw.Tick += new System.EventHandler(this.TmrRedraw_Tick);
            // 
            // sheet
            // 
            this.sheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sheet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.sheet.BarPadding = 0F;
            this.sheet.BaseNote = 0.25D;
            this.sheet.Font = new System.Drawing.Font("Verdana", 10F);
            this.sheet.ForeColor = System.Drawing.Color.GhostWhite;
            this.sheet.Length = System.TimeSpan.Parse("00:00:00");
            this.sheet.Location = new System.Drawing.Point(1, 71);
            this.sheet.Margin = new System.Windows.Forms.Padding(4);
            this.sheet.MinimumSize = new System.Drawing.Size(220, 0);
            this.sheet.Name = "sheet";
            this.sheet.NotesPerBar = 4;
            this.sheet.NoteWidth = 40F;
            this.sheet.RowPadding = 95F;
            this.sheet.SheetPadding = new System.Windows.Forms.Padding(35, 30, 20, 10);
            this.sheet.Size = new System.Drawing.Size(1025, 546);
            this.sheet.SpaceHeight = 15F;
            this.sheet.StaffSpacing = 55F;
            this.sheet.Synthesizing = false;
            this.sheet.TabIndex = 4;
            this.sheet.Tempo = 60D;
            this.sheet.Time = System.TimeSpan.Parse("-00:00:00.0000001");
            this.sheet.UpdateTime += new Harmony.MusicSheet.UpdateTimeDelegate(this.sheet_UpdateTime);
            this.sheet.InnerKeyUp += new Harmony.MusicSheet.KeyDelegate(this.sheet_KeyUp);
            this.sheet.SynthStopped += new System.EventHandler<System.EventArgs>(this.sheet_SynthStopped);
            this.sheet.SynthStarted += new System.EventHandler<System.EventArgs>(this.sheet_SynthStarted);
            this.sheet.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sheet_KeyUp);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1021, 616);
            this.Controls.Add(this.BtnSynth);
            this.Controls.Add(this.LbTip);
            this.Controls.Add(this.BtnConvert);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.sheet);
            this.Controls.Add(this.BtnPlay);
            this.Controls.Add(this.BtnOpen);
            this.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(350, 39);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Colors of Harmony";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sheet_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.Button BtnPlay;
        private MusicSheet sheet;
        private System.Windows.Forms.Timer TmrUpdate;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnConvert;
        private System.Windows.Forms.Label LbTip;
        private System.Windows.Forms.Button BtnSynth;
        private System.Windows.Forms.Timer TmrRedraw;
        private System.Windows.Forms.ToolTip tt;
    }
}

