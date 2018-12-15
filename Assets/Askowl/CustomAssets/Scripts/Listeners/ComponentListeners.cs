// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Base class for listeners that need a float parameter</a> //#TBD#//
  public abstract class FloatListener<T> : ComponentListenerBase<T, Float, float> where T : Object { }

  /// <a href="">Base class for listeners that need an integer parameter</a> //#TBD#//
  public abstract class IntegerListener<T> : ComponentListenerBase<T, Integer, int> where T : Object { }

  /// <a href="">Base class for listeners that need an boolean parameter</a> //#TBD#//
  public abstract class BooleanListener<T> : ComponentListenerBase<T, Boolean, bool> where T : Object { }

  /// <a href="">Converts custom asset to a string for components that deal with string data</a> //#TBD#//
  public abstract class StringListener<T> :
    ComponentListenerBase<T> where T : Object {
    /// <a href="">Reference to the Asset we are listening to</a> //#TBD#//
    public string Asset => Listener.AssetToMonitor.ToString();

    /// <a href="">Called with new value of the data within the custom asset</a> //#TBD#//
    protected abstract void OnChange(string value);

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() {
      if (!Equals(Asset)) OnChange(Asset);
    }

    /// <a href="">We need to compare value from target against change to see if we need to change</a> //#TBD#//
    protected abstract bool Equals(string value);
  }

  /// <a href="">Listener that triggers without complaint</a> //#TBD#//
  public abstract class TriggerListener<T> : ComponentListenerBase<T> where T : Object { }

  /// <a href="">Internal Base class for component listeners that need to process the custom asset data</a> //#TBD#//
  public abstract class ComponentListenerBase<TC, TA, TD> : ComponentListenerBase<TC>
    where TC : Object where TA : OfType<TD> {
    /// <a href="">Reference to the Asset we are listening to</a> //#TBD#//
    public TA Asset => Listener.AssetToMonitor as TA;

    /// <a href="">Called with new value of the data within the custom asset</a> //#TBD#//
    protected abstract void OnChange(TD value);

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() {
      if (!Equals(Asset)) OnChange(Asset);
    }

    /// <a href="">We need to compare value from target against change to see if we need to change</a> //#TBD#//
    protected abstract bool Equals(TD value);
  }

  /// <a href="">Internal Base class for component listeners that need to process the custom asset data</a> //#TBD#//
  public abstract class ComponentListenerBase<TC> : ListenerComponent where TC : Object {
    /// <a href="">Component we are going to give the custom asset data to</a> //#TBD#//
    [SerializeField, Tooltip("Optional")] protected TC Target;

    private void Awake() {
      if (Target == null) Target = GetComponent<TC>();
      if (Target != null) return;
      Debug.LogErrorFormat("No target '{0}' on '{1}'", typeof(TC).Name, Objects.Path(gameObject));
      Deregister();
    }
  }
}