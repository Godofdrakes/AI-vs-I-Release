using UnityEngine;
using UnityEngine.UI;


namespace Interface_Expanded.Collections.String {

    public class StringDisplay : MonoBehaviour {

        [ SerializeField ]
        private Text m_text;

        public void SetText( object sourceData ) {
            m_text.text = sourceData == null ? "null" : sourceData.ToString();
        }

        private void Reset() { m_text = GetComponentInChildren<Text>(); }

    }

}
