using UnityEngine;


namespace AI_vs_I.Units {

    [ CreateAssetMenu( fileName = "NewUnitObject", menuName = "AI vs I/Units/Definition Object" ) ]
    public class UnitDefinitionObject : ScriptableObject {

        [ SerializeField ]
        private UnitDefinition m_unitDefinition = new UnitDefinition();

        public UnitDefinition UnitDefinition {
            get { return m_unitDefinition; }
        }

    }

}
