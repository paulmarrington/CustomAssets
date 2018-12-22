using Askowl;
using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#// <inheritdoc cref="Decoupled.GpsService" />
  [CreateAssetMenu(menuName = "Custom Assets/Device/GPS"), Labels("Device")]
  public class GpsAsset : OfType<GpsService> {
    /// <a href="">Access to the underlying service. <see cref="OfType{T}.Value"/></a> //#TBD#//
    public GpsService Device { get => Value; set => Value = value; }

    /// <a href="">Poll to see if GPS is ready to use - take care, it may be still settling</a> //#TBD#//
    public bool Ready => Device.Running;

    /// <a href="">Poll to see if the GPS is still initialising</a> //#TBD#//
    public bool Initialising => Device.Initialising;

    /// <a href="">The GPS is offline if we cannot access or enable it</a> //#TBD#//
    public bool Offline => Device.Offline;

    /// <a href="">Retrieve the current location in Geodetic coordinates</a> //#TBD#//
    public Geodetic.Coordinates Here => Location(Device.Location);

    /// <a href="">Convert device service coordinates to Geodetic ones for further calculations</a> //#TBD#//
    public static Geodetic.Coordinates Location(GpsService.LocationData here) =>
      Geodetic.Coords(here.Latitude, here.Longitude);

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void Initialise() => Device = GpsService.Instance;
  }
}