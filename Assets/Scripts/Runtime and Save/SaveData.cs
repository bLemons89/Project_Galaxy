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
public class SaveData : MonoBehaviour
{
    public string currentSceneName;
    public Vector3 playerPosition;

    //store positions in each scene
    public Dictionary<string, Vector3> scenePositions = new Dictionary<string, Vector3>();
    public List<SceneObjectData> sceneObjects = new List<SceneObjectData>();    //store object statuses per scene
}
