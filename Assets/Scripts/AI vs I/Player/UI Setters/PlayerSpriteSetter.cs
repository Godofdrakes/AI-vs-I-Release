using System;
using AI_vs_I.Player.Zenject;
using SmallTools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace AI_vs_I.Player.UI_Setters {

    [ RequireComponent( typeof( Image ) ) ]
    public class PlayerSpriteSetter : CachedMonoBehaviour {

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        public Sprite Sprite {
            get { return GetCachedComponent<Image>().sprite; }
        }

        public void SetPlayerSprite() { throw new NotImplementedException(); }

    }

}
