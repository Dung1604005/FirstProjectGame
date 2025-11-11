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
    public void CalculateFlowField()
    {
        List<List<GridCell>> gridCells = gridManagement.GridBuilder.GridCells;
        for (int x = 0; x < gridCells.Count; x++)
        {
            for (int y = 0; y < gridCells[x].Count; y++)
            {
                if (gridCells[x][y].CellType != CellType.Walkable) continue;
                Vector2 dir = Vector2.zero;
                float min_distance = float.MaxValue;
                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    int nextX = x + dx[i];
                    int nextY = y + dy[i];
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

                    if (gridCells[nextX][nextY].DistanceFromPlayer == min_distance)
                    {
                        count++; ;
                        dir += gridManagement.GridBuilder.GridToWorldPosition(new Vector2(nextX, nextY)) - gridManagement.GridBuilder.GridToWorldPosition(new Vector2(x, y));

                    }

                }
                dir = (dir / count).normalized;
                gridCells[x][y].SetFlowDirection(dir);

            }
        }


    }

}
