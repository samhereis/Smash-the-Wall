using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class EventBasedSound
    {
        [field: SerializeField] public string eventName { get; private set; }
        [field: SerializeField] public SoundBase sound { get; set; } = new SoundBase();

        public EventBasedSound(string newEventName)
        {
            eventName = newEventName;
        }
    }
}