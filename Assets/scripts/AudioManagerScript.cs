using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Triggers
using TMPro; // TextMesh
using System; // Allow TimeSpan class

public class AudioManagerScript : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource effectsSource;

    [Header("Clips")]
    public AudioClip songToPlay;
    public AudioClip incompleteBoardAttemptSound;
    public AudioClip enterNextLevelSound;

    [Header("Beat Mechanic and Other")]
    public GameObject smallBeatPrefab;
    public float bpm;
    [SerializeField] private Intervals[] intervals;
    [SerializeField] private GameObject beatContainer;
    [SerializeField] private GameObject mainBeatTracker;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private UnityEvent songOverTrigger;
    private float currentTimerValue;

    private bool hasStarted;
    
    private void Start()
    {
        currentTimerValue = songToPlay.length; // Set timer limit
        timer.SetText("Time left: " + currentTimerValue.ToString());
    }

    private void Update()
    {
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
                musicSource.clip = songToPlay;
                musicSource.Play();
            }
        } 
        else
        {
            foreach (Intervals interval in intervals)
            {
                // Get time elapsed divided by number of intervals, getting time elapsed per interval
                float sampledTime = (musicSource.timeSamples / (musicSource.clip.frequency * interval.GetIntervalLength(bpm)));
                if (interval.CheckForNewInterval(sampledTime))
                {
                    GameObject smallBeat = Instantiate(smallBeatPrefab, beatContainer.transform);
                } 
                currentTimerValue = currentTimerValue - Time.deltaTime; // Tick timer down
            }
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTimerValue);
        timer.SetText("Time left: " + time.Minutes.ToString() + ": " + time.Seconds.ToString());
    
        if (currentTimerValue <= 0 && hasStarted)
        {
            Debug.Log("Song end");
            songOverTrigger.Invoke(); // Game over, song finished
        }
    }

    public void AddEnemyInstance(int typeOfEnemy)
    {
        foreach(Intervals i in intervals)
        {
            i.AddListenerToPulse(typeOfEnemy); // Attach new listener to each interval
        }
    }
}

// Interval class for tracking beats
[System.Serializable]
public class Intervals 
{
    [SerializeField] private float beatType;
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;

    // Length of current beat to track
    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * beatType);
    }

    // Check if new beat has passed
    public bool CheckForNewInterval (float interval)
    {
        // Check every whole number, meaning passing a new beat
        if (Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            trigger.Invoke();
            return true;
        }
        return false;
    }

    // Adds newly spawned enemies to listen to each pulse beat
    public void AddListenerToPulse(int typeOfEnemy)
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var targetEnemy = enemies[enemies.Length - 1]; // Get reference to latest created enemy

        // 0 is orange spider, 1 blue spider, 2 grey slime
        switch (typeOfEnemy)
        {
            case 0:
                OrangeSpiderScript orangeSpider = targetEnemy.GetComponent<OrangeSpiderScript>();
                if (!orangeSpider.isTracked)
                {
                    // Start tracking new object
                    orangeSpider.isTracked = true;

                    // Call enemy pulse method every time trigger is invoked
                    trigger.AddListener(orangeSpider.PulseAction);
                }
                break;
            case 1:
                BlueSpiderScript blueSpider = targetEnemy.GetComponent<BlueSpiderScript>();
                if (!blueSpider.isTracked)
                {
                    blueSpider.isTracked = true;
                    trigger.AddListener(blueSpider.PulseAction);
                }
                break;
            case 2:
                GreySlimeScript greySlime = targetEnemy.GetComponent<GreySlimeScript>();
                if (!greySlime.isTracked)
                {
                    greySlime.isTracked = true;
                    trigger.AddListener(greySlime.PulseAction);
                }
                break;
            default:
                Debug.Log("Spawning error...");
                break;
        }
    }
}
