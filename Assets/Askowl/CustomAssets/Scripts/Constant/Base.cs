using Askowl;
using UnityEngine;

namespace CustomAsset {
  /// <a href="http://bit.ly/2CwSS8S">Base class for all custom assets - implementing initialisation</a>
  public class Base : ScriptableObject {
    /// <a href="http://bit.ly/2CzuMKF">If this is a project asset, then you will need to reference it somewhere. Other classes can get a reference using `Instance()` or `Instance(string name)`. Also useful for creating in-memory versions to share between hosts</a>
    public static T Instance<T>(string name) where T : Base => Objects.Find<T>(name);

    [SerializeField, Multiline] private string description = default;

    /// <a href="http://bit.ly/2CwSVS6">Editor only description of what the asset is all about</a>
    public string Description => description;

    /// <a href="http://bit.ly/2CzuMKF"></a>
    protected bool Initialised;

    /// <a href="http://bit.ly/2CzuMKF">Called by Managers MonoBehaviour</a>
    public void Initialiser() {
      if (Initialised) return;
      Initialised = true;
      Initialise();
    }
    /// <a href="http://bit.ly/2CzuMKF">Called by Managers MonoBehaviour or when mutual data is first accessed</a>
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