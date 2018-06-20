// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Constant {
  /// <inheritdoc />
  /// <summary>
  /// Support class for audio clip playing.
  /// </summary>
  [Serializable]
  public sealed class AudioClipSet : Set<AudioClip> {
    [SerializeField, Header("Audio")]     private Range volume   = new Range(1, 1);
    [SerializeField, RangeBounds(0, 2)]   private Range pitch    = new Range(1, 2);
    [SerializeField, RangeBounds(0, 999)] private Range distance = new Range(1, 999);

    /// <summary>
    /// Find an AudioSource to use for playing the sounds
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#audioclips">More...</a></remarks>
    /// <see cref="Play(AudioSource)"/>
    /// <param name="gameObject">GameObject instance with an attached audio source</param>
    public void Play(GameObject gameObject) { Play(gameObject.GetComponent<AudioSource>()); }

    /// <summary>
    /// Play a random, exhaustive random or sequential sound - with random variations of volume, pitch and distance heard.
    /// </summary>
    /// <see cref="Play(GameObject)"/>
    /// <remarks><a href="http://customassets.marrington.net#audioclips">More...</a></remarks>
    public void Play(AudioSource source) {
      source.clip        = Pick();
      source.pitch       = pitch.Pick();
      source.volume      = volume.Pick();
      source.minDistance = distance.Min;
      source.maxDistance = distance.Max;
      source.Play();
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// Create an asset to store a list of sounds and play one randomly or cyclicly.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#audioclips">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Audio Clips", fileName = "AudioClips")]
  public sealed class AudioClips : OfType<AudioClipSet> {
    /// <summary>
    /// Retrieve a reference to a named AudioClips asset
    /// </summary>
    /// <param name="name">Name of asset</param>
    public new static AudioClips Instance(string name) { return Instance<AudioClips>(name); }

    /// <summary>
    /// Audio Clip Player <see cref="AudioClipSet"/>
    /// </summary>
    public AudioClipSet Picker { get { return Value; } }
  }
}