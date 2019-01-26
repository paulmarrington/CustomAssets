// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;
#if TemplateServiceFor
// Add using statements for service library here
#endif

namespace CustomAsset.Services {
  /// <a href=""></a><inheritdoc /> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Template/Service", fileName = "TemplateServiceFor")]
  public abstract class TemplateServiceFor : TemplateServiceAdapter {
    #if TemplateServiceFor
    protected override void Prepare() => base.Prepare();

    protected override void LogOnResponse(Emitter emitter) => base.LogOnResponse(emitter);

    // Implement all interface methods that call concrete service adapters need to implement

    /// <a href=""></a> //#TBD#//
    protected override void TemplateServiceMethod(Emitter emitter, TemplateServiceMethodResult result) {
      // Access the external service here. Save and call emitter.Fire when service call completes
      // or set result.ErrorMessage if the service call fails to initialise
    }

    #endif
  }
}