using Askowl;
using UnityEngine;

namespace CustomAsset {
  public class Base : ScriptableObject {
    /// <a href="">If this is a project asset, then you will need to reference it somewhere. Other classes can get a reference using `Instance()` or `Instance(string name)`. Also useful for creating in-memory versions to share between hosts</a> //#TBD#//
    public static T Instance<T>(string name) where T : Base => Objects.Find<T>(name);

    [SerializeField, Multiline] private string description = default;

    /// <a href="">Editor only description of what the asset is all about</a> //#TBD#//
    public string Description => description;
  }
}