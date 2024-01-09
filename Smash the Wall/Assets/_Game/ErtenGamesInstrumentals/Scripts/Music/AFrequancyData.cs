using DependencyInjection;
using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Music
{
    [CreateAssetMenu(fileName = "A Frequancy Data", menuName = "Scriptables/Music/A Frequancy Data")]
    public class AFrequancyData : ScriptableObject, IInitializable, INeedDependencyInjection
    {
        [FoldoutGroup("Multipliers"), SerializeField] private float _multiplier = 1;
        [FoldoutGroup("Multipliers"), SerializeField] public float defaultMultiplier { get; private set; } = 1;

        [FoldoutGroup("Frequency Ranges"), SerializeField] private int _rangeStart = 1;
        [FoldoutGroup("Frequency Ranges"), SerializeField] private int _rangeEnd = 5;

        [Inject]
        [FoldoutGroup("SO"), SerializeField] private SpectrumData _playingMusicFrequencies;

        [FoldoutGroup("Debug"), SerializeField, ReadOnly] public float value { get; private set; }
        [FoldoutGroup("Debug"), SerializeField] public float valueWithDefaultMultiplier => value * defaultMultiplier;

        public virtual void Initialize()
        {
            DependencyContext.InjectDependencies(this);

            _playingMusicFrequencies.onValueChanged -= GetData;
            _playingMusicFrequencies.onValueChanged += GetData;
        }

        protected virtual void GetData(float[] spectrumData)
        {
            value = _playingMusicFrequencies.GetData(_rangeStart, _rangeEnd, _multiplier);
        }
    }
}