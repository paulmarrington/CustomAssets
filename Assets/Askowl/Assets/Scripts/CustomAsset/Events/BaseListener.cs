using UnityEngine;

namespace Events {
  public abstract class BaseListener : MonoBehaviour, Listener {
    [SerializeField] private Channel channel;

    public Channel Channel { get { return channel; } }

    public abstract void OnTriggered(Listener listener);

    private void OnEnable() { channel.Register(this); }

    private void OnDisable() { channel.Deregister(this); }
  }
}