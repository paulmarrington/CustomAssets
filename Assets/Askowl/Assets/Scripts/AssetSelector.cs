namespace Askowl {
  using UnityEngine;

  public class AssetSelector<T> : CustomAsset<T>, IPick<T> where T : Object {
    public T[] AssetList;

    public Selector<T> Select;

    public void OnEnable() { Select = new Selector<T>(choices: AssetList); }

    public T[] Assets { get { return AssetList; } }

    public virtual T Pick() { return Select.Pick(); }
  }
}