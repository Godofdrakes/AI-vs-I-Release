using System;
using UnityEngine;
using Zenject;


namespace AI_vs_I.Player.Zenject {

    [ AddComponentMenu( "ZenJect Installers/AI vs I/Player Save Data" ) ]
    public class PlayerSaveDataInstaller : MonoInstaller {

        [ SerializeField ]
        private Material m_enemyUiMaterial = null;

        [ SerializeField ]
        private Material m_playerUiMaterial = null;

        [ InjectOptional ]
        private PlayerSaveData m_playerSaveData = null;

        public override void InstallBindings() {
            Container.Bind<PlayerSaveData>().ToSingle();
            Container.Bind<IDisposable>().ToSingle<PlayerSaveData>();
            Container.Bind<bool>( "Save Data Exists" )
                     .ToMethod( context=>PlayerPrefs.HasKey( PlayerSaveData.PrefsKey ) );
            Container.Bind<Material>( "Enemy UI" ).ToMethod( MakeEnemyMaterial );
            Container.Bind<Material>( "Player UI" ).ToMethod( MakePlayerMaterial );

            Container.Inject( this );

            // TODO : Update old code so changing the original material is no longer necessary
            m_playerSaveData.OnSaveDataChanged += UpdateMaterialColors;

            Container.Bind<Material>( "Enemy UI" ).ToMethod( MakeEnemyMaterial );
            Container.Bind<Material>( "Player UI" ).ToMethod( MakePlayerMaterial );
        }

        private Material MakeEnemyMaterial( InjectContext injectContext ) {
            Material material = Instantiate( m_enemyUiMaterial );
            material.color = m_playerSaveData.EnemyColor;
            return material;
        }

        private Material MakePlayerMaterial( InjectContext injectContext ) {
            Material material = Instantiate( m_playerUiMaterial );
            material.color = m_playerSaveData.PlayerColor;
            return material;
        }

        // To preserve support for outdated code
        private void UpdateMaterialColors( PlayerSaveData saveData ) {
            m_enemyUiMaterial.color = saveData.EnemyColor;
            m_playerUiMaterial.color = saveData.PlayerColor;
        }

    }

}
