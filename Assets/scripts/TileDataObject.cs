using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileDataObject : ScriptableObject
{
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite flaggedTile;
    [SerializeField] private List<Sprite> numberedTiles;
    [SerializeField] private Sprite mineTile;
    [SerializeField] private Sprite incorrectFlagTile;
    [SerializeField] private Sprite mineHitTile;

    private SpriteRenderer sRenderer;
    public GameManager gameManager;
    public bool flagged = false;
    public bool active = true;
    public bool isMine = false;
    public int mineCount = 0;

    public TileBase[] tiles;
}
