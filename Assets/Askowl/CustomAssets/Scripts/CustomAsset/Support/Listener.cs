using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  public abstract class Listener : MonoBehaviour {
    [SerializeField] private Base customAsset;

    /// <summary>
    /// THe channel that holds the event of interest, as set as part of the asset.
    /// </summary>
    [UsedImplicitly]
    public Base CustomAsset { get { return customAsset; } }

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    public abstract void OnTriggered();

    private void OnEnable() { customAsset.Register(this); }

    private void OnDisable() { customAsset.Deregister(this); }
  }
}