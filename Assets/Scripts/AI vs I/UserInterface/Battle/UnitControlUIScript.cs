using System.Collections.Generic;
using AI_vs_I.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AI_vs_I.UserInterface
{

    public class UnitControlUIScript : MonoBehaviour
    {
        Button[] buttons;

        UnitController controlleRef;

        void Start()
        {
            buttons = GetComponentsInChildren<Button>();

            controlleRef = FindObjectOfType<UnitController>();
        }


        void Update()
        {
            if (controlleRef.SelectedInstance != null)
            {
                foreach (Button i in buttons)
                {
                    i.gameObject.SetActive(true);
                }
                GetButton("End Turn").gameObject.SetActive(false);
                // ======
                if (controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(1) != null)
                {
                    GetButton("Act 1").gameObject.SetActive(true);
                    GetButton("Act 1").GetComponentInChildren<Text>().text = controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(1).ModName;
                }
                else
                {
                    GetButton("Act 1").gameObject.SetActive(false);
                    GetButton("Act 1").GetComponentInChildren<Text>().text = "null";
                }
                // ======
                if (controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(2) != null)
                {
                    GetButton("Act 2").gameObject.SetActive(true);
                    GetButton("Act 2").GetComponentInChildren<Text>().text = controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(2).ModName;
                }
                else
                {
                    GetButton("Act 2").gameObject.SetActive(false);
                    GetButton("Act 2").GetComponentInChildren<Text>().text = "null";
                }
                // ======
                if (controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(3) != null)
                {
                    GetButton("Act 3").gameObject.SetActive(true);
                    GetButton("Act 3").GetComponentInChildren<Text>().text = controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(3).ModName;
                }
                else
                {
                    GetButton("Act 3").gameObject.SetActive(false);
                    GetButton("Act 3").GetComponentInChildren<Text>().text = "null";
                }
                // ======
                if (controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(4) != null)
                {
                    GetButton("Act 4").gameObject.SetActive(true);
                    GetButton("Act 4").GetComponentInChildren<Text>().text = controlleRef.SelectedInstance.Definition.GetActionModuleInclusive(4).ModName;
                }
                else
                {
                    GetButton("Act 4").gameObject.SetActive(false);
                    GetButton("Act 4").GetComponentInChildren<Text>().text = "null";
                }
                // ======
            }
            else
            {
                foreach (Button i in buttons)
                {
                    i.gameObject.SetActive(false);
                }
                GetButton("End Turn").gameObject.SetActive(true);
            }
        }

        Button GetButton(string name)
        {
            foreach (Button i in buttons)
            {
                if (i.name == name)
                {
                    return i;
                }
            }
            return null;
        }
    }

}