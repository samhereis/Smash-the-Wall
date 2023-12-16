using DG.Tweening;
using Helpers;
using Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements
{
    public class Star_CustomControl : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private SingleStarIdentifier _starsPrefab;

        [Header("Components")]
        [SerializeField] private Transform _starsParent;

        [Header("Addressables")]
        [SerializeField] private AssetReferenceAudioClip _starAudio;

        [Header("Debug")]
        [SerializeField] private List<SingleStarIdentifier> _instantiatedStars = new List<SingleStarIdentifier>();
        [SerializeField] private int _starsCount = 0;
        [SerializeField] private SimpleSound _currentStarAudio;

        private async void Awake()
        {
            _currentStarAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_starAudio));
        }

        private void OnDisable()
        {
            foreach (var star in GetComponentsInChildren<SingleStarIdentifier>(true))
            {
                star.transform.DOKill();
            }
        }

        public void SetStarCount(int starCount)
        {
            _starsCount = starCount;

            foreach (var star in GetComponentsInChildren<SingleStarIdentifier>(true))
            {
                Destroy(star.gameObject);
            }

            for (int i = 0; i < _starsCount; i++)
            {
                var starInstance = Instantiate(_starsPrefab, _starsParent);
                _instantiatedStars.Add(starInstance);
            }
        }

        public void SetActiveStars(int newActiveStarsCount)
        {
            for (int i = 0; i < _instantiatedStars.Count; i++)
            {
                var starInstance = _instantiatedStars[i];
                starInstance.transform.localScale = Vector3.zero;
            }

            StartCoroutine(SetActiveStarsEnumerator(newActiveStarsCount));
        }

        private IEnumerator SetActiveStarsEnumerator(int newActiveStarsCount)
        {
            for (int i = 0; i < _instantiatedStars.Count; i++)
            {
                var starInstance = _instantiatedStars[i];
                float scale = 0.5f + NumberHelper.GetPercentageOf1(i + 1, _starsCount) / 2;
                starInstance.transform.DOScale(scale, 0.5f);

                _currentStarAudio.volume = scale;
            }

            for (int i = 0; i < _instantiatedStars.Count; i++)
            {
                if (i < newActiveStarsCount)
                {
                    var starInstance = _instantiatedStars[i];
                    starInstance.Activate();

                    SoundPlayer.instance.TryPlay(_currentStarAudio);

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}