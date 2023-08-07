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

        private static LevelSave_DTO _levelSave_DTO;
        private static Weapons_DTO _weapons_DTO;

        public static Weapons_DTO GetWeaponsSave()
        {
            UpdateSaves();
            return _weapons_DTO;
        }

        public static LevelSave_DTO GetLevelSave()
        {
            UpdateSaves();

            return _levelSave_DTO;
        }

        public static int GetLevelIndex()
        {
            UpdateSaves();
            return _levelSave_DTO.levelIndex;
        }

        public static void IncreaseLevelIndex()
        {
            UpdateSaves();
            _levelSave_DTO.levelIndex++;
        }

        public static void SaveLevel()
        {
            UpdateSaves();
            SaveHelper.SaveToJson(_levelSave_DTO, _levelSaveFolderPath, _levelSaveFileName);
        }

        public static void SaveWeapons()
        {
            UpdateSaves();
            SaveHelper.SaveToJson(_weapons_DTO, _weaponsSaveFolderPath, _weaponsSaveFileName);
        }

        public static void SaveAll()
        {
            UpdateSaves();

            SaveWeapons();
            SaveLevel();
        }

        private static void UpdateSaves()
        {
            if (_levelSave_DTO == null)
            {
                _levelSave_DTO = SaveHelper.GetStoredDataClass<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);

                if (_levelSave_DTO == null)
                {
                    _levelSave_DTO = new LevelSave_DTO();
                }
            }

            if (_weapons_DTO == null)
            {
                _weapons_DTO = SaveHelper.GetStoredDataClass<Weapons_DTO>(_weaponsSaveFolderPath, _weaponsSaveFileName);

                if (_weapons_DTO == null)
                {
                    _weapons_DTO = new Weapons_DTO();
                }
            }
        }
    }
}