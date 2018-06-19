// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  [Serializable]
  public class Listener {
    #pragma warning disable 649
    [SerializeField] private HasEmitter assetToMonitor;
    [SerializeField] private string     forTarget;

    // ReSharper disable once MemberCanBePrivate.Global
    /// <summary>
    /// Must be implemented in containing class as it is called if the listener is triggered.
    /// </summary>
    public Func<object[], bool> OnChange;

    /// <summary>
    /// Used to classes that have a listener can get back the Custom Asset that triggered it.
    /// </summary>
    public HasEmitter AssetToMonitor { get { return assetToMonitor; } }

    /// <summary>
    /// Register an action so that if the custom asset member changes anyone can be told
    /// </summary>
    /// <param name="onChange">void OnChange(string) reference</param>
    public void Register(Func<object[], bool> onChange) {
      forTarget = forTarget.Trim();
      if (string.IsNullOrEmpty(forTarget)) forTarget = null;
      OnChange = onChange;
      assetToMonitor.Emitter.Register(this);
    }

    /// <summary>
    /// Call to stop receiving change calls
    /// </summary>
    public void Deregister() { assetToMonitor.Emitter.Deregister(this); }

    /// <summary>
    /// Called by the channel when an event occurs.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    public bool OnTriggered(params object[] data) {
      if ((forTarget != null) && ((data.Length == 0) || forTarget.Equals(data[0]))) return false;

      return OnChange(data);
    }
  }
}