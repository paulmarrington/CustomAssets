// With thanks to Jason Weimann  -- jason@unity3d.college

using Askowl;
using CustomAsset.Constant;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href="">Unity editor for AudioClip components. Adds a play button</a> //#TBD#//
  [CustomEditor(typeof(AudioClips))]
  public class AudioClipsEditor : PreviewEditor<AudioSource> {
    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void Preview() => ((AudioClips) target).Picker.Play(Source);
  }
}