using System;
using System.Collections.Generic;
using AI_vs_I.Modules;
using AI_vs_I.Player.Events;
using AI_vs_I.Units;
using Newtonsoft.Json;
using Serializers;
using SmallTools;
using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR

#endif

namespace AI_vs_I.Player {

    /// <summary>
    ///     The player's game data.
    /// </summary>
    [ Serializable, JsonObject( MemberSerialization.OptIn ) ]
    [ Obsolete ]
    public class PlayerData {
        #region Static Properties

        /// <summary>
        ///     Singleton access.
        /// </summary>
        public static PlayerData Instance {
            get { return s_instance; }
            set {
                s_instance = value;
                NotifyChange();
            }
        }

        /// <summary>
        ///     Called when any change to the player's data has occured.
        /// </summary>
        public static event UnityAction OnPlayerDataUpdated {
            add { s_playerDataUpdated.AddListener( value ); }
            remove { s_playerDataUpdated.RemoveListener( value ); }
        }

        #endregion


        #region Static Functions

        /// <summary>
        ///     Assigns any save data within <see cref="PlayerPrefs" /> to <see cref="Instance" />.
        /// </summary>
        /// <param name="defaultData">
        ///     Default data to load if no save exists.
        /// </param>
        public static void Load( PlayerData defaultData = null ) {
            Instance = PlayerPrefsHelper.TryLoad( typeof( PlayerData ).Name,
                                                  defaultData ?? new PlayerData() );
        }

        /// <summary>
        ///     Wipes <see cref="Instance" /> and deletes any existing save data within <see cref="PlayerPrefs" />.
        /// </summary>
        public static void Reset() {
            Instance = new PlayerData();
            PlayerPrefs.DeleteKey( typeof( PlayerData ).Name );
        }

        /// <summary>
        ///     Saves all <see cref="PlayerData" /> within <see cref="Instance" /> to <see cref="PlayerPrefs" />.
        /// </summary>
        /// <returns>
        ///     True if the save was successful.
        ///     False otherwise.
        /// </returns>
        public static bool Save() {
            return PlayerPrefsHelper.TrySave( typeof( PlayerData ).Name, Instance );
        }

        /// <summary>
        ///     Notifies all subscribers that changes have been made.
        /// </summary>
        public static void NotifyChange() {
            s_playerDataUpdated.Invoke();
        }

        #endregion


        #region Static Fields

        private static PlayerDataUpdated s_playerDataUpdated = new PlayerDataUpdated();

        private static PlayerData s_instance;

        #endregion


        #region Instance Fields

        [ SerializeField, JsonProperty ]
        private string m_playerName = typeof( PlayerData ).Name;

        [ SerializeField, JsonProperty ]
        [ JsonConverter( typeof( UnityResourceConverter ) ) ]
        private Sprite m_playerSprite;

        [ SerializeField, JsonProperty ]
        [ JsonConverter( typeof( ColorConverter ) ) ]
        private Color m_playerColor = Color.green;

        [ SerializeField, JsonProperty ]
        [ JsonConverter( typeof( ColorConverter ) ) ]
        private Color m_enemyColor = Color.red;

        [ SerializeField, JsonProperty ]
        private List<BaseUnitModule> m_modules = new List<BaseUnitModule>();

        [ SerializeField ]
        private List<UnitDefinition> m_unitDefinitions;

        [ SerializeField ]
        private int m_money;

        #endregion


        #region Instance Properties

        public string PlayerName {
            get { return m_playerName; }
            set {
                m_playerName = value;
                s_playerDataUpdated.Invoke();
            }
        }

        /// <summary>
        ///     The Player's <see cref="Sprite" /> avatar.
        /// </summary>
        public Sprite PlayerSprite {
            get { return m_playerSprite; }
            set {
                m_playerSprite = value;
                s_playerDataUpdated.Invoke();
            }
        }

        /// <summary>
        ///     The color that represents the player.
        /// </summary>
        public Color PlayerColor {
            get { return m_playerColor; }
            set {
                m_playerColor = value;
                s_playerDataUpdated.Invoke();
            }
        }

        /// <summary>
        ///     The color that represents the enemy.
        /// </summary>
        public Color EnemyColor {
            get { return m_enemyColor; }
            set {
                m_enemyColor = value;
                s_playerDataUpdated.Invoke();
            }
        }

        /// <summary>
        ///     The player's inventory of modules.
        /// </summary>
        public List<BaseUnitModule> Modules {
            get { return m_modules; }
            set {
                m_modules = value;
                s_playerDataUpdated.Invoke();
            }
        }

        /// <summary>
        ///     THe player's inventory of Units.
        /// </summary>
        public List<UnitDefinition> UnitDefinitions {
            get { return m_unitDefinitions; }
            set {
                m_unitDefinitions = value;
                s_playerDataUpdated.Invoke();
            }
        }

        public int Money {
            get { return m_money; }
            set {
                m_money = value;
                s_playerDataUpdated.Invoke();
            }
        }

        #endregion
    }

}
