using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Moves")]
    private bool isPaused = false;
    public Transform playerMovePoint;
    public float moveSpeed = 5f;
    public Vector2 moveDirection = Vector2.zero;
    public PlayerInputActionScript playerController;
    private InputAction move;
    private InputAction reveal;
    private InputAction flag;
    private InputAction pause;
    private bool axisInUse = false;

    [Header("Other")]
    public Vector3 originalPosition; // Keep track of original position in case of collision
    private BeatTrackerScript beatTracker;
    private DeleteSmallBeatScript missedBeatTracker; // Keep track of consecutive missed beats
    private Rigidbody2D rb;
    private Grid grid;

    [Header("Triggers")]
    [SerializeField] private UnityEvent moveTrigger;
    [SerializeField] private UnityEvent revealTrigger;
    [SerializeField] private UnityEvent flagTrigger;
    [SerializeField] private UnityEvent pauseTrigger;

    private void Awake()
    {
        originalPosition = transform.position;
        playerMovePoint.transform.position = transform.position;
        playerMovePoint.parent = null;
        rb = GetComponent<Rigidbody2D>();
        playerController = new PlayerInputActionScript();
        beatTracker = GameObject.Find("BeatTracker").GetComponent<BeatTrackerScript>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        missedBeatTracker = GameObject.Find("ExitBeatCollider").GetComponent<DeleteSmallBeatScript>();
    }

    private void OnEnable()
    {
        move = playerController.Player.Move;
        move.Enable();

        reveal = playerController.Player.Reveal;
        reveal.Enable();
        reveal.performed += Reveal; // Attach reveal event

        flag = playerController.Player.Flag;
        flag.Enable();
        flag.performed += Flag;

        pause = playerController.Player.Pause;
        pause.Enable();
        pause.performed += Pause;
    }

    public void OnDisable()
    {
        move.Disable();
        reveal.Disable();
        reveal.performed -= Reveal; // Prevent memory leaks upon re-enabling
        flag.Disable();
        flag.performed -= Flag;
        pause.Disable();
        pause.performed -= Pause;
    }

    // Upon hitting pause menu button
    public void SwitchPlayerToUI()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0; // Pause scene
            playerController.Player.Disable();
            playerController.UI.Enable();
        }
    }

    // Exiting pause menu
    public void SwitchUIToPlayer()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            playerController.UI.Disable();
            playerController.Player.Enable();
        }
    }

    // Detect input every frame
    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    // Movement logic ever fixed update
    private void FixedUpdate()
    {
        // If movepoint as changed, move player position to that movepoint
        transform.position = Vector3.MoveTowards(transform.position, playerMovePoint.position, moveSpeed * Time.deltaTime);

        // If movepoint has changed, attempt logic to move a tile and update movepoint
        // Only done when the beat is currently pulsing
        if ((Vector2.Distance(transform.position, playerMovePoint.position) <= .5f) && beatTracker.isSmallerBeatColliding == true)
        {
            if ((((Mathf.Abs(moveDirection.x)) == 1) || (Mathf.Abs(moveDirection.x) == -1)) && (axisInUse == false))
            {
                MoveTile(new Vector3(moveDirection.x, 0f, 0f));
                moveTrigger.Invoke();
            }
            else if ((((Mathf.Abs(moveDirection.y)) == 1) || (Mathf.Abs(moveDirection.y) == -1)) && (axisInUse == false))
            {
                MoveTile(new Vector3(0f, moveDirection.y, 0f));
                moveTrigger.Invoke();
            }

            // Release axis
            if ((Mathf.Abs(moveDirection.x) == 0) && (Mathf.Abs(moveDirection.y) == 0))
            {
                axisInUse = false;
            }
        }
    }

    private void MoveTile(Vector3 direction)
    {
        missedBeatTracker.missedBeatCount = 0; // Reset number of consecutive missed beats
        originalPosition = transform.position;
        Vector3 newPos = playerMovePoint.position + direction;

        axisInUse = true; // Only moving in one axis at a time (no diagonal)
        playerMovePoint.position = newPos;
        Vector3Int cellPos = grid.WorldToCell(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            if (col.gameObject.CompareTag("Next"))
            {
                NextLevelSceneSwitch nextLevelScript = GameObject.Find("Walls").GetComponent<NextLevelSceneSwitch>();
                if (nextLevelScript.isOpen)
                {
                    //Debug.Log("Next collision");
                    return; // Allow player to stand on open door tile
                }
            }
            //Debug.Log("Player colliding with wall");
            playerMovePoint.position = originalPosition; // Sets player back to original position
        } 
    }

    private void Reveal(InputAction.CallbackContext context)
    {
        revealTrigger.Invoke();
    }

    private void Flag(InputAction.CallbackContext context)
    {
        flagTrigger.Invoke();
    }

    private void Pause(InputAction.CallbackContext context)
    {
        pauseTrigger.Invoke();
        SwitchPlayerToUI();
    }
}

