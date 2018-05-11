namespace CustomAsset {
  using UnityEngine;
  using UnityEngine.Events;

  /// <inheritdoc />
  /// <summary>
  /// Fire a Unity event when a custom asset value changes
  /// </summary>
  public sealed class UnityListener : Listener {
    [SerializeField] private UnityEvent unityEvent;

    /// <inheritdoc />
    protected override void OnChange(string memberName) { unityEvent.Invoke(); }
  }
}