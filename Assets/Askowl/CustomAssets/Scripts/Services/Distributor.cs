// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset;
using UnityEngine;

namespace Decoupled {
  /// <a href=""></a> //#TBD#//
  public class Distributor<TS, TD> : Manager where TS : Service {
    /// <a href=""></a> //#TBD#//
    // ReSharper disable MissingXmlDoc
    public enum Order { Random, ExhaustiveRandom, Sequential, RoundRobin }
    // ReSharper restore MissingXmlDoc

    [SerializeField] private TS[]  services = default;
    [SerializeField] private Order order    = Order.Sequential;
  }
}