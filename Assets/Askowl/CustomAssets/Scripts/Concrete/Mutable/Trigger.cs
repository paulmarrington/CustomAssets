// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using CustomAsset.Mutable;

namespace CustomAsset {
  using UnityEngine;

  /// <inheritdoc cref="HasEmitter" />
  /// <summary>
  /// Dynamic custom asset without any values. Use it to trigger and listen to events.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#trigger">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Trigger")]
  public class Trigger : ScriptableObject, HasEmitter {
    private Emitter emitter = new Emitter();

    /// <inheritdoc />
    public Emitter Emitter { get { return emitter; } }

    /// <summary>
    /// Call to fire off a Changed event, since we have no data to change...
    /// </summary>
    public void Fire() { emitter.Fire(); }
  }
}