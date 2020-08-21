using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementLogic : MonoBehaviour
{
    public static MovementLogic Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Path Generation + AStar Logic
    public bool movementPaused;
    #region
    public Stack<Node> GeneratePath(Point start, Point end)
    {
        return AStar.GetPath(start, end);
    }
    public void SetPath(LivingEntity characterMoved, Stack<Node> path)
    {
        characterMoved.path = path;
        characterMoved.gridPosition = path.Peek().GridPosition;
        characterMoved.destination = path.Peek().WorldPosition;
    }
    #endregion

    // Conditional Checks 
    #region
    public bool IsLocationMoveable(Tile destination, LivingEntity characterMoved, int range)
    {
        //List<Tile> validTilesWithinMovementRange = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, LevelManager.Instance.Tiles[characterMoved.gridPosition]);
        List<Tile> validTilesWithinMovementRange = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, characterMoved.tile);

        if (validTilesWithinMovementRange.Contains(destination) &&
            destination.IsEmpty &&
            destination.IsWalkable
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Move Teleport + Knock Back Logic
    #region

    // Movement 
    public OldCoroutineData MoveEntity(LivingEntity characterMoved, Tile destination, float speed = 3, bool freeStrikeImmune = false)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(MoveEntityCoroutine(characterMoved, destination, action, speed, freeStrikeImmune));
        return action;
    }
    public IEnumerator MoveEntityCoroutine(LivingEntity characterMoved, Tile destination, OldCoroutineData action, float speed = 3, bool freeStrikeImmune = false)
    {
        // Set properties
        float originalSpeed = characterMoved.movementAnimSpeed;
        float speedOfThisMovement = speed;
        bool hasCompletedMovement = false;
        bool freeStrikesOnThisTileResolved = false;
        bool overwatchOnThisTileResolved = false;

        // Set path + destination
        SetPath(characterMoved, GeneratePath(characterMoved.gridPosition, destination.GridPosition));

        // Play movement animation
        characterMoved.PlayMoveAnimation();

        // flip the sprite's x axis depending on the direction of movement
        PositionLogic.Instance.CalculateWhichDirectionToFace(characterMoved, destination);  

        // Commence movement
        while (hasCompletedMovement == false)
        {
            // First, check for free strikes
            if (freeStrikesOnThisTileResolved == false && characterMoved.path.Count > 0)
            {
                Debug.Log("Checking for free strikes...");
                Tile nextTile = LevelManager.Instance.GetTileFromPointReference(characterMoved.path.Peek().GridPosition);                
                OldCoroutineData freeStrikeCheck = ResolveFreeStrikes(characterMoved, characterMoved.tile, nextTile);
                yield return new WaitUntil(() => freeStrikeCheck.ActionResolved() == true);
                freeStrikesOnThisTileResolved = true;
            }

            // Second, check for overwatch attacks
            if (overwatchOnThisTileResolved == false && characterMoved.path.Count > 0)
            {
                Debug.Log("Checking for overwatch attacks...");
                Tile nextTile = LevelManager.Instance.GetTileFromPointReference(characterMoved.path.Peek().GridPosition);
                OldCoroutineData overwatchCheck = ResolveOverwatchAttacks(characterMoved, characterMoved.tile);
                yield return new WaitUntil(() => overwatchCheck.ActionResolved() == true);
                overwatchOnThisTileResolved = true;
            }

            Debug.Log("Moving to next tile on path...");
            characterMoved.transform.position = Vector2.MoveTowards(characterMoved.transform.position, characterMoved.destination, speedOfThisMovement * Time.deltaTime);

            if (characterMoved.transform.position == characterMoved.destination)
            {
                // if we have reached the next tile in our path
                if (characterMoved.path != null && characterMoved.path.Count > 0)
                {
                    Debug.Log("Next tile on path reached...");
                    Tile previousTile = characterMoved.tile;
                    characterMoved.gridPosition = characterMoved.path.Peek().GridPosition;
                    // Free up the tile we were standing on before we moved
                    LevelManager.Instance.SetTileAsUnoccupied(characterMoved.tile);
                    // Set our current tile to the tile we ended up at the end of the move
                    characterMoved.tile = LevelManager.Instance.GetTileFromPointReference(characterMoved.gridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.
                    LevelManager.Instance.SetTileAsOccupied(characterMoved.tile);
                    //Action moveToNewLocation = OnLocationMovedTo(characterMoved, characterMoved.tile, previousTile, freeStrikeImmune);
                    //yield return new WaitUntil(() => moveToNewLocation.ActionResolved() == true);
                    characterMoved.destination = characterMoved.path.Pop().WorldPosition;
                    freeStrikesOnThisTileResolved = false;
                    overwatchOnThisTileResolved = false;
                }

                // if we have reached the final destination
                else if (characterMoved.path != null && characterMoved.path.Count == 0)
                {
                    Debug.Log("Last tile on path reached...");
                    Tile previousTile = characterMoved.tile;
                    // Free up the tile we were standing on before we moved
                    LevelManager.Instance.SetTileAsUnoccupied(characterMoved.tile);
                    // Set our current tile to the tile we ended up at the end of the move
                    characterMoved.tile = LevelManager.Instance.GetTileFromPointReference(characterMoved.gridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.
                    LevelManager.Instance.SetTileAsOccupied(characterMoved.tile);
                    // Prevent character from being able to move again
                    //hasMovedThisTurn = true;
                    //Action moveToNewLocation = OnLocationMovedTo(characterMoved, characterMoved.tile, previousTile, freeStrikeImmune);
                    //yield return new WaitUntil(() => moveToNewLocation.ActionResolved() == true);
                    Debug.Log("Final point reached, movement finished");
                    characterMoved.PlayIdleAnimation();
                    freeStrikesOnThisTileResolved = false;
                    overwatchOnThisTileResolved = false;
                    hasCompletedMovement = true;
                    action.coroutineCompleted = true;
                }
            }

            
            // Reset speed
            characterMoved.movementAnimSpeed = originalSpeed;
            //yield return null;
            yield return new WaitForEndOfFrame();
        }
        //yield return null;

    }

    // Teleportation
    public OldCoroutineData TeleportEntity(LivingEntity target, Tile destination, bool switchingPosWithAnotherEntity = false)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(TeleportEntityCoroutine(target, destination,action, switchingPosWithAnotherEntity));
        return action;
    }
    public IEnumerator TeleportEntityCoroutine(LivingEntity target, Tile destination, OldCoroutineData action, bool switchingPosWithAnotherEntity = false)
    {
        if (!switchingPosWithAnotherEntity)
        {
            LevelManager.Instance.SetTileAsUnoccupied(target.tile);
        }

        // Make target vanish
        target.myModelParent.SetActive(false);
        target.myWorldSpaceCanvasParent.SetActive(false);
        StartCoroutine(VisualEffectManager.Instance.CreateTeleportEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // Move to new location
        target.gridPosition = destination.GridPosition;
        target.tile = destination;
        target.transform.position = destination.WorldPosition;

        // Make target reappear
        StartCoroutine(VisualEffectManager.Instance.CreateTeleportEffect(target.transform.position));
        VisualEffectManager.Instance.CreateHardLandingEffect(target.transform.position);
        target.myModelParent.SetActive(true);
        target.myWorldSpaceCanvasParent.SetActive(true);

        // Retrigger animation
        target.myModel.SetIdleAnim();

        // Set new tile location, resolve event
        LevelManager.Instance.SetTileAsOccupied(destination);
        OnNewTileSet(target);
        action.coroutineCompleted = true;
        yield return null;
    }

    // Knock Back
    public OldCoroutineData KnockBackEntity(LivingEntity attacker, LivingEntity target, int pushBackDistance)
    {
        OldCoroutineData action = new OldCoroutineData(true);

        // Check for knock back immunity
        if (target.myPassiveManager.unleashed)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Knock Back Immune!");
            action.coroutineCompleted = true;
            return action;
        }

        else
        {
            Debug.Log("CreateKnockBackEvent() called, starting new knockback event...");
            // First, deal the initial bolt damage
            // HandleDamage(CalculateDamage(damageAmount, target, attacker), attacker);

            Tile TileCurrentlyOn = attacker.tile;
            string direction = "unassigned";
            Tile targetTile = target.tile;
            Tile finalDestination = targetTile;
            List<Tile> tilesOnPath = new List<Tile>();
            List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
            LivingEntity characterKnockedInto = null;

            // Second, calculate which direction the target will be moved towards when shot

            // South
            if (TileCurrentlyOn.GridPosition.X == targetTile.GridPosition.X &&
                TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
            {
                direction = "South";
                Debug.Log("Knockback target is south...");
                // Find all tiles that the target will move over during the knock back, then add them to a list
                foreach (Tile tile in allTiles)
                {
                    if (
                        tile.GridPosition.X == targetTile.GridPosition.X &&
                        tile.GridPosition.Y > targetTile.GridPosition.Y &&
                        tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance
                        )
                    {
                        tilesOnPath.Add(tile);
                        Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                    }
                }

                // Order the list of tiles, so that the tiles closest to the target are at the start of the list
                List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.Y).ToList();

                // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
                // This is determined looking at the next tile, and checking if it contains an enemy on it already.
                foreach (Tile tile in SortedList)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }
            }

            // North
            else if (TileCurrentlyOn.GridPosition.X == targetTile.GridPosition.X &&
                     TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
            {
                direction = "North";

                foreach (Tile tile in allTiles)
                {
                    if (
                        tile.GridPosition.X == targetTile.GridPosition.X &&
                        tile.GridPosition.Y < targetTile.GridPosition.Y &&
                        tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance
                        )
                    {
                        tilesOnPath.Add(tile);
                        Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                    }
                }

                List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.Y).ToList();
                SortedList.Reverse();

                foreach (Tile tile in SortedList)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }


                LevelManager.Instance.SetTileAsUnoccupied(target.tile);
                target.gridPosition = finalDestination.GridPosition;
                target.tile = finalDestination;
                target.transform.position = finalDestination.WorldPosition;
                LevelManager.Instance.SetTileAsOccupied(finalDestination);
            }

            // East
            else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                     TileCurrentlyOn.GridPosition.Y == targetTile.GridPosition.Y)
            {
                direction = "East";
                // Find all tiles that the target will move over during the knock back, then add them to a list
                foreach (Tile tile in allTiles)
                {
                    if (
                        tile.GridPosition.Y == targetTile.GridPosition.Y &&
                        tile.GridPosition.X > targetTile.GridPosition.X &&
                        tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance
                        )
                    {
                        tilesOnPath.Add(tile);
                        Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                    }
                }

                // Order the list of tiles, so that the tiles closest to the target are at the start of the list
                List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

                // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
                // This is determined looking at the next tile, and checking if it contains an enemy on it already.
                foreach (Tile tile in SortedList)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }
            }

            // West
            else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                     TileCurrentlyOn.GridPosition.Y == targetTile.GridPosition.Y)
            {
                direction = "West";
                // Find all tiles that the target will move over during the knock back, then add them to a list
                foreach (Tile tile in allTiles)
                {
                    if (
                        tile.GridPosition.Y == targetTile.GridPosition.Y &&
                        tile.GridPosition.X < targetTile.GridPosition.X &&
                        tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance
                        )
                    {
                        tilesOnPath.Insert(0, tile);
                        Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                    }
                }

                // Order the list of tiles, so that the tiles closest to the target are at the start of the list
                //List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();
                //tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

                // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
                // This is determined looking at the next tile, and checking if it contains an enemy on it already.
                foreach (Tile tile in tilesOnPath)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }
            }

            // South West
            else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                     TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
            {
                direction = "South West";
                List<Tile> tempList = new List<Tile>();
                int xPos = 1;
                int yPos = 1;
                // Find all tiles that the target will move over during the knock back, then add them to a list

                foreach (Tile tile in allTiles)
                {
                    if (tile.GridPosition.Y > targetTile.GridPosition.Y && tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance &&
                        tile.GridPosition.X < targetTile.GridPosition.X && tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance)
                    {
                        tempList.Add(tile);
                    }
                }

                Loop:
                foreach (Tile tile in tempList)
                {
                    if (tile.GridPosition.X == targetTile.GridPosition.X - xPos)
                    {
                        if (tile.GridPosition.Y == targetTile.GridPosition.Y + yPos)
                        {
                            tilesOnPath.Add(tile);
                            xPos++;
                            yPos++;
                            goto Loop;
                        }
                    }
                }

                // Order the list of tiles, so that the tiles closest to the target are at the start of the list
                List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

                // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
                // This is determined looking at the next tile, and checking if it contains an enemy on it already.
                foreach (Tile tile in tilesOnPath)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }
            }

            // South East
            else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                     TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
            {
                direction = "South East";
                List<Tile> tempList = new List<Tile>();
                int xPos = 1;
                int yPos = 1;
                // Find all tiles that the target will move over during the knock back, then add them to a list

                foreach (Tile tile in allTiles)
                {
                    if (tile.GridPosition.Y > targetTile.GridPosition.Y && tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance &&
                        tile.GridPosition.X > targetTile.GridPosition.X && tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance)
                    {
                        tempList.Add(tile);
                    }
                }

                Loop:
                foreach (Tile tile in tempList)
                {
                    if (tile.GridPosition.X == targetTile.GridPosition.X + xPos)
                    {
                        if (tile.GridPosition.Y == targetTile.GridPosition.Y + yPos)
                        {
                            tilesOnPath.Add(tile);
                            xPos++;
                            yPos++;
                            goto Loop;
                        }
                    }
                }

                // Order the list of tiles, so that the tiles closest to the target are at the start of the list
                List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

                // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
                // This is determined looking at the next tile, and checking if it contains an enemy on it already.
                foreach (Tile tile in tilesOnPath)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }
            }

            // North West
            else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                     TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
            {
                direction = "North West";
                List<Tile> tempList = new List<Tile>();
                int xPos = 1;
                int yPos = 1;
                // Find all tiles that the target will move over during the knock back, then add them to a list

                foreach (Tile tile in allTiles)
                {
                    if (tile.GridPosition.Y < targetTile.GridPosition.Y && tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance &&
                        tile.GridPosition.X < targetTile.GridPosition.X && tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance)
                    {
                        tempList.Add(tile);
                    }
                }

                Loop:
                foreach (Tile tile in tempList)
                {
                    if (tile.GridPosition.X == targetTile.GridPosition.X - xPos)
                    {
                        if (tile.GridPosition.Y == targetTile.GridPosition.Y - yPos)
                        {
                            tilesOnPath.Add(tile);
                            xPos++;
                            yPos++;
                            goto Loop;
                        }
                    }
                }

                // Order the list of tiles, so that the tiles closest to the target are at the start of the list
                List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

                // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
                // This is determined looking at the next tile, and checking if it contains an enemy on it already.
                foreach (Tile tile in tilesOnPath)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }
            }

            // North East
            else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                     TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
            {
                direction = "North East";
                List<Tile> tempList = new List<Tile>();
                int xPos = 1;
                int yPos = 1;
                // Find all tiles that the target will move over during the knock back, then add them to a list

                foreach (Tile tile in allTiles)
                {
                    if (tile.GridPosition.Y < targetTile.GridPosition.Y && tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance &&
                        tile.GridPosition.X > targetTile.GridPosition.X && tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance)
                    {
                        tempList.Add(tile);
                    }
                }

                Loop:
                foreach (Tile tile in tempList)
                {
                    if (tile.GridPosition.X == targetTile.GridPosition.X + xPos)
                    {
                        if (tile.GridPosition.Y == targetTile.GridPosition.Y - yPos)
                        {
                            tilesOnPath.Add(tile);
                            xPos++;
                            yPos++;
                            goto Loop;
                        }
                    }
                }

                // Order the list of tiles, so that the tiles closest to the target are at the start of the list
                List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

                // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
                // This is determined looking at the next tile, and checking if it contains an enemy on it already.
                foreach (Tile tile in tilesOnPath)
                {
                    if (tile.IsEmpty && tile.IsWalkable)
                    {
                        finalDestination = tile;
                    }
                    else
                    {
                        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                        {
                            if (enemy.tile == tile && enemy != target)
                            {
                                characterKnockedInto = enemy;
                                break;
                            }
                        }
                    }
                }
            }

            LevelManager.Instance.SetTileAsUnoccupied(target.tile);
            target.gridPosition = finalDestination.GridPosition;
            target.tile = finalDestination;
            StartCoroutine(KnockBackEntityCoroutine(target, finalDestination.WorldPosition, action));
            LevelManager.Instance.SetTileAsOccupied(finalDestination);


            Debug.Log("Tiles on path: " + tilesOnPath.Count.ToString());
            Debug.Log("Target is " + direction + " of the the attacker");
            return action;
        }


    }
    public IEnumerator KnockBackEntityCoroutine(LivingEntity entityMoved, Vector3 destination, OldCoroutineData action)
    {
        Debug.Log("KnockBackMove() called by CombatLogic.cs....");
        bool movementCompleted = false;

        while (movementCompleted == false)
        {
            Debug.Log("KnockBackMove() moving entity...");
            entityMoved.transform.position = Vector2.MoveTowards(entityMoved.transform.position, destination, 8f * Time.deltaTime);
            if (entityMoved.transform.position == destination)
            {
                movementCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

        OnNewTileSet(entityMoved);
        action.coroutineCompleted = true;
    }
    #endregion

    // New Location Set Logic
    #region
    public OldCoroutineData OnLocationMovedTo(LivingEntity character, Tile newLocation, Tile previousLocation)
    {
        Debug.Log("OnLocationMovedToCalled() called....");
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(OnLocationMovedToCoroutine(character, newLocation, previousLocation,action));
        return action;
    }
    public IEnumerator OnLocationMovedToCoroutine(LivingEntity character, Tile newLocation, Tile previousLocation, OldCoroutineData action)
    {
        Debug.Log("OnLocationMovedToCalledCoroutine() called....");        

        OnNewTileSet(character);       
        action.coroutineCompleted = true;
        yield return null;
        
    }
    public void OnNewTileSet(LivingEntity character)
    {
        // TO DO: in future, if we have any tiles that apply effects the instant a character moves on it, the logic would go here
    }
    public OldCoroutineData ResolveFreeStrikes(LivingEntity characterMoved, Tile previousLocation, Tile newLocation)
    {
        OldCoroutineData action = new OldCoroutineData(true);
        
        // If charcter is free strike immune, dont bother resolving free strike coroutine
        if (characterMoved.myPassiveManager.slippery)
        {
            action.coroutineCompleted = true;
        }
        else
        {
            StartCoroutine(ResolveFreeStrikesCoroutine(characterMoved, action, previousLocation, newLocation));
        }
        
        return action;
    }
    private IEnumerator ResolveFreeStrikesCoroutine(LivingEntity characterMoved, OldCoroutineData action, Tile previousLocation, Tile newLocation)
    {
        Debug.Log("MovementLogic.ResolveFreeStrikesCoroutine() called....");
        List<LivingEntity> unfriendlyEntities = new List<LivingEntity>();

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (CombatLogic.Instance.IsTargetFriendly(characterMoved, entity) == false)
            {
                unfriendlyEntities.Add(entity);
            }
        }

        foreach (LivingEntity entity in unfriendlyEntities)
        {
            if (PositionLogic.Instance.GetTilesInCharactersMeleeRange(entity).Contains(previousLocation) &&
                PositionLogic.Instance.GetTilesInCharactersMeleeRange(entity).Contains(newLocation) == false &&
                characterMoved.inDeathProcess == false)
            {
                Debug.Log("ResolveFreeStrikesCoroutine() detected that " + characterMoved.name + " triggered a free strike from " + entity.name);
                movementPaused = true;
                characterMoved.PlayIdleAnimation();
                OldCoroutineData freeStrikeAction = AbilityLogic.Instance.PerformFreeStrike(entity, characterMoved);
                yield return new WaitUntil(() => freeStrikeAction.ActionResolved() == true);

                // Resume movement
                characterMoved.PlayMoveAnimation();
                movementPaused = false;
            }
        }

        action.coroutineCompleted = true;
    }

    public OldCoroutineData ResolveOverwatchAttacks(LivingEntity characterMoved, Tile previousLocation)
    {        
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(ResolveOverwatchAttacksCoroutine(characterMoved, action, previousLocation));
        return action;
    }
    private IEnumerator ResolveOverwatchAttacksCoroutine(LivingEntity characterMoved, OldCoroutineData action, Tile previousLocation)
    {
        Debug.Log("MovementLogic.ResolveOverwatchAttacks() called....");
        List<LivingEntity> unfriendlyEntities = new List<LivingEntity>();

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (entity.myPassiveManager.overwatch)
            {
                if (CombatLogic.Instance.IsTargetFriendly(characterMoved, entity) == false)
                {
                    unfriendlyEntities.Add(entity);
                }
            }
            
        }

        foreach (LivingEntity entity in unfriendlyEntities)
        {
            if (LevelManager.Instance.IsTileYWithinRangeOfTileX(entity.tile, previousLocation, 5) &&
                EntityLogic.IsTargetVisible(entity, characterMoved) &&
                characterMoved.inDeathProcess == false)
            {
                Debug.Log("ResolveOverwatchCoroutine() detected that " + characterMoved.name + " triggered an overwatch shot from " + entity.name);
                movementPaused = true;
                characterMoved.PlayIdleAnimation();
                OldCoroutineData overwatchShotAction = AbilityLogic.Instance.PerformOverwatchShot(entity, characterMoved);
                yield return new WaitUntil(() => overwatchShotAction.ActionResolved() == true);

                // Resume movement
                characterMoved.PlayMoveAnimation();
                movementPaused = false;
            }
        }

        action.coroutineCompleted = true;
    }
    #endregion

    // New Movement Logic 
    #region
    public OldCoroutineData MoveAttackerToTargetNodeAttackPosition(LivingEntity attacker, LivingEntity target )
    {
        Debug.Log("LivingEntityManager.MoveAttackerToTargetNodeAttackPosition() called...");
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(MoveAttackerToTargetNodeAttackPositionCoroutine(attacker, target, action));
        return action;
    }
    public void MoveAttackerToTargetNodeAttackPosition2(LivingEntity attacker, LivingEntity target, CoroutineData data)
    {
        Debug.Log("LivingEntityManager.MoveAttackerToTargetNodeAttackPosition() called...");
        StartCoroutine(MoveAttackerToTargetNodeAttackPositionCoroutine(attacker, target, data));
    }

    public IEnumerator MoveAttackerToTargetNodeAttackPositionCoroutine(LivingEntity attacker, LivingEntity target, OldCoroutineData action)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(target.levelNode.nose.position.x, target.levelNode.nose.position.y, 0);
        float moveSpeed = 15;

        // Face direction of destination
        PositionLogic.Instance.TurnFacingTowardsLocation(attacker, target.transform.position);

        // Play movement animation
        attacker.PlayMoveAnimation();

        while (reachedDestination == false)
        {
            attacker.transform.position = Vector2.MoveTowards(attacker.transform.position, destination, moveSpeed * Time.deltaTime);

            if (attacker.transform.position == destination)
            {
                Debug.Log("LivingEntityManager.MoveAttackerToTargetNodeAttackPositionCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        action.coroutineCompleted = true;

    }
    private IEnumerator MoveAttackerToTargetNodeAttackPositionCoroutine(LivingEntity attacker, LivingEntity target, CoroutineData action)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(target.levelNode.nose.position.x, target.levelNode.nose.position.y, 0);
        float moveSpeed = 15;

        // Face direction of destination
        PositionLogic.Instance.TurnFacingTowardsLocation(attacker, target.transform.position);

        // Play movement animation
        attacker.PlayMoveAnimation();

        while (reachedDestination == false)
        {
            attacker.transform.position = Vector2.MoveTowards(attacker.transform.position, destination, moveSpeed * Time.deltaTime);

            if (attacker.transform.position == destination)
            {
                Debug.Log("LivingEntityManager.MoveAttackerToTargetNodeAttackPositionCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        action.MarkAsCompleted();

    }
    public OldCoroutineData MoveEntityToNodeCentre(LivingEntity entity, LevelNode node)
    {
        Debug.Log("LivingEntityManager.MoveEntityToNodeCentre() called...");
        OldCoroutineData action = new OldCoroutineData(true);
        StartCoroutine(MoveEntityToNodeCentreCoroutine(entity, node, action));
        return action;
    }
    public void MoveEntityToNodeCentre2(LivingEntity entity, LevelNode node, CoroutineData data)
    {
        Debug.Log("LivingEntityManager.MoveEntityToNodeCentre2() called...");
        StartCoroutine(MoveEntityToNodeCentreCoroutine2(entity, node, data));
    }
    private IEnumerator MoveEntityToNodeCentreCoroutine(LivingEntity entity, LevelNode node, OldCoroutineData action)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(node.transform.position.x, node.transform.position.y, 0);
        float moveSpeed = 15;

        // Face direction of destination node
        PositionLogic.Instance.TurnFacingTowardsLocation(entity, node.transform.position);

        // Play movement animation
        entity.PlayMoveAnimation();

        // Move
        while (reachedDestination == false)
        {
            entity.transform.position = Vector2.MoveTowards(entity.transform.position, destination, moveSpeed * Time.deltaTime);

            if (entity.transform.position == destination)
            {
                Debug.Log("LivingEntityManager.MoveEntityToNodeCentreCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        // Reset facing, depending on living entity type
        
        if (entity.defender)
        {
            PositionLogic.Instance.SetDirection(entity, "Right");
            //PositionLogic.Instance.TurnFacingTowardsLocation(entity, new Vector3(1000,0,0));
        }
        else if (entity.enemy)
        {
            PositionLogic.Instance.SetDirection(entity, "Left");
            //PositionLogic.Instance.TurnFacingTowardsLocation(entity, new Vector3(-1000, 0, 0));
        }
        

        // Idle anim
        entity.PlayIdleAnimation();

        // Resolve event
        action.coroutineCompleted = true;

    }
    private IEnumerator MoveEntityToNodeCentreCoroutine2(LivingEntity entity, LevelNode node, CoroutineData action)
    {
        // Set up
        bool reachedDestination = false;
        Vector3 destination = new Vector3(node.transform.position.x, node.transform.position.y, 0);
        float moveSpeed = 15;

        // Face direction of destination node
        PositionLogic.Instance.TurnFacingTowardsLocation(entity, node.transform.position);

        // Play movement animation
        entity.PlayMoveAnimation();

        // Move
        while (reachedDestination == false)
        {
            entity.transform.position = Vector2.MoveTowards(entity.transform.position, destination, moveSpeed * Time.deltaTime);

            if (entity.transform.position == destination)
            {
                Debug.Log("LivingEntityManager.MoveEntityToNodeCentreCoroutine() detected destination was reached...");
                reachedDestination = true;
            }
            yield return null;
        }

        // Reset facing, depending on living entity type

        if (entity.defender)
        {
            PositionLogic.Instance.SetDirection(entity, "Right");
            //PositionLogic.Instance.TurnFacingTowardsLocation(entity, new Vector3(1000,0,0));
        }
        else if (entity.enemy)
        {
            PositionLogic.Instance.SetDirection(entity, "Left");
            //PositionLogic.Instance.TurnFacingTowardsLocation(entity, new Vector3(-1000, 0, 0));
        }


        // Idle anim
        entity.PlayIdleAnimation();

        // Resolve event
        action.MarkAsCompleted();

    }
    #endregion


}
