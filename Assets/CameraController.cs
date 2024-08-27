using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject parent;                  // Parent gameobject of camera.
    [SerializeField] private float mouseSensitivity = 100f;     // mouse drag sensitivity.
    [SerializeField] private float maxZoomIn = 10f;
    [SerializeField] private float maxZoomOut = 100f;
    private float panRotation;                                  // mouse pan rotation.

    void Update()
    {
        CameraPan();
        CameraZoom();
    }

    void CameraPan(){
        // Getting mouse X and Y coordinates.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Initializing the X coordinates and limiting it between -60 to 60 digree.
        panRotation -= mouseY;
        panRotation = Mathf.Clamp(panRotation, -60f, 60f);

        // Rotating camera Up or Down in its local axis.
        transform.localRotation = Quaternion.Euler(panRotation, 0f, 0f);

        // Roation camera pareant Right or Left.
        parent.transform.Rotate(Vector3.up * mouseX);
    }

    void CameraZoom(){

        // Initializing Field of View of Main Camera.
        float zoom = Camera.main.fieldOfView;

        // Mouse Scroll binding for zoom-in and zoom-out.
        zoom += Input.GetAxis("Mouse ScrollWheel") * 20f * Time.deltaTime;
        // Key bindig for zoom-in and zoom-out.
        if(Input.GetKey(KeyCode.Z)){
            zoom += 20f * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.X)){
            zoom -= 20f * Time.deltaTime;
        }

        // Range of Zoom.
        zoom = Mathf.Clamp(zoom, maxZoomIn, maxZoomOut);
        Camera.main.fieldOfView = zoom;
    }
}
