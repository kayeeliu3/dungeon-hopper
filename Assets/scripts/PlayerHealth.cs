using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Image[] hearts;
    [SerializeField] private UnityEvent playerHurtTrigger;
    [SerializeField] private UnityEvent playerDeadTrigger;

    private void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    // Player damage
    public void Hurt()
    {
        health--;
        playerHurtTrigger.Invoke();
        CheckNoHealth();
    }

    // Check if no health left
    public void CheckNoHealth()
    {
        if (health <= 0)
        {
            playerDeadTrigger.Invoke(); // Disable player input and objects via trigger
        }
    }
}
