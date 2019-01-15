// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Services {
  /// <a href="">Separate selection and service from context for easy Inspector configuration</a> //#TBD#//
  public class Referent<TE, TS, TC> : Manager
    where TE : Referent<TE, TS, TC>.Elector
    where TS : Referent<TE, TS, TC>.Service
    where TC : Referent<TE, TS, TC>.Context {
    /// <a href="">Select service based on context and order</a> //#TBD#//
    [SerializeField] private TS elector = default;
    /// <a href="">Context the service is to run in (must include environment)</a> //#TBD#//
    [SerializeField] private TC context = default;

    /// <inheritdoc />
    protected override void Initialise() => elector.Prepare(context);

    /// <a href="">Class to call to select service based on context and order</a> //#TBD#//
    public class Elector : Base {
      /// <a href=""></a> //#TBD#//
      // ReSharper disable MissingXmlDoc
      public enum Order { Random, ExhaustiveRandom, Sequential, RoundRobin }
      // ReSharper restore MissingXmlDoc

      [SerializeField] private TS[]  services = default;
      [SerializeField] private Order order    = Order.Sequential;

      /// <a href=""></a> //#TBD#//
      [HideInInspector] public TC context;

      // ReSharper disable once ParameterHidesMember
      internal void Prepare(TC context) { this.context = context; }
    }

    /// <a href="">Parent class for decoupled services</a>
    [Serializable] public class Service : Base {
      /// <a href=""></a> //#TBD#//
      [SerializeField] public int priority = 1;
      /// <a href=""></a> //#TBD#//
      [SerializeField] public int usageBalance = 1;

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

    /// <a href=""></a> //#TBD#//
    public class Context : Base {
      /// <a href="">Production, Staging, Test, Dev or user defined environment</a> //#TBD#//
      [SerializeField] private Enum environment = default;

      /// <a href=""></a> //#TBD#//
      protected bool Equals(Context other) => base.Equals(other) && Equals(environment, other.environment);

      /// <inheritdoc />
      public override int GetHashCode() {
        unchecked { return (base.GetHashCode() * 397) ^ (environment != null ? environment.GetHashCode() : 0); }
      }
    }
  }
}