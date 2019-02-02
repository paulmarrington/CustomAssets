// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System.Collections;
using Askowl;
using CustomAsset;
using CustomAsset.Services;
using UnityEngine.TestTools;
// ReSharper disable MissingXmlDoc

namespace Tests {
  public class ServiceExamples : PlayModeTests {
    [UnityTest] public IEnumerator Basic() {
      var manager = Manager.Load<ServiceExampleServicesManager>("ServiceExampleServicesManager");
      var service = manager.Instance;
      var emitter = service.Call<Services<ServiceExampleServiceAdapter, ServiceExampleContext>.ServiceAdapter.ServiceDto>();
      yield return null;
    }
  }
}