using Askowl;
using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="Decoupled.GyroService" />
  [CreateAssetMenu(menuName = "Custom Assets/Device/Gyroscope"), ValueName("Device")]
  public class GyroAsset : OfType<GyroService> {
    [SerializeField] private float smoothing = 0.2f;

    public static GyroAsset Instance { get { return Instance<GyroAsset>(); } }

    /// <see cref="OfType{T}.Value"/>
    public GyroService Device { get { return Value; } set { Value = value; } }

    private float  settleTime;
    private bool   settled;
    private Tetrad rotateFrom = new Tetrad(), rotateTo = new Tetrad(), rotation = new Tetrad();

    public bool Ready {
      get {
        if (settled) return true;

        if (Device.Attitude == Quaternion.identity) return false;

        rotateFrom.Set(Device.Attitude);
        settleTime = Time.realtimeSinceStartup - settleTime;
        return (settled = true);
      }
    }

    public float SecondsSettlingTime { get { return settleTime; } }

    /*
     * Jose Rodríguez-Rosa and Jorge Martín-Gutiérrez / Procedia Computer Science 25 (2013) 436 – 442
     * page 438:
     *
     * However, we encountered some issues when using gyroscope input data since it is very device dependent:
     * Sometimes gyroscope attitude readings are noisy, update slowly or require a quaternion correction if
     * the device screen orientation is changed (e.g. from Portrait to Landscape)[1].
     *
     * This was addressed using a quaternion spherical linear interpolation (slerp) between the current
     * camera attitude and the desired one (which might be multiplied by a second quaternion to correct the
     * screen orientation if needed) using a step factor of 0.2. This value was determined empirically and
     * effectively makes the interpolation to act as a low-pass filter, stabilizing the gyroscope reading
     * while keeping enough responsiveness for a good user experience. The sampling rate used for this sensor
     * was 15Hz (higher rates shortens device battery).
     *
     * In our tests, we found gyroscope on Apple iPhone smartphone to be very stable and accurate over time;
     * on the other hand, Android it ́s very device dependent, but having the low-pass filter implemented as
     * stated above, effectively fixes this problem on most devices.
     */
    public Tetrad Attitude {
      get {
        rotateTo.Set(Device.Attitude).RightToLeftHanded();
        rotation.Slerp(rotateFrom, rotateTo, smoothing);
        rotateFrom.Set(rotateTo);
        return rotation;
      }
    }

    /// <inheritdoc />
    public override GyroService Initialise() {
      Device     = GyroService.Instance;
      settleTime = Time.realtimeSinceStartup;
      return Device;
    }
  }
}