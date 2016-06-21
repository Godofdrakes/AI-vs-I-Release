using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Interface_Expanded.Collections {

    /// <summary>
    ///     Displays a collection of items.
    /// </summary>
    [ AddComponentMenu( "Interface Expanded/Selection View/Selection View" ) ]
    public class SelectionView : MonoBehaviour {

        /// <summary>
        ///     See <see cref="Source" />.
        /// </summary>
        private IEnumerator m_source;

        private List<SelectionItem> m_selectionItems = new List<SelectionItem>();


        #region Events

        [ Serializable ]
        public class SelectionChangedEvent : UnityEvent<object> {

        }

        #endregion


        #region Methods

        private void NewSelectionItem() {
            SelectionItem item = Instantiate( m_itemPrefab );
            item.SelectionView = this;
            item.transform.SetParent( m_contentArea, false );
            m_selectionItems.Add( item );
        }

        public void UpdateItems() {
            Source.Reset();
            bool keepMoving;
            for( int index = 0;
                 (keepMoving = Source.MoveNext()) || index < m_selectionItems.Count;
                 index++ ) {
                if( index >= m_selectionItems.Count ) { NewSelectionItem(); }
                m_selectionItems[index].gameObject.SetActive( keepMoving );
                m_selectionItems[index].SourceObject = keepMoving ? Source.Current : null;
            }
        }

        #endregion


        #region Inspector Fields

        [ SerializeField,
          Tooltip( "Newly created SelectionItems will become children of this Trasform." +
                   " Example: Set this to a ScrollBox's content transform." ) ]
        private Transform m_contentArea = null;

        [ SerializeField,
          Tooltip( "Like WPF's DataTemplate this prefab will be used to populate the list." ) ]
        private SelectionItem m_itemPrefab = null;

        [ SerializeField ]
        private SelectionChangedEvent m_selectionChangedEvent = new SelectionChangedEvent();

        private object m_selectedObject;

        #endregion


        #region Properties

        /// <summary>
        ///     The current source of data.
        /// </summary>
        public IEnumerator Source {
            get { return m_source; }
            set {
                // All hail the nullobject pattern, destroyer of null checks
                m_source = value;
                m_source.Reset();
                UpdateItems();
                m_selectionChangedEvent.Invoke( null );
            }
        }

        public object SelectedObject {
            get { return m_selectedObject; }
            set {
                m_selectedObject = value;
                m_selectionChangedEvent.Invoke( m_selectedObject );
            }
        }

        /// <summary>
        ///     Invoked when something is selected (object will be the selected object)
        ///     or when the <see cref="Source" /> changes (object will be null).
        /// </summary>
        public event UnityAction<object> OnSelectionChanged {
            add { m_selectionChangedEvent.AddListener( value ); }
            remove { m_selectionChangedEvent.RemoveListener( value ); }
        }

        #endregion
    }

}
