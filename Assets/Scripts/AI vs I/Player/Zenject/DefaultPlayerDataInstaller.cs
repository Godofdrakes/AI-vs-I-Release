using System.Collections.Generic;
using AI_vs_I.Modules;
using AI_vs_I.Units;
using UnityEngine;
using Zenject;


namespace AI_vs_I.Player.Zenject {

    public class DefaultPlayerDataInstaller : MonoInstaller {

        [ SerializeField ]
        private string m_name = "Tommy Test";

        [ SerializeField ]
        private int m_money = 1000;

        [ SerializeField ]
        private Color m_playerColor = Color.green;

        [ SerializeField ]
        private List<BaseUnitModule> m_modules = new List<BaseUnitModule>();

        [ SerializeField ]
        private List<UnitDefinition> m_units = new List<UnitDefinition>();

        public override void InstallBindings() {
            Container.Bind<string>( "Default Player Name" ).ToInstance( m_name );
            Container.Bind<int>( "Default Money" ).ToInstance( m_money );
            Container.Bind<Color>( "Default Player Color" ).ToInstance( m_playerColor );
            Container.Bind<List<BaseUnitModule>>().ToInstance( m_modules );
            Container.Bind<List<UnitDefinition>>().ToInstance( m_units );
        }

    }

}
