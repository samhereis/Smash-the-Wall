using DTO.Save;
using Helpers;

namespace Managers
{
    public class GameSaveManager
    {
        private const string _levelSaveFolderPath = "Saves";
        private const string _levelSaveFileName = "LevelSave";

        private const string _weaponsSaveFolderPath = "Saves";
        private const string _weaponsSaveFileName = "WeaponsSave";

        public static LevelSave_DTO GetLevelSave()
        {
            var levelSave = SaveHelper.GetStoredDataClass<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);

            if (levelSave == null)
            {
                levelSave = new LevelSave_DTO();
            }

            return levelSave;
        }

        public static void SaveLevel(LevelSave_DTO levelSave)
        {
            SaveHelper.SaveToJson(levelSave, _levelSaveFolderPath, _levelSaveFileName);
        }

        public static Weapons_DTO GetWeaponsSave()
        {
            var weaponsSave = SaveHelper.GetStoredDataClass<Weapons_DTO>(_weaponsSaveFolderPath, _weaponsSaveFileName);

            if (weaponsSave == null)
            {
                weaponsSave = new Weapons_DTO();
            }

            return weaponsSave;
        }

        public static void SaveWeapons(Weapons_DTO weaponsSave)
        {
            SaveHelper.SaveToJson(weaponsSave, _weaponsSaveFolderPath, _weaponsSaveFileName);
        }
    }
}