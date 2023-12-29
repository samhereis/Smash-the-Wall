using SO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP.GoapDataClasses
{
    [Serializable]
    public class GOAPActionBaseSettings
    {
        // TODO: Refactor all GOAP scripts

        public NavMeshAgent agent { get => _agent; set { _agent = value; } }

        public AString_SO actionName => _actionName;
        public float cost => _cost;
        public float duration => _duration;

        public GoapState[] preConditionsInspector => _preConditionsInspector;
        public GoapState[] afterEffectsInspector => _afterEffectsInspector;
        public GoapStates localStates { get => _localStates; set { _localStates = value; } }

        public bool running { get => _running; set { _running = value; } }
        public GOAPInventory inventory { get => _inventory; set { _inventory = value; } }
        public GameObject target { get => _target; set { _target = value; } }

        public Dictionary<AString_SO, int> preConditions { get => _preConditions; set { _preConditions = value; } }
        public Dictionary<AString_SO, int> afterEffects { get => _afterEffects; set { _afterEffects = value; } }

        [Header("Components")]
        [SerializeField] private NavMeshAgent _agent;

        [Header("Settings")]
        [SerializeField] private AString_SO _actionName;
        [SerializeField] private float _cost = 1.0f;
        [SerializeField] private float _duration = 0.0f;

        [Header("Goap States")]
        [SerializeField] private GoapState[] _preConditionsInspector;
        [SerializeField] private GoapState[] _afterEffectsInspector;
        [SerializeField] private GoapStates _localStates;

        [Header("Debug")]
        [SerializeField] private bool _running = false;
        [SerializeField] private GOAPInventory _inventory = new GOAPInventory();
        [SerializeField] private GameObject _target;

        private Dictionary<AString_SO, int> _preConditions = new Dictionary<AString_SO, int>();
        private Dictionary<AString_SO, int> _afterEffects = new Dictionary<AString_SO, int>();
    }
}