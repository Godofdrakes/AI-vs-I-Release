using System.Collections.Generic;
using AI_vs_I.Units;
using AI_vs_I.Modules;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class UnitInspectorUIScript : MonoBehaviour {

    private Text basicInfo;
    private Text actionInfo;
    private Image window;

    public UnitInstance inspectTarget = null;
    //private string basicInfoString;

	// Use this for initialization
	void Start () {

        basicInfo = GetComponentsInChildren<Text>().Where(win => win.gameObject.name == "Basic").FirstOrDefault();
        actionInfo = GetComponentsInChildren<Text>().Where(win => win.gameObject.name == "Actions").FirstOrDefault();
        window = GetComponentInChildren<Image>();

        window.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	    
        if (inspectTarget == null)
        {
            window.gameObject.SetActive(false);
        }
        else
        {
            window.gameObject.SetActive(true);
            basicInfo.text = inspectTarget.Definition.Name;
            if (inspectTarget.CurrentStatus != StatusEffect.Normal)
            {
                basicInfo.text += " (" + inspectTarget.CurrentStatus + " " + inspectTarget.StatusDuration + ")";
            }
            basicInfo.text += "\nMax Size: " + inspectTarget.MaxHealth;
            basicInfo.text += "\nMove: " + inspectTarget.MaxMove;
            basicInfo.text += "\nActions:";

            actionInfo.text = "";
            foreach (ActionModule i in inspectTarget.Definition.ActionModules)
            {
                actionInfo.text += i.name + ":\n";
                if (i.TargetEffects.Any())
                {
                    foreach (ActionEffect j in i.TargetEffects)
                    {
                        actionInfo.text += ActionEffectDescription(j);
                    }
                    actionInfo.text += " to target. ";
                }
                if (i.UserEffects.Any())
                {
                    foreach (ActionEffect j in i.UserEffects)
                    {
                        actionInfo.text += ActionEffectDescription(j);
                    }
                    actionInfo.text += " to user.";
                }
                actionInfo.text += "Range " + i.RangeValue + ".\n";
            }
        }
	}

    string ActionEffectDescription(ActionEffect effect)
    {
        string ret = "";

        switch (effect.EffectType)
        {
            default:
                ret += effect.EffectStrength + " " + effect.EffectType.ToString();
                break;

            case ActionEffectType.SpeedBoost:
            case ActionEffectType.HPBoost:
                ret += effect.EffectStrength + " " + effect.EffectType.ToString() + " for " + effect.EffectDuration + " turns";
                break;

            case ActionEffectType.Corrupt:
            case ActionEffectType.Freeze:
                ret += effect.EffectType.ToString() + " for " + effect.EffectDuration + " turns";
                break;
        }
        ret += ", ";

        return ret;
    }
}
