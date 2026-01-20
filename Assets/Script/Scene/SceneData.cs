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

    [SerializeField] private int idSceneData;

    public int IdSceneData => idSceneData;
    [SerializeField] private String nameScene;
    public String NameScene => nameScene;

    [SerializeField] private String nameBounder;

    public String NameBounder =>  nameBounder;

    [SerializeField] private TypeMap typeMap;


    public TypeMap TypeMap => typeMap;

    [SerializeField] private EnvironmentType environmentType;

    public EnvironmentType EnvironmentType => environmentType;

    [SerializeField] private float lightIntense;

    public float LightIntense => lightIntense;

    [SerializeField] private Color32 lightColor;

    public Color32 LightColor => lightColor;

    [SerializeField] private SceneData parentSceneData;

    public SceneData ParentSceneData => parentSceneData;







    

    

    
}
