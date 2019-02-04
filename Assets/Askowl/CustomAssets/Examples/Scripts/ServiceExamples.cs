// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages
#if UNITY_EDITOR && CustomAssets

using System.Collections;
using CustomAsset;
using CustomAsset.Mutable;
using CustomAsset.Services;
using NUnit.Framework;
using UnityEngine.TestTools;
// ReSharper disable MissingXmlDoc

namespace Askowl.Examples {
  public class ServiceExamples : PlayModeTests {
    [UnityTest] public IEnumerator Basic() {
      var mockState = Manager.Load<String>("MockState.asset");

      var manager = Manager.Load<ServiceExampleServicesManager>("ServiceExampleServicesManager.asset");
      var mathServer  = (ServiceExampleServiceAdapter) manager.Instance;
      var addService = mathServer.Service<ServiceExampleServiceAdapter.AddDto>();
      if (addService.Error) Assert.Fail(addService.ErrorMessage);

      var dto = addService.Dto;
      dto.request = new ServiceExampleServiceAdapter.AddDto.Request {firstValue = 21, secondValue = 22};
      dto.result  = 0; // don't need to set this except to ensure test validity

      yield return Fiber.Start
                        .WaitFor(mathServer.Call(addService))
                        .Do(_ => Assert.AreEqual(43, dto.result))
                        .AsCoroutine();
    }
  }
}
#endif