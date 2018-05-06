namespace CustomAsset {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Base class for listeners that need a float parameter
  /// </summary>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class FloatListener<T> :
    ComponentListenerBase<T, Float, float> where T : Object { }

  /// <inheritdoc />
  /// <summary>
  /// Base class for listeners that need an integer parameter
  /// </summary>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class IntegerListener<T> :
    ComponentListenerBase<T, Integer, int> where T : Object { }

  /// <inheritdoc />
  /// <summary>
  /// Base class for listeners that need an boolean parameter
  /// </summary>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class BooleanListener<T> :
    ComponentListenerBase<T, Boolean, bool> where T : Object { }

  /// <inheritdoc />
  /// <summary>
  /// Internal Base class for component listeners that need to process the custom asset data.
  /// </summary>
  /// <typeparam name="TC">Type of component we are modifying on demand</typeparam>
  /// <typeparam name="TA">Type of custom asset</typeparam>
  /// <typeparam name="TD">Type of data contained in the custom asset</typeparam>
  public abstract class ComponentListenerBase<TC, TA, TD> : ComponentListenerBase<TC>
    where TC : Object where TA : OfType<TD> {
    /// <inheritdoc />
    ///  <summary>
    /// On a change the listener needs a copy of the changed data to react to
    ///  </summary>
    public override void OnTriggered() { Change((TD) ((TA) CustomAsset)); }

    /// <summary>
    /// Called with new value of the data within the custom asset
    /// </summary>
    /// <param name="value">Reference to the changed value</param>
    protected abstract void Change(TD value);
  }

  /// <inheritdoc />
  /// <summary>
  /// Internal base class for adding component functionality
  /// </summary>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class ComponentListenerBase<T> : Listener where T : Object {
    /// <summary>
    /// Component we are going to give the custom asset data to.
    /// </summary>
    protected T Component;

    private void Awake() {
      Component = GetComponent<T>();

      if (Component == null) CustomAsset.Deregister(this);
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// Converts custom asset to a string for components that deal with string data.
  /// </summary>
  /// <typeparam name="T">Type of component we are modifying on demand</typeparam>
  public abstract class StringListener<T> : ComponentListenerBase<T> where T : Object {
    /// <summary>
    /// Called with new value of the data within the custom asset
    /// </summary>
    /// <param name="value">Reference to a string representation of the changed value</param>
    protected abstract void Change(string value);

    /// <inheritdoc />
    /// <summary>
    /// Good for any listener that can deal with the changed data as a string. Changing a float, for example will still need a string representation to display
    /// </summary>
    public override void OnTriggered() { Change(CustomAsset.ToString()); }
  }
}