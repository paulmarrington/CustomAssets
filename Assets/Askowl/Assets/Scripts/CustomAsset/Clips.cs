namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  [CreateAssetMenu(menuName = "Custom Assets/Sound Clips", fileName = "Clips")]
  public sealed class Clips : SelectAsset<AudioClip> {
    [UsedImplicitly]
    public void Play() { AudioSource.PlayClipAtPoint(clip: Pick(), position: Vector3.zero); }
  }
}