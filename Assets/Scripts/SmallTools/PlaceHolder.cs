using UnityEngine;


namespace SmallTools {

    public class PlaceHolder<T> : ScriptableObject {

        [ SerializeField ]
        private T m_data = default ( T );

        public T Data {
            get { return m_data; }
        }

    }

}
