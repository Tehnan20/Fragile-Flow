using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum TimerStates
{
    IDLE = 0,
    STARTED = 1,
    RUNNING = 2,
    ENDED = 3
}

public class TimerScript
{
    private float CurrentSeconds = 0;
    private float StartSeconds = 0;
    private TimerStates State;

    public System.Action OnTimerStarted;
    public System.Action OnTimerRunning;
    public System.Action OnTimerCompleted;

    public TimerScript(float seconds)
    {
        this.Set(seconds);
        OnTimerStarted = null;
        OnTimerRunning = null;
        OnTimerCompleted = null;
    }

    public void Set(float seconds)
    {
        StartSeconds = CurrentSeconds = seconds;
        State = TimerStates.IDLE;
    }

    public void Reset()
    {
        CurrentSeconds = StartSeconds;
        State = TimerStates.IDLE;
    }

    public void AddSeconds(float seconds)
    {
        CurrentSeconds += seconds;

        if(CurrentSeconds >= StartSeconds)
        {
            CurrentSeconds = StartSeconds;
        }
    }

    public void SubtractSeconds(float seconds)
    {
        CurrentSeconds -= seconds;

        if(CurrentSeconds <= 0)
        {
            CurrentSeconds = 0;
        }
    }

    public float GetCurrentSeconds()
    {
        return CurrentSeconds;
    }

    public float GetTotalSeconds()
    {
        return StartSeconds;
    }    

    public float GetProgress0To1Range()
    {
        return (CurrentSeconds / StartSeconds);
    }

    public string GetTimeInSSFormat()
    {
        string timeStr = CurrentSeconds.ToString("00");
        return timeStr;
    }

    public string GetTimeInMMSSFormat()
    {
        int minutes = ((int)CurrentSeconds / 60);
        int seconds = TimeSpan.FromSeconds(CurrentSeconds).Seconds;

        string timeStr = minutes.ToString("00") + ":" + seconds.ToString("00");
        return timeStr;
    } 

    public string GetTimeInHHMMSSFormat()
    {
        int hours = ((int)CurrentSeconds / (60 * 60));
        int minutes = ((int)CurrentSeconds / 60) % 60;
        int seconds = ((int)CurrentSeconds) % 60;

        string timeStr = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        return timeStr;
    } 

    public string GetTimeInHHMMFormat()
    {
        int hours = ((int)CurrentSeconds / (60 * 60));
        int minutes = ((int)CurrentSeconds / 60) % 60;

        string timeStr = hours.ToString("00") + ":" + minutes.ToString("00");
        return timeStr;
    } 

    public void Start()
    {
        if(State == TimerStates.IDLE)
        {
            State = TimerStates.STARTED;
        }        
    }

    public void Stop()
    {
        if(State != TimerStates.IDLE)
        {
            this.Reset();
            State = TimerStates.IDLE;
        }
    }

    public void Run(float deltaTime)
    {
        if(State == TimerStates.IDLE)
        {

        }
        else
        if(State == TimerStates.STARTED)
        {
            State = TimerStates.RUNNING;
            if(OnTimerStarted != null)
            {
                OnTimerStarted();
            }           
        }
        else
        if(State == TimerStates.RUNNING)
        {
            if(OnTimerRunning != null)
            {
                OnTimerRunning();
            }

            CurrentSeconds -= deltaTime;

            if(CurrentSeconds <= 0)
            {
                State = TimerStates.ENDED;
            }
        }
        else
        if(State == TimerStates.ENDED)
        {
            State = TimerStates.IDLE;            
            if(OnTimerCompleted != null)
            {
                OnTimerCompleted();
            }
        }
    }
}
