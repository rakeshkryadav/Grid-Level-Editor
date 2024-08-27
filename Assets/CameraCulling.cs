using UnityEngine;

public class CameraCulling : MonoBehaviour
{
    [SerializeField] private GameObject[] voxelObjectList;      // Array of voxels.
    [SerializeField] private new Camera camera;                 // Main Camera.

    // Method to return if voxel is visible to the camera or not.
    private bool IsVisible(Camera camera, GameObject voxel)
    {
        // Adding all the faces of geometry/3D model which are visible in the camrea.
        Plane[] geometry = GeometryUtility.CalculateFrustumPlanes(camera);
        Vector3 posititon = voxel.transform.position;

        foreach (Plane face in geometry){
            if (face.GetDistanceToPoint(posititon) < 0){
                return false;
            }
        }
        return true;
    }

    private void Update ()
    {
        // Add all the gameobjects which are tagged as Voxel.
        voxelObjectList = GameObject.FindGameObjectsWithTag("Voxel");

        // Foreach loop to check each voxel.
        foreach(GameObject voxel in voxelObjectList){

            // Calling is visible method and checking the returned condition.
            if (IsVisible(camera, voxel)){
                // Enable the mesh renderer of voxel if it is visible to the camera.
                voxel.GetComponent<MeshRenderer>().enabled = true;
            }
            else{
                // Disable the mesh renderer of voxel if it is not visible to the camera.
                voxel.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
