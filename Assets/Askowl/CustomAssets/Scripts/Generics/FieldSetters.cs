﻿// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;

namespace CustomAsset.Mutable {
  using UnityEngine;

  /// <summary>
  /// Static helper class for setting fields in a compound object
  /// </summary>
  public static class FieldSetters {
    #region Generic Setters
    /// <summary>
    /// Set a field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change.
    /// </summary>
    /// <param name="field">ref myCustomAsset.aField</param>
    /// <param name="from">Value to set the field to if all checks pass</param>
    /// <typeparam name="TF">Anything that is a direct field in the CustomAsset</typeparam>
    public static bool Set<TF>(ref TF field, TF from) {
      return Set(ref field, from, (a, b) => Equals(a, b));
    }

    /// <summary>
    /// Set a field inside a CustomAsset compound object where comparison is not straightforward. It checks for read/write and that the field is different before triggering a change.
    /// </summary>
    /// <param name="field">ref myCustomAsset.aField</param>
    /// <param name="from">Value to set the field to if all checks pass</param>
    /// <param name="equals">Comparison operator. Returns true if the items are equal or close enough</param>
    /// <typeparam name="TF">Anything that is a direct field in the CustomAsset</typeparam>
    public static bool Set<TF>(ref TF field, TF from, Func<TF, TF, bool> equals) {
      if (equals(field, from)) return false;

      field = from;
      return true;
    }
    #endregion

    #region Setters
    /// <summary>
    /// Set a float field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref float myCustomAsset.aField to update</param>
    /// <param name="from">float to set the field to if all checks pass</param>
    public static bool Set(ref float field, float from) {
      return Set(ref field, from, AlmostEqual);
    }

    /// <summary>
    /// Set a double field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref double myCustomAsset.aField to update</param>
    /// <param name="from">double to set the field to if all checks pass</param>
    public static bool Set(ref double field, double from) {
      return Set(ref field, from, AlmostEqual);
    }

    /// <summary>
    /// Set a int field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref int myCustomAsset.aField to to update</param>
    /// <param name="from">int to set the field to if all checks pass</param>
    public static bool Set(ref int field, int from) { return Set<int>(ref field, from); }

    /// <summary>
    /// Set a long field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref long myCustomAsset.aField to update</param>
    /// <param name="from">long to set the field to if all checks pass</param>
    public static bool Set(ref long field, long from) { return Set<long>(ref field, from); }

    /// <summary>
    /// Set a bool field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref bool myCustomAsset.aField to update</param>
    /// <param name="from">bool to set the field to if all checks pass</param>
    public static bool Set(ref bool field, bool from) { return Set<bool>(ref field, from); }

    /// <summary>
    /// Set a float string inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    public static bool Set(ref string field, string from) { return Set<string>(ref field, from); }

    /// <summary>
    /// Set a Vector2 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    public static bool Set(ref Vector2 field, Vector2 from) {
      return Set(ref field, from, (a, b) => a == b);
    }

    /// <summary>
    /// Set a Vector3 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    public static bool Set(ref Vector3 field, Vector3 from) {
      return Set(ref field, from, (a, b) => a == b);
    }

    /// <summary>
    /// Set a Vector4 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    public static bool Set(ref Vector4 field, Vector4 from) {
      return Set(ref field, from, (a, b) => a == b);
    }

    /// <summary>
    /// Set a Quaternion inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    public static bool Set(ref Quaternion field, Quaternion from) {
      return Set(ref field, from, (a, b) => a == b);
    }
    #endregion

    #region Comparators
    /// <summary>
    /// Check two floating point numbers to be within rounding tolerance.
    /// </summary>
    public static bool AlmostEqual(float a, float b) { return Math.Abs(a - b) < 1e-5; }

    /// <summary>
    /// Check two double floating point numbers to be within rounding tolerance.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static bool AlmostEqual(double a, double b) { return Math.Abs(a - b) < 1e-5; }
    #endregion
  }
}