using System;
using System.Collections;
using System.Linq;
using SmallTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = TypeSafe.Scene;


namespace SceneTransitions {

    public class SceneSwitcher : Singleton<SceneSwitcher> {

        [ SerializeField ]
        private string m_firstScene = string.Empty;

        [ SerializeField ]
        private string m_transitionScene = string.Empty;

        public static Coroutine SceneTransitionCoroutine { get; private set; }

        public static Scene GetSceneByName( string sceneName ) {
            return SRScenes.All.FirstOrDefault( scene=>
                                                string.Equals( scene.name,
                                                               sceneName,
                                                               StringComparison
                                                                   .CurrentCultureIgnoreCase ) );
        }

        /// <summary>
        ///     Loads a new scene and sets it to the active scene.
        /// </summary>
        /// <param name="desiredScene">
        ///     The name of the scene you want to load.
        ///     Make sure it's included in the build settings or it won't get found.
        /// </param>
        /// <returns>
        ///     The coroutine handling the loading of the scene.
        ///     Yielding to it will wait until the new scene has finished loading.
        /// </returns>
        public static Coroutine LoadScene( string desiredScene ) {
            Scene scene = GetSceneByName( desiredScene );
            Scene transition = GetSceneByName( Instance.m_transitionScene );
            if( SceneTransitionCoroutine != null ) {
                Debug.LogErrorFormat( Instance,
                                      "#{0}# Cannot load scene while another scene is still loading.",
                                      typeof( SceneSwitcher ).Name );
                return null;
            }

            if( scene == null ) {
                Debug.LogErrorFormat( Instance,
                                      "#{0}# Scene {1} is not a valid scene.",
                                      typeof( SceneSwitcher ).Name,
                                      desiredScene );
                return null;
            }

            SceneTransitionCoroutine =
                Instance.StartCoroutine( transition != null
                                             ? CorTransitionScene( scene, transition )
                                             : CorLoadScene( scene, SceneManager.sceneCount > 1 ) );

            return SceneTransitionCoroutine;
        }

        private static IEnumerator CorLoadScene( Scene scene, bool unloadPrevious ) {
            UnityEngine.SceneManagement.Scene previousScene = SceneManager.GetActiveScene();
            yield return scene.LoadAdditiveAsync();

            SceneManager.SetActiveScene( SceneManager.GetSceneByName( scene.name ) );
            if( unloadPrevious ) { SceneManager.UnloadScene( previousScene.name ); }
            SceneTransitionCoroutine = null;
        }

        private static IEnumerator CorTransitionScene( Scene finalScene, Scene intermediateScene ) {
            SceneTransitionCoroutine = Instance.StartCoroutine( CorLoadScene( intermediateScene,
                                                                              SceneManager
                                                                                  .sceneCount > 1 ) );
            yield return SceneTransitionCoroutine;

            SceneTransitionCoroutine = Instance.StartCoroutine( CorLoadScene( finalScene, true ) );
            yield return SceneTransitionCoroutine;
        }

        private void Start() {
            if( !ThisIsTheSingleton ) return;

            if( !string.IsNullOrEmpty( m_firstScene ) ) { LoadScene( m_firstScene ); }
        }

    }

}
