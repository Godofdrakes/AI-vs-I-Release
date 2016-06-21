using AI_vs_I.Modules;
using PowerGridInventory;
using SmallTools;
using UnityEngine;


namespace AI_vs_I.UserInterface.Inventory {

    [ RequireComponent( typeof( RectTransform ), typeof( PGISlotItem ) ) ]
    public class ModuleItem : CachedMonoBehaviour {

        [ SerializeField ]
        private BaseUnitModule m_module;

        public PGISlotItem Item {
            get { return GetCachedComponent<PGISlotItem>(); }
        }

        public RectTransform RectTransform {
            get { return GetCachedComponent<RectTransform>(); }
        }

        public BaseUnitModule Module {
            get { return m_module; }
            set {
                m_module = value;
                if( m_module != null ) {
                    Item.Icon = m_module.ModuleIcon;
                    Item.CellHeight = m_module.InventoryHeight;
                    Item.CellWidth = m_module.InventoryWidth;
                }
            }
        }

    }

}
