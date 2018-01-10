using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Cantus.Core;
using Cantus.Core.CommonTypes;
using NAudio.Wave;
using System.Threading;

namespace Harmony
{

    public partial class MusicSheet : UserControl
    {

        public delegate void UpdateTimeDelegate(object sender, TimeSpan time);
        /// <summary>
        /// Raised when the currently select time on this music sheet has been updated by the user.
        /// </summary>
        public event UpdateTimeDelegate UpdateTime;


        public delegate void KeyDelegate(object sender, KeyEventArgs time);
        /// <summary>
        /// Raised when a key up event occurs inside the sheet
        /// </summary>
        public event KeyDelegate InnerKeyUp;

        /// <summary>
        /// Raised when the synthesizer has stopped synthesizing
        /// </summary>
        public event EventHandler<EventArgs> SynthStopped;

        /// <summary>
        /// Raised when the synthesizer has started synthesizing
        /// </summary>
        public event EventHandler<EventArgs> SynthStarted;

        /// <summary>
        /// The value of the base note
        /// </summary>
        public double BaseNote { get; set; } = 0.25;

        /// <summary>
        /// True if the sheet is currently synthesizing music.
        /// </summary>
        public bool Synthesizing { get; set; }

        private double _tempo = 60;
        /// <summary>
        /// The number of base notes per minute
        /// </summary>
        public double Tempo {
            get
            {
                return _tempo;
            }
            set
            {
                try {
                    NTempo.Value = (decimal)value;
                    _tempo = value;
                }
                catch { }
            }
        }

        /// <summary>
        /// The number of base notes to include in each bar
        /// </summary>
        public int NotesPerBar { get; set; } = 4;

        private TimeSpan _time = new TimeSpan(-1);

