using Newtonsoft.Json;
using Serializers;
using UnityEngine;


namespace AI_vs_I.Modules {

    [ CreateAssetMenu( fileName = "NewCoreModule", menuName = "AI vs I/Units/Core" ) ]
    public class CoreModule : BaseUnitModule {

        [ SerializeField ]
        private Sprite m_unitCoreSprite = null;

        /// <summary>
        ///     The Core Sprite for the unit in game.
        /// </summary>
        public Sprite UnitCoreSprite {
            get { return m_unitCoreSprite; }
        }

    }

}
