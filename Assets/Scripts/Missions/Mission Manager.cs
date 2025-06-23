using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; } 
    private List<Mission> activeMissions = new List<Mission>(); 
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // If there’s another, remove it
        }
    }


    public class QuestEvent
    {
        public string EventName { get; private set; }

        public QuestEvent(string eventName)
        {
            EventName = eventName;
        }
    }




    public class Mission
    {
        public string MissionName { get; set; }

        public Mission(string missionName)
        {
            MissionName = missionName;
        }

        public void ProcessEvent(QuestEvent questEvent)
        {
            // Handle event logic here
        }

    
    }


    
    public void AddMission(Mission mission)
    {
        activeMissions.Add(mission);
        Debug.Log($"New Mission Added: {mission.MissionName}");
    }

    
    public void HandleEvent(QuestEvent questEvent)
    {
        foreach (Mission mission in activeMissions)
        {
            mission.ProcessEvent(questEvent); // Pass the event to each mission
        }
    }
}

