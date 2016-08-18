using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Media;
using System.Numerics;

using MediaToolkit;
using MediaToolkit.Model;
using NAudio.Wave;

using Cantus.Core.CommonTypes;
using Cantus.Core;
using System.Threading;

namespace Harmony
{
    /// <summary>
    /// A class for reading, interpreting, and playing audio files
    /// </summary>
    public class AudioFile : IDisposable
    {

        #region "Events"
        /// <summary>
        /// Event data for Fourier Progress event, representing the result of a partial calculation
        /// </summary>
        public sealed class FourierProgressEventArgs : EventArgs
        {
            /// <summary>
            /// Time for which we analyzed the frequencies
            /// </summary>
            public TimeSpan Time { get; set; }
            /// <summary>
            /// IEnumerable of frequencies detected
            /// </summary>
            public IList<double> Frequency { get; set; }

            /// <summary>
            /// IEnumerable of amplitudes detected
            /// </summary>
            public IList<double> Amplitude { get; set; }

            /// <summary>
            /// IEnumerable of lengths detected
            /// </summary>
            public IList<TimeSpan> Length { get; set; }
        }
        public delegate void FourierProgressDelegate(object sender, FourierProgressEventArgs e);

        /// <summary>
        /// Raised when the frequency analysis has revealed a new note
        /// </summary>
        public event FourierProgressDelegate FourierProgress;

        public delegate void LoadCompleteDelegate(object sender, EventArgs e);

        /// <summary>
        /// Raised when loading of the file is complete
        /// </summary>
        public event LoadCompleteDelegate LoadComplete;

        /// <summary>
        /// Raised when playback is stopped
        /// </summary>
        public event EventHandler<StoppedEventArgs> Stopped
        {
            add
            {
                _player.PlaybackStopped += value;
            }
            remove
            {
                _player.PlaybackStopped -= value;
            }
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// If true, deletes the file when disposed
        /// </summary>
        private bool _isTemp = false;
        private string _filePath = "";
        /// <summary>
        /// The path to the file read, if available. Blank otherwise.
        /// </summary>
        public string FilePath { get { return _filePath; } }

        private uint _fileLength = 0;
        /// <summary>
        /// The length of this audio file, excluding the WAVE and RIFF markings in the header
        /// </summary>
        public uint FileLength { get { return _fileLength; } }

        // RIFF information
        private string _riffType = "WAVE";
        private ushort _formatType = 1;

        private ushort _channels;
        /// <summary>
        /// The number of channels in this WAV file. 1 is mono, 2 is stereo.
        /// </summary>
        public ushort Channels { get { return _channels; } }

        private uint _sampleRate;
        /// <summary>
        /// The sample rate of the audio, in samples per second
        /// </summary>
        public uint SampleRate { get { return _sampleRate; } }

        private ushort _bitsPerSample;
        /// <summary>
        /// The number of bits in each sample. Normally 8, 16, etc.
        /// </summary>
        public ushort BitsPerSample { get { return _bitsPerSample; } }

        private uint _totalSamples;
        /// <summary>
        /// The total number of samples in this file
        /// </summary>
        public uint TotalSamples { get { return _totalSamples; } }

        private uint _blocksPerSecond;
        /// <summary>
        /// The average number of audio blocks per second. 
        /// A block is a group of samples, representing a point in time across all the channels.
        /// </summary>
        public uint BlocksPerSecond { get { return _blocksPerSecond; } }

        private ushort _bytesPerBlock;
        /// <summary>
        /// The number of bytes in a block. ('BlockAlign')
        /// A block is a group of samples, representing a point in time across all the channels.
        /// </summary>
        public uint BytesPerBlock { get { return _bytesPerBlock; } }
        #endregion

        #region "Private Variables"
        /// <summary>
        /// The content of the WAV file, represented as a 2D array of doubles (first dimension represents channels)
        /// </summary>
        private double[][] _data;

        /// <summary>
        /// Player for the audio file
        /// </summary>
        private IWavePlayer _player { get; set; } = new WaveOutEvent();

        /// <summary>
        /// The NAudio audio file reader
        /// </summary>
        private WaveFileReader _audio { get; set; }

        /// <summary>
        /// True if the audio file is currently playing
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return _player.PlaybackState == PlaybackState.Playing;
            }
        }

