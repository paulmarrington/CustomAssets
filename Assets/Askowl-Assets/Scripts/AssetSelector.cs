using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AssetSelector<T>: CustomAsset<T>, Pick<T> where T : UnityEngine.Object {
  public T[] assets;

  public Selector<T> Select;

  public void OnEnable() {
    Select = new Selector<T> (assets);
  }

  public T[] Assets {
    get { return assets; }
  }

  public virtual T Pick() {
    return Select.Pick();
  }
}