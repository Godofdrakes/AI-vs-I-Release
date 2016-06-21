using System;
using System.Collections.Generic;
using AI_vs_I.GameEventSystem.Events;
using UnityEngine;


namespace AI_vs_I.GameEventSystem {

    public class GameEvents : IGameEventManager {

        private Dictionary<Type, Action<IGameEvent>> m_eventSubscribers;

        public GameEvents() { m_eventSubscribers = new Dictionary<Type, Action<IGameEvent>>(); }

        public void Subscribe( Action<IGameEvent> methodTarget, params Type[] eventTypes ) {
            if( eventTypes == null ) { throw new ArgumentNullException( "eventTypes" ); }
            if( methodTarget == null ) { throw new ArgumentNullException( "methodTarget" ); }

            for( int index = 0; index < eventTypes.Length; index++ ) {
                if( !typeof( IGameEvent ).IsAssignableFrom( eventTypes[index] ) ) {
                    throw new ArgumentException(
                        string.Format( "eventTypes[{0}] (Type: {1}) is not an IGameEvent",
                                       index,
                                       eventTypes[index] ),
                        "eventTypes" );
                }

                // Make sure the element exists to subscribe to
                if( !m_eventSubscribers.ContainsKey( eventTypes[index] ) ) {
                    m_eventSubscribers.Add( eventTypes[index], null );
                }

                m_eventSubscribers[eventTypes[index]] += methodTarget;
            }
        }

        public void UnsubscribeAll( Action<IGameEvent> methodTarget ) {
            if( methodTarget == null ) { throw new ArgumentNullException( "methodTarget" ); }

            foreach( Type type in m_eventSubscribers.Keys ) {
                m_eventSubscribers[type] -= methodTarget;
            }
        }

        public void Unsubscribe( Action<IGameEvent> methodTarget, params Type[] eventTypes ) {
            if( eventTypes == null ) { throw new ArgumentNullException( "eventTypes" ); }
            if( methodTarget == null ) { throw new ArgumentNullException( "methodTarget" ); }

            for( int index = 0; index < eventTypes.Length; index++ ) {
                if( !typeof( IGameEvent ).IsAssignableFrom( eventTypes[index] ) ) {
                    throw new ArgumentException(
                        string.Format( "eventTypes[{0}] (Type: {1}) is not an IGameEvent",
                                       index,
                                       eventTypes[index] ),
                        "eventTypes" );
                }

                // If the element doesn't exist we don;t need to bother trying to unsubscribe
                if( m_eventSubscribers.ContainsKey( eventTypes[index] ) ) {
                    m_eventSubscribers[eventTypes[index]] -= methodTarget;
                }
            }
        }

        public void Invoke( IGameEvent gameEvent ) {
            Type eventType = gameEvent.GetType();
            if( m_eventSubscribers.ContainsKey( eventType ) ) {
                m_eventSubscribers[eventType].Invoke( gameEvent );
            }
            else {
                Debug.LogFormat( "#{0}# Skipping invoke (Type: {1}), it has no subscribers.",
                                 typeof( GameEvents ).Name,
                                 eventType );
            }
        }

    }

}
