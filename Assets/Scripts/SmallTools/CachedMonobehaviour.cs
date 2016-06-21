using System;
using System.Collections.Generic;
using UnityEngine;


namespace SmallTools {

    public class CachedMonoBehaviour : MonoBehaviour {

        private Dictionary<Type, object> m_behaviourCache =
            new Dictionary<Type, object>();

        /// <summary>
        ///     Gets and caches a component.
        ///     Later calls for the same type will return the cached component for speed.
        /// </summary>
        /// <typeparam name="T">
        ///     A type of component.
        ///     Usually MonoBehaviours but unity also supports interfaces.
        /// </typeparam>
        /// <param name="forceGet">
        ///     Forces a GetComponent() call.
        ///     Useful if the component might have been deleted.
        /// </param>
        /// <returns>
        ///     A reference to the desired component, or null of none could be found.
        /// </returns>
        public T GetCachedComponent<T>( bool forceGet = false ) where T : class {
            T component = null;
            if( !forceGet && m_behaviourCache.ContainsKey( typeof( T ) ) ) {
                component = m_behaviourCache[typeof( T )] as T;
            }
            else {
                component = GetComponent<T>();
                if( component != null ) { m_behaviourCache[typeof( T )] = component; }
            }
            return component;
        }

    }

}
