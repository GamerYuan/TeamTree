using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> {}

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public CustomGameEvent response;

    protected virtual void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    protected virtual void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }
     
    public virtual void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
