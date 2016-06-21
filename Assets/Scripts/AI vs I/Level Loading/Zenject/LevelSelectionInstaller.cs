using UnityEngine;
using UnityEngine.Assertions;
using Zenject;


namespace AI_vs_I.Level_Loading.Zenject {

    /// <summary>
    ///     Handles loading and injection of loading of level prefabs.
    /// </summary>
    public class LevelSelectionInstaller : MonoInstaller {

        /// <summary>
        ///     The desired level prefab.
        /// </summary>
        public static GGGrid SelectedLevelPrefab;

        static LevelSelectionInstaller() { SelectedLevelPrefab = null; }

        public override void InstallBindings() {
            Container.Bind<GGGrid>( "Selected Level" )
                     .ToMethod( BuildGameGrid );
        }

        /// <summary>
        ///     Injects an instantiated level prefab. Favors existing instances over the selected prefab.
        /// </summary>
        /// <param name="injectContext"></param>
        /// <returns></returns>
        private GGGrid BuildGameGrid( InjectContext injectContext ) {
            GGGrid grid = FindObjectOfType<GGGrid>();

            if( grid == null ) {
                Assert.IsNotNull( SelectedLevelPrefab );
                grid =
                    Container.InstantiatePrefabForComponent<GGGrid>( SelectedLevelPrefab.gameObject );
                grid.transform.position = Vector3.zero;
            }

            return grid;
        }

    }

}
