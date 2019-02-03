// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages
#if UNITY_EDITOR && CustomAssets

using System.Collections;
using CustomAsset;
using CustomAsset.Services;
using NUnit.Framework;
using UnityEngine.TestTools;
// ReSharper disable MissingXmlDoc

namespace Askowl.Examples {
  public class ServiceExamples : PlayModeTests {
    [UnityTest] public IEnumerator Basic() {
      var manager = Manager.Load<ServiceExampleServicesManager>("ServiceExampleServicesManager.asset");
      var service = manager.Instance.Service<ServiceExampleServiceAdapter.AddDto>();

      var dto = service.Dto;
      dto.request = new ServiceExampleServiceAdapter.AddDto.Request {firstValue = 21, secondValue = 22};
      dto.result  = 0; // don't need to set this except to ensure test validity

      yield return Fiber.Start
                        .WaitFor(service.Call())
                        .Do(_ => Assert.AreEqual(43, dto.result))
                        .AsCoroutine();
    }
  }
}
#endif