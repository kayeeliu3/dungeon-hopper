using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(SpriteRenderer))]

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite flaggedTile;
    [SerializeField] private List<Sprite> numberedTiles;
    [SerializeField] private Sprite mineTile;
    [SerializeField] private Sprite incorrectFlagTile;
    [SerializeField] private Sprite mineHitTile;

    private SpriteRenderer sRenderer;
    public GameManager gameManager;
    public Transform playerTransform;
    public bool flagged = false;
    public bool active = true;
    public bool isMine = false;
    public bool mineRevealed = false;
    public int mineCount = 0;

    public GameObject flagAddAnimPrefab;
    public GameObject flagRemoveAnimPrefab;

    [SerializeField] private UnityEvent mineExplodeSoundTrigger;
    [SerializeField] private UnityEvent revealSuccessSoundTrigger;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    // Open tile
    public void Reveal()
    {
        // If not pressed
        if (active)
        {
            // Left click to reveal
            RevealTile();
            if (gameManager.firstReveal) { gameManager.firstReveal = false; }; // Disable first input for mine checks
        }
    }

    // Flag tile
    public void Flag()
    {
        // If not pressed
        if (active)
        {
            flagged = !flagged;
            if (flagged) 
            { 
                sRenderer.sprite = flaggedTile; 
                GameObject flagAnim = Instantiate(flagAddAnimPrefab, playerTransform);
            }
            else 
            { 
                sRenderer.sprite = unclickedTile; 
                GameObject flagAnim = Instantiate(flagRemoveAnimPrefab, playerTransform);
            }
            gameManager.DisplayMinesLeft();
        }
    }

    // Extra checks after opening a tile eg. game win conditions, neighbouring checks
    public void RevealTile()
    {
        if (active & !flagged) // Cannot reveal flagged tiles
        {
            active = false;
            if (isMine)
            {
                if (gameManager.firstReveal)
                {
                    gameManager.MoveMine(this);
                    sRenderer.sprite = numberedTiles[mineCount];
                    if (mineCount == 0)
                    {
                        // Open up surrounding tiles if blank
                        gameManager.RevealNeighbours(this);
                    }
                    return;
                }

                // SPAWN MONSTER (Mine revealed)
                sRenderer.sprite = mineHitTile;
                mineRevealed = true;
                gameManager.PlayerRevealedMine();
            } 
            else 
            {
                sRenderer.sprite = numberedTiles[mineCount];
                if (mineCount == 0)
                {
                    // Open up surrounding tiles if blank
                    gameManager.RevealNeighbours(this);
                }
            }
            gameManager.CheckGameOver();
            gameManager.DisplayMinesLeft();
        }
    }

    public void ShowGameOver()
    {
        if (active)
        {
            active = false;
            if (isMine & !flagged)
            {
                // Show all mines
                sRenderer.sprite = mineTile;
            } else if (flagged & !isMine)
            {
                // Show incorrectly flagged mines
                sRenderer.sprite = incorrectFlagTile;
            }
        }
    }

    // Flag all remaining mines upon board completion
    public void FlagMinesIfWin()
    {
        if (isMine)
        {
            flagged = true;
            sRenderer.sprite = flaggedTile;
        }
    }
}
