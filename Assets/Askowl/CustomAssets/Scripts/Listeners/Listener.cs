// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  public abstract class Listener : MonoBehaviour {
    [SerializeField]                      protected internal Base   BaseAsset;
    [SerializeField, Tooltip("Optional")] protected internal string ForMember;

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    [UsedImplicitly]
    public void OnTriggered() { OnChange(null); }

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    /// <param name="memberName">Member this event relates to</param>
    public void OnTriggered(string memberName) {
      if ((ForMember == null) || (memberName == ForMember)) OnChange(memberName);
    }

    /// <summary>
    /// After we have ensured the change is for the expected member, tell interested parties.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
    /// <param name="memberName"></param>
    protected abstract void OnChange(string memberName);

    private void OnEnable() {
      ForMember = ForMember.Trim();
      if (string.IsNullOrEmpty(ForMember)) ForMember = null;
      BaseAsset.Register(this);
    }

    private void OnDisable() { BaseAsset.Deregister(this); }
  }
}