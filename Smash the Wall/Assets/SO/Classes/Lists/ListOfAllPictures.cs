using Configs;
using DI;
using Helpers;
using IdentityCards;
using InGameStrings;
using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllPictures", menuName = "SO/Lists/ListOfAllPictures")]
    public class ListOfAllPictures : ConfigBase, IDIDependent
    {
        [field: SerializeField] public List<PictureIdentityCard> pictures { get; private set; } = new List<PictureIdentityCard>();

        [DI(DIStrings.gameSaveManager)][SerializeField] private GameSaveManager _gameSaveManager;

        public override void Initialize()
        {
            (this as IDIDependent).LoadDependencies();

            foreach (var picture in pictures)
            {
                picture.AutoSetTargetName();
            }
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
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

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