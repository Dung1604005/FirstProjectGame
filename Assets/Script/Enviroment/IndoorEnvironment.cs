using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IndoorEnvironment : EnviromentBase
{
    [SerializeField] private float lightIntense;

    [SerializeField] private Color32 lightColor;
    
    public override void Apply()
    {
        light2D.intensity = lightIntense;
        light2D.color = lightColor;
        
    }
    public override void SetActive(bool active)
    {
        this.gameObject.SetActive(true);
        
    }
}
