using System;
using UnityEngine;

[Serializable]
public enum TypeMap
{
    MAINMAP,
    SECONDARYMAP
}
[CreateAssetMenu(fileName = "SceneData", menuName = "Script/SceneData")]
public class SceneData : ScriptableObject
{
    [SerializeField] private String nameScene;
    public String NameScene => nameScene;

    [SerializeField] private String nameBounder;

    public String NameBounder =>  nameBounder;

    [SerializeField] private TypeMap typeMap;


    public TypeMap TypeMap => typeMap;

    [SerializeField] private SceneData parentSceneData;

    public SceneData ParentSceneData => parentSceneData;







    

    

    
}
