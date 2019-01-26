// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEditor;

namespace CustomAsset.Services {
  /// <a href=""></a><inheritdoc /> //#TBD#//
  public abstract class TemplateServiceAdapter : Services<TemplateServiceAdapter, TemplateContext>.ServiceAdapter {
    #region Service Support
    /// <a href=""></a> //#TBD#//
    public class Result {
      // enter all result data references here

      /// <a href="">Is default for no error, empty for no logging of a message else error message</a> //#TBD#//
      public string ErrorMessage;
      internal virtual void Clear() => ErrorMessage = default;
    }

    /// <a href=""></a> //#TBD#//
    protected override void Prepare() { }

    protected override void LogOnResponse(Emitter emitter) {
      var result = Result<Result>(emitter);
      if (result.ErrorMessage != default) {
        if (!string.IsNullOrEmpty(result.ErrorMessage)) Error($"Service Error: {result.ErrorMessage}");
      } else {
        Log("Warning", $"LogOnResponse for '{GetType().Name}");
      }
    }

    // Code that is common to all services belongs here
    #endregion

    #region Public Interface
    // Methods calling code will use to call a service - over and above concrete interface methods below ones defined below.
    #endregion

    #region Service Interface Methods
    // List of virtual interface methods that all concrete service adapters need to implement.

    // **************** Start of TemplateServiceMethod **************** //
    /// <a href=""></a> //#TBD#//
    public class TemplateServiceMethodResult : Result { }
    /// <a href=""></a> //#TBD#//
    public Emitter TemplateServiceMethod() {
      var emitter = GetAnEmitter<TemplateServiceMethodResult>();
      var result  = Result<TemplateServiceMethodResult>(emitter);
      result.Clear();
      TemplateServiceMethod(emitter, result);
      return result.ErrorMessage == default ? emitter : null;
    }
    /// <a href=""></a> //#TBD#//
    protected abstract void TemplateServiceMethod(Emitter emitter, TemplateServiceMethodResult result);
    // **************** End of TemplateServiceMethod **************** //
    #endregion

    #region Compiler Definition
    #if TemplateServiceFor
    public override bool IsExternalServiceAvailable() => true;
    #else
    public override bool IsExternalServiceAvailable() => false;
    #endif

    [InitializeOnLoadMethod] private static void DetectService() {
      bool usable = DefineSymbols.HasPackage("") || DefineSymbols.HasFolder("");
      DefineSymbols.AddOrRemoveDefines(addDefines: usable, named: "TemplateServiceFor");
    }
    #endregion
  }
}