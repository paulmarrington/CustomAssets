// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages
#if AskowlTests
using System.Collections;
using Askowl.Gherkin;
using CustomAsset;
using CustomAsset.Mutable;
using CustomAsset.Services;
using NUnit.Framework;
using UnityEngine.TestTools;
// ReSharper disable MissingXmlDoc

namespace Askowl.CustomAssets.Examples {
  public class ServiceExamples : PlayModeTests {
    [UnityTest] public IEnumerator TopDownSuccess() {
      addFiber = Fiber.Instance
                      .WaitFor(_ => mathServer.Call(addService))
                      .Do(_ => Assert.IsFalse(addService.Error, addService.ErrorMessage))
                      .Fire(_ => callCompleteEmitter);
    }
    private Fiber addFiber;

    [UnityTest] public IEnumerator TopDownSuccess1() {
      var mockState = Manager.Load<String>("MockState.asset");
      mockState.Text = "Success";

      var manager    = Manager.Load<ServiceExampleServicesManager>("ServiceExampleServicesManager.asset");
      var mathServer = (ServiceExampleServiceAdapter) manager.Instance;
      var addService = mathServer.Service<ServiceExampleServiceAdapter.AddDto>();

      var dto = addService.Dto;
      dto.request = new ServiceExampleServiceAdapter.AddDto.Request {firstValue = 21, secondValue = 22};
      dto.result  = 0; // don't need to set this except to ensure test validity

      yield return Fiber.Start
                        .WaitFor(mathServer.Call(addService))
                        .Do(_ => Assert.IsFalse(addService.Error, addService.ErrorMessage))
                        .Do(_ => Assert.AreEqual(43, dto.result))
                        .AsCoroutine();
    }

    [UnityTest] public IEnumerator ServiceError() {
      var mockState = Manager.Load<String>("MockState.asset");
      mockState.Text = "ServiceFailure";

      var manager    = Manager.Load<ServiceExampleServicesManager>("ServiceExampleServicesManager.asset");
      var mathServer = (ServiceExampleServiceAdapter) manager.Instance;
      var addService = mathServer.Service<ServiceExampleServiceAdapter.AddDto>();

      var dto = addService.Dto;
      dto.request = new ServiceExampleServiceAdapter.AddDto.Request {firstValue = 21, secondValue = 22};
      dto.result  = 0; // don't need to set this except to ensure test validity

      yield return Fiber.Start
                        .WaitFor(mathServer.Call(addService))
                        .Do(_ => Assert.IsFalse(addService.Error, addService.ErrorMessage))
                        .Do(_ => Assert.AreEqual(43, dto.result))
                        .AsCoroutine();
    }

    [UnityTest] public IEnumerator ServiceFallback() { yield return null; }

    [Step(@"^a mock state of ""(.*)""$")] public void MockStateOf(string[] matches) {
      mockState      = Manager.Load<String>("MockState.asset");
      mockState.Text = matches[0];
    }
    private String mockState;

    [Step(@"^an add service on the math server$")] public void MathServer() {
      var manager = Manager.Load<ServiceExampleServicesManager>("ServiceExampleServicesManager.asset");
      mathServer = (ServiceExampleServiceAdapter) manager.Instance;
      addService = mathServer.Service<ServiceExampleServiceAdapter.AddDto>();
    }
    private ServiceExampleServiceAdapter                 mathServer;
    private Service<ServiceExampleServiceAdapter.AddDto> addService;

    [Step(@"^we add (\d+) and (\d+)$")] public Emitter AddService(string[] matches) {
      dto                     = addService.Dto;
      dto.request.firstValue  = int.Parse(matches[0]);
      dto.request.secondValue = int.Parse(matches[1]);
      callCompleteEmitter     = Emitter.SingleFireInstance;
      addFiber.Go();
      return callCompleteEmitter;
    }
    private ServiceExampleServiceAdapter.AddDto dto;
    private Emitter                             callCompleteEmitter;

    [Step(@"^we will get a result of (\d+)$")] public void AddResult(string[] matches) {
      var expected = int.Parse(matches[0]);
      Assert.AreEqual(expected, dto.result);
    }
    /*
    [Step(@"^$")] public void () { }
    */
  }
}
#endif