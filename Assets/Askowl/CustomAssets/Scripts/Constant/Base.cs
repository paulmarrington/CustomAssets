using Askowl;
using UnityEngine;

namespace CustomAsset {
  public class Base : ScriptableObject {
    /// <a href="">If this is a project asset, then you will need to reference it somewhere. Other classes can get a reference using `Instance()` or `Instance(string name)`. Also useful for creating in-memory versions to share between hosts</a> //#TBD#//
    public static T Instance<T>(string name) where T : Base => Objects.Find<T>(name);

    [SerializeField, Multiline] private string description = default;

    /// <a href="">Editor only description of what the asset is all about</a> //#TBD#//
    public string Description => description;

    /// <a href=""></a> //#TBD#//
    protected bool Initialised;

    /// <a href="">Called by Managers MonoBehaviour</a> //#TBD#//
    public void Initialiser() {
      if (Initialised) return;
      Initialised = true;
      Initialise();
    }
    /// <a href="">Called by Managers MonoBehaviour or when mutual data is first accessed</a> //#TBD#//
    protected virtual void Initialise() { }

    protected virtual void OnEnable() {
      AssetsWaitingInitialisation.Push(this);
      if (AssetsWaitingInitialisation.Count == 1) InitialiseAssetEmitter.Fire();
    }
    protected static readonly Emitter    InitialiseAssetEmitter      = Emitter.Instance;
    protected static readonly Fifo<Base> AssetsWaitingInitialisation = Fifo<Base>.Instance;

    protected virtual void OnDisable() => Initialised = false;
  }
}