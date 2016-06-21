using UnityEngine;
using UnityEngine.UI;


namespace Interface_Expanded.Collections {

    /// <summary>
    ///     An item in a selection collection.
    /// </summary>
    [ RequireComponent( typeof( Button ) ) ]
    public abstract class SelectionItem : MonoBehaviour {

        private SelectionView m_selectionView;

        private object m_sourceObject;


        #region Inspector Fields

        [ SerializeField ]
        private Button m_button;

        #endregion


        #region MonoBehaviour

        protected virtual void Reset() { m_button = GetComponent<Button>(); }

        protected virtual void Start() { m_button.onClick.AddListener( OnClick ); }

        #endregion


        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="sourceData">
        /// </param>
        /// <remarks>
        ///     Must be able to handle null, and objects of types other than the data you expected.
        ///     Usually this means a null check and a type check for safety.
        /// </remarks>
        protected abstract void PopulateDisplay( object sourceData );

        private void OnClick() { SelectionView.SelectedObject = SourceObject; }

        #endregion


        #region Properties

        public Button Button {
            get { return m_button; }
        }

        public SelectionView SelectionView {
            get { return m_selectionView; }
            set { m_selectionView = value; }
        }

        public object SourceObject {
            get { return m_sourceObject; }
            set {
                m_sourceObject = value;
                PopulateDisplay( m_sourceObject );
            }
        }

        #endregion
    }

}
