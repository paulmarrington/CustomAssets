// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Static helper class for setting fields in a compound object</a> //#TBD#//
  public static class Field {
    #region Generic Setters

    /// <a href="">Set a field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set<TF>(ref TF field, TF from) => Set(ref field, from, (a, b) => Equals(a, b));

    /// <a href="">Set a field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set<TF>(this WithEmitter asset, ref TF field, TF from) {
      if (Set(ref field, from)) asset.Emitter.Fire();
    }

    /// <a href="">Set a field inside a CustomAsset compound object where comparison is not straightforward. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set<TF>(ref TF field, TF from, Func<TF, TF, bool> equals) {
      if (equals(field, from)) return false;

      field = from;
      return true;
    }

    /// <a href="">Set a field inside a CustomAsset compound object where comparison is not straightforward. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set<TF>(this WithEmitter   asset,
                               ref  TF            field,
                               TF                 from,
                               Func<TF, TF, bool> equals) {
      if (Set(ref field, from, equals)) asset.Emitter.Fire();
    }

    #endregion

    #region Setters

    /// <a href="">Set a float field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set(ref float field, float from) => Set(ref field, from, Compare.AlmostEqual);

    /// <a href="">Set a float field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref float field, float from) =>
      asset.Set(ref field, from, Compare.AlmostEqual);

    /// <a href="">Set a double field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set(ref double field, double from) => Set(ref field, from, Compare.AlmostEqual);

    /// <a href="">Set a double field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref double field, double from) =>
      asset.Set(ref field, from, Compare.AlmostEqual);

    /// <a href="">Set a int field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set(ref int field, int from) => Set<int>(ref field, from);

    /// <a href="">Set a int field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref int field, int from) {
      asset.Set<int>(ref field, from);
    }

    /// <a href="">Set a long field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set(ref long field, long from) => Set<long>(ref field, from);

    /// <a href="">Set a long field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref long field, long from) {
      asset.Set<long>(ref field, from);
    }

    /// <a href="">Set a bool field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set(ref bool field, bool from) => Set<bool>(ref field, from);

    /// <a href="">Set a bool field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref bool field, bool from) {
      asset.Set<bool>(ref field, from);
    }

    /// <a href="">Set a float string inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static bool Set(ref string field, string from) => Set<string>(ref field, from);

    /// <a href="">Set a float string inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref string field, string from) {
      asset.Set<string>(ref field, from);
    }

    /// <a href="">Set a Vector2 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static bool Set(ref Vector2 field, Vector2 from) => Set(ref field, from, (a, b) => a == b);

    /// <a href="">Set a Vector2 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref Vector2 field, Vector2 from) =>
      asset.Set(ref field, from, (a, b) => a == b);

    /// <a href="">Set a Vector3 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static bool Set(ref Vector3 field, Vector3 from) => Set(ref field, from, (a, b) => a == b);

    /// <a href="">Set a Vector3 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref Vector3 field, Vector3 from) =>
      asset.Set(ref field, from, (a, b) => a == b);

    /// <a href="">Set a Vector4 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static bool Set(ref Vector4 field, Vector4 from) => Set(ref field, from, (a, b) => a == b);

    /// <a href="">Set a Vector4 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref Vector4 field, Vector4 from) =>
      asset.Set(ref field, from, (a, b) => a == b);

    /// <a href="">Set a Quaternion inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static bool Set(ref Quaternion field, Quaternion from) => Set(ref field, from, (a, b) => a == b);


    /// <a href="">Set a Quaternion inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change</a> //#TBD#//
    public static void Set(this WithEmitter asset, ref Quaternion field, Quaternion from) =>
      asset.Set(ref field, from, (a, b) => a == b);

    #endregion
  }
}