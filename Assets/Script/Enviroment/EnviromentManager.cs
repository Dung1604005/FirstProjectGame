using UnityEngine;



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

    [SerializeField] private WeatherSystem weatherSystem;

    

    public void SwitchEnvironment(EnvironmentType environmentType)
    {
        currentType = environmentType;
        indoorEnvironment.SetActive(currentType == EnvironmentType.Indoor);
        outdoorEnvironment.SetActive(currentType == EnvironmentType.Outdoor);
            
        
    }





    
}
