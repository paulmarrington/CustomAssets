// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEditor;

namespace CustomAsset.Services {
  /// <a href=""></a><inheritdoc /> //#TBD#//
  public abstract class TemplateServiceAdapter : Services<TemplateServiceAdapter, TemplateContext>.ServiceAdapter {
    #region Service Support
    // Code that is common to all services belongs here
    #endregion

    #region Public Interface
    // Methods calling code will use to call a service - over and above abstract ones defined below.
    #endregion

    #region Abstract Service Interface Methods
    // List of abstract methods that all concrete service adapters need to implement
    #endregion

    #region Compiler Definition
    [InitializeOnLoadMethod] private static void DetectService() {
      bool usable = DefineSymbols.HasPackage("") || DefineSymbols.HasFolder("");
      DefineSymbols.AddOrRemoveDefines(addDefines: usable, named: "TemplateServiceFor");
    }
    #endregion
  }
}