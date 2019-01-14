//// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages
//
//using Askowl;
//using Decoupled;
//using UnityEngine;
//
//namespace CustomAsset.Mutable {
//  /// <a href="http://bit.ly/2RbzvKP">Retrieve compass heading</a> <inheritdoc cref="Decoupled.GyroService" />
//  [CreateAssetMenu(menuName = "Custom Assets/Device/Compass"), Labels("Device")]
//  public class CompassAsset : OfType<CompassService> {
//    /// <a href="http://bit.ly/2RbzvKP">Retrieve the singleton reference to the asset. Use Unity [SerializeField] by preference</a>
//    public static CompassAsset Instance => Instance<CompassAsset>();
//
//    /// <a href="http://bit.ly/2RbzvKP">Access to the underlying service. <see cref="OfType{T}.Value"/></a>
//    public CompassService Device { get => Value; set => Value = value; }
//
//    private float                    settleTime;
//    private bool                     settled;
//    private Quaternion               magneticHeading;
//    private float                    rotateFrom, rotateTo;
//    private float                    lastUpdateTime;
//    private ExponentialMovingAverage ema;
//
//    /// <a href="http://bit.ly/2RbzvKP">Poll to see if the compass is ready and settled.</a>
//    public bool Ready {
//      get {
//        if (settled) return true;
//        if (Time.realtimeSinceStartup < settleTime) return false;
//
//        ema        = new ExponentialMovingAverage(64);
//        rotateFrom = rotateTo = Device.MagneticHeading;
//
//        return (settled = true);
//      }
//    }
//
//    /// <summary>Poll to build exponential moving average of the magnetic heading</summary>
//    /// <remarks>
//    /// Jose Rodríguez-Rosa and Jorge Martín-Gutiérrez / Procedia Computer Science 25 (2013) 436 – 442
//    /// <para/>
//    /// page 439:
//    /// Another more efficient method is the exponential moving average...
//    /// <para/>
//    /// This method requires fewer operations and gives much better results when filtering noise from
//    /// compass sensor input data. This formula is computed in constant time. We empirically determined
//    /// the value of parameter=0.2 as an optimal setting for this method.
//    /// </remarks>
//    /// <a href="http://bit.ly/2RbzvKP">Poll to build exponential moving average of the magnetic heading</a> //#TBD#//
//    public void Calibrate() {
//      // Use the exponential moving average to help smooth out compass variations
//      rotateFrom     = rotateTo;
//      rotateTo       = ema.AverageAngle(Device.MagneticHeading);
//      lastUpdateTime = Time.realtimeSinceStartup;
//    }
//
//    /// <summary>
//    /// Use time and moving average to retrieve a smoothed guess at magnetic heading
//    /// </summary>
//    /// <remarks>
//    /// Jose Rodríguez-Rosa and Jorge Martín-Gutiérrez / Procedia Computer Science 25 (2013) 436 – 442
//    /// <para/>
//    /// page 439:
//    /// <para/>
//    /// The Sn value is the horizontal rotation (around the Y axis) in degrees. This value is converted
//    /// to a quaternion and interpolated (using slerp as done before with the gyroscope stabilization method)
//    /// with the previous value using the number of seconds since the last reading as interpolation step
//    /// parameter (since this method is executed several times per second, this is a fractional value between 0 and 1).
//    /// </remarks>
//    /// <a href="http://bit.ly/2RbzvKP">Use time and moving average to retrieve a smoothed guess at magnetic heading</a> //#TBD#//
//    public float MagneticHeading {
//      get {
//        float elapsedSeconds = Time.realtimeSinceStartup - lastUpdateTime;
//        return Mathf.LerpAngle(rotateFrom, rotateTo, elapsedSeconds);
//      }
//    }
//
//    /// <a href="http://bit.ly/2RbzvKP">Point custom asset to the device driver</a> <inheritdoc />
//    protected override void Initialise() {
//      settleTime = Time.realtimeSinceStartup + 1;
//      Device     = CompassService.Instance;
//    }
//  }
//}