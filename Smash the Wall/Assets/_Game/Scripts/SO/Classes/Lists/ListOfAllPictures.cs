using Configs;
using DependencyInjection;
using Helpers;
using IdentityCards;
using Managers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllPictures", menuName = "Scriptables/Lists/ListOfAllPictures")]
    public class ListOfAllPictures : ConfigBase, INeedDependencyInjection
    {
        public IEnumerable<PictureIdentityCard> pictures => _pictures;

        [Required]
        [SerializeField]
        private List<PictureIdentityCard> _pictures = new List<PictureIdentityCard>();

        [field: FoldoutGroup("Border Animation"), SerializeField]
        public Color borderDefaultColor { get; private set; } = Color.cyan;

        [field: FoldoutGroup("Border Animation"), SerializeField]
        public float borderMaterialAnimationDuration { get; private set; } = 1f;

        [Inject] private GameSaveManager _gameSaveManager;

        [Button]
        private void Validate()
        {
            foreach (var picture in _pictures)
            {
                picture.Setup();
            }
        }

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
    }
}