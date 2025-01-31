/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/30/2025
    Date Updated: 01/30/2025
    Description: Save data structure to store player position, collected items, and defeated enemies
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneObjectData
{
    public string uniqueID;     //for objects/enemies
    public bool isActive;       //whether or not to spawn the object
}

[Serializable]
public class SaveData
{
    public string currentSceneName;
    public Vector3 playerPosition;

    //store positions in each scene
    public List<ScenePositionData> scenePositions = new List<ScenePositionData>();
    public List<string> destroyedObjects = new List<String>();    //keep track of destroyed objects

    public void SavePlayerPosition(string sceneName, Vector3 position)
    {
        ScenePositionData existing = scenePositions.Find(sp => sp.sceneName == sceneName);
        if ((existing != null))
        {
            existing.position = position;
        }
        else
        {
            scenePositions.Add(new ScenePositionData(sceneName, position));
        }
    }

    public Vector3 GetPlayerPosition(string sceneName)
    {
        ScenePositionData existing = scenePositions.Find(sp => sp.sceneName == sceneName);
        return existing != null ? existing.position : Vector3.zero;
    }
}

[Serializable]
public class ScenePositionData
{
    public string sceneName;
    public Vector3 position;

    public ScenePositionData(string sceneName, Vector3 position)
    {
        this.sceneName = sceneName;
        this.position = position;
    }
}
