using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : LocalSingleton<TimeManager>
{
    [Header("Internal Clock")]
    [SerializeField] private GameTimestamp _timeStamp;

    [Header ("Day and Night cycle")]
    [SerializeField] private Transform _sunTransform;
    [SerializeField] private float _dayCycle = 1.0f;
    
    private readonly List<ITimeTracker> _listeners = new();
    public void Init()
    {
        _timeStamp = new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    private IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / _dayCycle);
        }
    }
    
    public void Tick()
    {
        _timeStamp.UpdateClock();
        foreach(var listener in _listeners)
        {
            listener.ClockUpdate(_timeStamp);
        }
        UpdateSunMovement(); 
    }

    public void SkipTime(GameTimestamp timeToSkipTo)
    {
        int timeToSkipInMinutes = GameTimestamp.TimeStampInMinutes(timeToSkipTo);
        int timeNowInMinutes = GameTimestamp.TimeStampInMinutes(_timeStamp);

        int differenceInMinutes = timeToSkipInMinutes - timeNowInMinutes;

        if(differenceInMinutes <= 0) return;

        for(int i = 0; i < differenceInMinutes; i++)
        {
            Tick();
        }
    }

    private void UpdateSunMovement()
    {
        var timeInMinutes = GameTimestamp.HoursToMinutes(_timeStamp.hour) + _timeStamp.minute;
        var sunAngle = .25f * timeInMinutes - 90;
        
        _sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }
    public GameTimestamp GetGameTimestamp()
    {
        return new GameTimestamp(_timeStamp);
    }
    
    public void RegisterTracker(ITimeTracker listener)
    {
        _listeners.Add(listener);
    }
    public void UnregisterTracker(ITimeTracker listener)
    {
        _listeners.Remove(listener);
    }
}