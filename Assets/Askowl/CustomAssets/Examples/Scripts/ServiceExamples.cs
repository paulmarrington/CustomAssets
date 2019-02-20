// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages
#if AskowlTests
using System.Collections;
using System.Collections.Generic;
using Askowl.Gherkin;
using CustomAsset;
using CustomAsset.Mutable;
using CustomAsset.Services;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
// ReSharper disable MissingXmlDoc

namespace Askowl.CustomAssets.Examples {
  public class ServiceExamples : PlayModeTests {
    /// Sample service call
    private Emitter CallService() {
      // Load the service manager for this service type. You can cache this. It does not change (except for testing)
      manager = Manager.Load<ServiceExampleServicesManager>($"{serviceManagerName}ServicesManager.asset");
      // Build Service DTO
      addService = Service<ServiceExampleServiceAdapter.AddDto>.Instance;
      // The DTO will have request data going in and response data coming back
      addService.Dto.request = (firstValue, secondValue);
      // Here we make the service call with fallback if available/necessary
      return manager.CallService(addService);
    }

    // ************ Everything below here is BDD and Test scaffolding ************

    private String                                       mockState;
    private int                                          firstValue, secondValue;
    private Service<ServiceExampleServiceAdapter.AddDto> addService;
    private string                                       serviceManagerName;
    private ServiceExampleServicesManager                manager;

    private IEnumerator ServiceTest(string label) {
      yield return Feature.Go("CustomAssetsDefinitions", featureFile: "Services", label).AsCoroutine();
    }

    [UnityTest] public IEnumerator TopDownSuccess()   { yield return ServiceTest("@TopDownSuccess"); }
    [UnityTest] public IEnumerator TopDownFailure()   { yield return ServiceTest("@TopDownFailure"); }
    [UnityTest] public IEnumerator TopDownFallback()  { yield return ServiceTest("@TopDownFallback"); }
    [UnityTest] public IEnumerator RoundRobin()       { yield return ServiceTest("@RoundRobin"); }
    [UnityTest] public IEnumerator Random()           { yield return ServiceTest("@Random"); }
    [UnityTest] public IEnumerator RandomExhaustive() { yield return ServiceTest("@RandomExhaustive"); }

    [Step(@"^a (\S+) stack with (\d+) services$")] public void ServerStack(string[] matches) =>
      serviceManagerName = matches[0];

    [Step(@"^server success of ""(.*)""$")] public void MockStateOf(string[] matches) {
      mockState      = Manager.Load<String>("MockState.asset");
      mockState.Text = matches[0];
    }

    [Step(@"^an add service on the math server$")] public void MathServer() { }

    [Step(@"^we add (\d+) and (\d+)$")] public void AddService(string[] matches) {
      firstValue  = int.Parse(matches[0]);
      secondValue = int.Parse(matches[1]);
    }

    [Step(@"^we will get a result of (\d+)$")] public Emitter AddResult(string[] matches) {
      var expected = int.Parse(matches[0]);
      return Fiber.Start.WaitFor(_ => CallService())
                  .Do(_ => Assert.AreEqual(expected, addService.Dto.response))
                  .OnComplete;
    }

    [Step(@"^we get a service error$")] public Emitter ServiceError() =>
      Fiber.Start.WaitFor(_ => CallService())
           .Do(_ => Assert.IsTrue(addService.Error))
           .OnComplete;

    [Step(@"^a service message of ""(.*)""$")] public void ServiceMessage(string[] matches) {
      Assert.AreEqual(expected: matches[0], actual: addService.ErrorMessage);
    }

    [Step(@"^we use service (\d+)$")] public Emitter WeUseService(string[] matches) {
      var serviceNumber = int.Parse(matches[0]);
      firstValue = secondValue = 0;
      return Fiber.Start.WaitFor(_ => CallService())
                  .Do(_ => Assert.AreEqual(serviceNumber, addService.Dto.response+1))
                  .OnComplete;
    }

    [Step(@"^we repeat the service call$")] public void RepeatServiceCall() { }

    [Step(@"^we will eventually get the same service number twice in a row$")]
    public Emitter TwiceInARow(string[] matches) {
      string services = "";
      firstValue = secondValue = 0;
      var fiber = Fiber.Start.Begin.Do(_ => services = "")
                       .Begin.WaitFor(_ => CallService())
                       .Do(_ => services += ((char) ('0' + addService.Dto.response)))
                       .Repeat(3)
                       .Until(_ => "0000 1111".Contains(services));
      return fiber.OnComplete;
    }

    [Step(@"^we will never get the same service number twice in a row$")]
    public Emitter NeverTwiceInARow(string[] matches) {
      string services = "";
      firstValue = secondValue = 0;
      var fiber = Fiber.Start.Begin.ExitOnError.Do(_ => services = "").Begin
                       .WaitFor(_ => CallService())
                       .Do(_ => services += ((char) ('0' + addService.Dto.response)))
                       .Repeat(3)
                       .Do(_ => Assert.IsFalse("0000 1111".Contains(services), services))
                       .Repeat(10);
      return fiber.OnComplete;
    }

    [Step(@"^each service is called (\d+) times in a row$")] public void InARow() { }

    [Step(@"^we get the same service twice cycling$")] public Emitter RepeatCycling() {
      string services = "";
      firstValue = secondValue = 0;
      var fiber = Fiber.Start.Begin
                       .WaitFor(_ => CallService())
                       .Do(_ => services += ((char) ('0' + addService.Dto.response)))
                       .Repeat(4).Do(_ => Assert.IsTrue("00110".Contains(services), services));
      return fiber.OnComplete;
    }

    /*
    [Step(@"^$")] public void () { }
    */
  }
}
#endif