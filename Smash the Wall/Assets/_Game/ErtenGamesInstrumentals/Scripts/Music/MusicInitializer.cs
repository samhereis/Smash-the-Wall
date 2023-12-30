using DependencyInjection;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class MusicInitializer : MonoBehaviour, IDIDependent
    {
        [Required]
        [SerializeField] private List<SpectrumData> _spectrumData;

        [Required]
        [SerializeField] private List<MusicList_SO> _musicLists;

        [Required]
        [FoldoutGroup("Frequencies"), SerializeField] private List<AFrequancyData> _listOfAllFrequencies = new List<AFrequancyData>();

        private void Awake()
        {
            foreach (var item in _spectrumData)
            {
                item.Initialize();
            }

            foreach (var item in _musicLists)
            {
                item.Initialize();
            }

            foreach (var item in _listOfAllFrequencies)
            {
                item.Initialize();
            }
        }
    }
}