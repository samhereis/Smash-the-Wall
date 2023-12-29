using Configs;
using DependencyInjection;
using ECS.Authoring;
using Helpers;
using IdentityCards;
using Managers;
using Services;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllPictures", menuName = "SO/Lists/ListOfAllPictures")]
    public class ListOfAllPictures : ConfigBase, IDIDependent, ISelfValidator
    {
#if UNITY_EDITOR

        [ShowInInspector] private Color _destroyborderInspectorColor = Color.cyan;
        [ShowInInspector] private Color _destroyWholeObjectInspectorColor = Color.red;

        [Required]
        [ListDrawerSettings(ElementColor = nameof(GetColor), AlwaysAddDefaultValue = true)]
        [ShowInInspector] private List<PictureAuthoring> _picturesEditor = new List<PictureAuthoring>();

        private Color GetColor(int index)
        {
            if (_picturesEditor.HasEnoughElementsForIndex(index) == false) return Color.black;
            if (_picturesEditor.ElementAt(index) == null) return Color.black;

            if (_picturesEditor.ElementAt(index).pictureMode == DataClasses.Enums.PictureMode.DestroyWholeObject)
            {
                return _destroyWholeObjectInspectorColor;
            }
            else if (_picturesEditor.ElementAt(index).pictureMode == DataClasses.Enums.PictureMode.DestroyBorder)
            {
                return _destroyborderInspectorColor;
            }

            return Color.black;
        }

#endif

        [Required]
        [ListDrawerSettings(ElementColor = nameof(GetColor))]
        [ShowInInspector, ReadOnly] public List<PictureIdentityCard> pictures { get; private set; } = new List<PictureIdentityCard>();

        [Inject][SerializeField] private GameSaveManager _gameSaveManager;
        [Inject][SerializeField] private LazyUpdator_Service _lazyUpdator;

        [Header("Border Animation")]
        [field: SerializeField] public Color borderDefaultColor = Color.cyan;
        [field: SerializeField] public float borderMaterialAnimationDuration = 1f;

        public override void Initialize()
        {
            DependencyInjector.InjectDependencies(this);
        }

        public PictureIdentityCard GetRandom()
        {
            return pictures.GetRandom();
        }

        public int GetRandomIndex()
        {
            return pictures.IndexOf(GetRandom());
        }

        public int GetCurrentIndex()
        {
            var save = _gameSaveManager.GetLevelSave();
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            if (pictureIndex >= pictures.Count)
            {
                pictureIndex = 0;
            }

            return pictureIndex;
        }

        public void SetNextPicture()
        {
            var save = _gameSaveManager.GetLevelSave();
            int pictureIndex = GetCurrentIndex();

            pictureIndex++;

            if (pictureIndex >= pictures.Count) { pictureIndex = 0; }

            save.pictureIndex = pictureIndex;
        }

        public PictureIdentityCard GetCurrent()
        {
            int pictureIndex = GetCurrentIndex();

            return pictures[pictureIndex];
        }

        public void Validate(SelfValidationResult result)
        {
            if (_picturesEditor.Count != pictures.Count)
            {
                pictures.Clear();

                foreach (var pictureInEditor in _picturesEditor)
                {
                    if (pictureInEditor == null) continue;

                    PictureIdentityCard pictureIdentityCard = new PictureIdentityCard();
                    pictureIdentityCard.SetTarget(pictureInEditor);
                    pictures.Add(pictureIdentityCard);
                }
            }
        }

        [Button]
        private void Validate()
        {
            foreach (var picture in pictures)
            {
                picture.Validate();
            }

            _picturesEditor = AutoSort();
        }

        private List<PictureAuthoring> AutoSort()
        {
            List<PictureAuthoring> sortedPictureAuthorings = new();

            var groupedPictureAuthorings = _picturesEditor.GroupBy(w => w.pictureMode).ToList();

            int maxCount = groupedPictureAuthorings.Max(g => g.Count());

            for (int i = 0; i < maxCount; i++)
            {
                foreach (var group in groupedPictureAuthorings)
                {
                    if (i < group.Count())
                    {
                        sortedPictureAuthorings.Add(group.ElementAt(i));
                    }
                }
            }

            return sortedPictureAuthorings;
        }
    }
}