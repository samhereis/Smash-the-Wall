using Sirenix.OdinInspector;
using SO;
using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class SimpleSound : SoundBase
    {
        [SerializeField] public AString_SO soundName { get; private set; }
    }
}