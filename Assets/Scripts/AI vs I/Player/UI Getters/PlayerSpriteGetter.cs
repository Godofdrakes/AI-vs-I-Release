using System;
using AI_vs_I.Player.Zenject;
using SmallTools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace AI_vs_I.Player.UI_Getters {

    /// <summary>
    ///     Handles display of the player's chosen <see cref="Sprite" /> avatar.
    /// </summary>
    [ RequireComponent( typeof( Image ) ) ]
    public class PlayerSpriteGetter : CachedMonoBehaviour {

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        [ SerializeField ]
        private Sprite m_fallback = null;

        public Image Image {
            get { return GetCachedComponent<Image>(); }
        }

        private void OnDisable() { m_saveData.OnSaveDataChanged -= SaveDataChanged; }

        private void OnEnable() {
            m_saveData.OnSaveDataChanged += SaveDataChanged;
            SaveDataChanged( m_saveData );
        }

        private void SaveDataChanged( PlayerSaveData playerSaveData ) {
            // TODO
            Image.sprite = m_fallback;
            throw new NotImplementedException();
        }

    }

}
