using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OutdoorEnvironment : EnviromentBase
{
    [SerializeField] private float dayIntense;

    [SerializeField] private Color32 lightColorDay;

    [SerializeField] private float midDayIntense;

    [SerializeField] private Color32 lightColorMidDay;

    [SerializeField] private float nightIntense;

    [SerializeField] private Color32 lightColorNight;

    [SerializeField] private float midNightIntense;

    [SerializeField] private Color32 lightColorMidNight;

    [SerializeField] private WeatherSystem weatherSystem;

    
    public AnimationCurve animationCurve;


    public override void Apply()
    {
        animationCurve = new AnimationCurve();
        animationCurve.AddKey(0f, midNightIntense);
        animationCurve.AddKey(0.1f, dayIntense);
        animationCurve.AddKey(new Keyframe(0.2f, midDayIntense, 0f, 0f));
        animationCurve.AddKey(new Keyframe(0.4f, midDayIntense, 0f, 0f));
        animationCurve.AddKey(0.5f, nightIntense);
        animationCurve.AddKey(0.65f, midNightIntense);
        animationCurve.AddKey(1f, midNightIntense);

        TimeState timeState = GameManageMent.Instance.TimeManager.TimeState;
        if (timeState == TimeState.Day)
        {
            light2D.color = lightColorDay;
        }
        else if(timeState == TimeState.MidDay)
        {
            light2D.color = lightColorMidDay;
        }
        else if (timeState == TimeState.Night)
        {
            light2D.color = lightColorNight;
        }
        else
        {
            light2D.color = lightColorMidNight;
        }
    }
    public override void SetActive(bool active)
    {
        if(active){
            GameManageMent.Instance.TimeManager.ChangeToDay += OnDay;
            GameManageMent.Instance.TimeManager.ChangeToMidDay += OnMidDay;
            GameManageMent.Instance.TimeManager.ChangeToNight += OnNight;
            GameManageMent.Instance.TimeManager.ChangeToMidNight += OnMidNight;
        }
        else{
            GameManageMent.Instance.TimeManager.ChangeToDay -= OnDay;
            GameManageMent.Instance.TimeManager.ChangeToMidDay -= OnMidDay;
            GameManageMent.Instance.TimeManager.ChangeToNight -= OnNight;
            GameManageMent.Instance.TimeManager.ChangeToMidNight -= OnMidNight;
        }
        this.gameObject.SetActive(active);
        weatherSystem.SetActive(active);
        Apply();
    }
    private void OnDay()
    {
        weatherSystem.RandomWeatherState();
        light2D.color = lightColorDay;
    }
    private void OnMidDay()
    {
        weatherSystem.RandomWeatherState();
        light2D.color = lightColorMidDay;
    }
    private void OnNight()
    {
        weatherSystem.RandomWeatherState();
        light2D.color = lightColorNight;
    }
    private void OnMidNight()
    {
        weatherSystem.RandomWeatherState();
        light2D.color = lightColorMidNight;
    }
    void Start()
    {
        
        
    }
    void Update()
    {
        float t = (GameManageMent.Instance.TimeManager.ElapseTime / (GameManageMent.Instance.TimeManager.TimerPerDay * 60f)) % 1f;
        light2D.intensity = animationCurve.Evaluate(t);
    }
}
