// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  [Serializable]
  public class Listener {
    #pragma warning disable 649
    [SerializeField] private Base assetToMonitor;
    #pragma warning restore 649
    [SerializeField] private string forMember;

    // ReSharper disable once MemberCanBePrivate.Global
    /// <summary>
    /// Must be implemented in containing class as it is called if the listener is triggered.
    /// </summary>
    public Action<string> OnChange;

    /// <summary>
    /// Used to classes that have a listener can get back the Custom Asset that triggered it.
    /// </summary>
    public Base AssetToMonitor { get { return assetToMonitor; } }

    /// <summary>
    /// Register an action so that if the custom asset member changes anyone can be told
    /// </summary>
    /// <param name="onChange">void OnChange(string) reference</param>
    public void Register(Action<string> onChange) {
      forMember = forMember.Trim();
      if (string.IsNullOrEmpty(forMember)) forMember = null;
      OnChange = onChange;
      assetToMonitor.Register(this);
    }

    /// <summary>
    /// Call to stop receiving change calls
    /// </summary>
    public void Deregister() { assetToMonitor.Deregister(this); }

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
      if ((forMember == null) || (memberName == forMember)) OnChange(memberName);
    }
  }
}