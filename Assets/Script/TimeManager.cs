using System;
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
    
    public TimeState timeState;
    public TimeState TimeState => timeState;

    [SerializeField] private float timerPerDay;

    public float TimerPerDay => timerPerDay;
   
    private float elapseTime = 0f;

    public float ElapseTime => elapseTime;

    public event Action ChangeToDay;

    public event Action ChangeToMidDay, ChangeToNight, ChangeToMidNight;
    
    private void Awake()
    {
        timeState = TimeState.MidNight;
        

    }
    void Start()
    {
        
    }
    
    public void Update()
    {
        elapseTime += Time.deltaTime;

        float t = (elapseTime / (timerPerDay * 60f)) % 1f;
        
        if (t <= 0.15f)
        {
            if(timeState != TimeState.Day)
            {
                ChangeToDay?.Invoke();
                timeState = TimeState.Day;
            }
            
           
        }
        else if (t <= 0.4f)
        {
            if(timeState != TimeState.MidDay)
            {
                ChangeToMidDay?.Invoke();
                timeState = TimeState.MidDay;
            }
            
        }
        else if (t <= 0.55f)
        {
            if(timeState != TimeState.Night)
            {
                ChangeToNight?.Invoke();
                timeState = TimeState.Night;
            }
            
        }
        else if (t <= 1f)
        {
            if(timeState != TimeState.MidNight)
            {
                ChangeToMidNight?.Invoke();
                timeState = TimeState.MidNight;
            }
           
        }
        

    }
    
}
