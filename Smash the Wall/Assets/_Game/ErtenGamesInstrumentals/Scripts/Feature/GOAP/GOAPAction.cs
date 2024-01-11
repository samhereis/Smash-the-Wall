using GOAP.GoapDataClasses;
using SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public abstract class GOAPAction : MonoBehaviour
    {
        [field: SerializeField] public GOAPActionBaseSettings baseSettings { get; set; } = new GOAPActionBaseSettings();

        protected virtual void Awake()
        {
            baseSettings.agent = this.gameObject.GetComponent<NavMeshAgent>();

            if (baseSettings.preConditionsInspector != null)
            {
                foreach (GoapState w in baseSettings.preConditionsInspector)
                {
                    baseSettings.preConditions.Add(w.key, w.value);
                }
            }

            if (baseSettings.afterEffectsInspector != null)
            {
                foreach (GoapState w in baseSettings.afterEffectsInspector)
                {
                    baseSettings.afterEffects.Add(w.key, w.value);
                }
            }

            baseSettings.inventory = this.GetComponent<GOAPAgent>().baseSettings.inventory;
            baseSettings.localStates = this.GetComponent<GOAPAgent>().baseSettings.localStates;
        }


        public bool IsAhievableGiven(Dictionary<GOAP_String_SO, int> conditions)
        {
            return baseSettings.preConditions.All(x => conditions.ContainsKey(x.Key));
        }

        public virtual void SetIsRunning(bool isRunning)
        {
            baseSettings.running = isRunning;
        }

        public abstract bool IsAchievable();
        public abstract bool TryBeggin();
        public abstract bool TryComplete();
        public abstract bool IsCompleted();

        public virtual bool IsActionValid()
        {
            return true;
        }

        public virtual void OnActionFail()
        {

        }
    }
}