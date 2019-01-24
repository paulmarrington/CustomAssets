// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Template/ServiceForMock", fileName = "TemplateServiceForMock")]
  public class TemplateServiceForMock : TemplateServiceAdapter {
    /// <a href="">Prepare the mock service for operations</a> //#TBD#//
    protected override void Prepare() => throw new NotImplementedException();

    /// <a href="">Use Log and Error to record analytics based on service responses</a> //#TBD#//
    protected override void LogOnResponse() => throw new NotImplementedException();

    /// <inheritdoc />
    public override bool IsExternalServiceAvailable() => true;
  }
}