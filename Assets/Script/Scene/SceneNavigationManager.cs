using System.Collections.Generic;
using System.Data;
using NUnit.Framework.Internal;
using UnityEngine;

public class SceneNavigationManager : MonoBehaviour
{
    [SerializeField] private SceneGraph sceneGraph;


    private Dictionary<Pair<int, int>, Vector3> mapPositionPortal;

    private List<List<int>> adj;

    private void BuildGraphDictionary()
    {
        
        mapPositionPortal = new Dictionary<Pair<int, int>, Vector3>();
        List<SceneEdge> sceneEdges = sceneGraph.SceneEdges;
        adj = new List<List<int>>();
        for(int i = 0; i < SceneLoader.Instance.TotalSceneData; i++)
        {
            adj.Add(new List<int>());

        }
        foreach(SceneEdge sceneEdge in sceneEdges)
        {
            mapPositionPortal.Add(new Pair<int, int> (sceneEdge.From, sceneEdge.To), sceneEdge.PosPortal);
            adj[sceneEdge.From].Add(sceneEdge.To);
        }

    }

    public Vector3 GetNextPosition(int startId, int endId, Vector3 positionEnd)
    {
        if(startId== endId)
        {
            return positionEnd;
        }
        int[] trace = new int[SceneLoader.Instance.TotalSceneData + 5];
        for(int i = 0; i < trace.Length; i++)
        {
            trace[i] =-1;
        }
        Queue<int> bfs = new Queue<int>();
        bfs.Enqueue(startId);
        while(bfs.Count > 0)
        {
            int from = bfs.Peek();
            bfs.Dequeue();
            for(int i = 0; i < adj[from].Count; i++)
            {
                int to = adj[from][i];
                if(trace[to] == -1)
                {
                    trace[to] = from;
                    if(to == endId)
                    {
                        break;
                    }
                    bfs.Enqueue(to);
                }    
            }
        }
        List<int> path = new List<int>();
        int current = endId;
        path.Add(endId);
        while(trace[current] != -1)
        {
            current = trace[current];
            path.Add(current);
        }
        // Day path bi nguoc nen start la vi tri cuoi  
        int _from = startId;
        int _to = path[path.Count - 2];
        if(mapPositionPortal.ContainsKey(new Pair<int, int>(_from, _to)))
        {
            return mapPositionPortal[new Pair<int, int>(_from, _to)];
        }
        else
        {
            Debug.LogError("CANNOT FIND PORTAL");
            return  Vector3.zero;
        }
    }
    void Start()
    {
        BuildGraphDictionary();
    }

    

    
} 
