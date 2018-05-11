using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  public abstract class Listener : MonoBehaviour {
    [SerializeField]                      protected internal Base   BaseAsset;
    [SerializeField, Tooltip("Optional")] protected internal string ForMember;

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    [UsedImplicitly]
    public void OnTriggered() { OnChange(null); }

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    /// <param name="memberName">Member this event relates to</param>
    public void OnTriggered(string memberName) {
      if (memberName == ForMember) OnChange(memberName);
    }

    protected abstract void OnChange(string memberName);

    private void OnEnable() {
      ForMember = ForMember.Trim();
      if (string.IsNullOrEmpty(ForMember)) ForMember = null;
      BaseAsset.Register(this);
    }

    private void OnDisable() { BaseAsset.Deregister(this); }
  }
}