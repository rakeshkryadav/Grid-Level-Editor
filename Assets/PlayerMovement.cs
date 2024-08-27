using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int movementSpeed = 10;        // movement speed;
    [SerializeField] float jumpForce = 2.5f;                // jump force;
    new Rigidbody rigidbody;
    Vector3 jump;

    void Start(){
        rigidbody = gameObject.GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }
    void Update()
    {
        // Move the player forward or backward.
        transform.Translate(0f, 0f, Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);;

        // Rotate the player right or left side.
        transform.Translate(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0f, 0f);

        // If Space key is pressed player will jump.
        if(Input.GetKeyDown(KeyCode.Space)){
            rigidbody.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
    }
}
