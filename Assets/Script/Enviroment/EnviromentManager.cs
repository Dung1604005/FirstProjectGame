using UnityEngine;
using UnityEngine.Rendering.Universal;



public enum EnvironmentType
{
    Indoor,
    Outdoor
}
public class EnviromentManager : MonoBehaviour
{
    public EnvironmentType currentType {get; private set;}

    [SerializeField] private IndoorEnvironment indoorEnvironment;

    [SerializeField] private OutdoorEnvironment outdoorEnvironment;

    

    

    public void SwitchEnvironment(EnvironmentType environmentType)
    {
        currentType = environmentType;
        indoorEnvironment.SetActive(currentType == EnvironmentType.Indoor);
        outdoorEnvironment.SetActive(currentType == EnvironmentType.Outdoor);
            
    }

    public void SetInfoForIndoorEnvironment(float lightIntense, Color32 lightColor)
    {
        indoorEnvironment.SetInfo(lightIntense, lightColor);
    }
    void Awake()
    {
        
        SwitchEnvironment(EnvironmentType.Outdoor);
    }




    
}
