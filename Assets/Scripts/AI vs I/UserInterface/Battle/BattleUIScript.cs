using System.Collections.Generic;
using AI_vs_I.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;


namespace AI_vs_I.UserInterface
{

    public class BattleUIScript : MonoBehaviour
    {
        UnitControlUIScript unitControl;
        SpawnMenuUIScript spawnMenu;

        EndBattleWindow victory;
        EndBattleWindow defeat;

        void Start()
        {
            unitControl = GetComponentInChildren<UnitControlUIScript>();
            spawnMenu = GetComponentInChildren<SpawnMenuUIScript>();

            victory = GetComponentsInChildren<EndBattleWindow>().Where(win => win.gameObject.name == "Victory").FirstOrDefault();
            defeat = GetComponentsInChildren<EndBattleWindow>().Where(win => win.gameObject.name == "Defeat").FirstOrDefault();
        }

        // Update is called once per frame
        void Update()
        {
            if (FindObjectOfType<UnitController>().CurrentBattleState == UnitController.BattleState.Spawning)
            {
                spawnMenu.gameObject.SetActive(true);
            }
            else
            {
                spawnMenu.gameObject.SetActive(false);
            }

            if (FindObjectOfType<UnitController>().CurrentBattleState == UnitController.BattleState.TurnOne 
                /*&& FindObjectOfType<UnitController>().SelectedInstance != null*/)
            {
                unitControl.gameObject.SetActive(true);
            }
            else
            {
                unitControl.gameObject.SetActive(false);
            }

            if (FindObjectOfType<UnitController>().CurrentBattleState == UnitController.BattleState.Victory)
            {
                victory.gameObject.SetActive(true);
            }
            else
            {
                victory.gameObject.SetActive(false);
            }

            if (FindObjectOfType<UnitController>().CurrentBattleState == UnitController.BattleState.Defeat)
            {
                defeat.gameObject.SetActive(true);
            }
            else
            {
                defeat.gameObject.SetActive(false);
            }
        }
    }

}