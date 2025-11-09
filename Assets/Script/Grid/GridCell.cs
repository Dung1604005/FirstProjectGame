using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
   Walkable  ,
   Blocked     ,
   Breakable
}
public class GridCell
{
   [SerializeField] private Vector2 worldPosition;
   [SerializeField] private CellType cellType;

   [SerializeField] private Vector2 gridPosition;

   [SerializeField] private float distanceFromPlayer;
   public Vector2 WorldPosition => worldPosition;
   public CellType CellType => cellType;

   public float DistanceFromPlayer => distanceFromPlayer;
   public void SetDistanceFromPlayer(float distance)
   {
      distanceFromPlayer = distance;
   }

   public Vector2 GridPosition => gridPosition;

   public void Initialize(Vector2 gridPosition, Vector2 position, CellType type)
   {
      worldPosition = position;
      cellType = type;
      this.gridPosition = gridPosition;
      distanceFromPlayer = float.MaxValue;
   }
   public void SetCellType(CellType type)
   {
      cellType = type;
   }

}
