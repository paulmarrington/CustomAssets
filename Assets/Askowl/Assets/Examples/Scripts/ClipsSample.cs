#if UNITY_EDITOR
using Askowl;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Examples/Sound Clips", fileName = "Clips", order = 1)]
public sealed class ClipsSample : CustomAsset.Select<AudioClip> {
  [SerializeField] private AudioClip[] clips;

  private readonly System.Random random = new System.Random();

  [UsedImplicitly]
  public void Play() {
    AudioSource.PlayClipAtPoint(clip: clips[random.Next(minValue: 0, maxValue: clips.Length)],
                                position: new Vector3(x: 0, y: 0, z: 0));
  }
}
#endif