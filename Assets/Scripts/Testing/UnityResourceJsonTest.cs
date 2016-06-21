using System.Collections.Generic;
using AI_vs_I.Units;
using Newtonsoft.Json;
using UnityEngine;


namespace Testing {

    public class UnityResourceJsonTest : MonoBehaviour {

        public UnitInstance TargetInstance = null;

        private void Start() {
            if( TargetInstance == null ) { return; }
            string jsonConvertString = TargetInstance.ToJson( Formatting.Indented );
            Debug.Log( jsonConvertString );
            UnitInstance.NewFromJson( jsonConvertString );
        }

    }

}
