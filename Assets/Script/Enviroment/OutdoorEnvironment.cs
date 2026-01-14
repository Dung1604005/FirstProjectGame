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

    
    public AnimationCurve animationCurve;


    public override void Apply()
    {
        animationCurve = new AnimationCurve();
        animationCurve.AddKey(0f, midNightIntense);
        animationCurve.AddKey(0.1f, dayIntense);
        animationCurve.AddKey(0.3f, midDayIntense);
        animationCurve.AddKey(0.5f, nightIntense);
        animationCurve.AddKey(0.65f, midNightIntense);
        animationCurve.AddKey(1f, midNightIntense);
    }
    public override void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
    private void OnDay()
    {
        light2D.color = lightColorDay;
    }
    private void OnMidDay()
    {
        light2D.color = lightColorMidDay;
    }
    private void OnNight()
    {
        light2D.color = lightColorNight;
    }
    private void OnMidNight()
    {
        light2D.color = lightColorMidNight;
    }
    void Start()
    {
        GameManageMent.Instance.TimeManager.ChangeToDay += OnDay;
        GameManageMent.Instance.TimeManager.ChangeToMidDay += OnMidDay;
        GameManageMent.Instance.TimeManager.ChangeToNight += OnNight;
        GameManageMent.Instance.TimeManager.ChangeToMidNight += OnMidNight;
        
    }
    void Update()
    {
        float t = (GameManageMent.Instance.TimeManager.ElapseTime / (GameManageMent.Instance.TimeManager.TimerPerDay * 60f)) % 1f;
        light2D.intensity = animationCurve.Evaluate(t);
    }
}
