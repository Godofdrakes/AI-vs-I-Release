using System;
using AI_vs_I.GameEventSystem.Events;


namespace AI_vs_I.GameEventSystem {

    public interface IGameEventManager {

        void Subscribe( Action<IGameEvent> methodTarget, params Type[] eventTypes );

        void UnsubscribeAll( Action<IGameEvent> methodTarget );

        void Unsubscribe( Action<IGameEvent> methodTarget, params Type[] eventTypes );

    }

}
