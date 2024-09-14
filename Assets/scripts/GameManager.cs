using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events; // Triggers
using TMPro;

// Handles main Minesweeper logic
public class GameManager : MonoBehaviour
{
    [Header("Minesweeper Mechanics")]
    public int width;
    public int height;
    public float gridOffsetX;
    public float gridOffsetY;
    public GameObject textAnimPrefab;
    public bool firstReveal = true; // Ensure first reveal is not a mine
    public bool willEnemiesSpawn; // Medium and Hard enables enemies
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private UnityEvent boardCompleteTrigger;
    private bool isGameOver = false;
    private List<Tile> tiles = new(); // List reference to all tiles
    private readonly float tileSize = 1f;
    private int difficulty;
    private int numOfMines;

    [Header("Enemy Management")]
    public GameObject orangeSpiderPrefab;
    public GameObject blueSpiderPrefab;
    public GameObject greySlimePrefab;


    [Header("Text Holders")]
    [SerializeField] private TextMeshProUGUI minesRemainingText;
    [SerializeField] private TextMeshProUGUI loseText;


    [Header("Sounds")]
    public AudioSource mineRevealedSound;
    public AudioSource revealSuccessSound;
    public AudioSource flagSound;
    public AudioSource boardCompleteSound;
    public AudioSource loseSound;

    [Header("Miscellaneous")]
    [SerializeField] private Animator transitionAnim; // Transition if player dies
    private Coroutine trackedCoroutine; // Reference to check when coroutine ends

