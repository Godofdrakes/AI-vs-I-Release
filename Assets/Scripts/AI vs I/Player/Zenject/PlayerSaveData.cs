using System;
using System.Collections.Generic;
using AI_vs_I.Modules;
using AI_vs_I.Units;
using Newtonsoft.Json;
using Serializers;
using UnityEngine;
using Zenject;


namespace AI_vs_I.Player.Zenject {

    [ Serializable, JsonObject( MemberSerialization.OptIn ) ]
    public class PlayerSaveData : IDisposable {

        public PlayerSaveData() {
            PlayerName = typeof( PlayerSaveData ).Name;
            PlayerColor = Color.green;
            EnemyColor = Color.red;
            Modules = new List<BaseUnitModule>();
            UnitDefinitions = new List<UnitDefinition>();
            Money = 1000;
            m_serializerSettings = new JsonSerializerSettings();
            m_serializerFormatting = Formatting.None;
            m_onSaveDataChanged = null;
        }

        public static string PrefsKey {
            get { return typeof( PlayerSaveData ).Name; }
        }

        public static bool SaveDataExists {
            get { return PlayerPrefs.HasKey( PrefsKey ); }
        }

        public override string ToString() {
            return JsonConvert.SerializeObject( this, m_serializerFormatting, m_serializerSettings );
        }


        #region Public Methods

        [ PostInject ]
        public void Load() {
            if( SaveDataExists ) {
                string json = PlayerPrefs.GetString( PrefsKey );
                if( !string.IsNullOrEmpty( json ) ) {
                    try {
                        JsonConvert.PopulateObject( json, this, m_serializerSettings );
                    }
                    catch( Exception e ) {
                        Debug.LogException( e );
                        Debug.LogErrorFormat( "Failed to deserialize Json:\n{0}", json );
                        throw;
                    }

                    Debug.LogFormat( "#{0}# Loaded.", typeof( PlayerSaveData ).Name );
                }

                NotifyChanges();
            }
            else {
                Debug.LogWarningFormat( "#{0}# No save data exists. " +
                                        "This is normal for a first run.",
                                        typeof( PlayerSaveData ).Name );
            }
        }

        public void Save() {
            string json = JsonConvert.SerializeObject( this,
                                                       m_serializerFormatting,
                                                       m_serializerSettings );
            PlayerPrefs.SetString( PrefsKey, json );
            Debug.LogFormat( "#{0}# Saved", typeof( PlayerSaveData ).Name );
        }

        public void DeleteSaveData() {
            PlayerPrefs.DeleteKey( PrefsKey );
            NotifyChanges();
        }

        public void Dispose() { Save(); }

        /// <summary>
        ///     Notifys all subscribers that a change has occured.
        /// </summary>
        public void NotifyChanges() {
            if( m_onSaveDataChanged == null ) { return; }

            m_onSaveDataChanged.Invoke( this );
        }

        #endregion


        #region Private Fields

        [ Inject ]
        private JsonSerializerSettings m_serializerSettings;

        [ Inject ]
        private Formatting m_serializerFormatting;

        private Action<PlayerSaveData> m_onSaveDataChanged;

        [ InjectOptional( "Default Player Name" ) ]
        [ SerializeField, JsonProperty ]
        private string m_playerName;

        [ InjectOptional( "Default Player Color" ) ]
        [ SerializeField, JsonProperty ]
        [ JsonConverter( typeof( ColorConverter ) ) ]
        private Color m_playerColor;

        [ InjectOptional( "Default Enemy Color" ) ]
        [ SerializeField, JsonProperty ]
        [ JsonConverter( typeof( ColorConverter ) ) ]
        private Color m_enemyColor;

        [ InjectOptional( "Default Module Inventory" ) ]
        [ SerializeField, JsonProperty ]
        private List<BaseUnitModule> m_modules;

        [ InjectOptional( "Default Unit Inventory" ) ]
        [ SerializeField, JsonProperty ]
        private List<UnitDefinition> m_unitDefinitions;

        [ InjectOptional( "Default Money" ) ]
        [ SerializeField, JsonProperty ]
        private int m_money;

        #endregion


        #region Properties

        public string PlayerName {
            get { return m_playerName; }
            set { m_playerName = value; }
        }

        /// <summary>
        ///     The color that represents the player.
        /// </summary>
        public Color PlayerColor {
            get { return m_playerColor; }
            set { m_playerColor = value; }
        }

        /// <summary>
        ///     The color that represents the enemy.
        /// </summary>
        public Color EnemyColor {
            get { return m_enemyColor; }
            set { m_enemyColor = value; }
        }

        /// <summary>
        ///     The player's inventory of modules.
        /// </summary>
        public List<BaseUnitModule> Modules {
            get { return m_modules; }
            set { m_modules = value; }
        }

        /// <summary>
        ///     The player's inventory of Units.
        /// </summary>
        public List<UnitDefinition> UnitDefinitions {
            get { return m_unitDefinitions; }
            set { m_unitDefinitions = value; }
        }

        public int Money {
            get { return m_money; }
            set { m_money = value; }
        }

        public event Action<PlayerSaveData> OnSaveDataChanged {
            add { m_onSaveDataChanged += value; }
            remove { m_onSaveDataChanged -= value; }
        }

        #endregion
    }

}
