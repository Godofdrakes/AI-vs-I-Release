using System.Collections.Generic;
using System.Linq;
using AI_vs_I.Modules;
using AI_vs_I.UserInterface.Inventory;
using PowerGridInventory;
using SmallTools;
using UnityEngine;


namespace Collections {

    [ RequireComponent( typeof( PGIModel ) ) ]
    public class ModuleModel : CachedMonoBehaviour {

        public PGIModel PGIModel {
            get { return GetCachedComponent<PGIModel>(); }
        }

        public IEnumerable<BaseUnitModule> GetModules() {
            return
                PGIModel.AllItems
                        .Select( item=>item.GetComponent<ModuleItem>() )
                        .Where( instance=>instance != null )
                        .Select( instance=>instance.Module );
        }

        public IEnumerable<PGISlotItem> RemoveAllItems() {
            PGISlotItem[] items = PGIModel.AllItems;
            foreach( PGISlotItem item in items ) { PGIModel.Drop( item ); }

            return items;
        }

    }

}
