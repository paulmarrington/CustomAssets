#if UNITY_EDITOR
using Askowl;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Examples/Sound Clips", fileName = "Clips", order = 1)]
public sealed class ClipsSample : CustomAsset<AudioClip> {
  public AudioClip[] Clips;

  private readonly System.Random random = new System.Random();

  [UsedImplicitly]
  public void Play() {
    AudioSource.PlayClipAtPoint(clip: Clips[random.Next(minValue: 0, maxValue: Clips.Length)],
                                position: new Vector3(x: 0, y: 0, z: 0));
  }
}
#endif