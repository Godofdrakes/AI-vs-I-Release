using AI_vs_I.Player.Zenject;
using SmallTools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace AI_vs_I.Player.UI_Getters {

    /// <summary>
    ///     Handles displaying of the player's name.
    /// </summary>
    [ RequireComponent( typeof( Text ) ) ]
    public class PlayerNameGetter : CachedMonoBehaviour {

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        /// <summary>
        ///     The format that will be pumped into string.Format.
        /// </summary>
        [ SerializeField,
          Tooltip( "The display format. The text '{0}' will be replaced with the current value." ) ]
        private string m_displayFormat = "{0}";

        public Text Text {
            get { return GetCachedComponent<Text>(); }
        }

        private void OnDisable() { m_saveData.OnSaveDataChanged -= SaveDataChanged; }

        private void OnEnable() {
            m_saveData.OnSaveDataChanged += SaveDataChanged;
            SaveDataChanged( m_saveData );
        }

        private void SaveDataChanged( PlayerSaveData playerSaveData ) {
            Text.text = string.Format( m_displayFormat, playerSaveData.PlayerName );
        }

    }

}
