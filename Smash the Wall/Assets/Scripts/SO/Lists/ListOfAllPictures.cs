using Configs;
using DTO.Save;
using Helpers;
using IdentityCards;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllPictures", menuName = "SO/Lists/ListOfAllPictures")]
    public class ListOfAllPictures : ConfigBase
    {
        private const string _levelSaveFolderPath = "Save/LevelSave";
        private const string _levelSaveFileName = "LevelSave";

        [field: SerializeField] public List<PictureIdentityCard> pictures { get; private set; } = new List<PictureIdentityCard>();

        public override void Initialize()
        {

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
            var save = SaveHelper.GetStoredDataClass<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            if (pictureIndex >= pictures.Count) { pictureIndex = 0; }

            return pictureIndex;
        }

        public async Task SetNextLevelAsync()
        {
            var save = await SaveHelper.GetStoredDataClassAsync<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            pictureIndex++;

            if (pictureIndex >= pictures.Count) { pictureIndex = 0; }

            save = new LevelSave_DTO() { pictureIndex = pictureIndex };

            await SaveHelper.SaveToJsonAsync(save, _levelSaveFolderPath, _levelSaveFileName);
        }

        public void SetNextLevel()
        {
            var save = SaveHelper.GetStoredDataClass<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            pictureIndex++;

            if (pictureIndex >= pictures.Count) { pictureIndex = 0; }



            save = new LevelSave_DTO() { pictureIndex = pictureIndex };

            SaveHelper.SaveToJson(save, _levelSaveFolderPath, _levelSaveFileName);
        }

        public async Task<PictureIdentityCard> GetCurrentAsync()
        {
            var save = await SaveHelper.GetStoredDataClassAsync<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            return pictures[pictureIndex];
        }

        public PictureIdentityCard GetCurrent()
        {
            var save = SaveHelper.GetStoredDataClass<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);
            int pictureIndex = 0;

            if (save != null) { pictureIndex = save.pictureIndex; }

            return pictures[pictureIndex];
        }
    }
}