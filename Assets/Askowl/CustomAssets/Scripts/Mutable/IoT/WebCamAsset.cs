using Askowl;
using Decoupled;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset.Mutable {
  /// <remarks><a href="http://unitydoc.marrington.net/Mars#asset-3">More...</a></remarks>
  /// <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Device/WebCam"), ValueName("Device")]
  public class WebCamAsset : OfType<WebCamService> {
    private RawImage          rawImage;
    private AspectRatioFitter aspectRatioFitter;
    private int               lastVerticalMirror, lastRotationAngle = 1;
    private bool              ready;

    /// <see cref="OfType{T}.Value"/>
    public WebCamService Device { get { return Value; } private set { Value = value; } }

    /// <summary>
    /// The camera can be considered ready when it has done the first update.
    /// </summary>
    public bool Ready => ready || (ready = Device.DidUpdateThisFrame);

    /// <inheritdoc />
    public override void Initialise() { Device = WebCamService.Instance; }

    /// <summary>
    /// Given a canvas, project the camera image on it
    /// </summary>
    public void Project(GameObject background) {
      rawImage         = background.GetComponent<RawImage>() ?? background.AddComponent<RawImage>();
      rawImage.texture = Device.Texture;

      aspectRatioFitter = background.GetComponent<AspectRatioFitter>() ??
                          background.AddComponent<AspectRatioFitter>();

      aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;

      Device.Playing = true;
    }

    /// <summary>
    /// When the phone is rotated the image can rotate to match. This changes
    /// the aspect ratio. Call this method occasionally to make the changes.
    /// </summary>
    public void CorrectForDeviceScreenOrientation() {
      aspectRatioFitter.aspectRatio = Device.AspectRatio;

      int verticalMirror = Device.VerticalMirror ? -1 : 1;

      if (verticalMirror != lastVerticalMirror) {
        rawImage.rectTransform.localScale = new Vector3(x: 1, y: verticalMirror, z: 1);
        lastVerticalMirror                = verticalMirror;
      }

      int rotationAngle = Device.RotationAngle;

      if (rotationAngle != lastRotationAngle) {
        rawImage.rectTransform.localEulerAngles = new Vector3(x: 0, y: 0, z: -rotationAngle);
        lastRotationAngle                       = rotationAngle;
      }
    }
  }
}