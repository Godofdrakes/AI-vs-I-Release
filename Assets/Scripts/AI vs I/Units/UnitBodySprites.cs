using UnityEngine;


namespace AI_vs_I.Units {

    [ CreateAssetMenu( fileName = "NewBodySprites", menuName = "AI vs I/Units/BodySprites" ) ]
    public class UnitBodySprites : ScriptableObject {

        [ SerializeField ]
        private Sprite[] m_sprites = new Sprite[16];

        public Sprite[] Sprites {
            get { return m_sprites; }
        }

    }

}
