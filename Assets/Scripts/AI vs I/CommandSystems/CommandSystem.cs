using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AI_vs_I.CommandSystems.Commands;
using AI_vs_I.Units;
using Newtonsoft.Json;
using UnityEngine;


namespace AI_vs_I.CommandSystems {

    public class CommandSystem : MonoBehaviour {
        #region Instance Fields

        private List<BaseCommand> m_commands = new List<BaseCommand>();

        #endregion


        #region Properties

        public ReadOnlyCollection<BaseCommand> CommandHistory {
            get { return m_commands.AsReadOnly(); }
        }

        #endregion


        #region Functions

        public void RebuildHistory( string json ) {
            Debug.Log( "#CommandSystem# Loading history from json." );
            LoadHistoryFromJson( json );
            Debug.Log( "#CommandSystem# Rebuilding game state from history." );
            foreach( BaseCommand command in m_commands ) { command.Do(); }

            Debug.Log( "#CommandSystem# Updating game view." );
            foreach( IAlteredUnits unitList in CommandHistory.OfType<IAlteredUnits>() ) {
                foreach( UnitInstance unit in unitList.AlteredUnits ) { unit.DrawBody(); }
            }
        }

        public Coroutine SlowRebuildHistory( string json, float waitTime ) {
            StopCoroutine( "CorSlowRebuildHistory" );
            return StartCoroutine( CorSlowRebuildHistory( json, waitTime ) );
        }

        private IEnumerator CorSlowRebuildHistory( string json, float waitTime ) {
            LoadHistoryFromJson( json );
            foreach( BaseCommand command in m_commands ) {
                command.Do();

                IAlteredUnits altered = command as IAlteredUnits;
                if( altered != null ) {
                    foreach( UnitInstance unit in altered.AlteredUnits ) { unit.DrawBody(); }
                }

                yield return new WaitForSeconds( waitTime );
            }
        }

        /*public void ReplayHistory(string json)
        {
            Debug.Log("#CommandSystem# Loading history from json.");
            LoadHistoryFromJson(json);


        }*/

        public void AddCommand( BaseCommand command ) {
            m_commands.Add( command );
            /*Debug.LogFormat( "#CommandSystem# Added {0} to history.",
                             command.GetType()
                                    .Name );*/
        }

        #endregion


        #region Json Conversion

        private JsonSerializerSettings m_jsonSerializerSettings =
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public string SaveHistoryToJson( Formatting formatting = Formatting.None ) {
            return JsonConvert.SerializeObject( m_commands,
                                                formatting,
                                                m_jsonSerializerSettings );
        }

        public void LoadHistoryFromJson( string json ) {
            m_commands = JsonConvert.DeserializeObject<List<BaseCommand>>( json,
                                                                           m_jsonSerializerSettings );
        }

        #endregion
    }

}
