using Newtonsoft.Json;
using UnityEngine;
using Zenject;


namespace Serializers {

    [ AddComponentMenu( "ZenJect Installers/Json.Net" ) ]
    public class JsonDotNetInstaller : MonoInstaller {

        public override void InstallBindings() {
#if UNITY_EDITOR
            Formatting formatting = Formatting.Indented;
#else
            Formatting formatting = Formatting.None;
#endif
            JsonSerializerSettings settings =
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            Container.BindInstance( formatting );
            Container.BindInstance( settings );
        }

    }

}
