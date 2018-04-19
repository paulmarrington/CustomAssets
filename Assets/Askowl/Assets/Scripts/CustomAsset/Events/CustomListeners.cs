using System;

namespace Events {
  using UnityEngine;
  using CustomAsset;

  public abstract class StringListener<T> :
    ComponentListener<T, String, string> where T : Object { }

  public abstract class FloatListener<T> :
    ComponentListener<T, Float, float> where T : Object { }

  public abstract class IntegerListener<T> :
    ComponentListener<T, Integer, int> where T : Object { }

  public abstract class BooleanListener<T> :
    ComponentListener<T, Boolean, bool> where T : Object { }

  public abstract class ComponentListener<TC, TA, TD> : BaseListener
    where TC : Object where TA : CustomAsset<TD> {
    [SerializeField] protected TA CustomAsset;
    [SerializeField] protected TC Component;

    private Action changer;

    private void Awake() {
      if (Component == null) Component = GetComponent<TC>();

      if (Component != null) {
        changer = () => Change(CustomAsset.Value);
      } else {
        changer = () => { };
      }
    }

    public override void OnTriggered(Listener _) { changer(); }

    protected abstract void Change(TD value);
  }
}