using Configs;
using DTO.Save;
using Helpers;
using IdentityCards;
using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllPictures", menuName = "SO/Lists/ListOfAllPictures")]
    public class ListOfAllPictures : ConfigBase
    {
        [field: SerializeField] public List<PictureIdentityCard> pictures { get; private set; } = new List<PictureIdentityCard>();

        public override void Initialize()
        {
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

        public int GeCurrentIndex()
        {
            var save = GameSaveManager.GetLevelSave();
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            if (pictureIndex >= pictures.Count) { pictureIndex = 0; }

            return pictureIndex;
        }

        public void SetNextLevel()
        {
            var save = GameSaveManager.GetLevelSave();
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            pictureIndex++;

            if (pictureIndex >= pictures.Count) { pictureIndex = 0; }

            save = new LevelSave_DTO() { pictureIndex = pictureIndex };

            GameSaveManager.SaveLevel(save);
        }

        public PictureIdentityCard GetCurrent()
        {
            var save = GameSaveManager.GetLevelSave();
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            return pictures[pictureIndex];
        }
    }
}