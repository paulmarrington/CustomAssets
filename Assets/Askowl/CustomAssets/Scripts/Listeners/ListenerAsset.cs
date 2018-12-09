// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Common code for all event listeners. It registers and deregisters the listener with the channel</a> //#TBD#// <inheritdoc cref="MonoBehaviour" />
  public abstract class ListenerAsset : OfType<Listener> {
    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnEnable() => Value.Register(OnChange);

    /// <a href=""></a> //#TBD#// <inheritdoc cref="ListenerAsset()" />
    protected void OnDisable() => Value.Deregister();

    /// <a href="">Reference to the Asset we are listening to</a> //#TBD#//
    public WithEmitter Asset => Value.AssetToMonitor;

    /// <a href="">After we have ensured the change is for the expected member, tell interested parties</a> //#TBD#//
    protected abstract void OnChange();
  }
}