using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Examples/Sound Clips", fileName = "Clips", order = 1)]
public sealed class ClipsSample: CustomAsset<AudioClip> {
  public AudioClip[] Clips;

  private readonly System.Random random = new System.Random ();

  [UsedImplicitly]
  public void Play() {
    AudioSource.PlayClipAtPoint(Clips [random.Next(0, Clips.Length)], new Vector3 (0, 0, 0));
  }
}