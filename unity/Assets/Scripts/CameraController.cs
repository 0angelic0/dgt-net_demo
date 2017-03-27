using UnityEngine; 
using System.Collections;

public class CameraController : MonoBehaviour {
	// This is whatever you set the game viewport width to at design time.  At this width whatever camera size you
	//  have set should be the optimal width (your game should look good at design time).
	public int DesignTimeScreenWidth = 640;

	// Same as above, but for height.
	public int DesignTimeScreenHeight = 960;

	// Max amount, in world units, that you want to allow to be cropped off the bottom of view in order to
	//  attempt to maintain the fixed width.
	//  Easy way to get this value:
	//   Drag camera up (increase Y value) as high as you want to allow (stop before anything important is
	//   hidden), then copy/paste the Y value of camera to this property.
	public float MaxCropWorldUnits = 1;

	// The ortho size of your camera should be set to whatever looks best at design time.  The value will
	//  be saved in Awake and used for calculations in Update.
	float DesignTimeOrthographicSize;

	// See Awake for this simple calculation.
	float DesignTimeAspectRatio;

	void Awake()
	{
		DesignTimeOrthographicSize = GetComponent<Camera>().orthographicSize;

		// We just calculate the design-time aspect ratio based on width and height settings.  We could remove width
		//  and height and just let you enter the aspect ratio, but this way is easier for most people.
		DesignTimeAspectRatio = (float)DesignTimeScreenWidth / DesignTimeScreenHeight;
	}

	void Update ()
	{
		// Calculate the current aspect ratio.
		float aspectRatio = (float)Screen.width / Screen.height;

		// I don't know a good way to explain this calculation.  Because of the way Unity auto-adjusts to keep a
		//  fixed height when the aspect ratio changes -- it's complicated (for me anyway).
		//  This gives us the correct camera size to maintain a fixed width, but we might adjust below.
		float size = (float)DesignTimeOrthographicSize/((float)aspectRatio/DesignTimeAspectRatio);
//		SKDebuger.Log(DesignTimeOrthographicSize + "/" + aspectRatio + "/" + DesignTimeAspectRatio + "=" + size);

		// Calculate the new camera Y location.  This is simply the difference between our design-time ortho size
		//  and the new size we calculated above.
		float cameraY = DesignTimeOrthographicSize - size;

		// This is the amount, in world units, that will cropped from the bottom of the screen.
		//  We use *2 because the camera size is also changing by an equal amount, which also causes cropping,
		//   so *2 accounts for both the position and size cropping.
		float cropAmount = cameraY * 2;

		// If new size is going to crop more than our max...
		if (cropAmount > MaxCropWorldUnits)
		{
			// Set cameraY to half of our max (since position is half of the cropping).
			cameraY = MaxCropWorldUnits / 2;

			// Adjust size by same amount we adjust cameraY (since size is the other half of cropping).
			size = DesignTimeOrthographicSize - cameraY;
		}
		/*else if (cropAmount < MaxExpandWorldUnits)
         {
          * We could have another variable for max expansion, but to avoid that we would have to let the width get
          * narrower, and that will not work for a lot of game designs, so not adding extra code for it.
          * NOTE: cropAmount will be negative if height is increasing (taller than design time aspect ratio)
         }*/

		// Set new camera size and Y position.  We set local position so you can always place the camera inside
		//  another game object and use that to adjust Y if you want a camera that isn't Y=0 based but do not
		//  want to modify this script to support that.  Since we don't adjust X this should work for a side-scroller
		//  type camera as well.
		GetComponent<Camera>().orthographicSize = size;
//		Vector3 v3 = camera.transform.localPosition;
//		v3.y = cameraY;
//		camera.transform.localPosition = v3;
	}
//	// Use this for initialization
//	void Start () {
//		Vector2 screenPosition = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
//		SKDebuger.Log ("x: " + screenPosition.x + " y: " + screenPosition.y);
////		Camera.main.orthographicSize = 1136f / screenPosition.x * screenPosition.y / 2f;
////		Camera.main.orthographicSize = 640.0f / screenPosition.x * screenPosition.y / 2f;
////		Camera.main.orthographicSize = 320.0f/Screen.width * Screen.height/2f;
////		float orthoWidth = 480f / 1136f * 640f;
////		SKDebuger.Log ("orthoWidth: " + orthoWidth);
////		Camera.main.orthographicSize = orthoWidth / screenPosition.x * screenPosition.y;
////		this.camera.aspect = this.camera.camera.pixelWidth / this.camera.camera.pixelHeight;
//		Screen.SetResolution (640, 960, false);
//		float screenRatio = (Screen.width / Screen.height);
//		SKDebuger.Log ("width: " + Screen.width + " heigh: " + Screen.height);
//		this.camera.orthographicSize = screenRatio;// * (Screen.height / Screen.width);
//	}
}
