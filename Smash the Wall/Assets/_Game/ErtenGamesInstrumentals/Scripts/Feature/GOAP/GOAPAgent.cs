using GOAP.GoapDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class GOAPAgent : MonoBehaviour
    {
        [field: SerializeField] public GAgentBaseSettings baseSettings { get; protected set; } = new GAgentBaseSettings();

        protected virtual void Start()
        {
            GOAPAction[] acts = this.GetComponents<GOAPAction>();
            foreach (GOAPAction a in acts) baseSettings.actions.Add(a);
        }

        private void LateUpdate()
        {
            bool currentActionExistsAndRunning = baseSettings.currentAction != null && baseSettings.currentAction.baseSettings.running;

            if (currentActionExistsAndRunning)
            {
                TryCompleteAction();
                return;
            }

            bool plannerAndActionQueueEmpty = baseSettings.planner == null || baseSettings.actionQueue == null;

            if (plannerAndActionQueueEmpty)
            {
                CreatePlannerAndPopulateActionGoals();
            }

            bool actionQueueIsEmpty = baseSettings.actionQueue != null && baseSettings.actionQueue.Count == 0;

            if (actionQueueIsEmpty)
            {
                if (baseSettings.currentGoal.remove)
                {
                    baseSettings.goals.Remove(baseSettings.currentGoal);
                }

                baseSettings.planner = null;
            }

            bool actionQueueExists = baseSettings.actionQueue != null && baseSettings.actionQueue.Count > 0;

            if (actionQueueExists)
            {
                TryBegginNewAction();
            }
        }

        private void TryCompleteAction()
        {
            if (baseSettings.currentAction.TryComplete())
            {
                if (baseSettings.currentAction.IsCompleted())
                {
                    baseSettings.currentAction.SetIsRunning(false);
                    baseSettings.invoked = false;
                }
            }
        }

        private void TryBegginNewAction()
        {
            baseSettings.currentAction = baseSettings.actionQueue.Dequeue();

            if (baseSettings.currentAction.TryBeggin())
            {
                if (baseSettings.currentAction.baseSettings.target != null)
                {
                    baseSettings.currentAction.SetIsRunning(true);

                    baseSettings.destination = baseSettings.currentAction.baseSettings.target.transform.position;
                    Transform dest = baseSettings.currentAction.baseSettings.target.transform.Find("Destination");
                    if (dest != null) baseSettings.destination = dest.position;
                    if (baseSettings.currentAction.baseSettings.agent.destination != baseSettings.destination) baseSettings.currentAction.baseSettings.agent.SetDestination(baseSettings.destination);
                }
            }
            else
            {
                baseSettings.actionQueue = null;
            }
        }

        private void CreatePlannerAndPopulateActionGoals()
        {
            baseSettings.planner = new GOAPPlanner();

            var sortedGoals = from entry in baseSettings.goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoals, int> sortedGoal in sortedGoals)
            {
                baseSettings.actionQueue = baseSettings.planner.Plan(baseSettings.actions, sortedGoal.Key.subGoals, baseSettings.localStates);

                if (baseSettings.actionQueue != null)
                {
                    baseSettings.currentGoal = sortedGoal.Key;
                    break;
                }
            }
        }
    }
}