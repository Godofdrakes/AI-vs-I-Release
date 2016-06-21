using AI_vs_I.Units;
using PowerGridInventory;
using SmallTools;
using UnityEngine;


namespace AI_vs_I.UserInterface.Inventory {

    [ RequireComponent( typeof( RectTransform ), typeof( PGISlotItem ) ) ]
    public class UnitItem : CachedMonoBehaviour {

        [ SerializeField ]
        private UnitDefinition m_unit;

        public PGISlotItem Item {
            get { return GetCachedComponent<PGISlotItem>(); }
        }

        public UnitDefinition Unit {
            get { return m_unit; }
            set {
                m_unit = value;
                if( m_unit != null ) { Item.Icon = m_unit.CoreSprite; }
            }
        }

    }

}
