// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/ServiceExample/Context", fileName = "ServiceExampleContext")]
  public class ServiceExampleContext : Services<ServiceExampleServiceAdapter, ServiceExampleContext>.Context {
    /// <a href=""></a> //#TBD#//
    protected bool Equals(ServiceExampleContext other) => base.Equals(other);
  }
}