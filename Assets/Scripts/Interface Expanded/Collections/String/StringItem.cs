using UnityEngine;
using UnityEngine.UI;


namespace Interface_Expanded.Collections.String {

    [ AddComponentMenu( "Interface Expanded/Selection View/String Item" ) ]
    public class StringItem : SelectionItem {

        [ SerializeField ]
        private Text m_text;

        protected override void PopulateDisplay( object sourceData ) {
            string text = sourceData as string;
            if( text != null ) { m_text.text = text; }
        }

        protected override void Reset() {
            base.Reset();
            m_text = GetComponentInChildren<Text>();
            
        }

    }

}
