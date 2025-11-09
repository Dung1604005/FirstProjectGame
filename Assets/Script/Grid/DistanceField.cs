using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DistanceField 
{
    [SerializeField] private GridManagement gridManagement;

    int[] dx = new int[] { -1, 1, 0, 0 };
    int[] dy = new int[] { 0, 0, -1, 1 };

    public void ResetDistanceField()
    {
        List<List<GridCell>> gridCells = gridManagement.GridBuilder.GridCells;
        for (int x = 0; x < gridCells.Count; x++)
        {
            for (int y = 0; y < gridCells[x].Count; y++)
            {
                gridCells[x][y].SetDistanceFromPlayer(float.MaxValue);
            }

        }
    }


    public DistanceField(GridManagement gridManagement)
    {
        this.gridManagement = gridManagement;
    }
    
    public float getDistanceAtWorldPosition(Vector2 worldPosition)
    {
        Vector2 gridPosition = gridManagement.GridBuilder.WorldToGridPosition(worldPosition);
        if (gridManagement.GridBuilder.IsValidGridPosition(gridPosition))
        {
            GridCell cell = gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y];
            return cell.DistanceFromPlayer * gridManagement.GridBuilder.CellSize;
        }
        return float.MaxValue;
    }

    public void CalculateDistanceField(Vector2 playerPosition)
    {
        Vector2 playerGridPosition = gridManagement.GridBuilder.WorldToGridPosition(playerPosition);
        if(gridManagement.GridBuilder.IsValidGridPosition(playerGridPosition) == false)
        {
            Debug.LogError("Player position is out of grid bounds: " + playerPosition);
            return;
        }
        ResetDistanceField();
        
        
        Queue<Pair<int, Vector2>> queue = new Queue<Pair<int, Vector2>>();
        queue.Enqueue(new Pair<int, Vector2>(0, playerGridPosition));
        gridManagement.GridBuilder.GridCells[(int)playerGridPosition.x][(int)playerGridPosition.y].SetDistanceFromPlayer(0);
        while(queue.Count > 0)
        {
            Pair<int, Vector2> current = queue.Dequeue();
            int currentDistance = current.First;
            int x = (int)current.Second.x;
            int y = (int)current.Second.y;
            for (int i = 0; i < 4; i++)
            {
                int nextX = x + dx[i];
                int nextY = y + dy[i];
                if(gridManagement.GridBuilder.IsValidGridPosition(new Vector2(nextX, nextY)))
                {
                    GridCell nextCell = gridManagement.GridBuilder.GridCells[nextX][nextY];
                    if(nextCell.CellType == CellType.Walkable)
                    {
                        if(nextCell.DistanceFromPlayer > currentDistance + 1)
                        {
                            nextCell.SetDistanceFromPlayer(currentDistance + 1);
                            queue.Enqueue(new Pair<int, Vector2>(currentDistance + 1, new Vector2(nextX, nextY)));
                        }
                    }
                   
                }
            }
            
        }
    }
    
}
