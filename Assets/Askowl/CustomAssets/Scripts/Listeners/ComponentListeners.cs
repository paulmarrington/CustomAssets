// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset {
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
    ComponentListenerBase<T, String, string> where T : Object {
    /// <inheritdoc />
    protected override void OnChange(string memberName) {
      Change((memberName == null) ? Asset.ToString() : Asset.ToStringForMember(memberName));
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// Internal Base class for component listeners that need to process the custom asset data.
  /// </summary>
  /// <typeparam name="TC">Type of component we are modifying on demand</typeparam>
  /// <typeparam name="TA">Type of custom asset</typeparam>
  /// <typeparam name="TD">Type of data contained in the custom asset</typeparam>
  public abstract class ComponentListenerBase<TC, TA, TD> : ListenerComponent<TA>
    where TC : Object where TA : OfType<TD> {
    /// <summary>
    /// Component we are going to give the custom asset data to.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    protected TC Component;

    /// <summary>
    /// Called with new value of the data within the custom asset
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    /// <param name="value">Reference to the changed value</param>
    protected abstract void Change(TD value);

    /// <inheritdoc />
    ///  <summary>
    /// On a change the listener needs a copy of the changed data to react to
    ///  </summary>
    /// <remarks><a href="http://customassets.marrington.net#generic-component-listeners">More...</a></remarks>
    protected override void OnChange(string memberName) {
      Change((memberName == null) ? (TD) Asset : Asset[memberName]);
    }

    private void Awake() {
      Component = GetComponent<TC>();
      if (Component == null) Deregister();
    }
  }
}