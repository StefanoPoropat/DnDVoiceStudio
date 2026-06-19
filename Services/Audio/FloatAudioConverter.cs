namespace DnDVoiceStudio.Services.Audio;

public static class FloatAudioConverter
{
    public static float[] BytesToFloats(
        byte[] buffer,
        int bytesRecorded)
    {
        int sampleCount = bytesRecorded / 2;

        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            short sample =
                BitConverter.ToInt16(buffer, i * 2);

            samples[i] =
                sample / 32768f;
        }

        return samples;
    }

    public static byte[] FloatsToBytes(
        float[] samples)
    {
        byte[] bytes =
            new byte[samples.Length * 2];

        for (int i = 0; i < samples.Length; i++)
        {
            float value =
                Math.Clamp(
                    samples[i],
                    -1f,
                    1f);

            short sample = (short)(value * 32767);

            byte[] sampleBytes =
                BitConverter.GetBytes(sample);

            bytes[i * 2] = sampleBytes[0];
            bytes[i * 2 + 1] = sampleBytes[1];
        }

        return bytes;
    }
}