        /// <summary>
        /// The currently selected time on this music sheet
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                canvas.Invalidate();
            }
        }

        /// <summary>
        /// The time spent playing each base note
        /// </summary>
        public TimeSpan TimePerNote
        {
            get
            {
                return FromSeconds(Tempo / 60);
            }
        }

        /// <summary>
        /// The length in time of this music sheet
        /// </summary>
        public TimeSpan Length { get; set; }

        /// <summary>
        /// The width of each bar
        /// </summary>
        public double BarWidth
        {
            get
            {
                return 2 * BarPadding + NotesPerBar * NoteWidth;
            }
        }

        /// <summary>
        /// The maximal number of bars representable in each line (computed)
        /// </summary>
        public int BarsPerLine
        {
            get
            {
                return (int)Math.Floor((this.Width - SheetPadding.Left - SheetPadding.Right) / BarWidth);
            }
        }

        /// <summary>
        /// The maximal number of base notes representable in each line (computed)
        /// </summary>
        public int NotesPerLine
        {
            get
            {
                return BarsPerLine * NotesPerBar;
            }
        }

        /// <summary>
        /// The height of each line
        /// </summary>
        public double LineHeight
        {
            get
            {
                return 8 * SpaceHeight + StaffSpacing + 2 * RowPadding;
            }
        }

        /// <summary>
        /// The number of lines in the sheet (computed)
        /// </summary>
        public int Lines
        {
            get
            {
                return (int)decimal.Floor(Length.Ticks / (NotesPerLine * TimePerNote.Ticks) + 1);
            }
        }

        /// <summary>
        /// Standard dynamics markings representing note intensity
        /// </summary>
        public static class Intensity
        {
            public const double sforzando = 1.0;
            public const double sf = 1.0;
            public const double ffff = 1.0;
            public const double fff = 0.999;
            public const double Fortissimo = 0.99;
            public const double ff = Fortissimo;
            public const double Forte = 0.85;
            public const double f = Forte;
            public const double MezzoForte = 0.75;
            public const double mf = MezzoForte;
            public const double MezzoPiano = 0.65;
            public const double mp = MezzoPiano;
            public const double Piano = 0.55;
            public const double p = Piano;
            public const double Piannisimo = 0.4;
            public const double pp = Piannisimo;
            public const double ppp = 0.3;
            public const double pppp = 0.2;
            public const double ppppp = 0.15;
            public const double silent = 0;
        }

        /// <summary>
        /// Representations of standard note values.
        /// </summary>
        public static class NoteValue
        {
            public const double TwoHundredFiftySixth = 1 / 256;
            public const double DottedTwoHundredFiftySixth = 3 / 512;
            public const double HundredTwentyEighth = 1 / 128;
            public const double DottedHundredTwentyEighth = 3 / 256;
            public const double SixtyFourth = 1 / 64;
            public const double DottedSixtyFourth = 3 / 128;
            public const double DoubleDottedSixtyFourth = 3 / 128 + 1 / 256;
            public const double ThirtySecond = 1 / 32;
            public const double DottedThirtySecond = 3 / 64;
            public const double DoubleDottedThirtySecond = 3 / 64 + 1 / 128;
            public const double Sixteenth = 1 / 16;
            public const double DottedSixteenth = 3 / 32;
            public const double DoubleDottedSixteenth = 3 / 32 + 1 / 64;
            public const double Eighth = 1 / 8;
            public const double DottedEighth = 3 / 16;
            public const double DoubleDottedEighth = 3 / 16 + 1 / 32;
            public const double Quarter = 1 / 4;
            public const double DottedQuarter = 3 / 8;
            public const double DoubleDottedQuarter = 3 / 8 + 1 / 16;
            public const double Half = 1 / 2;
            public const double DottedHalf = 3 / 4;
            public const double DoubleDottedHalf = 3 / 4 + 1 / 8;
            public const double Whole = 1;
            public const double DottedWhole = 1.5;
            public const double DoubleDottedWhole = 1.75;
            public const double DoubleWhole = 2;
            public const double DottedDoubleWhole = 3;
            public const double QuadrupleWhole = 4;
            public const double DottedQuadrupleWhole = 6;
            public const double OctupleWhole = 8;
        }

        /// <summary>
        /// Represents a clef
        /// </summary>
        public enum Clef
        {
            Treble = 0,
            Bass,
            /// <summary>
            /// Not implemented
            /// </summary>
            Tenor,
            /// <summary>
            /// Not implemented
            /// </summary>
            Alto,
            /// <summary>
            /// Treble if higher or equal to middle C, bass if lower
            /// </summary>
            Auto = -1,
            /// <summary>
            /// Notes set to this clef will not be displayed
            /// </summary>
            Hidden = -2
        }

        public sealed class Note
        {
            /// <summary>
            /// The frequency of Middle C
            /// </summary>
            public const double MIDDLE_C_FREQ = 261.626;

            /// <summary>
            /// The frequency of A4, the tuning reference note
            /// </summary>
            public const double A4_FREQ = 440;

            /// <summary>
            /// The speed of sound
            /// </summary>
            public const double SOUND_SPEED = 343.2;

            /// <summary>
            /// If true, this note will be white to show that it is focused.
            /// </summary>
            public bool Focused { get; set; }

            /// <summary>
            /// The position of this note on the sheet, as a timespan
            /// </summary>
            public TimeSpan Position { get; set; }

            /// <summary>
            /// The parent note that this note extends, if applicable.
            /// </summary>
            public Note Parent { get; set; }

            /// <summary>
            /// The frequency, in hertz, of the note
            /// </summary>
            public double Frequency { get; set; }

            /// <summary>
            /// The wavelength, in meters
            /// </summary>
            public double Wavelength
            {
                get
                {
                    // f = v / lambda; lambda = v/f
                    return SOUND_SPEED / Frequency;
                }
                set
                {
                    Frequency = SOUND_SPEED / value;
                }
            }

            /// <summary>
            /// The intensity of the note
            /// </summary>
            public double Intensity { get; set; }

            /// <summary>
            /// The length in time of this note
            /// </summary>
            public TimeSpan Value { get; set; }

            private Clef _clef;
            public Clef Clef
            {
                get
                {
                    if (_clef == Clef.Auto)
                    {
                        if (Frequency >= MIDDLE_C_FREQ) return Clef.Treble;
                        else return Clef.Bass;
                    }
                    return _clef;
                }
                set
                {
                    _clef = value;
                }
            }

            /// <summary>
            /// Base 2 logarithm of the frequency of the note C0 (equal temperament)
            /// </summary>
            private const decimal LG_C0 = 4.03133451968853538576731686887580044546196M;

            /// <summary>
            /// Base 2 logarithm difference in frequency between each semitone (equal temperament)
            /// </summary>
            private const decimal LG_SEMITONE = 0.0833337753304583196193397474971821524404579M;

            /// <summary>
            /// Base 2 logarithm of the frequency of middle C (equal temperament)
            /// </summary>
            private const decimal LG_MIDDLE_C = 8.0313621106074863587903113877378714985668M;

            /// <summary>
            /// True if this note is a rest
            /// </summary>
            public bool IsRest
            {
                get
                {
                    return Frequency < 0;
                }
            }

            /// <summary>
            /// Custom modulo function
            /// </summary>
            private static decimal Modulo(decimal x, decimal m)
            {
                return (x % m + m) % m;
            }

            /// <summary>
            /// Get the octave number of this note
            /// </summary>
            public int Octave
            {
                get
                {
                    if (Frequency < 0) return -1;
                    decimal offset = (decimal)Math.Log10(Frequency) / (decimal)Math.Log10(2.0) - LG_C0;
                    decimal octaveNum = offset / (12 * LG_SEMITONE);
                    if (Modulo(octaveNum, 1) >= (23M / 24M))
                    {
                        octaveNum += 1; // round up if last semitone
                    }
                    return (int)Math.Truncate(octaveNum);
                }
                set
                {
                    string nm = Name;
                    Frequency = (double)(LG_C0 + LG_SEMITONE * 12 * value + _noteIndex[nm[0]] * LG_SEMITONE);
                    foreach (char c in nm)
                    {
                        if (c == '#')
                            Frequency += (double)LG_SEMITONE;
                        else if (c == 'b')
                            Frequency -= (double)LG_SEMITONE;
                        else if (c == 'x')
                            Frequency += 2 * (double)LG_SEMITONE;
                    }
                    Frequency = Math.Pow(2, Frequency);
                }
            }

            /// <summary>
            /// List of note names in each octave, going up by semitone
            /// </summary>
            private static string[] _noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B", "C", "C#" };

            /// <summary>
            /// Dictionary for looking up the index of a note based on its name.
            /// </summary>
            private static Dictionary<char, int> _noteIndex = new Dictionary<char, int>
                { { 'C', 0 }, { 'D', 2 }, { 'E', 4 }, { 'F', 5 }, { 'G', 7 },
                  { 'A', 9 } , { 'B', 11 } };

            /// <summary>
            /// Dictionary for looking up the index on the staff of a note based on its name.
            /// </summary>
            private static Dictionary<char, int> _noteSpaceIndex = new Dictionary<char, int>
                { { 'C', 0 }, { 'D', 1 }, { 'E', 2 }, { 'F', 3 }, { 'G', 4 },
                  { 'A', 5 } , { 'B', 6 } };

            /// <summary>
            /// Get the name of the note (e.g. C#)
            /// </summary>
            public string Name
            {
                get
                {
                    if (Frequency < 0) return "Rest";
                    decimal offset = (decimal)Math.Log10(Frequency) / (decimal)Math.Log10(2.0) - LG_C0;
                    return _noteNames[(int)Math.Round((Modulo(offset, (12 * LG_SEMITONE)) / LG_SEMITONE))];
                }
                set
                {
                    if (value.ToLower() == "rest") { Frequency = -1; return; }
                    Frequency = (double)(LG_C0 + LG_SEMITONE * 12 * Octave + _noteIndex[value[0]] * LG_SEMITONE);
                    foreach (char c in value)
                    {
                        if (c == '#')
                            Frequency += (double)LG_SEMITONE;
                        else if (c == 'b')
                            Frequency -= (double)LG_SEMITONE;
                        else if (c == 'x')
                            Frequency += 2 * (double)LG_SEMITONE;
                    }
                    Frequency = Math.Pow(2, Frequency);
                }
            }

            /// <summary>
            /// Get the y-offset of this note from middle C
            /// 0 is middle c, 1 is the space above, 3 is the first treble space, etc.
            /// </summary>
            public float Offset
            {
                get
                {
                    decimal freq = (decimal)Math.Log10(Frequency) / (decimal)Math.Log10(2);
                    decimal baseFreq = LG_MIDDLE_C;
                    if (baseFreq > freq)
                    {
                        decimal tmp = freq;
                        freq = baseFreq;
                        baseFreq = tmp;
                    }

                    string name = this.Name;
                    decimal diff = (Octave - 4) * 7;
                    diff += _noteSpaceIndex[name[0]];
                    return (float)diff;
                }
            }

            /// <summary>
            /// Get the name of the note with the specified offset from middle C
            /// </summary>
            public static string[] NameFromOffset(float offset)
            {
                int octave = (int)(4 + Math.Floor(offset / 7));
                offset = (float)Modulo((decimal)offset, 7);
                string name = ((char)('A' + (int)Modulo((decimal)offset + 2, ('H' - 'A')))).ToString();
                return new[] { name, octave.ToString() };
            }

            /// <summary>
            /// Get the full name of this note, including the name and the octave.
            /// </summary>
            public string FullName
            {
                get
                {
                    return Name + Octave;
                }
            }

            /// <summary>
            /// Create a new note from a frequency.
            /// </summary>
            /// <param name="position">The position of this note on the sheet</param>
            /// <param name="frequency">The frequency of this note, in hertz</param>
            /// <param name="value">The length value of this note</param>
            /// <param name="intensity">The intensity/dynamics marking of this note</param>
            /// <param name="isRest">If true, makes this note a rest</param>
            public Note(TimeSpan position, double frequency = 440, TimeSpan? value = null, double intensity = 0.75, bool isRest = false)
            {
                if (value == null) value = TimeSpan.FromSeconds(1);
                this.Position = position;
                this.Frequency = frequency;
                this.Intensity = intensity;
                this.Value = (TimeSpan)value;
                if (isRest) this.Frequency = -1;
                this._clef = Clef.Auto;
            }

            /// <summary>
            /// Create a new note from a note name and an octave.
            /// </summary>
            /// <param name="position">The position of this note on the sheet</param>
            /// <param name="name">The name of this note (A, B, C, etc.)</param>
            /// <param name="octave">The octave of this note (Middle C is in octave 4)</param>
            /// <param name="value">The length value of this note</param>
            /// <param name="intensity">The intensity/dynamics marking of this note</param>
            /// <param name="isRest">If true, makes this note a rest</param>
            public Note(TimeSpan position, string name, int octave = 4, TimeSpan? value = null, double intensity = 0.75, bool isRest = false)
            {
                if (value == null) value = TimeSpan.FromSeconds(1);
                this.Position = position;
                int idx = _noteIndex[char.ToUpperInvariant(name[0])];
                if (name.Length > 0)
                {
                    name = name.Substring(1);
                    foreach (char c in name)
                    {
                        if (c == 'b') idx -= 1;
                        else if (c == '#') idx += 1;
                        else if (c == 'x') idx += 2;
                    }
                }
                this.Frequency = Math.Pow(2, (double)(LG_C0 + LG_SEMITONE * octave * 12 + LG_SEMITONE * idx));
                this.Intensity = intensity;
                this.Value = (TimeSpan)value;
                if (isRest) this.Frequency = -1;
                this._clef = Clef.Auto;
            }
        }

        /// <summary>
        /// The width of each note in pixels
        /// </summary>
        public float NoteWidth { get; set; } = 40;

        /// <summary>
        /// Padding on the left and right of each bar, in pixels
        /// </summary>
        public float BarPadding { get; set; } = 0;

        /// <summary>
        /// The padding above and below each line
        /// </summary>
        public float RowPadding { get; set; } = 95;

        /// <summary>
        /// The height of each of the four spaces in each staff
        /// </summary>
        public float SpaceHeight { get; set; } = 15;

        /// <summary>
        /// The spacing between the staffs
        /// </summary>
        public float StaffSpacing { get; set; } = 55;

        /// <summary>
        /// The paddings around the sheet
        /// </summary>
        public Padding SheetPadding { get; set; } = new Padding(35, 30, 20, 10);

        /// <summary>
        /// The back buffer
        /// </summary>
        private Bitmap _buf;

        /// <summary>
        /// If true, does not update the canvas the next time the time textbox is changed
        /// </summary>
        private bool _noUpdate;

        /// <summary>
        /// All the notes shown in this music sheet
        /// </summary>
        private SortedDictionary<TimeSpan, Dictionary<string, Note>> _notes = new SortedDictionary<TimeSpan, Dictionary<string, Note>>();

        /// <summary>
        /// Get the number of notes in this music sheet
        /// </summary>
        public int NoteCount { get { return _notes.Count; } }

        /// <summary>
        /// Create a new music sheet control
        /// </summary>
        public MusicSheet()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// Add a new note to the music sheet
        /// </summary>
        /// <param name="autoCombine">If true, automatically combines this note with the previous note, if they have similar pitch.</param>
        public void AddNote(Note note)
        {
            if (!_notes.ContainsKey(note.Position))
                _notes[note.Position] = new Dictionary<string, Note>();
            if (!_notes[note.Position].ContainsKey(note.FullName) || note.Value > _notes[note.Position][note.FullName].Value)
            {
                _notes[note.Position][note.FullName] = note;
            }
        }

        /// <summary>
        /// Get the note at the specified time and with the specified name. Returns null if no such note exists.
        /// </summary>
        public Note GetNote(TimeSpan time, string name)
        {
            if (!_notes.ContainsKey(time) || !_notes[time].ContainsKey(name)) return null;
            return _notes[time][name];
        }

        /// <summary>
        /// Remove a note from the music sheet
        /// </summary>
        public void RemoveNote(Note note)
        {
            try
            {
                _notes[note.Position].Remove(note.FullName);
            }
            catch
            {
                MessageBox.Show("Note does not exist in this music sheet.");
            }
        }

        /// <summary>
        /// Clear all notes from this music sheet
        /// </summary>
        public void ClearNotes()
        {
            _notes.Clear();
            _notes = new SortedDictionary<TimeSpan, Dictionary<string, Note>>();
        }

        /// <summary>
        /// Create a new timespan from the given number of seconds.
        /// </summary>
        private TimeSpan FromSeconds(double seconds)
        {
            int hours = (int)Math.Floor(seconds / 3600);
            seconds %= 3600;
            int minutes = (int)Math.Floor(seconds / 60);
            seconds %= 60;
            int secs = (int)Math.Floor(seconds);
            seconds %= 1;
            int ms = (int)Math.Floor(seconds * 1000);
            return new TimeSpan(0, hours, minutes, secs, ms);
        }

        /// <summary>
        /// Convert a client coordinate to the point in time represented by that coordinate
        /// </summary>
        public TimeSpan PosToTime(PointF pt)
        {
            int line = (int)Math.Floor((pt.Y - SheetPadding.Top) / LineHeight);
            int bar = (int)Math.Floor((pt.X - SheetPadding.Left) / BarWidth);
            double note = (pt.X - SheetPadding.Left) % BarWidth / NoteWidth;
            return TimeSpan.FromTicks((long)((line * NotesPerLine + bar * NotesPerBar + note) * TimePerNote.Ticks));
        }

        /// <summary>
        /// Converts a client distance from the top to a note name
        /// </summary>
        public string[] YPosToNote(double top)
        {
            double y = (top - SheetPadding.Top) % LineHeight;
            y -= RowPadding + SpaceHeight * 4 + StaffSpacing / 2;
            if (y < 0)
            {
                y += StaffSpacing / 2 - SpaceHeight;
            }
            else if (y > SpaceHeight) y -= StaffSpacing / 2 - SpaceHeight;
            return Note.NameFromOffset(-(float)(y / SpaceHeight * 2));
        }

        public PointF TimeToPos(TimeSpan t)
        {
            double notes = t.TotalSeconds * (1 / (Tempo / 60));
            int line = (int)Math.Floor(notes / NotesPerLine);
            int bar = (int)Math.Floor(notes % NotesPerLine / NotesPerBar);
            notes = notes % NotesPerBar;
            float x = (float)(SheetPadding.Left + bar * BarWidth + BarPadding + notes * NoteWidth);
            float y = (float)(SheetPadding.Top + line * (StaffSpacing + 8 * SpaceHeight + 2 * RowPadding));
            return new PointF(x, y);
        }

        /// <summary>
        /// Get the color that represents the given intensity
        /// </summary>
        private Color IntensityColor(double intensity)
        {
            if (intensity == 0)
                return Color.Transparent;
            else if (intensity <= 0.1)
                return Color.FromArgb(255, 129, 194, 60);
            else if (intensity <= 0.2)
                return Color.FromArgb(255, 119, 177, 0);
            else if (intensity <= 0.3)
                return Color.FromArgb(255, 92, 227, 34);
            else if (intensity <= 0.4)
                return Color.FromArgb(255, 136, 224, 74);
            else if (intensity <= 0.5)
                return Color.FromArgb(255, 181, 219, 55);
            else if (intensity <= 0.6)
                return Color.FromArgb(255, 212, 212, 0);
            else if (intensity <= 0.7)
                return Color.FromArgb(255, 222, 185, 0);
            else if (intensity <= 0.8)
                return Color.FromArgb(255, 212, 152, 0);
            else if (intensity <= 0.9)
                return Color.FromArgb(255, 222, 104, 0);
            else
                return Color.FromArgb(255, 219, 73, 59);
        }

        /// <summary>
        /// Get the physical position of the specified note
        /// </summary>
        private PointF GetNotePos(Note note)
        {
            PointF pos = TimeToPos(note.Position);
            float y = pos.Y + RowPadding + 4 * SpaceHeight + SpaceHeight / 2;
            float offset = note.Offset;
            y -= offset * SpaceHeight / 2 - 1;
            if (offset == 0) y += StaffSpacing / 2 - SpaceHeight;
            else if (note.Clef == Clef.Bass) y += StaffSpacing / 2 - 3;

            float x = pos.X + 1;

            return new PointF(x, y);
        }

        /// <summary>
        /// Redraw the music sheet based on the notes, etc. present
        /// </summary>
        public void Redraw()
        {
                int high = (int)(Lines * (2 * RowPadding + StaffSpacing + 2 * 4 * SpaceHeight)) +
                                                    SheetPadding.Top + SheetPadding.Bottom;
                Bitmap oldBuf = _buf;
                if (high >= scroller.Height)
                    _buf = new Bitmap(scroller.Width - 20, high);
                else
                    _buf = new Bitmap(scroller.Width, high);

                if (oldBuf != null) oldBuf.Dispose();

                Graphics g = Graphics.FromImage(_buf);
                g.Clear(scroller.BackColor);
                // draw background past end
                using (SolidBrush br = new SolidBrush(Color.FromArgb(35, 35, 35)))
                {
                    PointF pt = TimeToPos(Length);
                    if (pt.X == SheetPadding.Left) pt.X = 0;
                    if (pt.Y == SheetPadding.Top) pt.Y = 0;
                    g.FillRectangle(br, pt.X, pt.Y, _buf.Width - pt.X, _buf.Height - pt.Y);
                }

                // draw lines 
                float lineWidth = (float)BarWidth * BarsPerLine;
                float barHeight = SpaceHeight * 4;
                using (Pen linePen = new Pen(Color.DarkGray))
                {
                    for (int l = 0; l < Lines; ++l)
                    {
                        linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        float top = SheetPadding.Top + l * (2 * RowPadding + StaffSpacing + 2 * 4 * SpaceHeight) + RowPadding;
                        for (int sl = 0; sl < 10; ++sl)
                        {
                            g.DrawLine(linePen, SheetPadding.Left, top, SheetPadding.Left + lineWidth, top);
                            if (sl == 4)
                                top += StaffSpacing;
                            else
                                top += SpaceHeight;
                        }
                        double left = SheetPadding.Left;
                        top = SheetPadding.Top + l * (2 * RowPadding + StaffSpacing + 2 * 4 * SpaceHeight) + RowPadding;

                        string bn = (BarsPerLine * l + 1).ToString();
                        SizeF bnSz = g.MeasureString(bn, this.Font);
                        g.DrawString(bn, this.Font, Brushes.LightGray, (float)left, top - bnSz.Height - 2);

                        // draw barlines
                        g.DrawLine(linePen, (float)left, top, (float)left, top + barHeight * 2 + StaffSpacing);

                        for (int bar = 0; bar < BarsPerLine - 1; bar += 1)
                        {
                            left += BarWidth;
                            g.DrawLine(linePen, (float)left, top, (float)left, top + barHeight);
                            g.DrawLine(linePen, (float)left, top + StaffSpacing + barHeight, (float)left, top + StaffSpacing + barHeight + barHeight);
                        }

                        left += BarWidth;
                        g.DrawLine(linePen, (float)left, top, (float)left, top + barHeight * 2 + StaffSpacing);
                    }
                    // update total time
                    string tmr = string.Format("{0}:{1}:{2}.{3}",
                        Length.Hours.ToString().PadLeft(2, '0'), Length.Minutes.ToString().PadLeft(2, '0'),
                        Length.Seconds.ToString().PadLeft(2, '0'), ((double)Length.Milliseconds / 1000).ToString("F4").Substring(2));
                    if (tmr.StartsWith("00:")) tmr = tmr.Substring(3);
                    LbTotalTime.Text = "/ " + tmr;
                }

                // draw notes
                double rightLim = SheetPadding.Left + BarsPerLine * BarWidth;
                using (SolidBrush noteBr = new SolidBrush(Color.White))
                {
                    foreach (KeyValuePair<TimeSpan, Dictionary<string, Note>> kvp in _notes)
                    {
                        foreach (Note n in kvp.Value.Values)
                        {
                            if (n.Clef == Clef.Hidden) continue;
                            PointF pos = GetNotePos(n);
                            double intensity = Math.Min(Math.Max(n.Intensity, 0), 1.0);

                            if (n.Focused)
                                noteBr.Color = Color.White;
                            else if (n.IsRest)
                                noteBr.Color = Color.DarkGray;
                            else
                                noteBr.Color = IntensityColor(intensity);

                            bool cutOff = false;

                            float wid = (float)(((double)n.Value.Ticks / TimePerNote.Ticks) * NoteWidth) - 1;
                            float fullWid = wid;
                            if (pos.X + wid > rightLim) { // extend to next line
                                wid = (float)rightLim - pos.X;
                                cutOff = true;
                            }

                            for (int i = 0; i < 2; ++i)
                            {
                                g.FillRectangle(noteBr, pos.X, pos.Y, wid, SpaceHeight - 1);

                                // display note name, if big enough
                                string s = n.FullName;
                                SizeF sz = g.MeasureString(s, this.Font);
                                if (wid > sz.Width + 2)
                                {
                                    g.DrawString(s, this.Font, n.Focused ? Brushes.DimGray : Brushes.GhostWhite, pos.X + wid / 2 - sz.Width / 2,
                                                                              pos.Y + (SpaceHeight - 1) / 2 - sz.Height / 2 + 1);
                                }
                                else if (s.Length > 2)
                                {
                                    // draw sharp only, if applicable
                                    s = "#";
                                    sz = g.MeasureString(s, this.Font);
                                    if (wid > sz.Width + 2)
                                    {
                                        g.DrawString(s, this.Font, n.Focused ? Brushes.DimGray : Brushes.GhostWhite, pos.X + wid / 2 - sz.Width / 2,
                                                                                  pos.Y + (SpaceHeight - 1) / 2 - sz.Height / 2 + 1);
                                    }
                                }
                                if (!cutOff) break;
                                // if note was cut off, continue on next line.
                                wid = fullWid - wid;
                                pos = new PointF(SheetPadding.Left, (float)(pos.Y + LineHeight));
                            }
                        }
                    }
                }

                canvas.Height = _buf.Height;
                canvas.Width = _buf.Width;

                canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            if (_buf != null)
            {
                e.Graphics.DrawImageUnscaled(_buf, 0, 0);
                if (_time.Ticks >= 0)
                {
                    using (Pen timePen = new Pen(Color.Gainsboro))
                    {
                        PointF pt = TimeToPos(_time);
                        e.Graphics.DrawLine(timePen, pt.X, pt.Y + 20, pt.X, pt.Y + StaffSpacing + 8 * SpaceHeight + 2 * RowPadding);
                        string tmr = string.Format("{0}:{1}:{2}.{3}",
                            _time.Hours.ToString().PadLeft(2, '0'), _time.Minutes.ToString().PadLeft(2, '0'),
                            _time.Seconds.ToString().PadLeft(2, '0'), ((double)_time.Milliseconds / 1000).ToString("F4").Substring(2));
                        if (tmr.StartsWith("00:")) tmr = tmr.Substring(3);
                        _noUpdate = true;
                        TbTime.BackColor = Color.White;
                        TbTime.Text = tmr;
                        SizeF sz = e.Graphics.MeasureString(tmr, this.Font);
                        e.Graphics.DrawString(tmr, this.Font, Brushes.Gainsboro, pt.X - sz.Width / 2, pt.Y);
                    }
                }
            }
        }

        private void MusicSheet_Load(object sender, EventArgs e)
        {
            this.ResizeRedraw = false;

            // Fake data for test
            //AddNote(new Note(new TimeSpan(0, 0, 16), "F", octave: 6, value: 1, intensity: 1));
            //AddNote(new Note(new TimeSpan(0, 0, 16), "A", octave: 6, value: 1, intensity: 1));
            // End fake data

            Redraw();
        }

        private void MusicSheet_Resize(object sender, EventArgs e)
        {
            if (BarWidth + SheetPadding.Left + SheetPadding.Right > this.Width)
            {
                NotesPerBar = (int)Math.Floor((this.Width - SheetPadding.Left - SheetPadding.Right) / NoteWidth);
                NBar.Value = NotesPerBar;
            }
            Redraw();
        }

        bool _drag = false;
        bool _wasSynthesizing = false;
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _drag = true;
                _wasSynthesizing = Synthesizing;
                if (Synthesizing)
                {
                    StopSynthesize();
                }
                canvas_MouseMove(sender, e);
            }
            else if (e.Button == MouseButtons.Right)
            {
                TimeSpan ts = PosToTime(e.Location);
                int ct = 0;
                for (; ct < 5000000; ++ct)
                {

                    if (_notes.ContainsKey(ts))
                    {
                        for (int y = e.Y - 5; y <= e.Y + 5; y += 5)
                        {
                            string name =string.Join("", YPosToNote(y));
                            for (int i = 0; i < 2; ++i)
                            {
                                if (_notes[ts].ContainsKey(name))
                                {
                                    if (_notes[ts][name].Parent != null || _notes[ts][name].Clef == Clef.Hidden) continue;
                                    EditNote(_notes[ts][name]);
                                    return;
                                }
                                name = name.Insert(1, "#");
                            }
                        }
                    }
                    ts = ts.Subtract(TimeSpan.FromTicks(1));
                }
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_drag && Length.Ticks > 0)
            {
                _time = PosToTime(e.Location);
                if (_time.Ticks < 0)
                    _time = new TimeSpan(0);
                if (_time.Ticks > Length.Ticks)
                    _time = Length;
                if (UpdateTime != null) UpdateTime(this, _time);
                canvas.Invalidate();
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (_drag)
            {
                if (_wasSynthesizing)
                {
                    Synthesize();
                }
                _drag = false;
            }
        }

        private void nTempo_ValueChanged(object sender, EventArgs e)
        {
            Tempo = (double)NTempo.Value;
            Redraw();
        }

        private void nBar_ValueChanged(object sender, EventArgs e)
        {
            NotesPerBar = (int)NBar.Value;
            Redraw();
        }

        /// <summary>
        /// Saves an image of the canvas to the specified path.
        /// </summary>
        public void ExportCanvas(string path)
        {
             _buf.Save(path);
        }

        /// <summary>
        /// Scroll to the specified distance from the top (default 0)
        /// </summary>
        public void ScrollTop(int value = 0)
        {
            scroller.VerticalScroll.Value = value;
        }

        private void TbBaseNote_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //using (CantusEvaluator eval = new CantusEvaluator(CantusEvaluator.OutputFormat.Raw))
                using (CantusEvaluator eval = new CantusEvaluator())
                {
                    object res = eval.EvalExprRaw(TbBaseNote.Text);
                    if (res is Cantus.Core.CommonTypes.BigDecimal)
                    {
                        res = (double)(Cantus.Core.CommonTypes.BigDecimal)res;
                    }
                    TbBaseNote.BackColor = Color.White;
                    if ((double)res <= 0) throw new ArgumentOutOfRangeException();
                    BaseNote = (double)res;
                }
                _noUpdate = true;
                Redraw();
            }
            catch
            {
                TbBaseNote.BackColor = Color.IndianRed;
            }
        }

        private void TbTime_TextChanged(object sender, EventArgs e)
        {
            if (_noUpdate)
            {
                _noUpdate = false;
                return;
            }
            try
            {
                TimeSpan res;
                string[] spl = (TbTime.Text.Split(':'));
                double hr = 0, min;
                double sec;
                //using (CantusEvaluator eval = new CantusEvaluator(CantusEvaluator.OutputFormat.Raw))
                using (CantusEvaluator eval = new CantusEvaluator())
                {
                    if (spl.Length == 3)
                    {
                        hr = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[0]);
                        min = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[1]);
                        sec = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[2].TrimStart('0'));
                    }
                    else if (spl.Length == 2)
                    {
                        min = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[0]);
                        sec = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[1].TrimStart('0'));
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                res = FromSeconds(sec + 60 * min + 3600 * hr);

                if (res.Ticks < 0 || res.Ticks > Length.Ticks) throw new ArgumentOutOfRangeException();
                _time = res;
                UpdateTime(this, _time);
                TbTime.BackColor = Color.White;
                Redraw();
            }
            catch
            {
                TbTime.BackColor = Color.IndianRed;
            }
        }

        private void MusicSheet_KeyUp(object sender, KeyEventArgs e)
        {
            if (_editNote != null && PnlEditNote.Visible)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    BtnDelNote.PerformClick();
                }
                else if (e.KeyCode == Keys.D)
                {
                    BtnDuplicate.PerformClick();
                }
            }

            if (e.KeyCode == Keys.N)
            {
                Point pt = canvas.PointToClient(Cursor.Position);
                string[] name = YPosToNote(pt.Y);
                AddNote(new Note(PosToTime(pt), name:name[0], octave : int.Parse( name[1]), value:TimePerNote));
                Redraw();
            }

            if (this.InnerKeyUp != null)
                this.InnerKeyUp(sender, e);
        }

        private void BtnClosePnl_Click(object sender, EventArgs e)
        {
            if (_editNote != null)
            {
                _editNote.Focused = false;
                _editNote = null;
                Redraw();
            }
            PnlEditNote.Hide();
        }

        private Note _editNote;
        private void TbNoteName_TextChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            try
            {
                TbNoteName.BackColor = Color.White;
                if (!_noUpdateNote)
                {
                    _editNote.Name = TbNoteName.Text;
                    _noUpdateNote = true;
                    TbWL.Text = _editNote.Wavelength.ToString();
                    TbWL.BackColor = Color.White;
                    _noUpdateNote = true;
                    TbFreq.Text = _editNote.Frequency.ToString();
                    TbFreq.BackColor = Color.White;
                    Redraw();
                }
                else _noUpdateNote = false;
            }
            catch
            {
                TbNoteName.BackColor = Color.IndianRed;
            }
        }

        private void NOctave_ValueChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            try
            {
                if (!_noUpdateNote)
                {
                    _editNote.Octave = (int)NOctave.Value;
                    _noUpdateNote = true;
                    TbWL.Text = _editNote.Wavelength.ToString();
                    TbWL.BackColor = Color.White;
                    _noUpdateNote = true;
                    TbFreq.Text = _editNote.Frequency.ToString();
                    TbFreq.BackColor = Color.White;
                    Redraw();
                }
                else _noUpdateNote = false;
            }
            catch { }
        }

        private void TbStart_TextChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            try
            {
                TimeSpan res;
                string[] spl = (TbStart.Text.Split(':'));
                double hr = 0, min;
                double sec;
                //using (CantusEvaluator eval = new CantusEvaluator(CantusEvaluator.OutputFormat.Raw))
                using (CantusEvaluator eval = new CantusEvaluator())
                {
                    if (spl.Length == 3)
                    {
                        hr = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[0]);
                        min = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[1]);
                        sec = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[2].TrimStart('0'));
                    }
                    else if (spl.Length == 2)
                    {
                        min = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[0]);
                        sec = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(spl[1].TrimStart('0'));
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                res = FromSeconds(sec + 60 * min + 3600 * hr);

                if (res.Ticks < 0 || res.Ticks > Length.Ticks) throw new ArgumentOutOfRangeException();

                _editNote.Position = res;

                TbStart.BackColor = Color.White;
                Redraw();
            }
            catch
            {
                TbStart.BackColor = Color.IndianRed;
            }
        }

        bool _noValUpdate = true;
        bool _noDurUpdate = true;
        private void TbDur_TextChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            try
            {
                TimeSpan res;
                double sec;
                using (CantusEvaluator eval = new CantusEvaluator())
                {
                    sec = (double)(Cantus.Core.CommonTypes.BigDecimal)eval.EvalExprRaw(TbDur.Text.TrimStart('0'));
                }
                res = FromSeconds(sec);

                if (res.Ticks < 0 || res.Ticks > Length.Ticks) throw new ArgumentOutOfRangeException();

                _editNote.Value = res;
                if (!_noDurUpdate)
                {
                    _noValUpdate = true;
                    TbVal.Text = (res.TotalSeconds * (Tempo / 60) * BaseNote).ToString();
                    Redraw();
                }
                else
                {
                    _noDurUpdate = false;
                }

                TbDur.BackColor = Color.White;
                TbVal.BackColor = Color.White;
            }
            catch
            {
                TbDur.BackColor = Color.IndianRed;
            }
        }

        private void TbVal_TextChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            try
            {
                //using (CantusEvaluator eval = new CantusEvaluator(CantusEvaluator.OutputFormat.Raw))
                using (CantusEvaluator eval = new CantusEvaluator())
                {
                    object res = eval.EvalExprRaw(TbVal.Text);
                    if (res is Cantus.Core.CommonTypes.BigDecimal)
                    {
                        res = (double)(Cantus.Core.CommonTypes.BigDecimal)res;
                    }
                    TbVal.BackColor = Color.White;

                    if ((double)res <= 0) throw new ArgumentOutOfRangeException();

                    _editNote.Value = TimeSpan.FromSeconds((double)res / BaseNote / (Tempo / 60));
                    if (!_noValUpdate)
                    {
                        _noDurUpdate = true;
                        TbDur.BackColor = Color.White;
                        TbDur.Text = _editNote.Value.TotalSeconds.ToString();
                        Redraw();
                    }
                    else
                    {
                        _noValUpdate = false;
                    }
                }
            }
            catch
            {
                TbVal.BackColor = Color.IndianRed;
            }
        }

        private void NIntensity_ValueChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            _editNote.Intensity = (double)NIntensity.Value;
            PbIntensity.BackColor = IntensityColor(_editNote.Intensity);
            Redraw();
        }

        private void canvas_Resize(object sender, EventArgs e)
        {
            if (PnlEditNote.Visible)
                BtnClosePnl.PerformClick();
        }

        /// <summary>
        /// Show the edit panel for a note
        /// </summary>
        /// <param name="note"></param>
        private void EditNote(Note note)
        {
            if (_editNote != null)
            {
                _editNote.Focused = false;
                _editNote = null;
            }

            TbNoteName.Text = note.Name;
            TbNoteName.BackColor = Color.White;
            NOctave.Value = note.Octave;

            TbStart.Text = note.Position.ToString();
            TbStart.Text = TbStart.Text.Remove(13);
            TbStart.BackColor = Color.White;

            TbDur.Text = note.Value.TotalSeconds.ToString();
            TbDur.BackColor = Color.White;

            TbVal.Text = (note.Value.TotalSeconds * (Tempo / 60) * BaseNote).ToString();
            TbVal.BackColor = Color.White;

            TbFreq.Text = note.Frequency.ToString();
            TbFreq.BackColor = Color.White;

            TbWL.Text = note.Wavelength.ToString();
            TbWL.BackColor = Color.White;

            NIntensity.Value = (decimal)note.Intensity;
            PbIntensity.BackColor = IntensityColor(note.Intensity);

            _editNote = note;
            _editNote.Focused = true;

            if (!PnlEditNote.Visible)
            {
                PnlEditNote.BringToFront();
                PnlEditNote.Top = scroller.Top + scroller.Height / 2 - PnlEditNote.Height / 2;
                PnlEditNote.Left = scroller.Left + scroller.Width / 2 - PnlEditNote.Width / 2;
                Point pt = PnlEditNote.PointToClient(Cursor.Position);
                if (pt.X >= -10 && pt.X <= PnlEditNote.Width && pt.Y >= -10 && pt.Y <= PnlEditNote.Height)
                {
                    PnlEditNote.Left = 10;
                    pt = PnlEditNote.PointToClient(Cursor.Position);
                    if (pt.X >= -10 && pt.X <= PnlEditNote.Width && pt.Y >= -10 && pt.Y <= PnlEditNote.Height)
                    {
                        PnlEditNote.Left = scroller.Right - PnlEditNote.Width - 30;
                    }
                }
                PnlEditNote.Show();
            }

            Redraw();

            TbNoteName.Select();
        }

        private void BtnDelNote_Click(object sender, EventArgs e)
        {
            RemoveNote(_editNote);
            _editNote = null;
            PnlEditNote.Hide();
            Redraw();
        }

        private void NIntensity_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                BtnClosePnl.PerformClick();
            }
        }

        bool _pnlDrag = false;
        Point _pnlPpt;
        private void label11_MouseDown(object sender, MouseEventArgs e)
        {
            _pnlDrag = true;
            _pnlPpt = e.Location;
        }

        private void label11_MouseMove(object sender, MouseEventArgs e)
        {
            if (_pnlDrag)
            {
                if (PnlEditNote.Right > scroller.Right && e.X > _pnlPpt.X || PnlEditNote.Bottom > scroller.Bottom && e.Y > _pnlPpt.Y ||
                    PnlEditNote.Top < 0 && e.Y < _pnlPpt.Y || PnlEditNote.Left < 0 && e.X < _pnlPpt.X)
                    return;
                PnlEditNote.Left += e.X - _pnlPpt.X;
                PnlEditNote.Top += e.Y - _pnlPpt.Y;
            }
        }

        private void label11_MouseUp(object sender, MouseEventArgs e)
        {
            _pnlDrag = false;
        }

        bool _noUpdateNote = false;
        private void TbFreq_TextChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            if (_noUpdateNote)
            {
                _noUpdateNote = false;
                return;
            }
            try
            {
                using (CantusEvaluator eval = new CantusEvaluator())
                {
                    object res = eval.EvalRaw(TbFreq.Text);
                    if (res is BigDecimal) res = (double)(BigDecimal)res;
                    if (double.IsNaN((double)res) || (double)res <= 0) throw new ArgumentException();

                    _editNote.Frequency = (double)res;

                    _noUpdateNote = true;
                    NOctave.Value = _editNote.Octave;
                    _noUpdateNote = true;
                    TbNoteName.Text = _editNote.Name;
                    TbNoteName.BackColor = Color.White;
                    _noUpdateNote = true;
                    TbWL.Text = _editNote.Wavelength.ToString();
                    TbFreq.BackColor = Color.White;
                }
            }
            catch
            {
                TbFreq.BackColor = Color.IndianRed;
            }
        }

        private void TbWL_TextChanged(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            if (_noUpdateNote)
            {
                _noUpdateNote = false;
                return;
            }
            try
            {
                using (CantusEvaluator eval = new CantusEvaluator())
                {
                    object res = eval.EvalRaw(TbWL.Text);
                    if (res is BigDecimal) res = (double)(BigDecimal)res;
                    if (double.IsNaN((double)res) || (double)res <= 0) throw new ArgumentException();
                    _editNote.Wavelength = (double)res;

                    _noUpdateNote = true;
                    NOctave.Value = _editNote.Octave;
                    _noUpdateNote = true;
                    TbNoteName.Text = _editNote.Name;
                    TbNoteName.BackColor = Color.White;
                    _noUpdateNote = true;
                    TbFreq.Text = _editNote.Frequency.ToString();
                    TbWL.BackColor = Color.White;
                }
            }
            catch
            {
                TbWL.BackColor = Color.IndianRed;
            }
        }

        private void BtnDuplicate_Click(object sender, EventArgs e)
        {
            if (_editNote == null) return;
            _editNote.Focused = false;
            _editNote = new Note(_editNote.Position.Add(TimeSpan.FromSeconds(0.5)),
                _editNote.Frequency, _editNote.Value, _editNote.Intensity, _editNote.IsRest);
            _editNote.Focused = true;
            AddNote(_editNote);
            Redraw();
        }

        private bool _stop = false;
        private List<Tuple< WaveOut, TimeSpan>> _synthSounds = new List<Tuple<WaveOut, TimeSpan>>();

        /// <summary>
        /// Stop synthesizing
        /// </summary>
        public void StopSynthesize()
        {
            _stop = true;
            for (int i=0; i< _synthSounds.Count; ++i)
            {
                if (i >= _synthSounds.Count) break;
                try {
                    _synthSounds[i].Item1.Stop();
                    _synthSounds[i].Item1.Dispose();
                }
                catch { }
            }
            _synthSounds.Clear();
            Synthesizing = false;
            if (SynthStopped != null) SynthStopped(this, new EventArgs());
        }

        /// <summary>
        /// Begin playing synthesized notes.
        /// </summary>
        public void Synthesize()
        {
            try {
                Synthesizing = true;
                _stop = false;
                if (SynthStarted != null) SynthStarted(this, new EventArgs());

                Thread th = new Thread(() => {
                    int oldNotesCount = _notes.Count;

                    foreach (KeyValuePair<TimeSpan, Dictionary<string, Note>> chord in _notes.ToArray())
                    {
                        try {
                            if (chord.Key < _time) continue;

                            Thread.Sleep((int)(chord.Key - _time).TotalMilliseconds);
                            _time = chord.Key;
                            canvas.Invoke(new Action(() => canvas.Invalidate()));

                            for (int i = _synthSounds.Count - 1; i >= 0; --i)
                            {
                                WaveOut w = _synthSounds[i].Item1;
                                TimeSpan endTime = _synthSounds[i].Item2;
                                if (endTime <= _time)
                                {
                                    w.Stop();
                                    w.Dispose();
                                    _synthSounds.RemoveAt(i);
                                }
                            }

                            if (_stop)
                            {
                                foreach (var w in _synthSounds)
                                {
                                    w.Item1.Stop();
                                    w.Item1.Dispose();
                                }
                                _synthSounds.Clear();
                                return;
                            }

                            foreach (Note n in chord.Value.Values)
                            {
                                if (n.Parent != null) continue;

                                SineWaveProvider32 sineWaveProvider = new SineWaveProvider32();
                                sineWaveProvider.SetWaveFormat(16000, 1);
                                sineWaveProvider.Frequency = (float)n.Frequency;
                                sineWaveProvider.Amplitude = Math.Min((float)n.Intensity, 1);

                                WaveOut waveOut = new WaveOut();
                                waveOut.Init(sineWaveProvider);
                                waveOut.Play();

                                _synthSounds.Add(new Tuple<WaveOut, TimeSpan>(waveOut, n.Value + n.Position));
                            }

                        }catch { }
                    }

                    if (_time.Ticks < Length.Ticks - 10000 && _notes.Count > oldNotesCount)
                        Synthesize(); // new notes were added, continue synthesizing
                    else
                        StopSynthesize(); // we're done
                });
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                StopSynthesize();
            }
        }
    }

    
    /// <summary>
    /// Sine wave generator, courtesy Mark Heath https://mark-dot-net.blogspot.ca/2009/10/playback-of-sine-wave-in-naudio.html
    /// </summary>
    public class SineWaveProvider32 : WaveProvider32
    {
        int sample;

        public SineWaveProvider32()
        {
            Frequency = 1000;
            Amplitude = 0.25f; // let's not hurt our ears            
        }

        public float Frequency { get; set; }
        public float Amplitude { get; set; }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            int sampleRate = WaveFormat.SampleRate;
            for (int n = 0; n < sampleCount; n++)
            {
                buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));
                sample++;
                if (sample >= sampleRate) sample = 0;
            }
            return sampleCount;
        }
    }
}
