// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Services {
  /// Services Manager resides in project hierarchy to load and initialise service management
  [CreateAssetMenu(menuName = "Custom Assets/Services/ServiceExample/Service", fileName = "ServiceExampleServicesManager")]
  public class ServiceExampleServicesManager : Services<ServiceExampleServiceAdapter, ServiceExampleContext> { }
}