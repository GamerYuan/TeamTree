using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

[System.Serializable]
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
