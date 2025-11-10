using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{

    List<List<GridCell>> gridCells = new List<List<GridCell>>();

    public List<List<GridCell>> GridCells => gridCells;
    [SerializeField] private float cellSize = 0.7f;
    public float CellSize => cellSize;
    [SerializeField] private Vector2 gridSize = new Vector2(720, 515);

    [SerializeField] private Vector2 originPosition = new Vector2(-181, -240);

    public void initGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            List<GridCell> column = new List<GridCell>();
            for (int y = 0; y < gridSize.y; y++)
            {
                GridCell newCell = new GridCell();
                CellType type = CellType.Walkable; // Default type  
                Collider2D hitCollider = Physics2D.OverlapBox(originPosition + new Vector2(x * cellSize, y * cellSize), new Vector2(cellSize, cellSize), 0f);
                if (hitCollider != null)
                {
                    if (hitCollider.CompareTag(GameConfig.BLOCK_OBJECT_TAG) || hitCollider.CompareTag(GameConfig.DESTROYABLE_OBJECT_TAG))
                    {
                        type = CellType.Blocked;
                    }
                    else if (hitCollider.CompareTag(GameConfig.PLAYER_WALL))
                    {
                        type = CellType.Breakable;
                    }
                }
                newCell.Initialize(new Vector2(x, y), originPosition + new Vector2(x * cellSize, y * cellSize), type);
                column.Add(newCell);
               
            }
            gridCells.Add(column);
        }
        

    }
    public Vector2 WorldToGridPosition(Vector2 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
        return new Vector2(x, y);
    }
    public bool IsValidGridPosition(Vector2 gridPosition)
    {
        int x = (int)gridPosition.x;
        int y = (int)gridPosition.y;
        if (x >= 0 && x < gridCells.Count && y >= 0 && y < gridCells[0].Count)
        {
            return true;
        }
        return false;
    }

    public Vector2 GridToWorldPosition(Vector2 gridPosition)
    {
        float x = originPosition.x + gridPosition.x * cellSize;
        float y = originPosition.y + gridPosition.y * cellSize;
        return new Vector2(x, y);
    }
    // void OnDrawGizmos()
    // {
       
        
    //     if (gridCells.Count == 0) return;

    //     for (int x = 0; x < gridCells.Count; x++)
    //     {
    //         if(gridCells[x].Count == 0) continue;
    //         foreach (var cell in gridCells[x])
    //         {

    //             Gizmos.color = Color.cyan;
    //             Gizmos.DrawRay(new Vector3(cell.WorldPosition.x, cell.WorldPosition.y, 0), cell.FlowDirection * 0.3f);
    //         }
    //     }


    // }
    


}
