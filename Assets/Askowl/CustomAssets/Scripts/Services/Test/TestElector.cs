// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Test/Selector", fileName = "TestSelector")]
  public class TestElector : Referent<TestElector, TestService, TestContext>.Elector { }
}