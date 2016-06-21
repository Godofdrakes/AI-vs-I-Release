using Newtonsoft.Json;
using Serializers;
using UnityEngine;


namespace AI_vs_I.Modules {

    [ JsonConverter( typeof( UnityResourceConverter ) ) ]
    public class BaseUnitModule : ScriptableObject {
        #region Inspector Fields

        [ SerializeField, Tooltip( "The point cost of this module." ) ]
        private int m_moduleCost = 1;

        [ SerializeField, Tooltip( "The sprite displayed to represent this module in a PGIView." ) ]
        private Sprite m_moduleIcon = null;

        [ SerializeField, Tooltip( "The height of this module when displayed in a PGIView." ) ]
        private int m_inventoryHeight = 1;

        [ SerializeField, Tooltip( "The width of this module when displayed in a PGIView." ) ]
        private int m_inventoryWidth = 1;

        [ SerializeField, Tooltip( "The user friendly name of this module." ) ]
        private string m_name = string.Empty;

        #endregion


        #region Properties

        /// <summary>
        ///     The width of this module when displayed in a PGIView.
        /// </summary>
        public int InventoryWidth {
            get { return m_inventoryWidth; }
        }

        /// <summary>
        ///     The height of this module when displayed in a PGIView.
        /// </summary>
        public int InventoryHeight {
            get { return m_inventoryHeight; }
        }

        /// <summary>
        ///     The Name of the module.
        /// </summary>
        public string ModName {
            get { return m_name; }
        }

        /// <summary>
        ///     The Icon of the module.
        /// </summary>
        public Sprite ModuleIcon {
            get { return m_moduleIcon; }
        }

        /// <summary>
        ///     The point cost of this module. For use in limiting the total cost of a unit.
        /// </summary>
        public int ModuleCost {
            get { return m_moduleCost; }
        }

        #endregion


        #region JSON Save/Load

        public T RefFromJson<T>( string json ) where T : BaseUnitModule {
            return JsonConvert.DeserializeObject<T>( json );
        }

        public string ToJson( Formatting formatting = Formatting.None ) {
            return JsonConvert.SerializeObject( this, formatting );
        }

        #endregion
    }

}
