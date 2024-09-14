using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NextLevelSceneSwitch : MonoBehaviour
{
    public TileBase closedExit;
    public TileBase openExit;
    public bool isOpen;
    private Tilemap tileMap;

    private void Start() 
    {
        tileMap = GetComponent<Tilemap>(); // get the tile map you want to draw tiles over
        isOpen = false;
    }

    // Change tile into open door upon board completion
    public void changeTile() 
    {
        isOpen = true;
        tileMap.SwapTile(closedExit, openExit);
    }
}
