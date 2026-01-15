using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class EnviromentBase : MonoBehaviour
{
    [SerializeField] protected Light2D light2D;
    public abstract void Apply();

    public abstract void SetActive(bool active);

    
}
