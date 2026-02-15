using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    private GridManagement gridManagement;
    int[] dx = new int[] { -1, 1, 0, 0, 1, -1, 1, -1 };
    int[] dy = new int[] { 0, 0, -1, 1, 1, -1, -1, 1 };

    public FlowField(GridManagement gridManagement)
    {
        this.gridManagement = gridManagement;
    }
    public void CalculateFlowField(Vector2 playerPosition)
    {
        Vector2 playerGridPosition = gridManagement.GridBuilder.WorldToGridPosition(playerPosition);
        if (gridManagement.GridBuilder.IsValidGridPosition(playerGridPosition) == false)
        {
            //Debug.Log("Player position is out of grid bounds: " + playerPosition);
            return;
        }
        List<List<GridCell>> gridCells = gridManagement.GridBuilder.GridCells;
        int halfChunkX = gridManagement.ChunkX / 2;
        int halfChunkY = gridManagement.ChunkY / 2;
        int startX = (int)playerGridPosition.x - halfChunkX;
        int endX = (int)playerGridPosition.x + halfChunkX;
        int startY = (int)playerGridPosition.y - halfChunkY;
        int endY = (int)playerGridPosition.y + halfChunkY;
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (!gridManagement.GridBuilder.IsValidGridPosition(new Vector2(x, y))) continue;
                if (gridCells[x][y].CellType != CellType.Walkable) continue;
                Vector2 dir = Vector2.zero;
                float min_distance = float.MaxValue;
                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    int nextX = x + dx[i];
                    int nextY = y + dy[i];
                    if(nextX < startX || nextX > endX || nextY < startY || nextY > endY)
                    {
                        continue;
                    }
                    if (gridManagement.GridBuilder.IsValidGridPosition(new Vector2(nextX, nextY)))
                    {
                        if (gridCells[nextX][nextY].DistanceFromPlayer < min_distance)
                        {
                            min_distance = gridCells[nextX][nextY].DistanceFromPlayer;

                        }
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    int nextX = x + dx[i];
                    int nextY = y + dy[i];
                    if (!gridManagement.GridBuilder.IsValidGridPosition(new Vector2(nextX, nextY))) continue;
                    if (nextX < startX || nextX > endX || nextY < startY || nextY > endY)
                    {
                        continue;
                    }

                    if (gridCells[nextX][nextY].DistanceFromPlayer == min_distance)
                    {
                        count++; ;
                        dir += gridManagement.GridBuilder.GridToWorldPosition(new Vector2(nextX, nextY)) - gridManagement.GridBuilder.GridToWorldPosition(new Vector2(x, y));

                    }

                }
                if(count > 0)
                {
                    dir = (dir / count).normalized;
                }
                
                gridCells[x][y].SetFlowDirection(dir);

            }
        }


    }

}
