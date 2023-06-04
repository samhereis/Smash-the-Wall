using Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Music
{
    [CreateAssetMenu(fileName = "Spectrum Width Holder", menuName = "Scriptables/Music")]
    public class SpectrumData : ScriptableObject
    {
        [field: SerializeField] public float[] frequencies { get; private set; } = new float[64];

        public Action<float[]> onValueChanged;

        public float[] SetSpectrumWidth(AudioSource audioSource)
        {
            audioSource.GetSpectrumData(frequencies, 0, FFTWindow.Blackman);

            onValueChanged?.Invoke(frequencies);

            return frequencies;
        }

        public float GetData(int start, int end, float multiplier)
        {
            return frequencies[start..end].Average() * multiplier;
        }

        public async Task<float> GetDataAsync(int start, int end, float multiplier)
        {
            await AsyncHelper.Delay();

            return frequencies[start..end].Average() * multiplier;
        }

        public float GetData(int start, int end, float multiplier, float minValue)
        {
            return minValue + frequencies[start..end].Average() * multiplier;
        }

        public async Task<float> SetDataAsync(int start, int end, float multiplier, float minValue)
        {
            await AsyncHelper.Delay();

            return minValue + frequencies[start..end].Average() * multiplier;
        }
    }
}