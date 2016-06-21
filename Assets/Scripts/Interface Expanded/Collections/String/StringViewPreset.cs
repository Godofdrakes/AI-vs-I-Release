using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


namespace Interface_Expanded.Collections.String {

    [ RequireComponent( typeof( SelectionView ) ) ]
    public class StringViewPreset : MonoBehaviour {

        [ SerializeField, UsedImplicitly ]
        private SelectionView m_selectionView;

        [ SerializeField, UsedImplicitly ]
        private List<string> m_stringPresets = new List<string>();

        private void Reset() { m_selectionView = GetComponent<SelectionView>(); }

        private void Start() { m_selectionView.Source = m_stringPresets.GetEnumerator(); }

    }

}
