// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using CustomAsset.Mutable;
using UnityEngine;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(
    menuName = "Custom Assets/Services/ServiceExample/ServiceForMock", fileName = "ServiceExampleServiceForMock")]
  public class ServiceExampleServiceForMock : ServiceExampleServiceAdapter {
    [SerializeField] private String mockState = default;

    /// <a href="">Prepare the mock service for operations</a> //#TBD#//
    protected override void Prepare() => base.Prepare();

    /// <a href="">Use Log and Error to record analytics based on service responses</a> //#TBD#//
    protected override void LogOnResponse(Emitter emitter) => base.LogOnResponse(emitter);

    public override Emitter Call(Service<AddDto> service) {
      switch (mockState.Text) {
        case "Success":
          service.Dto.result = service.Dto.request.firstValue + service.Dto.request.secondValue;
          Fiber.Start.WaitFor(seconds: 0.1f).Fire(service.Emitter);
          return service.Emitter;
        case "ServiceFailure":
          service.ErrorMessage = "Service Failure";
          return null;
        default:
          service.ErrorMessage = $"Unknown mock state {mockState.Text}";
          return null;
      }
    }
    /// <inheritdoc />
    public override bool IsExternalServiceAvailable() => true;
  }
}