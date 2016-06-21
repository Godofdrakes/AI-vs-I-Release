using AI_vs_I.Player.Zenject;
using SmallTools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace AI_vs_I.Player.UI_Setters {

    /// <summary>
    ///     Handles updating the player's chosen color.
    /// </summary>
    [ RequireComponent( typeof( Image ) ) ]
    public class PlayerColorSetter : CachedMonoBehaviour {

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        /// <summary>
        ///     Is this setting the player's color?
        /// </summary>
        [ SerializeField,
          Tooltip( "Is this setting the player's color (true) or the enemy's color (false)." ) ]
        private bool m_playerColor = true;

        public Color Color {
            get { return GetCachedComponent<Image>().color; }
        }

        public void SetColor() {
            if( m_playerColor ) { m_saveData.PlayerColor = Color; }
            else { m_saveData.EnemyColor = Color; }
            m_saveData.NotifyChanges();
        }

    }

}
