﻿// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Constant {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Integer CustomAsset contains an int value. Add listeners to your own classes with Register(this).
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#primitive-custom-assets">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Integer")]
  public sealed class Integer : OfType<int> { }
}