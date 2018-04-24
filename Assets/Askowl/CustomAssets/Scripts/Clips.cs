namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Create an asset to store a list of sounds and play one randomly or cyclicly.
  /// </summary>
  [CreateAssetMenu(menuName = "Custom Assets/Sound Clips", fileName = "Clips")]
  public sealed class Clips : Selector<AudioClip> {
    /// <summary>
    /// Play a random, exhaustive random or sequential sound.
    /// </summary>
    [UsedImplicitly]
    public void Play() { AudioSource.PlayClipAtPoint(clip: Pick(), position: Vector3.zero); }
  }
}