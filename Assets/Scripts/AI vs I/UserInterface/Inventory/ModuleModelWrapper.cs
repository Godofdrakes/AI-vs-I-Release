﻿using System.Linq;
using AI_vs_I.Modules;
using AI_vs_I.Player;
using AI_vs_I.Player.Zenject;
using JetBrains.Annotations;
using PowerGridInventory;
using SmallTools;
using UnityEngine;
using Zenject;


namespace AI_vs_I.UserInterface.Inventory {

    /// <summary>
    ///     Populates and keeps synced a <see cref="PGIModel" /> with the player's <see cref="PlayerSaveData.Modules" />.
    /// </summary>
    [RequireComponent( typeof( PGIModel ) ) ]
    public class ModuleModelWrapper : CachedMonoBehaviour {
        #region Private Fields

        [ Inject ]
        private PlayerSaveData m_saveData = null;

        #endregion


        #region Inspector Fields

        [ SerializeField ]
        private bool m_keepSyncedWithInventory = true;

        [ SerializeField ]
        private ModuleItem m_itemPrefab = null;

        #endregion


        #region MonoBehaviour

        [ UsedImplicitly ]
        private void OnEnable() {
            ClearDisplay();
            Display();
            PlayerData_Sub();
            PGI_Sub();
        }

        [ UsedImplicitly ]
        private void OnDisable() {
            PlayerData_Unsub();
            PGI_Unsub();
        }

        #endregion


        #region Functions

        private void PlayerData_Sub() { m_saveData.OnSaveDataChanged += PlayerDataUpdated; }

        private void PlayerData_Unsub() { m_saveData.OnSaveDataChanged -= PlayerDataUpdated; }

        private void PGI_Sub() {
            PGIModel.OnStoreItem.AddListener( OnStoreItem );
            PGIModel.OnRemoveItem.AddListener( OnRemoveItem );
        }

        private void PGI_Unsub() {
            PGIModel.OnStoreItem.RemoveListener( OnStoreItem );
            PGIModel.OnRemoveItem.RemoveListener( OnRemoveItem );
        }

        private void ClearDisplay() {
            foreach( PGISlotItem slotItem in PGIModel.AllItems ) {
                PGIModel.Drop( slotItem );
                Destroy( slotItem.gameObject );
            }
        }

        private void Display() {
            foreach( BaseUnitModule module in m_saveData.Modules ) {
                ModuleItem moduleItem = Instantiate( m_itemPrefab );
                moduleItem.Module = module;
                PGIModel.StoreAtFirstFreeSpaceIfPossible( moduleItem.Item );
            }
        }

        #endregion


        #region Properties

        public PGIModel PGIModel {
            get { return GetCachedComponent<PGIModel>(); }
        }

        public ModuleItem[] AllItems {
            get {
                if( PGIModel != null ) {
                    return
                        PGIModel.AllItems
                                .Where( item=>item.GetComponent<ModuleItem>() != null )
                                .Select( item=>item.GetComponent<ModuleItem>() )
                                .ToArray();
                }

                return new ModuleItem[0];
            }
        }

        #endregion


        #region Event Listeners

        /// <summary>
        ///     Updates the <see cref="PlayerSaveData" /> to matcha a change in the <see cref="PGIModel" />.
        /// </summary>
        /// <param name="arg0">The item removed.</param>
        /// <param name="pgiModel">The model ???</param>
        private void OnRemoveItem( PGISlotItem arg0, PGIModel pgiModel ) {
            if( !m_keepSyncedWithInventory ) { return; }

            PlayerData_Unsub();

            ModuleItem moduleItem = arg0.GetComponent<ModuleItem>();
            if( moduleItem != null ) {
                m_saveData.Modules.Remove( moduleItem.Module );
                m_saveData.NotifyChanges();
            }

            PlayerData_Sub();
        }

        /// <summary>
        ///     Updates the <see cref="PlayerSaveData" /> to matcha a change in the <see cref="PGIModel" />.
        /// </summary>
        /// <param name="arg0">The item added.</param>
        /// <param name="pgiModel">The model ???</param>
        private void OnStoreItem( PGISlotItem arg0, PGIModel pgiModel ) {
            if( !m_keepSyncedWithInventory ) { return; }

            PlayerData_Unsub();

            ModuleItem moduleItem = arg0.GetComponent<ModuleItem>();
            if( moduleItem != null ) {
                m_saveData.Modules.Add( moduleItem.Module );
                m_saveData.NotifyChanges();
            }

            PlayerData_Sub();
        }

        /// <summary>
        ///     Updates the <see cref="PGIModel" /> to match a change in the <see cref="PlayerSaveData" />.
        /// </summary>
        private void PlayerDataUpdated( PlayerSaveData saveData ) {
            if( !m_keepSyncedWithInventory ) { return; }

            PGI_Unsub();

            ClearDisplay();
            Display();

            PGI_Sub();
        }

        #endregion
    }

}
