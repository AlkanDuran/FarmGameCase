using UnityEngine;

[System.Serializable]
public class GameTimestamp 
{
    public int year; 
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    public Season season;

    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }

    public int day; 
    public int hour;
    public int minute; 

   
    public GameTimestamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }
    
    public GameTimestamp(GameTimestamp timeStamp)
    {
        this.year = timeStamp.year;
        this.season = timeStamp.season;
        this.day = timeStamp.day;
        this.hour = timeStamp.hour;
        this.minute = timeStamp.minute;
    }
    public void UpdateClock()
    {
        minute++; 
        
        if(minute >= 60)
        {
            minute = 0;
            hour++;
        }
        
        if(hour >= 24)
        {
            hour = 0;
            day++; 
        }
        
        if(day > 30)
        {
            day = 1;
            
            if(season == Season.Winter)
            {
                season = Season.Spring;
                year++; 
            } 
            else season++;
        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        var daysPassed = YearsToDays(year) + SeasonsToDays(season) + day;
        var dayIndex =  daysPassed % 7;
        
        return (DayOfTheWeek)dayIndex;
    }
    
    public static int HoursToMinutes(int hour)
    {
        return hour * 60; 
    }
    
    public static int DaysToHours(int days)
    {
        return days * 24; 
    }
    
    public static int SeasonsToDays(Season season)
    {
        var seasonIndex = (int)season;
        return seasonIndex * 30; 
    }
    
    public static int YearsToDays(int years)
    {
        return years * 4 * 30; 
    }

    public static int TimeStampInMinutes(GameTimestamp timestamp)
    {
        return HoursToMinutes(DaysToHours(YearsToDays(timestamp.year))+DaysToHours(SeasonsToDays(timestamp.season))+DaysToHours(timestamp.day)+timestamp.hour)+timestamp.minute;
    }
    
    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        var timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonsToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.hour;
        var timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonsToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.hour;
        var difference = timestamp2Hours - timestamp1Hours;
        
        return Mathf.Abs(difference);
    }
}