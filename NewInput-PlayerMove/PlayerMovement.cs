using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] InputActionReference move_action;
    [SerializeField] InputActionReference run_action;
    [SerializeField] InputActionReference interact_action;
	CharacterController controller;

    public Animator RendererAnimator;

    public bool isMovementBlocked;

    //public static float speed = 12f;
    public float gravity = -13f;
    public float jump = 1f;



    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;
    public LayerMask layerMask;

    //public GameObject CameraControllerCM;
    bool runned;

    float x, z;

    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();

        cam = Camera.main.transform;
    }

    private void OnEnable() {
        run_action.action.started += delegate {runned = true;};
        run_action.action.canceled += delegate {runned = false;};
        interact_action.action.started += Interact;
    }

    private void OnDisable() {
        run_action.action.started -= delegate {runned = true;};
        run_action.action.canceled += delegate {runned = false;};
        interact_action.action.started -= Interact;
    }

    void Interact(InputAction.CallbackContext callback)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3, layerMask);
            if (hitEnemies.Length > 0)
            {
                
                    //UIContainer._instance.InterractButton.GetComponent<Animator>().SetTrigger("Interract");
                    hitEnemies[0].GetComponent<Interactable>().OnInteract?.Invoke();
            }
    }

    // Update is called once per frame
    void Update()
    {
         isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        

        if (!isMovementBlocked) {
            float SpeedAccelerator = 1;
            //Vector2 readed = move_action.action.ReadValue<Vector2>().x;
            x = move_action.action.ReadValue<Vector2>().x;
             z = move_action.action.ReadValue<Vector2>().y;


            //Camera change
            
            
                //cam.localPosition = new Vector3(cam.localPosition.x, 1, cam.localPosition.z);
                //cam.GetComponent<CameraMovement>().enabled = false;
            
            //END CAMERA

            Vector3 move;
                move = new Vector3(x,0,z).normalized;

                if (move.magnitude >= 0.1f)
                {
                    

                    float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                    move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                }
            
			
			if (runned && move.magnitude >= 0.1f)
            {
					RendererAnimator.SetBool("Running", true);
                    //SpeedAccelerator = Player.playerRace.raceSpeed + BufferStats.playerSpeedDelta + 8;
                    //if (isGrounded)
                    {
                        SpeedAccelerator = 2;
                    }
            }
            else
            {
                
                RendererAnimator.SetBool("Running", false);
                SpeedAccelerator = 1;
            }
			
			
			
            controller.Move(move.normalized * SpeedAccelerator * Time.deltaTime * 12);

            if (move.magnitude > 0)
            {
                RendererAnimator.SetBool("Walking", true);
            }
            else
            {
                RendererAnimator.SetBool("Walking", false);
            }

            /*if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //velocity.y = Mathf.Sqrt((jump + GetComponent<BufferStats>().JumpDelta) * -2f * gravity);
            }*/
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);



        }

}
