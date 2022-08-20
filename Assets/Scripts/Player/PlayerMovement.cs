using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movement;

    [Header("Refrences")]
    public Camera playerCam;
    public GameObject mouseFollower;

    [Header("Player Stats")]
    public float speed = 5;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //move the player relative to camera rotation
        Vector3 targetDirection = new Vector3(movement.x * speed, 0, movement.y * speed);
        targetDirection = playerCam.transform.TransformDirection(targetDirection);
        targetDirection.y = rb.velocity.y;

        rb.velocity = targetDirection;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMovement(InputAction.CallbackContext ctx) {
        movement = ctx.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        //read the input value : returns mouse position in pixels
        Vector2 aim = ctx.ReadValue<Vector2>();

        //mouse and keyboard logic
        if (ctx.control.ToString().Contains("Mouse"))
        {

            //only detect objects in that layer which will only be used for mouse detection
            int layer_mask = LayerMask.GetMask("MouseRayField");

            RaycastHit hit;
            Ray ray = playerCam.ScreenPointToRay(new Vector3(aim.x, aim.y, 0));
            
            if (Physics.Raycast(ray, out hit, 500f, layer_mask))
            {
                // used for refrence only
                mouseFollower.transform.position = hit.point;

                // get the direction from the ray hit point and the player position than calculate the rotation for the player
                Vector3 direction = hit.point - transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            }

        }
        // gamepad logic
        else
        {
            
            // make the aim direction relative to the camera
            Vector3 targetRotation = new Vector3(aim.x, 0, aim.y);
            targetRotation = playerCam.transform.TransformDirection(targetRotation);
            targetRotation.y = 0;

            //Quaternion rotation = Quaternion.LookRotation(new Vector3(aim.x, 0, aim.y), Vector3.up);
            Quaternion rotation = Quaternion.LookRotation(targetRotation, Vector3.up);

            //to prevent the player from restting the last rotatoin looked at
            if (aim != Vector2.zero)
            {
                transform.rotation = rotation;
            }
            
        }

        
    }
}
