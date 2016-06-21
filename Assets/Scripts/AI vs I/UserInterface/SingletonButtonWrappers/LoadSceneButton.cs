using SceneTransitions;
using UnityEngine;
using UnityEngine.UI;
using AI_vs_I.Levels;
using AI_vs_I.Level_Loading.Zenject;


namespace AI_vs_I.UserInterface.SingletonButtonWrappers {

    [ RequireComponent( typeof( Button ) ) ]
    public class LoadSceneButton : MonoBehaviour {

        [ SerializeField ]
        private GGGrid m_selectedLevelPrefab = null;

        [ SerializeField ]
        private string m_sceneName = null;

        public void LoadLevel() {
            LevelSelectionInstaller.SelectedLevelPrefab = m_selectedLevelPrefab;
            SceneSwitcher.LoadScene( m_sceneName );
        }

    }

}
