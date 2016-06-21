using Newtonsoft.Json;
using Serializers;
using UnityEngine;


namespace AI_vs_I.Units {

    [ JsonConverter( typeof( UnityResourceConverter ) ) ]
    [ CreateAssetMenu( fileName = "NewUnitArtBundle", menuName = "AI vs I/Units/Art Bundle" ) ]
    public class UnitArtBundle : ScriptableObject {

        [ SerializeField ]
        private Sprite m_coreSprite = null;

        [ SerializeField ]
        private RuntimeAnimatorController m_animator = null;

        [ SerializeField ]
        private Sprite m_bodySprite = null;

        [ SerializeField ]
        private UnitBodySprites m_bodySprites = null;

        public Sprite CoreSprite {
            get { return m_coreSprite; }
        }

        public RuntimeAnimatorController Animator {
            get { return m_animator; }
        }

        public Sprite BodySprite {
            get { return m_bodySprite; }
        }

        public UnitBodySprites BodySprites {
            get { return m_bodySprites; }
        }

    }

}
