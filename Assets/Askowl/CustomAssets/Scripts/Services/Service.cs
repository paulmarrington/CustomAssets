using System;
using Askowl;
using CustomAsset;
using UnityEngine;

namespace Decoupled {
  /// <a href="">Parent class for decoupled services</a>
  [Serializable] public class Service : Manager {
    /// <a href=""></a> //#TBD#//
    [SerializeField] public int priority = 1;
    /// <a href=""></a> //#TBD#//
    [SerializeField] public int usageBalance = 1;

    /// <a href="">Set if mock instance to stop any other version overriding it</a>
    [NonSerialized] protected static bool HasMockInstance;
    /// <a href=""></a> //#TBD#//
    [NonSerialized] protected Emitter Emitter;
    /// <a href=""></a> //#TBD#//
    [NonSerialized] protected Log.MessageRecorder Log;
    /// <a href=""></a> //#TBD#//
    [NonSerialized] protected Log.EventRecorder Error;

    /// <a href="">Override to initialise concrete service instances</a>
    protected override void Initialise() {
      base.Initialise();
      Emitter = new Emitter();
      Log     = Askowl.Log.Messages();
      Error   = Askowl.Log.Errors();
    }
  }

  /// <a href="">Parent class for any decoupled service</a> <inheritdoc />
  public abstract class Service<T> : Service {
    /// <a href=""></a> //#TBD#//
    public abstract bool Has(T state);
  }
}