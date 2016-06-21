using System.Linq;
using UnityEngine;


namespace SmallTools {

    public class Singleton<T> : CachedMonoBehaviour
        where T : MonoBehaviour {

        private static Singleton<T> s_instance;

        public static T Instance {
            get { return s_instance as T; }
        }

        public bool ThisIsTheSingleton {
            get { return Instance == this; }
        }

        protected virtual void OnEnable() {
            if( FindObjectsOfType<T>()
                .Any( singleton=>singleton != this ) ) {
                Debug.LogWarningFormat( this,
                                        "#{0}# Instance already exists. " +
                                        "Enforcing singleton pattern by removing self (including own gameobject).",
                                        GetType().Name );
                Destroy( gameObject );
                return;
            }

            s_instance = this;
            Debug.LogFormat( this,
                             "#{0}# Instance has been created. " +
                             "Now enforcing singleton pattern.",
                             GetType().Name );
        }

    }

}
