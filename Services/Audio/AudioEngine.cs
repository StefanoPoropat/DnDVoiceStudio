using DnDVoiceStudio.Services.Ai;
using DnDVoiceStudio.Services.Audio.Effects;
using NAudio.Wave;

namespace DnDVoiceStudio.Services.Audio;

public class AudioEngine
{
    private WaveInEvent? _waveIn;
    private BufferedWaveProvider? _buffer;
    private WaveOutEvent? _waveOut;

    private readonly PitchShiftEffect _pitchEffect = new();
    private readonly ReverbEffect _reverbEffect = new();
    private readonly DistortionEffect _distortionEffect = new();

    private readonly FormantShiftEffect _formantEffect = new();
    private readonly BassBoostEffect _bassBoostEffect = new();
    private readonly TrebleBoostEffect _trebleBoostEffect = new();
    private readonly CompressionEffect _compressionEffect = new();

    private readonly DemonEffect _demonEffect = new();
    private readonly WhisperEffect _whisperEffect = new();
    private readonly RadioEffect _radioEffect = new();
    private readonly TitanEffect _titanEffect = new();

    private VoiceProfile _currentProfile = new();

    private bool _monitorEnabled = true;
    private float _outputVolume = 4.0f;

    private DateTime _lastLevelUpdate =
        DateTime.MinValue;

    private IAiVoiceEngine _aiEngine =
        new DummyAiVoiceEngine();

    private WaveOutEvent? _previewOutput;
    private BufferedWaveProvider? _previewBuffer;

    public event Action<double>? LevelChanged;

    public void Start(int inputDeviceNumber)
    {
        Stop();

        _waveIn = new WaveInEvent
        {
            DeviceNumber = inputDeviceNumber,
            WaveFormat = new WaveFormat(16000, 1)
        };

        _buffer = new BufferedWaveProvider(
            _waveIn.WaveFormat)
        {
            BufferDuration =
                TimeSpan.FromMilliseconds(200),

            DiscardOnBufferOverflow = true
        };

        _waveOut = new WaveOutEvent
        {
            DesiredLatency = 100
        };

        _waveOut.Init(_buffer);

        _waveIn.DataAvailable += (_, e) =>
        {
            try
            {
                byte[] inputBuffer =
                    new byte[e.BytesRecorded];

                Buffer.BlockCopy(
                    e.Buffer,
                    0,
                    inputBuffer,
                    0,
                    e.BytesRecorded);

                double peak = 0;

                for (int i = 0;
                     i < inputBuffer.Length - 1;
                     i += 2)
                {
                    short sample =
                        BitConverter.ToInt16(
                            inputBuffer,
                            i);

                    peak = Math.Max(
                        peak,
                        Math.Abs(sample));
                }

                if ((DateTime.Now - _lastLevelUpdate)
                    .TotalMilliseconds > 50)
                {
                    _lastLevelUpdate =
                        DateTime.Now;

                    LevelChanged?.Invoke(
                        peak /
                        short.MaxValue *
                        100.0);
                }

                if (!_monitorEnabled)
                    return;

                float[] floatSamples =
                    FloatAudioConverter
                    .BytesToFloats(
                        inputBuffer,
                        inputBuffer.Length);

                float[] processed =
                    ProcessAudio(
                        floatSamples);

                byte[] outputBytes =
                    FloatAudioConverter
                    .FloatsToBytes(
                        processed);

                _buffer?.AddSamples(
                    outputBytes,
                    0,
                    outputBytes.Length);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug
                    .WriteLine(ex.Message);
            }
        };

        _waveOut.Play();

        _waveIn.StartRecording();
    }

    public void Stop()
    {
        _waveIn?.StopRecording();
        _waveIn?.Dispose();
        _waveIn = null;

        _waveOut?.Stop();
        _waveOut?.Dispose();
        _waveOut = null;

        _buffer = null;
    }

    public void SetMonitoring(bool enabled)
    {
        _monitorEnabled = enabled;
    }

    private float[] ProcessAudio(
        float[] samples)
    {
        float[] processed =
            _pitchEffect.Process(samples);

        processed =
            _formantEffect.Process(
                processed);

        processed =
            _bassBoostEffect.Process(
                processed);

        processed =
            _trebleBoostEffect.Process(
                processed);

        processed =
            _compressionEffect.Process(
                processed);

        processed =
            _demonEffect.Process(
                processed);

        processed =
            _whisperEffect.Process(
                processed);

        processed =
            _radioEffect.Process(
                processed);

        processed =
            _titanEffect.Process(
                processed);

        if (_aiEngine.IsLoaded)
        {
            processed =
                _aiEngine.Process(
                    processed);
        }
        processed =
    _distortionEffect.Process(
        processed);

        processed =
            _reverbEffect.Process(
                processed);

        return processed;
    }

    public void SetProfile(
        VoiceProfile profile)
    {
        _currentProfile = profile;

        _pitchEffect.SetPitch(
            profile.Pitch);

        _reverbEffect.SetAmount(
            profile.Reverb / 100f);

        _distortionEffect.SetAmount(
            profile.Distortion / 100f);

        _formantEffect.SetAmount(
            profile.Formant);

        _bassBoostEffect.SetAmount(
            profile.BassBoost);

        _trebleBoostEffect.SetAmount(
            profile.TrebleBoost);

        _compressionEffect.SetAmount(
            profile.Compression);

        _demonEffect.SetAmount(
            profile.Demon);

        _whisperEffect.SetAmount(
            profile.Whisper);

        _radioEffect.SetAmount(
            profile.Radio);

        _titanEffect.SetAmount(
            profile.Titan);
    }

    public VoiceProfile CurrentProfile =>
        _currentProfile;

    public void SetOutputVolume(
        float volume)
    {
        _outputVolume = volume;
    }

    public bool LoadAiModel(
        string modelPath)
    {
        return _aiEngine.LoadModel(
            modelPath);
    }

    public void SetAiEngine(
        IAiVoiceEngine engine)
    {
        _aiEngine = engine;
    }

    public async Task PlayPreviewAsync(
        string filePath)
    {
        await Task.Run(() =>
        {
            StopPreview();

            using var reader =
                new AudioFileReader(
                    filePath);

            List<float> samples =
                new();

            float[] buffer =
                new float[1024];

            int read;

            while ((read =
                reader.Read(
                    buffer,
                    0,
                    buffer.Length)) > 0)
            {
                for (int i = 0;
                     i < read;
                     i++)
                {
                    samples.Add(
                        buffer[i]);
                }
            }

            float[] processed =
                ProcessAudio(
                    samples.ToArray());

            byte[] outputBytes =
                FloatAudioConverter
                .FloatsToBytes(
                    processed);

            _previewBuffer =
    new BufferedWaveProvider(
        new WaveFormat(
            44100,
            1))
    {
        BufferDuration =
        TimeSpan.FromSeconds(30),

        DiscardOnBufferOverflow = true
    };

            _previewBuffer.AddSamples(
                outputBytes,
                0,
                outputBytes.Length);

            _previewOutput =
                new WaveOutEvent();

            _previewOutput.Init(
                _previewBuffer);

            _previewOutput.Play();
        });
    }

    public void StopPreview()
    {
        _previewOutput?.Stop();
        _previewOutput?.Dispose();

        _previewOutput = null;
        _previewBuffer = null;
    }
}