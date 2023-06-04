using UnityEngine;
using Weapons;

namespace Spawners
{
    public class GunSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private WeaponBase _weapon;

        private void Awake()
        {
            var pictureInstance = Instantiate(_weapon, _parent);

            pictureInstance.transform.localPosition= Vector3.zero;
        }
    }
}