    private void Start()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty");
        SetNumberOfMines();
        CreateGameBoard();
        ResetMines();
        minesRemainingText.SetText("Mines left: " + numOfMines);
        willEnemiesSpawn = (difficulty == 1) ? false : true;
        //Debug.Log("Spawn: " + willEnemiesSpawn + " with difficulty: " + difficulty + " and no. mines: " + numOfMines);
    }

    public void DisplayMinesLeft()
    {
        int count = 0;
        foreach (Tile tile in tiles)
        {
            if (tile.flagged || tile.mineRevealed)
            {
                count++; // Count number of flagged and exploded tiles
            }
        }

        minesRemainingText.SetText("Mines left: " + (numOfMines - count));
        if (playerHealth.health == 0)
        {
            GameOver();
        }
    }

    // Create mineboard with parameters for number of mines for WxH
    private void CreateGameBoard()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                // Generating actual mine tiles
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder; // Parent tile to Mineboard

                // Positioning of tile
                float x = col - gridOffsetX;
                float y = row - gridOffsetY;
                tileTransform.position = new Vector3(x * tileSize, y * tileSize, 0f);

                // Keep track of all tiles
                Tile tile = tileTransform.GetComponent<Tile>();
                tiles.Add(tile); // Keep reference to specific tiles
                tile.gameManager = this;
                tile.playerTransform = playerTransform;
            }
        }
    }

    // Generate mines and establish surrounding mine numbers
    private void ResetMines()
    {
        // Array of random indexes for all mines
        int[] minePositions = Enumerable.Range(0, tiles.Count).OrderBy(x => Random.Range(0f, 1f)).ToArray();
        
        for (int i = 0; i < numOfMines; i++)
        {
            int pos = minePositions[i];
            tiles[pos].isMine = true;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].mineCount = GetNumberOfMines(i);
        }
    }

    // Reload scene to generate new mineboard to generate a new one
    public void ResetMineboard()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Depending on level and difficulty, set appropriate number of mines
    public void SetNumberOfMines()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (difficulty == 1)
            {
                numOfMines = 15;
            }
            else if (difficulty == 2)
            {
                numOfMines = 25;
            }
            else
            {
                numOfMines = 30;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (difficulty == 1)
            {
                numOfMines = 20;
            }
            else if (difficulty == 2)
            {
                numOfMines = 25;
            }
            else
            {
                numOfMines = 35;
            }
        }
        else
        {
            if (difficulty == 1)
            {
                numOfMines = 20;
            }
            else if (difficulty == 2)
            {
                numOfMines = 25;
            }
            else
            {
                numOfMines = 30;
            }
        }
    }

    // If first reveal is a mine, move mine elsewhere
    public void MoveMine(Tile tile)
    {
        int tileLocation = tiles.IndexOf(tile);
        tiles[tileLocation].isMine = false; // disable mine

        // Iterate through all tiles until blank one is found after index 2 (avoid corners)
        bool exit = false;
        int i = 2;
        while (!exit)
        {
            if (!tiles[i].isMine)
            {
                tiles[i].isMine = true;
                //Debug.Log("Moving first mine to index " + i);
                // Re-count neighbours
                for (int j = 0; j < tiles.Count; j++)
                {
                    tiles[j].mineCount = GetNumberOfMines(j);
                }

                exit = true;
            }
            i++;
        }
    }

    // Get number of mines surrounding a tile 
    private int GetNumberOfMines(int tile)
    {
        int count = 0;
        foreach (int pos in GetNeighbours(tile))
        {
            if (tiles[pos].isMine)
            {
                count++;
            }
        }

        return count;
    }

    // Return position of all neighbours from a given tile
    private List<int> GetNeighbours(int pos)
    {
        List<int> neighbours = new();
        int row = pos / width;
        int col = pos % width;
        // (0,0) is bottom left
        if (row < (height - 1))
        {
            neighbours.Add(pos + width); // N
            if (col > 0)
            {
                neighbours.Add(pos + width - 1); // NW
            }
            if (col < (width - 1))
            {
                neighbours.Add(pos + width + 1); // NE
            }
        }
        if (col > 0)
        {
            neighbours.Add(pos - 1); // W
        }
        if (col < (width - 1))
        {
            neighbours.Add(pos + 1); // E
        }
        if (row > 0)
        {
            neighbours.Add(pos - width); // S
            if (col > 0)
            {
                neighbours.Add(pos - width - 1) ; // SW
            }
            if (col < (width - 1))
            {
                neighbours.Add(pos - width + 1); // SE
            }
        }

        return neighbours;
    }

    // Check if neighbouring tiles can be cleared along with clicked tile
    public void RevealNeighbours(Tile tile)
    {
        int tileLocation = tiles.IndexOf(tile);
        foreach (int pos in GetNeighbours(tileLocation))
        {
            tiles[pos].RevealTile();
        }
    }

    // Once triggered from player input, get tile that player is on and reveal that tile
    public void RevealSpecificTile()
    {
        foreach (Tile tile in tiles)
        {
            // Get the tile that the player is on
            if (tile.transform.position == playerTransform.position)
            {
                if (tile.active)
                {
                    tile.Reveal(); // Reveal active tile
                }
                else
                {
                    ChordTile(tile); // Open surrounding tiles if surrounding tiles flagged
                }
                PlaySuccessRevealSound();
            }
        }
    }

    // Once triggered from player input, get tile that player is on and flag that tile
    public void FlagSpecificTile()
    {
        foreach (Tile tile in tiles)
        {
            // Get the tile that the player is on
            if (tile.transform.position == playerTransform.position)
            {
                tile.Flag();
            }
        }
    }

    // Player reveals mine, thus spawning monster and dealing 1 heart of damage
    public void PlayerRevealedMine()
    {
        playerHealth.Hurt(); // Deal damage
        PlayMineRevealedSound();
    }

    // Generate a random set of coordinates on the tilemap for monster spawns
    public void GenerateSpawnLocations()
    {
        // Choose random tile to spawn
        int spawnPos = Random.Range(0, tiles.Count);
        Vector3 spawnVectorPos = new Vector3(tiles[spawnPos].transform.position.x, tiles[spawnPos].transform.position.y, 0f);
        while (spawnVectorPos == playerTransform.position)
        {
            // Ensure monster not spawned at player position
            spawnPos = Random.Range(0, tiles.Count);
            spawnVectorPos = new Vector3(tiles[spawnPos].transform.position.x, tiles[spawnPos].transform.position.y, 0f);
        }
        SpawnMonster(spawnVectorPos, spawnPos);

        // Spawn second monster on Hard difficulty
        if (difficulty == 3)
        {
            int secondSpawnPos = Random.Range(0, tiles.Count);
            Vector3 secondVectorPos = new Vector3(tiles[secondSpawnPos].transform.position.x, tiles[secondSpawnPos].transform.position.y, 0f);
            while (secondVectorPos == playerTransform.position)
            {
                // Ensure monster not spawned at player position
                secondSpawnPos = Random.Range(0, tiles.Count);
                secondVectorPos = new Vector3(tiles[secondSpawnPos].transform.position.x, tiles[secondSpawnPos].transform.position.y, 0f);
            }
            SpawnMonster(secondVectorPos, secondSpawnPos);
        }
    }

    public void SpawnMonster(Vector3 vector, int spawnTile)
    {
        if (!willEnemiesSpawn)
        {
            return; // Do not spawn on easy difficulty
        }

        // Generate random number to choose which monster to spawn
        int num = Random.Range(0, 3);
        switch (num)
        {
            case 0:
                //Debug.Log("Orange spider at " + spawnTile + " with tile coordinates " + tiles[spawnTile].transform.position);
                Instantiate(orangeSpiderPrefab, vector, Quaternion.identity);
                break;
            case 1:
                //Debug.Log("Blue spider at " + spawnTile + " with tile coordinates " + tiles[spawnTile].transform.position);
                Instantiate(blueSpiderPrefab, vector, Quaternion.identity);
                break;
            case 2:
                //Debug.Log("Grey slime at " + spawnTile + " with tile coordinates " + tiles[spawnTile].transform.position);
                Instantiate(greySlimePrefab, vector, Quaternion.identity);
                break;
            default:
                //Debug.Log("Error choosing monster - not spawned.");
                break;
        }
    }
        
    // No health remaining
    public void GameOver()
    {
        if (!isGameOver)
        {
            //Debug.Log(":(");
            PlayLoseSound();
            PlayerDeadCutscene();
            isGameOver = true;
        }
        
    }

    // If number of unopened tiles matches number of mines, win
    public void CheckGameOver()
    {
        int count = 0;
        foreach (Tile tile in tiles)
        {
            if (tile.active)
            {
                count++;
            }
        }

        // Temporary workaround for board completing itself early...
        if (playerHealth.health == 1)
        {
            if ((count + 2) == numOfMines)
            {
                GameObject boardCompleteText = Instantiate(textAnimPrefab, playerTransform);
                boardCompleteText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Board Complete!");
                boardCompleteTrigger.Invoke();
                foreach (Tile tile in tiles)
                {
                    tile.active = false;
                    tile.FlagMinesIfWin();
                    PlayBoardCompleteSound();
                }
            }
            return;
        } 
        else if (playerHealth.health == 2)
        {
            if ((count + 1) == numOfMines)
            {
                GameObject boardCompleteText = Instantiate(textAnimPrefab, playerTransform);
                boardCompleteText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Board Complete!");
                boardCompleteTrigger.Invoke();
                foreach (Tile tile in tiles)
                {
                    tile.active = false;
                    tile.FlagMinesIfWin();
                    PlayBoardCompleteSound();
                }
            }
            return;
        }

        if ((count) == numOfMines)
        {
            GameObject boardCompleteText = Instantiate(textAnimPrefab, playerTransform);
            boardCompleteText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Board Complete!");
            boardCompleteTrigger.Invoke();
            foreach (Tile tile in tiles)
            {
                tile.active = false;
                tile.FlagMinesIfWin();
                PlayBoardCompleteSound();
            }
        }
    }

    // Chord tiles (reveal surrounding tiles if mines flagged)
    public void ChordTile(Tile tile)
    {
        int loc = tiles.IndexOf(tile);
        int numFlags = 0;

        foreach (int pos in GetNeighbours(loc))
        {
            if (tiles[pos].flagged || tiles[pos].mineRevealed)
            {
                numFlags++;
            }
        }

        if (numFlags == tile.mineCount)
        {
            RevealNeighbours(tile);
        }
    }

    public void PlayMineRevealedSound()
    {
        mineRevealedSound.Play();
    }

    public void PlaySuccessRevealSound()
    {
        revealSuccessSound.Play();
    }

    public void PlayFlagSound()
    {
        flagSound.Play();
    }

    public void PlayBoardCompleteSound()
    {
        boardCompleteSound.Play();
    }

    public void PlayLoseSound()
    {
        loseSound.Play();
    }

    // When no health remaining, start cutscene
    public void PlayerDeadCutscene()
    {
        trackedCoroutine = StartCoroutine(LoseTextDisplay());
        StartCoroutine(LoadLoseLevel());
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    // Load lose scene
    IEnumerator LoadLoseLevel()
    {
        while (trackedCoroutine != null)
        {
            yield return new WaitForSeconds(3); // Wait for other coroutine
        }
        loseText.text = "";
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(6);
    }

    // Load text animation for when lose cutscene runs
    IEnumerator LoseTextDisplay()
    {
        yield return new WaitForSeconds(2);
        string text = "Well... I guess this is where it ends."; 
        var waitTimer = new WaitForSeconds(.1f);
        foreach (char c in text)
        {
            loseText.text = loseText.text + c;
            yield return waitTimer;
        }
        trackedCoroutine = null;
    }
}