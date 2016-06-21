using AI_vs_I.Player.Zenject;
using UnityEngine;
using Zenject;


namespace AI_vs_I.Player.UI_Setters {

    public class PlayerNameSetter : MonoBehaviour {

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        public void SetName( string playerName ) {
            m_saveData.PlayerName = playerName;
            m_saveData.NotifyChanges();
        }

    }

}
