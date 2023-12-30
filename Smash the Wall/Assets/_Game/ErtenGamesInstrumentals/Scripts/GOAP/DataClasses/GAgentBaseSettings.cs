using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.GoapDataClasses
{
    [Serializable]
    public class GAgentBaseSettings
    {
        public List<GOAPAction> actions { get => _actions; set { _actions = value; } }
        public GOAPInventory inventory { get => _inventory; set { _inventory = value; } }
        public GoapStates localStates { get => _localStates; set { _localStates = value; } }
        public GOAPAction currentAction { get => _currentAction; set { _currentAction = value; } }
        public SubGoals currentGoal { get => _currentGoal; set { _currentGoal = value; } }
        public Vector3 destination { get => _destination; set { _destination = value; } }
        public bool invoked { get => _invoked; set { _invoked = value; } }

        public Dictionary<SubGoals, int> goals { get => _goals; set { _goals = value; } }

        public GOAPPlanner planner { get => _planner; set { _planner = value; } }
        public Queue<GOAPAction> actionQueue { get => _actionQueue; set { _actionQueue = value; } }

        [FoldoutGroup("Debug"), SerializeField] private List<GOAPAction> _actions = new List<GOAPAction>();
        [FoldoutGroup("Debug"), SerializeField] private GOAPInventory _inventory = new GOAPInventory();
        [FoldoutGroup("Debug"), SerializeField] private GoapStates _localStates = new GoapStates();
        [FoldoutGroup("Debug"), SerializeField] private GOAPAction _currentAction;
        [FoldoutGroup("Debug"), SerializeField] private SubGoals _currentGoal;
        [FoldoutGroup("Debug"), SerializeField] private Vector3 _destination = Vector3.zero;
        [FoldoutGroup("Debug"), SerializeField] private bool _invoked = false;

        [SerializeField] private Dictionary<SubGoals, int> _goals = new Dictionary<SubGoals, int>();

        private GOAPPlanner _planner;
        private Queue<GOAPAction> _actionQueue;
    }
}