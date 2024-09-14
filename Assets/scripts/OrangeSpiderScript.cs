using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrangeSpiderScript : MonoBehaviour
{
    // Movement
    public Transform orangeSpiderMovePoint;
    public bool isTracked = false; // Ensure each prefab object is tracked to listen for pulses in Audio Manager
    public AudioManagerScript audioManager; // Reference to attach to listen for pulse

    [SerializeField] private AudioSource monsterKilledSound;
    private Transform playerMovePoint;
    private PlayerMovement playerMovementScript;
    private PlayerHealth playerHealth;
    private int moveSpeed = 5;
    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private int pulseCount = 0;

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        AlertAudioManagerNewSpawn();
        originalPosition = transform.position;
        orangeSpiderMovePoint.transform.position = transform.position;
        orangeSpiderMovePoint.parent = null; // Detach parent
        rb = GetComponent<Rigidbody2D>();
        playerMovePoint = GameObject.Find("PlayerMovePoint").transform;
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, orangeSpiderMovePoint.position, moveSpeed * Time.deltaTime);
    }

    // Act according to beat of song - move towards player
    public void PulseAction()
    {
        pulseCount++;
        // Move one adjacent tile every 2 pulses (priorities horizontal tile movement first)
        if (pulseCount == 2)
        {
            if (transform.position.x > playerMovementScript.transform.position.x) // Left
            {
                MoveTile(new Vector3(-1f, 0f, 0f));
            } 
            else if (transform.position.x < playerMovementScript.transform.position.x) // Right
            {
                MoveTile(new Vector3(1f, 0f, 0f));
            } 
            else if (transform.position.y > playerMovementScript.transform.position.y) // Down
            {
                MoveTile(new Vector3(0f, -1f, 0f));
            } 
            else if (transform.position.y < playerMovementScript.transform.position.y) // Up
            {
                MoveTile(new Vector3(0f, 1f, 0f));
            }
            pulseCount = 0;
        }
    }

    // Logic for moving tiles
    private void MoveTile(Vector3 direction)
    {   
        originalPosition = transform.position;
        Vector3 newPos = orangeSpiderMovePoint.position + direction;
        orangeSpiderMovePoint.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            orangeSpiderMovePoint.position = originalPosition;  
        } 
        else if (col.gameObject.CompareTag("Player"))
        {
            // If player moves into spider move point at this point of the function, enemy hasn't moved and is destroyed
            if (playerMovePoint.position == transform.position)
            {
                playerMovePoint.position = playerMovementScript.originalPosition;
                monsterKilledSound.Play();
                Destroy(gameObject);
                return;
            }
            // If spider move point == player original position && player original == playerMovePoint, player damaged
            if (playerMovementScript.originalPosition == playerMovePoint.position && orangeSpiderMovePoint.position == playerMovementScript.originalPosition)
            {
                playerHealth.Hurt();
                orangeSpiderMovePoint.position = originalPosition; 
                return;
            }

            // If player movepoint == spider movepoint, player takes damage, allow player move into that tile
            if (playerMovePoint.position == orangeSpiderMovePoint.position && orangeSpiderMovePoint.position != originalPosition)
            {
                playerHealth.Hurt();
                orangeSpiderMovePoint.position = originalPosition; 
                return;
            }

            // If at this point of function, then player is hurt and both return to original position
            playerHealth.Hurt();
            orangeSpiderMovePoint.position = originalPosition; 
            playerMovePoint.position = playerMovementScript.originalPosition;
        }
        else if (col.gameObject.CompareTag("Enemy"))
        {
            orangeSpiderMovePoint.position = originalPosition; 
        }
    }

    // Tell audio manager of newly spawned instances to listen to pulse beats
    private void AlertAudioManagerNewSpawn()
    {
        audioManager.AddEnemyInstance(0);
    }
}

