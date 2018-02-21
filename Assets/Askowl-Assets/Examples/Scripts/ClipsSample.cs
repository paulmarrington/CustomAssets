using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Examples/Sound Clips", fileName = "Clips", order = 1)]
public class ClipsSample: CustomAsset<AudioClip> {
  public AudioClip[] clips;

  private System.Random random = new System.Random ();

  public void OnEnable() {
  }

  public void Play() {
    AudioSource.PlayClipAtPoint(clips [random.Next(0, clips.Length)], new Vector3 (0, 0, 0));
  }
}