        /// <summary>
        /// Total length in time of this audio file
        /// </summary>
        public TimeSpan TotalTime {
            get
            {
                return _audio.TotalTime;
            }
        }

        /// <summary>
        /// Current point in time of this audio file
        /// </summary>
        public TimeSpan CurrentTime {
            get
            {
                return _audio.CurrentTime;
            }
            set
            {
                if (_audio == null) return;
                _audio.CurrentTime = value;
            }
        }

        #endregion


        #region "Constructors"
        /// <summary>
        /// Open an audio file with the path specified. If one does not exist, raises an error.
        /// </summary>
        public AudioFile(string path)
        {
            Thread th = new Thread(() =>
            {
                try
                {
                    if (!path.EndsWith(".wav"))
                    {
                        path = Convert(path);
                        _isTemp = true;
                    }
                    _filePath = Path.GetFullPath(path);
                    ReadWAV(new FileStream(path, FileMode.Open));
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine(String.Format("\"{0}\": file does not exist.", path));
                }
                catch
                {
                    Console.Error.WriteLine(String.Format("Unable to open WAV file at \"{0}\" for reading.", path));
                }
                if (LoadComplete != null) LoadComplete(this, new EventArgs());
            });
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// Convert an input audio file to an arbitrary 
        /// </summary>
        private static string Convert(string input)
        {
            MediaFile inFile = new MediaFile(input);
            string output = Path.GetTempFileName() + ".wav";
            MediaFile outFile = new MediaFile(output);
            using (Engine eng = new Engine())
            {
                eng.Convert(inFile, outFile);
            }
            return output;
        }
        #endregion

        /// <summary>
        /// Play this audio file
        /// </summary>
        public void Play()
        {
            _player.Play();
        }

        /// <summary>
        /// Pause this audio file
        /// </summary>
        public void Pause()
        {
            _player.Pause();
        }

        /// <summary>
        /// Stop playing this audio file
        /// </summary>
        public void Stop()
        {
            _player.Stop();
            _audio.CurrentTime = TimeSpan.Zero;
        }

        private FFTProvider _fft = new FFTProvider();
        /// <summary>
        /// The interval at which frequencies are sampled to determine notes present
        /// </summary>
        public double AnalyzeLen
        {
            get
            {
                try {
                    double floor = Math.Floor((double)_data[0].Length / 8192);
                    return (double)(_data[0].Length) / floor;
                }
                catch
                {
                    return double.NaN;
                }
            }
        }

        /// <summary>
        /// Analyze the frequencies beginning at the specified point
        /// </summary>
        private void FourierAnalyze(double start)
        {
            start = Math.Floor(start);
            double[] data = new double[(int)AnalyzeLen];
            for (int i = (int)start; i < (int)start + (int)AnalyzeLen; ++i)
            {
                data[(i - (int)start)] = GetAverageIntensity(i);
            }
            double[] fft = _fft.FFT(data);
            int max = 0;

            FourierProgressEventArgs args = new FourierProgressEventArgs();
            List<double> freq = new List<double>();
            List<TimeSpan> len = new List<TimeSpan>();
            TimeSpan ts = SamplesToTime(AnalyzeLen);
            List<double> amp = new List<double>();

            for (int i = 1; i < Math.Min(fft.Count(), 1 + 4186.009 * 4 * (AnalyzeLen / SampleRate)); ++i)
                fft[i] = fft[i] * Math.Pow(0.998, i-100) / 9600;

            for (int i = 1; i<Math.Min(fft.Count(), 1 + 4186.009  * 4 * (AnalyzeLen /  SampleRate) ); ++i)
            {
                if (fft[i] > 1)
                {
                    double f = i * (SampleRate / AnalyzeLen) / 2;
                    double a = Math.Min(Math.Max(Math.Log10(Math.Abs(fft[i])), 0.000000001), 1.0);
                    if (freq.Count > 0 &&  (f / freq[freq.Count - 1]) < 1.1 )
                    {
                        if (a >= amp[amp.Count - 1])
                        {
                            freq[freq.Count - 1] = f;
                            amp[amp.Count - 1] = a;
                        }
                    }
                    else
                    {
                        freq.Add(f);
                        len.Add(ts);
                        amp.Add(a);
                    }
                }
                if (fft[i] > fft[max]) max = i;
            }
            if (freq.Count == 0)
            {
                freq.Add(max * (SampleRate / AnalyzeLen)/ 2);
                len.Add(ts);
                amp.Add(Math.Min(Math.Max(Math.Abs(fft[max]), 0), 1.0));
            }

            args.Frequency = freq;
            args.Amplitude = amp;
            args.Length = len;
            args.Time = SamplesToTime(start);
            if (FourierProgress != null)
                FourierProgress(this, args);
        }

        /// <summary>
        /// Convert a duration in time to a number of samples in this audio file
        /// </summary>
        public double TimeToSamples(TimeSpan time)
        {
            return ((double)time.Ticks / _audio.TotalTime.Ticks * _data[0].Length);
        }

        /// <summary>
        /// Convert a number of samples in this audio file to a duration in time
        /// </summary>
        public TimeSpan SamplesToTime(double samples)
        {
            return TimeSpan.FromTicks((long)(samples / _data[0].Length * _audio.TotalTime.Ticks)); 
        }

        /// <summary>
        /// Begin asynchronous analysis of frequencies in this piece of audio. 
        /// The FourierProgress event is raised when frequencies are found.
        /// </summary>
        public void Analyze()
        {
            Thread th = new Thread(() =>
            {
                for (double start = 0; start + AnalyzeLen <= _data[0].Length; start += AnalyzeLen)
                {
                    FourierAnalyze(start);
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// Get the intensity of the audio at the specified block number, on the specified channel 
        /// </summary>
        public double GetIntensity(int block, int channel = 0)
        {
            return _data[channel][block];
        }

        /// <summary>
        /// Get the average intensity of the audio at the specified block number, on the specified channel 
        /// </summary>
        public double GetAverageIntensity(int block)
        {
            double total = 0;
            for (int i = 0; i < _data.GetLength(0); ++i)
            {
                total += _data[i][block];
            }
            return total / _data.GetLength(0);
        }

        /// <summary>
        /// Types of chunk in the WAV format, used while reading the WAV file.
        /// </summary>
        private enum ChunkType
        {
            riff, fmt, data, info,
            scot,
            other = -1
        }

        /// <summary>
        /// Private method for interpreting the data from a stream
        /// </summary>
        private void ReadWAV(Stream data)
        {
            BinaryReader reader = new BinaryReader(data);

            try
            {
                ChunkType chunk = ChunkType.other;

                uint chunkSize = 0;
                uint totalSize = 0;
                while (data.Position + 4 < data.Length)
                {

                    // new section
                    try
                    {
                        string chunkName = (new string(reader.ReadChars(4)));
                        chunk = (ChunkType)Enum.Parse(typeof(ChunkType), chunkName.TrimEnd(), true);
                    }
                    catch
                    {
                        chunk = ChunkType.other;
                    }

                    chunkSize = reader.ReadUInt32();

                    if (chunk == ChunkType.riff)
                    {
                        // header
                        _fileLength = chunkSize;
                        _riffType = new string(reader.ReadChars(4));
                        totalSize = _fileLength;
                    }
                    else
                    {
                        switch (chunk)
                        {
                            case ChunkType.fmt: // format chunk
                                totalSize -= ReadFormat(reader, chunkSize);
                                break;

                            case ChunkType.data: // data chunk
                                // allocate memory for data
                                int nFrames = (int)(chunkSize / _channels * 8 / _bitsPerSample);
                                _data = new double[_channels][];
                                _totalSamples = chunkSize * 8 / _bitsPerSample;
                                for (int i = 0; i < _data.Length; ++i) _data[i] = new double[nFrames];
                                Console.WriteLine(chunkSize + " " + _channels.ToString() + " " + _bitsPerSample.ToString());
                                for (int frameId = 0; frameId < nFrames; ++frameId)
                                {
                                    for (int chnl = 0; chnl < _channels; ++chnl)
                                    {
                                        switch (_bitsPerSample)
                                        {
                                            case 8:
                                                _data[chnl][frameId] = (float)(reader.ReadSByte()) / 128F;
                                                break;
                                            case 16:
                                                _data[chnl][frameId] = (float)(reader.ReadInt16()) / 32768F;
                                                break;
                                            case 24:
                                                // 24-bit integer reading
                                                byte byte1 = reader.ReadByte();
                                                byte byte2 = reader.ReadByte();
                                                byte byte3 = reader.ReadByte();
                                                _data[chnl][frameId] = (float)(unchecked((int)((((uint)byte1) << 16) |
                                                    (((uint)byte2) << 8) |
                                                    ((uint)byte3)))) / 8388608F;
                                                break;
                                            case 32:
                                                // 32-bit audio is already a floating point value
                                                _data[chnl][frameId] = reader.ReadSingle();
                                                break;
                                        }
                                    }
                                }
                                break;
                            // metadata not yet implemented
                            //case ChunkType.info: 
                            //    break;
                            default:
                                totalSize -= chunkSize;
                                data.Seek(chunkSize, SeekOrigin.Current);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                Console.Error.WriteLine("Error reading wave (.wav) file! File format may be invalid.");
            }
            finally
            {
                data.Close();
                // setup sound player
                _audio = new WaveFileReader(FilePath);
                _player.Init(_audio);
            }
        }

        /// <summary>
        /// Method that reads information from the format chunk. Returns the number of extra bytes.
        /// </summary>
        private uint ReadFormat(BinaryReader reader, uint chunkSize)
        {
            int bytesRead = 0;
            _formatType = reader.ReadUInt16();
            bytesRead += 2; if (chunkSize <= bytesRead) return 0;

            _channels = reader.ReadUInt16();
            bytesRead += 2; if (chunkSize <= bytesRead) return 0;

            _sampleRate = reader.ReadUInt32();
            bytesRead += 4; if (chunkSize <= bytesRead) return 0;

            _blocksPerSecond = reader.ReadUInt32();
            bytesRead += 4; if (chunkSize <= bytesRead) return 0;

            _bytesPerBlock = reader.ReadUInt16();
            bytesRead += 2; if (chunkSize <= bytesRead) return 0;

            _bitsPerSample = reader.ReadUInt16();
            bytesRead += 2; if (chunkSize <= bytesRead) return 0;

            if (chunkSize > bytesRead)
            {
                reader.BaseStream.Seek(chunkSize - bytesRead, SeekOrigin.Current);
            }
            return (uint)(chunkSize - bytesRead);
        }

        public void Dispose()
        {
            _audio.Dispose();
            _player.Dispose();
            if (_isTemp)
            {
                try
                {
                    File.Delete(_filePath);
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// Class for calculating the Cooley-Tukey Fast Fourier Transform
    /// </summary>
    public class FFTProvider
    {

        /// <summary>
        /// Gets number of significant bytes.
        /// </summary>
        /// <param name="n">Number</param>
        /// <returns>Amount of minimal bits to store the number.</returns>
        private static int Log2(int n)
        {
            int i = 0;
            while (n > 0)
            {
                ++i; n >>= 1;
            }
            return i;
        }

        /// <summary>
        /// Reverses bits in the number.
        /// </summary>
        /// <param name="n">Number</param>
        /// <param name="bitsCount">Significant bits in the number.</param>
        /// <returns>Reversed binary number.</returns>
        private static int ReverseBits(int n, int bitsCount)
        {
            int reversed = 0;
            for (int i = 0; i < bitsCount; i++)
            {
                int nextBit = n & 1;
                n >>= 1;

                reversed <<= 1;
                reversed |= nextBit;
            }
            return reversed;
        }

        /// <summary>
        /// Checks if number is power of 2.
        /// </summary>
        /// <param name="n">number</param>
        /// <returns>true if n=2^k and k is positive integer</returns>
        private static bool IsPowerOfTwo(int n)
        {
            return n > 1 && (n & (n - 1)) == 0;
        }

        /// <summary>
        /// Compute the forward discrete FFT of an array of doubles.
        /// Note: length of array must be a power of 2!
        /// </summary>
        public double[] FFT(double[] x)
        {
            int length;
            int bitsInLength;
            if (IsPowerOfTwo(x.Length))
            {
                length = x.Length;
                bitsInLength = Log2(length) - 1;
            }
            else
            {
                bitsInLength = Log2(x.Length);
                length = 1 << bitsInLength;
                // the items will be pad with zeros
            }

            // bit reversal
            ComplexNumber[] data = new ComplexNumber[length];
            for (int i = 0; i < x.Length; i++)
            {
                int j = ReverseBits(i, bitsInLength);
                data[j] = new ComplexNumber(x[i]);
            }

            // Cooley-Tukey 
            for (int i = 0; i < bitsInLength; i++)
            {
                int m = 1 << i;
                int n = m * 2;
                double alpha = -(2 * Math.PI / n);

                for (int k = 0; k < m; k++)
                {
                    // e^(-2*pi/N*k)
                    ComplexNumber oddPartMultiplier = new ComplexNumber(0, alpha * k).PoweredE();

                    for (int j = k; j < length; j += n)
                    {
                        ComplexNumber evenPart = data[j];
                        ComplexNumber oddPart = oddPartMultiplier * data[j + m];
                        data[j] = evenPart + oddPart;
                        data[j + m] = evenPart - oddPart;
                    }
                }
            }

            // calculate spectrogram
            double[] spectrogram = new double[length];
            for (int i = 0; i < spectrogram.Length; i++)
            {
                spectrogram[i] = data[i].AbsPower2();
            }
            return spectrogram;
        }

    }
    /// <summary>
    /// Complex number.
    /// </summary>
    struct ComplexNumber
    {
        public double Re;
        public double Im;

        public ComplexNumber(double re)
        {
            this.Re = re;
            this.Im = 0;
        }

        public ComplexNumber(double re, double im)
        {
            this.Re = re;
            this.Im = im;
        }

        public static ComplexNumber operator *(ComplexNumber n1, ComplexNumber n2)
        {
            return new ComplexNumber(n1.Re * n2.Re - n1.Im * n2.Im,
                n1.Im * n2.Re + n1.Re * n2.Im);
        }

        public static ComplexNumber operator +(ComplexNumber n1, ComplexNumber n2)
        {
            return new ComplexNumber(n1.Re + n2.Re, n1.Im + n2.Im);
        }

        public static ComplexNumber operator -(ComplexNumber n1, ComplexNumber n2)
        {
            return new ComplexNumber(n1.Re - n2.Re, n1.Im - n2.Im);
        }

        public static ComplexNumber operator -(ComplexNumber n)
        {
            return new ComplexNumber(-n.Re, -n.Im);
        }

        public static implicit operator ComplexNumber(double n)
        {
            return new ComplexNumber(n, 0);
        }

        public ComplexNumber PoweredE()
        {
            double e = Math.Exp(Re);
            return new ComplexNumber(e * Math.Cos(Im), e * Math.Sin(Im));
        }

        public double Power2()
        {
            return Re * Re - Im * Im;
        }

        public double AbsPower2()
        {
            return Re * Re + Im * Im;
        }

        public override string ToString()
        {
            return String.Format("{0}+i*{1}", Re, Im);
        }
    }
}
