using Newtonsoft.Json;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SmallTools {

    public static class PlayerPrefsHelper {
#if UNITY_EDITOR
        [ MenuItem( "Assets/Clear Player Prefrences" ) ]
#endif
        public static void ClearPlayerPrefs() {
            PlayerPrefs.DeleteAll();
            Debug.LogFormat( "#{0}# PlayerPrefs have been cleared.",
                             typeof( PlayerPrefsHelper ).Name );
        }

        public static T TryLoad<T>( string key, T fallback = default ( T ) ) {
            T data = fallback;

            bool hasKey = PlayerPrefs.HasKey( key );
            if( hasKey ) {
                string json = PlayerPrefs.GetString( key );
                Debug.Log( json );
                data = JsonConvert.DeserializeObject<T>( json );
            }

            if( fallback != null && data == null ) { data = fallback; }
            return data;
        }

        public static bool TrySave<T>(
            string key,
            T value,
            JsonSerializerSettings serializerSettings = null ) {
            if( serializerSettings == null ) { serializerSettings = new JsonSerializerSettings(); }

            string json;

            try {
                json = JsonConvert.SerializeObject( value, Formatting.None, serializerSettings );
            }
            catch( JsonSerializationException ) {
                //Debug.LogException( e );
                return false;
            }

            PlayerPrefs.SetString( key, json );
            return true;
        }

    }

}
