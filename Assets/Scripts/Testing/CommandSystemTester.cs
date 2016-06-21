using AI_vs_I;
using AI_vs_I.CommandSystems;
using AI_vs_I.Player;
using AI_vs_I.Units;
using UnityEngine;


namespace Testing {

    [ RequireComponent( typeof( CommandSystem ) ) ]
    public class CommandSystemTester : MonoBehaviour {

        public CommandSystem CommandSystem;

        public bool ResetSave;

        public string SaveKey = "commandSystemTest000";

        public bool SlowRebuild;

        private void OnDisable() {
            PlayerPrefs.SetString( SaveKey, CommandSystem.SaveHistoryToJson() );
        }

        private void OnEnable() { }

        private void Reset() { CommandSystem = GetComponent<CommandSystem>(); }

        private void Start() {
            /*UnitController.Instance
                          .AddTestInstance( UnitController.Instance
                                                          .GGGrid.GetCell( new Vector3( 2, -4, 0 ) ),
                                            Players.One );
            UnitController.Instance
                          .AddTestInstance( UnitController.Instance
                                                          .GGGrid.GetCell( new Vector3( -3, -4, 0 ) ),
                                            Players.One );

            UnitController.Instance
                          .AddTestInstance( UnitController.Instance
                                                          .GGGrid.GetCell( new Vector3( 2, 4, 0 ) ),
                                            Players.Two );
            UnitController.Instance
                          .AddTestInstance( UnitController.Instance
                                                          .GGGrid.GetCell( new Vector3( -3, 4, 0 ) ),
                                            Players.Two );*/
            //UnitController.Instance.AddTestInstance(UnitController.Instance.GGGrid.GetCell(new Vector3(-5, 0, 0)), Players.Two);

            if( ResetSave ) {
                PlayerPrefs.DeleteKey( SaveKey );
            }
            else if( PlayerPrefs.HasKey( SaveKey ) ) {
                string json = PlayerPrefs.GetString( SaveKey );
                if( SlowRebuild ) {
                    CommandSystem.SlowRebuildHistory( json, 0.5f );
                }
                else {
                    CommandSystem.RebuildHistory( json );
                }
            }
        }

    }

}
