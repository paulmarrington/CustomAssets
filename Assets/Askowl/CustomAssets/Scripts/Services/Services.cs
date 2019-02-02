﻿// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Linq;
using Askowl;
using UnityEngine;

namespace CustomAsset.Services {
  /// <a href="">Separate selection and service from context for easy Inspector configuration</a> //#TBD#//
  public class Services<TS, TC> : Manager
    where TS : Services<TS, TC>.ServiceAdapter
    where TC : Services<TS, TC>.Context {
    /// <a href=""></a> //#TBD#//
    // ReSharper disable MissingXmlDoc
    public enum Order { TopDown, RoundRobin, Random, RandomExhaustive }
    // ReSharper restore MissingXmlDoc

    [SerializeField] private TS[]  services = default;
    [SerializeField] private TC    context  = default;
    [SerializeField] private Order order    = Order.TopDown;

    /// <a href="">Used in testing.</a> //#TBD#//
    public Selector<TS> selector;
    /// <a href=""></a> //#TBD#//
    public int usagesRemaining;
    /// <a href=""></a> //#TBD#//
    [HideInInspector] public TS currentService;

    /// <inheritdoc />
    protected override void Initialise() {
      // only use services that are valid for the current context
      var useful = services.Where(service => service.context.Equals(context) && service.IsExternalServiceAvailable());
      services = useful.ToArray();
      // service processing order is dependent on the priority each gives
      Array.Sort(array: services, comparison: (x, y) => x.priority.CompareTo(value: y.priority));

      selector = new Selector<TS> {
        IsRandom = order <= Order.RoundRobin, ExhaustiveBelow = order == Order.RandomExhaustive ? services.Length : 0
      };
    }

    /// <a href="">Get the next service instance given selection order and repetitions</a> //#TBD#//
    public ServiceAdapter Instance {
      get {
        if (--usagesRemaining > 0) return currentService;
        currentService  = selector.Pick();
        usagesRemaining = currentService.usageBalance;
        return currentService;
      }
    }

    /// <a href="">If the last service fails, ask for another. If none work, returns null</a> //#TBD#//
    public ServiceAdapter Next {
      get {
        currentService  = selector.Next();
        usagesRemaining = currentService.usageBalance;
        return currentService;
      }
    }

    /// <a href="">Parent class for decoupled services</a>
    [Serializable] public abstract class ServiceAdapter : Base {
      /// <a href=""></a> //#TBD#//
      [SerializeField] internal int priority = 1;
      /// <a href=""></a> //#TBD#//
      [SerializeField] internal int usageBalance = 1;
      /// <a href=""></a> //#TBD#//
      [SerializeField] public Context context = default;

      /// <a href=""></a> //#TBD#//
      protected Log.MessageRecorder Log;
      /// <a href=""></a> //#TBD#//
      protected Log.EventRecorder Error;

      /// <a href="">Concrete service implements this to prepare for action</a> //#TBD#//
      protected abstract void Prepare();

      /// <a href=""></a> //#TBD#//
      public abstract bool IsExternalServiceAvailable();

      /// <a href="">Registered with Emitter to provide common logging</a> //#TBD#/
      protected abstract void LogOnResponse(Emitter emitter);
      private Emitter.Action logOnResponse;

      /// <a href="">Override to initialise concrete service instances</a>
      protected override void Initialise() {
        base.Initialise();
        Log           = Askowl.Log.Messages();
        Error         = Askowl.Log.Errors();
        logOnResponse = LogOnResponse;
        Prepare();
      }

      /// <a href=""></a> //#TBD#//
      public interface ServiceDto {
        /// <a href="">Is default for no error, empty for no logging of a message else error message</a> //#TBD#//
        string ErrorMessage { get; set; }

        /// <a href=""></a> //#TBD#//
        Emitter Emitter { get; set; }

        /// <a href=""></a> //#TBD#//
        void Clear();
      }

      /// <a href=""></a> //#TBD#//
      protected virtual void Serve<T>(T dto) => throw new NotImplementedException();

      /// <a href=""></a> //#TBD#//
      public Emitter Call<T>() where T : Cached<T>, ServiceDto {
        T dto = Cached<T>.Instance; // disposed of when emitter dies (after one fire or on error)
        dto.Emitter      = Emitter.SingleFireInstance.Listen(logOnResponse).Context(dto);
        dto.ErrorMessage = default;
        dto.Clear();
        Serve(dto);
        if (dto.ErrorMessage == default) return dto.Emitter;
        dto.Emitter.Dispose();
        return null;
      }
    }

    /// <a href=""></a> //#TBD#//
    [Serializable] public class Context : Base {
      /// <a href="">Production, Staging, Test, Dev or user defined environment</a> //#TBD#//
      [SerializeField] private Environment environment = default;

      /// <a href=""></a> //#TBD#//
      protected virtual bool Equals(Context other) => base.Equals(other) && Equals(environment, other.environment);

      /// <inheritdoc />
      public override int GetHashCode() {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        unchecked { return (base.GetHashCode() * 397) ^ (environment != null ? environment.GetHashCode() : 0); }
        // ReSharper restore NonReadonlyMemberInGetHashCode
      }
    }
  }
}