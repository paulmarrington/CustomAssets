namespace CustomAsset {
  using UnityEngine;

  public abstract class BaseEventListener : MonoBehaviour, IEventListener {
    [SerializeField] private EventActor eventActor;

    public EventActor EventActor { get { return eventActor; } }

    public abstract void OnEventRaised(IEventListener listener);

    private void OnEnable() { eventActor.Register(this); }

    private void OnDisable() { eventActor.Deregister(this); }
  }
}