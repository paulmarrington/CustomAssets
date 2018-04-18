#if UNITY_EDITOR
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "Examples/Sound Clips", fileName = "Clips", order = 1)]
public sealed class ClipsSample : SelectAsset<AudioClip> {
  [SerializeField] private AudioClip[] clips;

  private readonly Random random = new Random();

  [UsedImplicitly]
  public void Play() {
    AudioSource.PlayClipAtPoint(clip: clips[random.Next(minValue: 0, maxValue: clips.Length)],
                                position: new Vector3(x: 0, y: 0, z: 0));
  }
}
#endif