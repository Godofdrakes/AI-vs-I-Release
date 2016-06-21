using System;
using AI_vs_I.Player.Zenject;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


namespace AI_vs_I.Player {

    public class SaveDataBind : MonoBehaviour {
        #region Private Fields

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        #endregion


        #region Inspector Events

        [ SerializeField ]
        [ Tooltip( "Called any time the player's save data is updated." ) ]
        private SaveDataUpdated m_saveDataUpdated = new SaveDataUpdated();

        #endregion


        #region MonoBehaviour Event Functions

        private void Awake() { m_saveData.OnSaveDataChanged += PlayerSaveDataUpdate; }

        #endregion


        #region Methods

        private void PlayerSaveDataUpdate( PlayerSaveData playerSaveData ) {
            m_saveDataUpdated.Invoke( playerSaveData );
        }

        #endregion


        #region Event Defines

        /// <summary>
        ///     Called any time the player's save data is updated.
        /// </summary>
        [ Serializable ]
        public class SaveDataUpdated : UnityEvent<PlayerSaveData> {

        }

        #endregion


        #region Public Functions

        public void DeleteSaveData() {
            Debug.LogFormat( this, "#{0}# Clearing saved data.", typeof( SaveDataBind ) );
            m_saveData.DeleteSaveData();
        }

        public void LoadSaveData() {
            Debug.LogFormat( this, "#{0}# Loading data.", typeof( SaveDataBind ) );
            m_saveData.Load();
        }

        public void SaveSaveData() {
            Debug.LogFormat( this, "#{0}# Saving data.", typeof( SaveDataBind ) );
            m_saveData.Save();
        }

        #endregion
    }

}
