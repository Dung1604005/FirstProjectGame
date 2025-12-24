using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Script/SceneData")]
public class SceneData : ScriptableObject
{
    [SerializeField] private String nameScene;
    public String NameScene => nameScene;

    [SerializeField] private GameObject startPoint;

    public Transform StartPoint => startPoint.transform;

    [SerializeField] private PolygonCollider2D polygonCollider2D;

    public PolygonCollider2D PolygonCollider2D => polygonCollider2D;
}
