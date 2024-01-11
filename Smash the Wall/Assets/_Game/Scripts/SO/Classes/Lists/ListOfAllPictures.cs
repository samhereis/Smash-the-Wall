using Configs;
using DependencyInjection;
using ECS.Authoring;
using Helpers;
using IdentityCards;
using Managers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllPictures", menuName = "Scriptables/Lists/ListOfAllPictures")]
    public class ListOfAllPictures : ConfigBase, INeedDependencyInjection, ISelfValidator
    {
        public IEnumerable<PictureIdentityCard> pictures => _pictures;

        [Required]
#if UNITY_EDITOR
        [ListDrawerSettings(ElementColor = nameof(GetColor), ListElementLabelName = ("targetName"))]
#endif
        [SerializeField]
        private List<PictureIdentityCard> _pictures = new List<PictureIdentityCard>();

        [field: FoldoutGroup("Border Animation"), SerializeField]
        public Color borderDefaultColor { get; private set; } = Color.cyan;

        [field: FoldoutGroup("Border Animation"), SerializeField]
        public float borderMaterialAnimationDuration { get; private set; } = 1f;

        [Inject] private GameSaveManager _gameSaveManager;

#if UNITY_EDITOR

        [FoldoutGroup("In Editor Settings"), SerializeField] private Color _destroyborderInspectorColor = Color.cyan;
        [FoldoutGroup("In Editor Settings"), SerializeField] private Color _destroyWholeObjectInspectorColor = Color.red;

        [Required]
        [ListDrawerSettings(ElementColor = nameof(GetColor), AlwaysAddDefaultValue = true)]
        [FoldoutGroup("In Editor Settings"), SerializeField] private List<PictureAuthoring> _picturesEditor = new List<PictureAuthoring>();

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

        public override void Initialize()
        {
            DependencyContext.InjectDependencies(this);
        }

        public PictureIdentityCard GetRandom()
        {
            return _pictures.GetRandom();
        }

        public int GetRandomIndex()
        {
            return _pictures.IndexOf(GetRandom());
        }

        public int GetCurrentIndex()
        {
            var save = _gameSaveManager.GetLevelSave();
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            if (pictureIndex >= _pictures.Count)
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

            if (pictureIndex >= _pictures.Count) { pictureIndex = 0; }

            save.pictureIndex = pictureIndex;
        }

        public PictureIdentityCard GetCurrent()
        {
            int pictureIndex = GetCurrentIndex();

            return _pictures[pictureIndex];
        }

        public void Validate(SelfValidationResult result)
        {
            if (_picturesEditor.Count != _pictures.Count)
            {
                Validate();
            }
        }

        [Button]
        private void Validate()
        {
            _picturesEditor = AutoSort();

            _pictures.Clear();

            foreach (var pictureInEditor in _picturesEditor)
            {
                if (pictureInEditor == null) continue;

                PictureIdentityCard pictureIdentityCard = new PictureIdentityCard(pictureInEditor);
                _pictures.Add(pictureIdentityCard);
            }

            foreach (var picture in _pictures)
            {
                picture.Setup();
            }
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