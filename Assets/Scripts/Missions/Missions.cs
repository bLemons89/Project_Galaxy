
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
    private List<Mission> missions;

    private void Start()
    {
        missions = new List<Mission>
        {
            new Mission("Collect 3 items", "Collect 3 items to complete the mission."),
            new Mission("Defeat the boss", "Defeat the boss to complete the mission.")
        };

        // Example: Mark the first mission as completed after 5 seconds
        StartCoroutine(CompleteMissionAfterTime(5f, 0));
    }

    public void CompleteMission(int missionIndex)
    {
        if (missionIndex >= 0 && missionIndex < missions.Count)
        {
            missions[missionIndex].isCompleted = true;
            NotificationManager.Instance.ShowNotification(missions[missionIndex].missionName + " Completed!");
        }
    }

    private IEnumerator CompleteMissionAfterTime(float delay, int missionIndex)
    {
        yield return new WaitForSeconds(delay);
        CompleteMission(missionIndex);
    }
}
