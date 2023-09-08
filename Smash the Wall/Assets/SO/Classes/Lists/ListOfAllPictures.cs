using Configs;
using DI;
using Helpers;
using IdentityCards;
using InGameStrings;
using LazyUpdators;
using Managers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllPictures", menuName = "SO/Lists/ListOfAllPictures")]
    public class ListOfAllPictures : ConfigBase, IDIDependent
    {
        [field: SerializeField] public List<PictureIdentityCard> pictures { get; private set; } = new List<PictureIdentityCard>();
        [field: SerializeField] public List<PictureIdentityCard> pictures_DestroyObject { get; private set; } = new List<PictureIdentityCard>();
        [field: SerializeField] public List<PictureIdentityCard> pictures_DestroyBorder { get; private set; } = new List<PictureIdentityCard>();
        [field: SerializeField] public List<PictureIdentityCard> pictures_Draw { get; private set; } = new List<PictureIdentityCard>();

        [DI(DIStrings.gameSaveManager)][SerializeField] private GameSaveManager _gameSaveManager;
        [DI(DIStrings.lazyUpdator)][SerializeField] private LazyUpdator_SO _lazyUpdator;

        [Header("Border Animation")]
        [field: SerializeField] public Color borderDefaultColor = Color.cyan;
        [field: SerializeField] public float borderMaterialAnimationDuration = 1f;

        [Header("Debug")]
        [SerializeField] private PictureIdentityCard _currentPicture;

        public override void Initialize()
        {
            (this as IDIDependent).LoadDependencies();

            foreach (var picture in pictures)
            {
                picture.AutoSetTargetName();
            }

#if UNITY_EDITOR

            _lazyUpdator.RemoveFromQueue(UpdateDebug);
            _lazyUpdator.AddToQueue(UpdateDebug);

#endif
        }

#if UNITY_EDITOR

        private async Task UpdateDebug()
        {
            await AsyncHelper.Delay();

            _currentPicture = GetCurrent();
        }

#endif

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
    }
}