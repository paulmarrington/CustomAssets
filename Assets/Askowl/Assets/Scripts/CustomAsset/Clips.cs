namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  [CreateAssetMenu(menuName = "Custom Assets/Sound Clips", fileName = "Clips", order = 1)]
  public sealed class Clips : Select<AudioClip> {
    [UsedImplicitly]
    public void Play() { AudioSource.PlayClipAtPoint(clip: Pick(), position: Vector3.zero); }
  }
}