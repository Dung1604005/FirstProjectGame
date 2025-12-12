using UnityEngine;

public class FireShoot: MonoBehaviour
{
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    void Start()
    {
        TurnOff();
    }
}
