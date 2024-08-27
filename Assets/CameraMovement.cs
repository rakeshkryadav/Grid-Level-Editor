using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] int movementSpeed = 10;        // movement speed.
    GameObject player;                              // player gameobject.

    void Start(){
        // Check for player.
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        // if player found else if not then execute the self movement statement.
        if(player){
            // Look to the player
            transform.LookAt(player.transform);
            Vector3 playerPosition = player.transform.position;

            // Follow the player position and rotation.
            transform.position = new Vector3(playerPosition.x, playerPosition.y + 3f, playerPosition.z -5f);
            transform.rotation = player.transform.rotation;
        }
        else{
            // Move the camera forward or backward.
            transform.Translate(0f, 0f, Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);;

            // Move the camera right or left side.
            transform.Translate(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0f, 0f);
        }
    }
}
