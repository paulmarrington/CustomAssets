using System;
using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="Decoupled.GyroService" />
  [CreateAssetMenu(menuName = "Custom Assets/Device/Compass"), ValueName("Device")]
  public class CompassAsset : OfType<CompassService> {
    public static CompassAsset Instance { get { return Instance<CompassAsset>(); } }

    /// <see cref="OfType{T}.Value"/>
    public CompassService Device { get { return Value; } set { Value = value; } }

    private float                    settleTime;
    private bool                     settled;
    private Quaternion               magneticHeading;
    private float                    rotateFrom, rotateTo;
    private float                    lastUpdateTime;
    private ExponentialMovingAverage ema;

    public bool Ready {
      get {
        if (settled) return true;
        if (Time.realtimeSinceStartup < settleTime) return false;

        ema        = new ExponentialMovingAverage(16);
        rotateFrom = rotateTo = Device.MagneticHeading;

        return (settled = true);
      }
    }

    /*
     * Jose Rodríguez-Rosa and Jorge Martín-Gutiérrez / Procedia Computer Science 25 (2013) 436 – 442
     * page 439:
     *
     * Another more efficient method is the exponential moving average...
     *
     * This method requires fewer operations and gives much better results when filtering noise from
     * compass sensor input data. This formula is computed in constant time. We empirically determined
     * the value of parameter=0.2 as an optimal setting for this method.
     */
    public void Calibrate() {
      // Use the exponential moving average to help smooth out compass variations
      rotateFrom     = rotateTo;
      rotateTo       = ema.AverageAngle(Device.MagneticHeading);
      lastUpdateTime = Time.realtimeSinceStartup;
    }

    /*
     * Jose Rodríguez-Rosa and Jorge Martín-Gutiérrez / Procedia Computer Science 25 (2013) 436 – 442
     * page 439:
     *
     * The Sn value is the horizontal rotation (around the Y axis) in degrees. This value is converted
     * to a quaternion and interpolated (using slerp as done before with the gyroscope stabilization method)
     * with the previous value using the number of seconds since the last reading as interpolation step
     * parameter (since this method is executed several times per second, this is a fractional value between 0 and 1).
     */
    public float MagneticHeading {
      get {
        float elapsedSeconds = Time.realtimeSinceStartup - lastUpdateTime;
        return Mathf.LerpAngle(rotateFrom, rotateTo, elapsedSeconds);
      }
    }

    /// <inheritdoc />
    public override void Initialise() {
      settleTime = Time.realtimeSinceStartup + 1;
      Device     = CompassService.Instance;
    }
  }
}