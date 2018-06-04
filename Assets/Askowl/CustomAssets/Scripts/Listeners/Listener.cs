// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  [Serializable]
  public class Listener {
    [SerializeField] private Base   baseAsset;
    [SerializeField] private string ForMember;

    public Action<string> OnChange;

    public Base BaseAsset { get { return baseAsset; } }

    public void Register(Action<string> onChange) {
      ForMember = ForMember.Trim();
      if (string.IsNullOrEmpty(ForMember)) ForMember = null;
      OnChange = onChange;
      baseAsset.Register(this);
    }

    public void Deregister() { baseAsset.Deregister(this); }

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    public void OnTriggered() { OnChange(null); }

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    /// <param name="memberName">Member this event relates to</param>
    public void OnTriggered(string memberName) {
      if ((ForMember == null) || (memberName == ForMember)) OnChange(memberName);
    }
  }
}