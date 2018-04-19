namespace Events {
  using UnityEngine;
  using UnityEngine.Events;

  public sealed class UnityListener : BaseListener {
    [SerializeField] private UnityEvent unityEvent;

    public override void OnTriggered(Listener _) { unityEvent.Invoke(); }
  }
}