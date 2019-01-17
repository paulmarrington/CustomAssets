// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Services.Test {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Test/Service", fileName = "TestSelector")]
  public class Service : Referent<Elector, Service, Context>.Service { }
}