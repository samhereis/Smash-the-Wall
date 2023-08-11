using DI;
using Displayers;
using Helpers;
using Identifiers;
using InGameStrings;
using Interfaces;
using PlayerInputHolder;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IInitializable, IHasInput
    {
        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] protected Input_SO _input;
        [DI(DIStrings.trajectoryDisplayer)][SerializeField] protected TrajectoryDisplayer _trajectoryDisplayer;

        [Header("Addressables")]
        [SerializeField] AssetReferenceGameObject _mesh;

        [Header("Components")]
        [SerializeField] protected Transform _gunPoint;
        [field: SerializeField] public Transform shootPosition { get; protected set; }

        [field: SerializeField, Header("Current State")] public bool canShoot { get; protected set; }

        [Header("Debug")]
        [SerializeField] private WeaponMeshIdentifier _weaponMesh;

        private void OnDisable()
        {
            _trajectoryDisplayer.HideTrajectory();
        }

        private void OnDestroy()
        {
            Clear();
        }

        public virtual void Initialize()
        {
            ChangeWeaponMesh();
        }

        public abstract void EnableInput();
        public abstract void DisableInput();
        protected abstract void Fire(InputAction.CallbackContext context);
        public abstract void OnFired();

        private async void ChangeWeaponMesh()
        {
            Clear();

            if (_mesh == null) { return; }

            _weaponMesh = await AddressablesHelper.InstantiateAsync<WeaponMeshIdentifier>(_mesh, parent: transform);

            if (_weaponMesh != null)
            {
                _weaponMesh.transform.parent = transform;
                _weaponMesh.transform.localPosition = Vector3.zero;
                _weaponMesh.transform.localRotation = Quaternion.identity;
            }
        }

        public void Clear()
        {
            Delete(_weaponMesh);

            foreach (var weaponMesh in FindObjectsOfType<WeaponMeshIdentifier>(true))
            {
                Delete(weaponMesh);
            }

            void Delete(WeaponMeshIdentifier weaponMesh)
            {
                if (weaponMesh == null) { return; }

                try
                {
                    AddressablesHelper.DestroyObject(weaponMesh.gameObject);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning("Could not delete weaponm esh: " + exception.Message);
                }
            }
        }


        public override string ToString()
        {
            return gameObject.name;
        }
    }
}