using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class GameInit : MonoBehaviour
    {
        [Required]
        [SerializeField] private GameObject _spawnOnStart;

        private void Start()
        {
            Instantiate(_spawnOnStart);
        }
    }
}