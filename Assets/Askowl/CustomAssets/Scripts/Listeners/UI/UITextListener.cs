// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Drop into the same Game Object as a text component to update text content whenever the string custom asset changes</a> //#TBD#// <inheritdoc />
  [RequireComponent(typeof(Textual))] public sealed class UiTextListener : StringListener<Textual> {
    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnChange(string value) => Target.text = value;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override bool Equals(string value) => (Target.text == value);
  }
}