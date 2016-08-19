using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using MediaToolkit.Model;
using MediaToolkit;
    
namespace Harmony
{
    public partial class FrmMain : Form
    {
        private AudioFile _file;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            sheet.Focus();
            using (OpenFileDialog diag = new OpenFileDialog()) {
                diag.Filter = @"Audio and Video|*.mp3; *.flac; *.wav; *.ogg; *.wma; *.alac; *.aac; *.gsm; *.mp2; *.mp1; *.mpg;
                                *.arf; *.aiff; *.opus; *.mp4a; *.ra; *.pcm; *.avi; *.mp4; *.mpeg; *.rmvb; *.flv; *.mov; *.wmv";
                diag.CheckPathExists = true;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(diag.FileName);
                }
            }
        }

        private void FourierComplete(object sender, EventArgs e)
        {
            TmrRedraw.Stop();
        }

        double _maxLen = 0;
        TimeSpan _prevTime = TimeSpan.FromTicks(-1);
        TimeSpan _curTime = TimeSpan.Zero;
        private void FourierProgress(object sender, AudioFile.FourierProgressEventArgs e)
        {
            if (e.Time != _curTime)
            {
                _prevTime = _curTime;
                _curTime = e.Time;
            }

            if (!sheet.InvokeRequired)
            {
                for (int i = 0; i < e.Frequency.Count(); ++i)
                {
                    if (e.Frequency[i] == 0 || e.Amplitude[i] <= 0.1) continue; // not significant, skip

                    MusicSheet.Note curNote = new MusicSheet.Note(e.Time, e.Frequency[i], e.Length[i], e.Amplitude[i]);
                    MusicSheet.Note prevNote = sheet.GetNote(_prevTime, curNote.FullName);

                    int ct = 1;
                    while (prevNote == null && ct < curNote.Value.Ticks / sheet.TimePerNote.Ticks)
                    {
                        prevNote = sheet.GetNote( _file.SamplesToTime( _file.TimeToSamples(_prevTime) - (int)_file.AnalyzeLen * ct), 
                            curNote.FullName);
                        ct ++; 
                    }
                    if (prevNote != null)
                    {
                        if (prevNote.Parent != null)
                        {
                            prevNote = prevNote.Parent;
                        }

                        curNote.Parent = prevNote;
                        curNote.Clef = MusicSheet.Clef.Hidden;
                        prevNote.Value += TimeSpan.FromTicks( curNote.Value.Ticks);
                        prevNote.Value += TimeSpan.FromTicks( curNote.Position.Ticks - prevNote.Position.Ticks - prevNote.Value.Ticks );

                        if (prevNote.Value.TotalSeconds > _maxLen &&
                            prevNote.Intensity > 0.5 && prevNote.Value > TimeSpan.FromTicks((long)(sheet.TimePerNote.Ticks * 0.125)))
                        {
                            if (prevNote.Value.TotalSeconds >= 16)
                            {
                                sheet.Tempo = 960.0 / prevNote.Value.TotalSeconds;
                            }
                            if (prevNote.Value.TotalSeconds >= 8)
                            {
                                sheet.Tempo = 480.0 / prevNote.Value.TotalSeconds;
                            }
                            else if (prevNote.Value.TotalSeconds >= 4)
                            {
                                sheet.Tempo = 240.0 / prevNote.Value.TotalSeconds;
                            }
                            else if (prevNote.Value.TotalSeconds >= 2)
                            {
                                sheet.Tempo = 120.0 / prevNote.Value.TotalSeconds;
                            }
                            else if (prevNote.Value.TotalSeconds >= 1)
                            {
                                sheet.Tempo = 60.0 / prevNote.Value.TotalSeconds;
                            }
                            else if (prevNote.Value .TotalSeconds > 0)
                            {
                                sheet.Tempo = 30.0 / prevNote.Value.TotalSeconds;
                            }
                            _maxLen = prevNote.Value.TotalSeconds;
                        }
                    }
                    sheet.AddNote(curNote);
                }
            }
            else
            {
                sheet.Invoke(new Action(delegate { FourierProgress(sender, e); }));
            }
        }

        private void PlaybackStopped(object sender, NAudio.Wave.StoppedEventArgs e)
        {
            BtnPlay.Text = "Play";
            TmrUpdate.Stop();
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            sheet.Focus();
            if (_file.IsPlaying)
            {
                _file.Pause();
                BtnPlay.Text = "Play";
                TmrUpdate.Stop();
            }
            else
            {
                _file.CurrentTime = sheet.Time;
                _file.Play();
                BtnPlay.Text = "Pause";
                TmrUpdate.Start();
            }
        }

        private void TmrUpdate_Tick(object sender, EventArgs e)
        {
            sheet.Time = _file.CurrentTime;
        }

        private void sheet_UpdateTime(object sender, TimeSpan time)
        {
            if (_file == null) return;
            _file.CurrentTime = time;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            bool first = true;
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (first) { first = false; continue; } // skip first arg;
                if (!string.IsNullOrEmpty(arg) && File.Exists(arg))
                {
                    LoadFile(arg);
                    break;
                }
            }
        }
        
        public void LoadFile(string path)
        {
            try {
                TmrUpdate.Stop();
                if (_file != null) { 
                    _file.FourierProgress -= FourierProgress;
                    _file.FourierComplete -= FourierComplete;
                    _file.Dispose();
                }
                BtnPlay.Show();
                BtnSave.Show();
                BtnSynth.Show();
                BtnConvert.Show();
                LbTip.Text = "Loading audio file, please wait...";
                LbTip.Show();
                _curTime = TimeSpan.Zero;

                sheet.ClearNotes();
                sheet.StopSynthesize();

                sheet.Redraw();
                sheet.ScrollTop();

                _file = new AudioFile(path);

                _file.LoadComplete += (object sender, EventArgs e) =>
                {
                    LbTip.Invoke(new Action(delegate {
                        LbTip.Text = "Right click near the left end of a note to edit it.\nCtrl + N to add a note.";
                    }));
                    _file.Stopped += PlaybackStopped;
                    _file.FourierProgress += FourierProgress;
                    _file.FourierComplete += FourierComplete;

                    _prevTime = TimeSpan.FromTicks(-1);
                    _curTime = TimeSpan.Zero;

                    sheet.Invoke (new Action(() =>
                    {
                        sheet.Length = _file.TotalTime;
                        sheet.Time = TimeSpan.Zero;

                        sheet.Redraw();
                        sheet.ScrollTop();
                        TmrRedraw.Start();
                    }));

                    _file.Analyze();
                };
                _maxLen = 0;

                this.Text = Path.GetFileName(path) + " - " + Application.ProductName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("Failed to load audio file! File may be corrupted or unsupported.","Load failed", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation) ;
            }
        }

        private void sheet_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.O)
                {
                    BtnOpen.PerformClick();
                }
                else if (e.KeyCode == Keys.E)
                {
                    BtnSave.PerformClick();
                }
                else if (e.KeyCode == Keys.T)
                {
                    BtnConvert.PerformClick();
                }
                else if (e.KeyCode == Keys.S)
                {
                    BtnSynth.PerformClick();
                }
                else if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    sheet.NoteWidth *= 1.1F;
                    sheet.Redraw();
                }
                else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
                {
                    sheet.NoteWidth *= 1/1.1F;
                    sheet.Redraw();
                }
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (_file == null) return;
                BtnPlay.PerformClick();
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (_file == null) return;
                _file.CurrentTime  = _file.CurrentTime.Subtract(TimeSpan.FromSeconds(0.25));
                sheet.Time = _file.CurrentTime;
                sheet.Redraw();
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (_file == null) return;
                _file.CurrentTime  = _file.CurrentTime.Add(TimeSpan.FromSeconds(0.25));
                sheet.Time = _file.CurrentTime;
                sheet.Redraw();
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try {
                _file.Dispose();
            }
            catch { }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            sheet.Focus();
            using (SaveFileDialog diag =new SaveFileDialog())
            {
                diag.Title = "Export";
                diag.Filter = "PNG Image|*.png|JPEG Image|*.jpeg,*.jpg|GIF Image|*.gif|Bitmap|*.bmp";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    try {
                        sheet.ExportCanvas(diag.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("Export failed! Make sure you have permissions to write to the directory.", "Export Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            sheet.Focus();
            MediaFile inFile = new MediaFile(_file.FilePath);
            using (SaveFileDialog diag =new SaveFileDialog())
            {
                diag.Title = "Convert To";
                diag.Filter = "MP3|*.mp3|FLAC|*.flac|WAV|*.wav|Vorbis|*.ogg|Apple Lossless|*.alac|WMA|*.wma|AAC|*.aac|GSM|*.gsm|Any File|*";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    try {
                        MediaFile outFile = new MediaFile(diag.FileName);
                        using (Engine eng = new Engine())
                        {
                            eng.Convert(inFile, outFile);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Convert failed! Make sure you have permissions to write to the directory.", "Convert Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void BtnSynth_Click(object sender, EventArgs e)
        {
            sheet.Focus();
            if (sheet.Synthesizing)
            {
                sheet.StopSynthesize();
                BtnSynth.Text = "Synth";
            }
            else
            {
                sheet.Synthesize();
                BtnSynth.Text = "Stop";
            }
        }

        private void sheet_SynthStopped(object sender, EventArgs e)
        {
            BtnSynth.Text = "Synth";
        }

        private void sheet_SynthStarted(object sender, EventArgs e)
        {
            BtnSynth.Text = "Stop";
        }

        private void TmrRedraw_Tick(object sender, EventArgs e)
        {
            sheet.Redraw();
        }
    }
}
