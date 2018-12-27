using Askowl;
using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2RbzvKP"></a> <inheritdoc cref="Decoupled.GpsService" />
  [CreateAssetMenu(menuName = "Custom Assets/Device/GPS"), Labels("Device")]
  public class GpsAsset : OfType<GpsService> {
    /// <a href="http://bit.ly/2RbzvKP">Access to the underlying service. <see cref="OfType{T}.Value"/></a>
    public GpsService Device { get => Value; set => Value = value; }

    /// <a href="">Poll to see if GPS is ready to use - take care, it may be still settling</a>
    public bool Ready => Device.Running;

    /// <a href="http://bit.ly/2RbzvKP">Poll to see if the GPS is still initialising</a>
    public bool Initialising => Device.Initialising;

    /// <a href="http://bit.ly/2RbzvKP">The GPS is offline if we cannot access or enable it</a>
    public bool Offline => Device.Offline;

    /// <a href="http://bit.ly/2RbzvKP">Retrieve the current location in Geodetic coordinates</a>
    public Geodetic.Coordinates Here => Location(Device.Location);

    /// <a href="http://bit.ly/2RbzvKP">Convert device service coordinates to Geodetic ones for further calculations</a>
    public static Geodetic.Coordinates Location(GpsService.LocationData here) =>
      Geodetic.Coords(here.Latitude, here.Longitude);

    /// <a href="http://bit.ly/2RbzvKP"></a> <inheritdoc />
    protected override void Initialise() => Device = GpsService.Instance;
  }
}