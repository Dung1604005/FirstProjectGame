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
    [SerializeField] private float dayIntense;
    [SerializeField] private float midDayIntense;
    [SerializeField] private float nightIntense;
    [SerializeField] private float midNightIntense;

    [SerializeField] private ParticleSystem rainEffect;
    private bool IsRainEffectRandom = false;
    private float elapseTime = 0f;
    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        

    }
    void Start()
    {
        
    }
    private void RandomRain()
    {
        if (IsRainEffectRandom)
        {
            return;
            
        }
        IsRainEffectRandom = true;
        rainEffect.Stop();
        int rad = UnityEngine.Random.Range(0, 100);
        Debug.Log(rad);
        if (rad <= 33)
        {
            rainEffect.Play();
        }

    }
    public void Update()
    {
        elapseTime += Time.deltaTime;

        float t = (elapseTime / (timerPerDay * 60f)) % 1f;
        
        if (t <= 0.15f)
        {
            
            RandomRain();
            OnDay();
        }
        else if (t <= 0.4f)
        {
            IsRainEffectRandom = false;
            OnMidDay();
        }
        else if (t <= 0.55f)
        {
            RandomRain();
            OnNight();
        }
        else if (t <= 1f)
        {
            IsRainEffectRandom = false;
            OnMidNight();
        }
        light2D.intensity = animationCurve.Evaluate(t);

    }
    public void OnDay()
    {
        
        light2D.color = new Color32(243, 199, 138, 255);
        animationCurve = AnimationCurve.EaseInOut(0, midNightIntense, 0.1f, dayIntense);
    }
    public void OnMidDay()
    {
        
        light2D.color = new Color32(255, 245, 224, 255);
        animationCurve = AnimationCurve.EaseInOut(0.2f, dayIntense, 0.3f, midDayIntense);

    }
    public void OnNight()
    {
        
        light2D.color = new Color32(233, 155, 116, 255);
        animationCurve = AnimationCurve.EaseInOut(0.4f, midDayIntense, 0.5f, nightIntense);
    }
    public void OnMidNight()
    {
        
        light2D.color = new Color32(24, 38, 61, 255);
        animationCurve = AnimationCurve.EaseInOut(0.55f, nightIntense, 0.65f, midNightIntense);
    }

}
