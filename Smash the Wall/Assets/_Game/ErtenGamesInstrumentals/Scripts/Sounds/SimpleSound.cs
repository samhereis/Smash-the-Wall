using Sirenix.OdinInspector;
using SO;
using System;

namespace Sound
{
    [Serializable]
    public class SimpleSound : SoundBase
    {
        [ShowInInspector] public AString_SO soundName { get; private set; }
    }
}