namespace CustomAsset {
  using UnityEngine;
  using UnityEngine.Events;

  public sealed class UnityEventListener : BaseEventListener {
    [SerializeField] private UnityEvent unityEvent;

    public override void OnEventRaised(IEventListener _) { unityEvent.Invoke(); }
  }
}