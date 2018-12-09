using Askowl;
using Decoupled;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#// <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Device/WebCam"), Labels("Device")]
  public class WebCamAsset : OfType<WebCamService> {
    private RawImage          rawImage;
    private AspectRatioFitter aspectRatioFitter;
    private int               lastVerticalMirror, lastRotationAngle = 1;
    private bool              ready;

    /// <a href=""></a> //#TBD#//
    public WebCamService Device {
      get => Value;
      private set => Value = value;
    }

    /// <a href="">The camera can be considered ready when it has done the first update</a> //#TBD#//
    public bool Ready => ready || (ready = Device.DidUpdateThisFrame);

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public override void Initialise() {
      Device = WebCamService.Instance;
    }

    /// <a href="">Given a canvas, project the camera image on it</a> //#TBD#//
    public void Project(GameObject background) {
      rawImage         = background.GetComponent<RawImage>() ?? background.AddComponent<RawImage>();
      rawImage.texture = Device.Texture;

      aspectRatioFitter = background.GetComponent<AspectRatioFitter>() ??
                          background.AddComponent<AspectRatioFitter>();

      aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;

      Device.Playing = true;
    }

    /// <a href="">When the phone is rotated the image can rotate to match. This changes the aspect ratio. Call this method occasionally to make the changes.</a> //#TBD#//
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