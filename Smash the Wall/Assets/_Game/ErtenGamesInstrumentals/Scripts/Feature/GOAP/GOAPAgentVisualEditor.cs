using GOAP.GoapDataClasses;
using SO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GOAP
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GOAPAgentVisual))]
    [CanEditMultipleObjects]
    public class GOAPAgentVisualEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            serializedObject.Update();

            GOAPAgentVisual agent = (GOAPAgentVisual)target;
            GUILayout.Label("Name: " + agent.name);
            GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GOAPAgent>().baseSettings.currentAction);
            GUILayout.Label("Actions: ");
            foreach (GOAPAction a in agent.gameObject.GetComponent<GOAPAgent>().baseSettings.actions)
            {
                string pre = "";
                string eff = "";

                foreach (KeyValuePair<GOAP_String_SO, int> p in a.baseSettings.preConditions)
                {
                    pre += p.Key + ", ";
                }

                foreach (KeyValuePair<GOAP_String_SO, int> e in a.baseSettings.afterEffects)
                {
                    eff += e.Key + ", ";
                }

                GUILayout.Label("====  " + a.baseSettings.actionName + "(" + pre + ")(" + eff + ")");
            }

            GUILayout.Label("Goals: ");

            foreach (KeyValuePair<SubGoals, int> g in agent.gameObject.GetComponent<GOAPAgent>().baseSettings.goals)
            {
                foreach (KeyValuePair<GOAP_String_SO, int> sg in g.Key.subGoals)
                    GUILayout.Label("=====  " + sg.Key);
            }

            GUILayout.Label("Beliefs: ");

            foreach (KeyValuePair<GOAP_String_SO, int> sg in agent.gameObject.GetComponent<GOAPAgent>().baseSettings.localStates.GetStates())
            {
                GUILayout.Label("=====  " + sg.Key);
            }

            GUILayout.Label("Inventory: ");

            foreach (GameObject g in agent.gameObject.GetComponent<GOAPAgent>().baseSettings.inventory.items)
            {
                GUILayout.Label("====  " + g.tag);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}