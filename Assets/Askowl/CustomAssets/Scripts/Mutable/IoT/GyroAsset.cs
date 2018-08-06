using Askowl;
using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <remarks><a href="http://unitydoc.marrington.net/Mars#asset-2">More...</a></remarks>
  /// <inheritdoc cref="Decoupled.GyroService" />
  [CreateAssetMenu(menuName = "Custom Assets/Device/Gyroscope"), ValueName("Device")]
  public class GyroAsset : OfType<GyroService> {
    [SerializeField, Tooltip("Used in Slerp to reduce jitter")]
    private float smoothing = 0.2f;

    /// <summary>
    /// Retrieve service singleton. By preference use the inspector.
    /// </summary>
    public static GyroAsset Instance => Instance<GyroAsset>();

    /// <see cref="OfType{T}.Value"/>
    public GyroService Device { get { return Value; } set { Value = value; } }

    private float  settleTime;
    private bool   settled;
    private Tetrad rotateFrom = new Tetrad(), rotateTo = new Tetrad(), rotation = new Tetrad();

    /// <summary>
    /// Poll at startup to see of the gyroscope is ready to use
    /// </summary>
    public bool Ready {
      get {
        if (settled) return true;

        if (Device.Attitude == Quaternion.identity) return false;

        rotateFrom.Set(Device.Attitude);
        settleTime = Time.realtimeSinceStartup - settleTime;
        return (settled = true);
      }
    }

    /// <summary>
    /// The number of seconds that the Gyroscope took to settle
    /// </summary>
    public float SecondsSettlingTime => settleTime;

    /// <summary>
    /// The smoothed orientation in space of the device as a Tetrad (Quaternion)
    /// </summary>
    /// <remarks>
    /// Jose Rodríguez-Rosa and Jorge Martín-Gutiérrez / Procedia Computer Science 25 (2013) 436 – 442
    /// <para/>
    /// page 438:
    /// <para/>
    /// However, we encountered some issues when using gyroscope input data since it is very device dependent:
    /// Sometimes gyroscope attitude readings are noisy, update slowly or require a quaternion correction if
    /// the device screen orientation is changed (e.g. from Portrait to Landscape)[1].
    /// <para/>
    /// This was addressed using a quaternion spherical linear interpolation (slerp) between the current
    /// camera attitude and the desired one (which might be multiplied by a second quaternion to correct the
    /// screen orientation if needed) using a step factor of 0.2. This value was determined empirically and
    /// effectively makes the interpolation to act as a low-pass filter, stabilizing the gyroscope reading
    /// while keeping enough responsiveness for a good user experience. The sampling rate used for this sensor
    /// was 15Hz (higher rates shortens device battery).
    /// <para/>
    /// In our tests, we found gyroscope on Apple iPhone smartphone to be very stable and accurate over time;
    /// on the other hand, Android it ́s very device dependent, but having the low-pass filter implemented as
    /// stated above, effectively fixes this problem on most devices.
    ///</remarks>
    public Tetrad Attitude {
      get {
        rotateTo.Set(Device.Attitude).RightToLeftHanded();
        rotation.Slerp(rotateFrom, rotateTo, smoothing);
        rotateFrom.Set(rotateTo);
        return rotation;
      }
    }

    /// <inheritdoc />
    public override void Initialise() {
      Device     = GyroService.Instance;
      settleTime = Time.realtimeSinceStartup;
    }
  }
}