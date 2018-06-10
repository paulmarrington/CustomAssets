/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

namespace CustomAsset {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Dynamic custom asset without any values. Use it to trigger and listen to events.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#trigger">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Trigger")]
  public class Trigger : Base {
    /// <summary>
    /// Call to fire off a Changed event, since we have no data to change...
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public void Fire() { Changed(); }
  }
}