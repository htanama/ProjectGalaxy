using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    public string missionName;
    public bool isCompleted;
    public string description;
    public string NotificationManager;

    public Mission(string name, string desc)
    {
        missionName = name;
        description = desc;
        isCompleted = false;
    }
}

public class MissionsManager : MonoBehaviour
{
    public List<Mission> missions; 

    private NotificationManager notificationManager;

    void Start()
    {
        // Initialize the missions list
        missions = new List<Mission>();
        missions.Add(new Mission("Mission 1", "Description of Mission 1"));
        missions.Add(new Mission("Mission 2", "Description of Mission 2"));

        
        notificationManager = NotificationManager.Instance;
    }

    public void CompleteMission(int missionIndex)
    {
        if (missionIndex >= 0 && missionIndex < missions.Count)
        {
            missions[missionIndex].isCompleted = true; 
           
            notificationManager.ShowNotification(missions[missionIndex].missionName + " Completed!");
        }
    }

    private IEnumerator CompleteMissionAfterTime(float delay, int missionIndex)
    {
        yield return new WaitForSeconds(delay);
        CompleteMission(missionIndex);
    }

    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }

        void Awake()
        {
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowNotification(string message)
        {
            Debug.Log(message); 
        }
    }

}

