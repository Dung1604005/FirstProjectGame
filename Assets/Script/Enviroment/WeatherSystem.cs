using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum WeatherState
{
    NORMAL, RAIN, FOG
}
public class WeatherSystem : MonoBehaviour
{
    [SerializeField] private  WeatherState currentWeatherState;

    [SerializeField] private List<Pair<WeatherState, int>> weatherWeightList;

    [SerializeField] private ParticleSystem rainParticalSystem;

    
    public void RandomWeatherState()
    {
        int sum = 0;
        foreach(Pair<WeatherState, int> weatherWeight in weatherWeightList)
        {
            sum += weatherWeight.Second;
        }
        int rad = Random.Range(1, sum);
        int currentWeight = 0;
        foreach(Pair<WeatherState, int> weatherWeight in weatherWeightList)
        {
            currentWeight += weatherWeight.Second;
            if(rad <= currentWeight)
            {
                currentWeatherState = weatherWeight.First;
                rainParticalSystem.Stop();
                ActiveRain(currentWeatherState == WeatherState.RAIN);
                break;
            }
        }
    }
    private void ActiveRain(bool active)
    {
        if (active)
        {
            rainParticalSystem.Play();
        }
    }
    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
        rainParticalSystem.Stop();
        if (active)
        {
            
            ActiveRain(currentWeatherState == WeatherState.RAIN);
        }
        else
        {
            
        }
    }



}
