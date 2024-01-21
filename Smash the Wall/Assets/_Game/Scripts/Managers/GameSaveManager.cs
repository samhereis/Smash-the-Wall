using DTO.Save;
using Helpers;
using Interfaces;
using UnityEngine;

namespace Managers
{
    public class GameSaveManager : MonoBehaviour, IInitializable
    {
        private const string _levelSaveFolderPath = "Saves";
        private const string _levelSaveFileName = "LevelSave";

        private const string _weaponsSaveFolderPath = "Saves";
        private const string _weaponsSaveFileName = "WeaponsSave";

        [SerializeField] private LevelSave_DTO _levelSave_DTO;
        [SerializeField] private Weapons_DTO _weapons_DTO;

        [ContextMenu(nameof(Initialize))]
        public void Initialize()
        {
            _levelSave_DTO = null;
            _weapons_DTO = null;

            UpdateSaves(true);
        }

        public Weapons_DTO GetWeaponsSave()
        {
            UpdateSaves();
            return _weapons_DTO;
        }

        public LevelSave_DTO GetLevelSave()
        {
            UpdateSaves();

            return _levelSave_DTO;
        }

        public int GetLevelIndex()
        {
            UpdateSaves();
            return _levelSave_DTO.levelIndex;
        }

        public void IncreaseLevelIndex()
        {
            _levelSave_DTO.levelIndex++;
            UpdateSaves();
        }

        public void SaveLevel()
        {
            SaveHelper.SaveToJson(_levelSave_DTO, _levelSaveFolderPath, _levelSaveFileName);
        }

        public void SaveWeapons()
        {
            SaveHelper.SaveToJson(_weapons_DTO, _weaponsSaveFolderPath, _weaponsSaveFileName);
        }

        public void SaveAll()
        {
            UpdateSaves();

            SaveWeapons();
            SaveLevel();
        }

        private void UpdateSaves(bool force = false)
        {
            if (force)
            {
                UpdateSavesForceUpdateSavesForce_LevelSave();
                UpdateSavesForceUpdateSavesForce_Weapons();
            }
            else
            {
                if (_levelSave_DTO == null)
                {
                    UpdateSavesForceUpdateSavesForce_LevelSave();
                }

                if (_weapons_DTO == null)
                {
                    UpdateSavesForceUpdateSavesForce_Weapons();
                }
            }

            void UpdateSavesForceUpdateSavesForce_LevelSave()
            {
                _levelSave_DTO = SaveHelper.GetStoredDataClass<LevelSave_DTO>(_levelSaveFolderPath, _levelSaveFileName);
                if (_levelSave_DTO == null) { _levelSave_DTO = new LevelSave_DTO(); }
            }

            void UpdateSavesForceUpdateSavesForce_Weapons()
            {
                _weapons_DTO = SaveHelper.GetStoredDataClass<Weapons_DTO>(_weaponsSaveFolderPath, _weaponsSaveFileName);
                if (_weapons_DTO == null) { _weapons_DTO = new Weapons_DTO(); }
            }
        }
    }
}