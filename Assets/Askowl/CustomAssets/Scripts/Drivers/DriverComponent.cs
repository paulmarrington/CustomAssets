// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Common code for all event listener MonoBehaviours. It registers and deregisters the listener with the channel</a> <inheritdoc />
  public abstract class DriverComponent<T> : ListenerComponent where T : Base {
    /// <a href="http://bit.ly/2QNmw2q">Reference to the Asset we are listening to</a>
    public T Asset => Listener.AssetToMonitor as T;
  }

  /// <a href="http://bit.ly/2QNmw2q">Common code for all event listener MonoBehaviours. It registers and deregisters the listener with the channel</a> <inheritdoc cref="MonoBehaviour" />
  public abstract class ListenerComponent : MonoBehaviour {
    [SerializeField] private Listener listener = default;

    /// <a href="http://bit.ly/2QNmw2q">Retrieve a reference to the listener attached to this component</a>
    public Listener Listener => listener;

    private void OnEnable() => listener.Register(OnChange);

    private void OnDisable() => Deregister();

    /// <a href="http://bit.ly/2QNmw2q">Stop listening to changes in the custom asset</a>
    public void Deregister() => listener.Deregister();

    /// <a href="http://bit.ly/2QNmw2q">After we have ensured the change is for the expected member, tell interested parties</a>
    protected abstract void OnChange();
  }
}