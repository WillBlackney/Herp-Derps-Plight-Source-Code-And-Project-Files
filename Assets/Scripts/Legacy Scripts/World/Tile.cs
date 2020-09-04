using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public enum TileType { None, Dirt, Grass, Tree, Water };

    [Header("Component References")]
    public Animator myAnimator;

    [Header("Properties")]
    public TileType myTileType;
    public bool IsEmpty;
    public bool IsWalkable;
    public bool BlocksLoS;
    public Point GridPosition { get; set; }
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f);
        }
    }

    // Initialization + Setup
    #region
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        if (myTileType == TileType.Dirt)
        {
            RunDirtTileSetup();
        }
        else if (myTileType == TileType.Grass)
        {
            RunGrassTileSetup();
        }
        
        else if (myTileType == TileType.Water)
        {
            RunWaterTileSetup();
        }

        GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        //LevelManager.Instance.Tiles.Add(gridPos, this);
    }
    #endregion

    // Set Tile Type
    #region
    public void RunDirtTileSetup()
    {
        IsWalkable = true;
        IsEmpty = true;
        BlocksLoS = false;
    }
    public void RunGrassTileSetup()
    {
        IsWalkable = true;
        IsEmpty = true;
        BlocksLoS = false;
    }   
    public void RunTreeTileSetup()
    {
        IsWalkable = false;
        IsEmpty = false;
        BlocksLoS = true;
    }
    public void RunWaterTileSetup()
    {
        IsWalkable = false;
        IsEmpty = false;
        BlocksLoS = false;
    }
    #endregion

    // Mouse + Click Events
    #region    
    private void OnMouseEnter()
    {
        //LevelManager.Instance.mousedOverTile = this;
        OnTileMouseEnter();
    }
    public void OnTileMouseEnter()
    {
        // Move tile hover over this
        TileHover.Instance.UpdatePosition(this);  


    }
    public void OnMouseDown()
    {
        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        // check consumables first
        if (ConsumableManager.Instance.awaitingFireBombTarget ||
            ConsumableManager.Instance.awaitingDynamiteTarget ||
            ConsumableManager.Instance.awaitingPoisonGrenadeTarget ||
            ConsumableManager.Instance.awaitingBottledFrostTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(this);
        }

        else if (ConsumableManager.Instance.awaitingBlinkPotionDestinationTarget &&
            IsWalkable &&
            IsEmpty &&
            LevelManager.Instance.GetTilesWithinRange(2, ConsumableManager.Instance.blinkPotionTarget.tile).Contains(this))
        {
            ConsumableManager.Instance.PerformBlinkPotion(this);
        }
       

    }
    #endregion

    
}
