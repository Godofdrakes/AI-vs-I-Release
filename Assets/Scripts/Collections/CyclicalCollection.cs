using System.Collections.Generic;
using System.Linq;


namespace Collections {

    /// <summary>
    /// A collection that will eternaly loop through the elements within it from front to back.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CyclicalCollection<T> {

        private int m_index;

        private T[] m_items;

        public CyclicalCollection( params T[] items ) {
            m_items = items;
            GoTo( 0 );
        }

        public CyclicalCollection( IEnumerable<T> items ) : this( items.ToArray() ) { }

        public T CurrentElement {
            get { return m_items[m_index]; }
        }

        public int Index {
            get { return m_index; }
        }

        /// <summary>
        ///     Sets the cyclical index to the provided value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>
        ///     The current element.
        /// </returns>
        public T GoTo( int index ) {
            while( index > m_items.Length ) { index -= m_items.Length; }

            m_index = index;
            return CurrentElement;
        }

        /// <summary>
        ///     Increments the cyclical index.
        /// </summary>
        /// <returns>
        ///     The current element.
        /// </returns>
        public T Next() { return GoTo( m_index + 1 ); }

    }

}
