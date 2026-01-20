using System;
using System.Security.Cryptography;
using NUnit.Framework.Internal;
using UnityEngine;

[Serializable]
public class SceneEdge
{
    [SerializeField]private int from;
    public int From => from;

    [SerializeField]private int to;

    public int To => to;
    
    [SerializeField] private Vector3 posPortal;
    public Vector3 PosPortal => posPortal;

}
