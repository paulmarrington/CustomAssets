// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;

namespace CustomAsset.Mutable {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Base class for listeners that need a float parameter
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#floatlistener">More...</a></remarks>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class FloatListener<T> :
    ComponentListenerBase<T, Float, float> where T : Object { }

  /// <inheritdoc />
  /// <summary>
  /// Base class for listeners that need an integer parameter
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#integerlistener">More...</a></remarks>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class IntegerListener<T> :
    ComponentListenerBase<T, Integer, int> where T : Object { }

  /// <inheritdoc />
  /// <summary>
  /// Base class for listeners that need an boolean parameter
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#booleanlistener">More...</a></remarks>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class BooleanListener<T> :
    ComponentListenerBase<T, Boolean, bool> where T : Object { }

  /// <inheritdoc />
  /// <summary>
  /// Converts custom asset to a string for components that deal with string data.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#stringlistener">More...</a></remarks>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class StringListener<T> :
    ComponentListenerBase<T, String, string> where T : Object { }

  /// <summary>
  /// Listener that triggers without complaint
  /// </summary>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class TriggerListener<T> : ComponentListenerBase<T> where T : Object {
    /// <summary>
    /// Called with new value of the data within the custom asset
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    /// <param name="value">Reference to the changed value</param>
    protected abstract bool OnChange();

    /// <inheritdoc />
    ///  <summary>
    /// On a change the listener needs a copy of the changed data to react to
    ///  </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    /// <returns>True of all ok (either equals or no change error</returns>
    protected override bool OnChange(object[] _) { return OnChange(); }
  }

  /// <inheritdoc />
  /// <summary>
  /// Internal Base class for component listeners that need to process the custom asset data.
  /// </summary>
  /// <typeparam name="TC">Type of component we are modifying on demand</typeparam>
  /// <typeparam name="TA">Type of custom asset</typeparam>
  /// <typeparam name="TD">Type of data contained in the custom asset</typeparam>
  public abstract class ComponentListenerBase<TC, TA, TD> : ComponentListenerBase<TC>
    where TC : Object where TA : OfType<TD> {
    /// <summary>
    /// Reference to the Asset we are listening to
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public TA Asset { get { return Listener.AssetToMonitor as TA; } }

    /// <summary>
    /// Called with new value of the data within the custom asset
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    /// <param name="value">Reference to the changed value</param>
    protected abstract bool OnChange(TD value);

    /// <inheritdoc />
    ///  <summary>
    /// On a change the listener needs a copy of the changed data to react to
    ///  </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    /// <returns>True of all ok (either equals or no change error</returns>
    protected override bool OnChange(object[] _) { return Equals(Asset) || OnChange(Asset); }

    /// <summary>
    /// We need to compare value from target against change to see if we need to change
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected abstract bool Equals(TD value);
  }

  /// <inheritdoc />
  /// <summary>
  /// Internal Base class for component listeners that need to process the custom asset data.
  /// </summary>
  /// <typeparam name="TC">Type of component we are modifying on demand</typeparam>
  public abstract class ComponentListenerBase<TC> : ListenerComponent
    where TC : Object {
    /// <summary>
    /// Component we are going to give the custom asset data to.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    [SerializeField, Tooltip("Optional")] protected TC Target;

    private void Awake() {
      if (Target == null) Target = GetComponent<TC>();

      if (Target == null) {
        Debug.LogErrorFormat("No target '{0}' on '{1}'", typeof(TC).Name, Objects.Path(gameObject));
        Deregister();
      }
    }
  }
}