// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages
#if AskowlTests
using System;
using System.Collections;
using Askowl.Gherkin;
using CustomAsset;
using CustomAsset.Services;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using String = CustomAsset.Mutable.String;
// ReSharper disable MissingXmlDoc

namespace Askowl.CustomAssets.Examples {
  public class ServiceExamples : PlayModeTests {
    /// Sample service call
    private Emitter CallService() {
      // Load the service manager for this service type. You can cache this. It does not change (except for testing)
      var manager = Manager.Load<ServiceExampleServicesManager>($"{serviceManagerName}ServicesManager.asset");
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

    private IEnumerator ServiceTest(string label) {
      yield return Feature.Go("CustomAssetsDefinitions", featureFile: "Services", label).AsCoroutine();
    }

    [UnityTest] public IEnumerator TopDownSuccess()  { yield return ServiceTest("@TopDownSuccess"); }
    [UnityTest] public IEnumerator TopDownFailure()  { yield return ServiceTest("@TopDownFailure"); }
    [UnityTest] public IEnumerator TopDownFallback() { yield return ServiceTest("@TopDownFallback"); }

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
      return Fiber.Start.WaitFor(CallService())
                  .Do(_ => Assert.AreEqual(expected, addService.Dto.response))
                  .OnComplete;
    }

    [Step(@"^we get a service error$")] public Emitter ServiceError() {
      return Fiber.Start.WaitFor(CallService())
                  .Do(_ => Assert.IsTrue(addService.Error))
                  .OnComplete;
    }

    [Step(@"^a service message of ""(.*)""$")] public void ServiceMessage(string[] matches) {
      Assert.AreEqual(expected: matches[0], actual: addService.ErrorMessage);
    }

    [Step(@"^we use service (\d+)$")] public Emitter WeUseService(string[] matches) {
      var serviceNumber = int.Parse(matches[0]);
    }

    /*
    [Step(@"^$")] public void () { }
    */
  }
}
#endif