using System;
using System.Collections;
using System.Collections.Generic;
using Askowl;
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Create an asset to store a list of sounds and play one randomly or cyclicly.
  /// </summary>
  /// <remarks><a href="http://customasset.marrington.net#sound-clips">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Audio Clips", fileName = "AudioClips")]
  public sealed class AudioClips : Set<AudioClip> {
    [SerializeField, Header("Audio")]     private Range volume   = new Range(1, 1);
    [SerializeField, RangeBounds(0, 2)]   private Range pitch    = new Range(1, 2);
    [SerializeField, RangeBounds(0, 999)] private Range distance = new Range(1, 999);

    /// <summary>
    /// Used for AudioClipsLoader or any other method to provide a reference to an audio source
    /// </summary>
    public AudioSource AudioSource { private get; set; }

    /// <summary>
    /// Play a random, exhaustive random or sequential sound - with random variations of volume, pitch and distance heard.
    /// Plays the audio with an already attached AudioSource
    /// </summary>
    /// <remarks>
    /// Play a random, exhaustive random or sequential sound - with random variations of volume, pitch and distance heard.
    /// </remarks>
    [UsedImplicitly]
    public void Play() { Play(AudioSource); }

    /// <summary>
    /// Find an AudioSource to use for playing the sounds
    /// </summary>
    /// <see cref="Play()"/>
    /// <param name="gameObject">GameObject instance with an attached audio source</param>
    [UsedImplicitly]
    public void Play(GameObject gameObject) { Play(gameObject.GetComponent<AudioSource>()); }

    /// <summary>
    /// Given an audio source, play my sound
    /// </summary>
    /// <see cref="Play()"/>
    /// <remarks><a href="http://customasset.marrington.net#sound-clips">More...</a></remarks>
    [UsedImplicitly]
    public void Play(AudioSource source) {
      source.clip        = Pick();
      source.pitch       = pitch.Pick();
      source.volume      = volume.Pick();
      source.minDistance = distance.Min;
      source.maxDistance = distance.Max;
      source.Play();
    }
  }
}