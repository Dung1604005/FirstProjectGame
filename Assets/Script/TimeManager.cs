using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public enum TimeState
{
    Day, MidDay, Night, MidNight
}
public class TimeManager : MonoBehaviour
{
    public AnimationCurve animationCurve;
    private Light2D light2D;
    public TimeState timeState;
    public TimeState TimeState => timeState;

    [SerializeField] private float timerPerDay;
    private float elapseTime = 0f;
    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        

    }
    void Start()
    {
        
    }
    public void Update()
    {
        elapseTime += Time.deltaTime;

        float t = (elapseTime /  (timerPerDay*60f)) % 1f;
        Debug.Log(t);
        if (t <= 0.25f)
        {
            OnDay();
        }
        else if (t <= 0.5f)
        {
            OnMidDay();
        }
        else if (t <= 0.75f)
        {
            OnNight();
        }
        else if (t <= 1f)
        {
            OnMidNight();
        }
        light2D.intensity = animationCurve.Evaluate(t);

    }
    public void OnDay()
    {
        
        light2D.color = new Color32(243, 199, 138, 255);
        animationCurve = AnimationCurve.EaseInOut(0, 0.2f, 0.25f, 0.55f);
    }
    public void OnMidDay()
    {
        
        light2D.color = new Color32(255, 245, 224, 255);
         animationCurve = AnimationCurve.EaseInOut(0.25f, 0.55f, 0.5f, 1f);

    }
    public void OnNight()
    {
        
        light2D.color = new Color32(233, 155, 116, 255);
        animationCurve = AnimationCurve.EaseInOut(0.5f, 1f, 0.75f, 0.55f);
    }
    public void OnMidNight()
    {
        
        light2D.color = new Color32(24, 38, 61, 255);
        animationCurve = AnimationCurve.EaseInOut(0.75f, 0.55f, 1f, 0.2f);
    }

}
