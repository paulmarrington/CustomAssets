// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEditor;
using UnityEngine;
#if TemplateServiceFor
// Add using statements for service library here
#endif

namespace CustomAsset.Services {
  /// <a href=""></a><inheritdoc /> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Template/Service", fileName = "TemplateServiceFor")]
  public abstract class TemplateServiceImplementation : TemplateServiceAdapter {
    #region Concrete implementations of Abstract Service Interface Methods
    // Implement all abstract methods that call concrete service adapters need to implement
    #endregion

    #region Service Library Access
    #if TemplateServiceFor
    public override bool IsExternalServiceAvailable() => true;
    // Add any code that accesses the service library here
    #else
    public override bool IsExternalServiceAvailable() => false;
    #endif
    #endregion

    #region Compiler Definition
    [InitializeOnLoadMethod] private static void DetectService() {
      bool usable = DefineSymbols.HasPackage("") || DefineSymbols.HasFolder("");
      DefineSymbols.AddOrRemoveDefines(addDefines: usable, named: "TemplateServiceFor");
    }
    #endregion
  }
}