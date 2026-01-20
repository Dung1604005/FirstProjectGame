using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneGraph", menuName = "SceneGraph")]
public class SceneGraph : ScriptableObject
{

    [SerializeField] private List<SceneEdge> sceneEdges;

    public List<SceneEdge> SceneEdges => sceneEdges;
}
