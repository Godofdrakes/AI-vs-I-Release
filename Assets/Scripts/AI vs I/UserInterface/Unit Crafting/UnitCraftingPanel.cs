using System.Collections.Generic;
using System.Linq;
using AI_vs_I.Modules;
using AI_vs_I.Player.Zenject;
using AI_vs_I.Units;
using AI_vs_I.UserInterface.Inventory;
using AI_vs_I.UserInterface.Unit_Crafting.Events;
using PowerGridInventory;
using SmallTools;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


// TODO : Add handling for returning modules to the inventory.

namespace AI_vs_I.UserInterface.Unit_Crafting {

    /// <summary>
    ///     Handles crafting of new units from the <see cref="ModuleItem" />s contained within a <see cref="PGIModel" />.
    /// </summary>
    [ RequireComponent( typeof( PGIModel ) ) ]
    public class UnitCraftingPanel : CachedMonoBehaviour {
        #region Private Fields

        private bool m_isValidCraft;

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        #endregion


        #region Inspector Fields

        [ SerializeField ]
        private ModuleModelWrapper m_moduleInventory = null;

        [ SerializeField ]
        private UnitArtBundle m_artBundle = null;

        [ SerializeField ]
        private ValidateCraftEvent m_validateCraft = new ValidateCraftEvent();

        [ SerializeField ]
        private UnitPreviewChangedEvent m_unitPreviewChanged = new UnitPreviewChangedEvent();

        #endregion


        #region PGI Events

        public void OnStoreItem( PGISlotItem item, PGIModel model ) {
            if( !enabled ) { return; } // Don't bother if we are disabled

            if( item.GetComponent<ModuleItem>() != null ) { UpdatePreview(); }
        }

        public void OnRemoveItem( PGISlotItem item, PGIModel model ) {
            if( !enabled ) { return; } // Don't bother if we are disabled

            if( item.GetComponent<ModuleItem>() != null ) { UpdatePreview(); }
        }

        #endregion


        #region Public Properties

        public PGIModel UnitModel {
            get { return GetCachedComponent<PGIModel>(); }
        }

        /// <summary>
        ///     Called whenever a change to the <see cref="UnitModel" /> has occured.
        /// </summary>
        public event UnityAction<UnitDefinition> OnUnitPreviewChanged {
            add { m_unitPreviewChanged.AddListener( value ); }
            remove { m_unitPreviewChanged.RemoveListener( value ); }
        }

        /// <summary>
        ///     Called whenever a change to the <see cref="UnitModel" /> has occured.
        ///     <para />
        ///     Bool represents if the current selection would create a valid unit.
        /// </summary>
        public event UnityAction<bool> OnValidateCraft {
            add { m_validateCraft.AddListener( value ); }
            remove { m_validateCraft.RemoveListener( value ); }
        }

        #endregion


        #region Functions

        private void ValidateCraft() {
            BaseUnitModule[] modules = GetModules().ToArray();
            m_isValidCraft = modules.Any() &&
                             modules.OfType<ActionModule>().Any() &&
                             modules.OfType<MovementModule>().Any() &&
                             modules.OfType<MaxHealthModule>().Any() &&
                             m_artBundle != null;
            m_validateCraft.Invoke( m_isValidCraft );
        }

        private void UpdatePreview() {
            UnitDefinition unit = GetUnitDefinition();
            unit.RecalculateStats();
            m_unitPreviewChanged.Invoke( unit );
        }

        /// <summary>
        ///     Removes and destroys all contained items.
        /// </summary>
        public void DropAll() {
            foreach( PGISlotItem slotItem in UnitModel.AllItems ) {
                UnitModel.Drop( slotItem );
                Destroy( slotItem.gameObject );
            }

            ValidateCraft();
            UpdatePreview();
        }

        /// <summary>
        ///     Removes and returns all contained items.
        /// </summary>
        public void ReturnAll() {
            foreach( PGISlotItem slotItem in UnitModel.AllItems ) {
                UnitModel.Drop( slotItem );
                m_moduleInventory.PGIModel.StoreAtFirstFreeSpaceIfPossible( slotItem );
            }

            ValidateCraft();
            UpdatePreview();
        }

        /// <summary>
        ///     Returns every BaseUnitModule contained within <see cref="UnitModel" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseUnitModule> GetModules() {
            return UnitModel.AllItems
                            .Where( item=>item.GetComponent<ModuleItem>() != null )
                            .Select( item=>item.GetComponent<ModuleItem>().Module );
        }

        /// <summary>
        ///     Returns a unit build from the <see cref="BaseUnitModule" />s contained within <see cref="UnitModel" />.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     Does not clear out the inventory.
        /// </remarks>
        public UnitDefinition GetUnitDefinition() {
            return new UnitDefinition( m_artBundle, GetModules().ToArray() );
        }

        /// <summary>
        ///     Builds a new unit form the currently selected <see cref="BaseUnitModule" />s and adds it to the player's inventory.
        /// </summary>
        public void CompileUnit() {
            if( !GetModules().Any() ) {
                Debug.LogWarningFormat( this,
                                        "#{0}# Can't compile a new unit when no modules have been selected!",
                                        typeof( UnitCraftingPanel ).Name );
                return;
            }

            m_saveData.UnitDefinitions.Add( GetUnitDefinition() );
            DropAll();
        }

        #endregion


        #region MonoBehaviour

        private void OnDisable() {
            // Make sure to give the player's inventory back all the modules we took
            // Otherwise they will lose them!
            m_saveData.Modules.AddRange( GetModules() );
            DropAll();
            // TODO : Fix modules not being removed from crafting panel if any remain when player switches to inventory view.
        }

        private void OnEnable() {
            ValidateCraft();
            UpdatePreview();
        }

        #endregion
    }

}
