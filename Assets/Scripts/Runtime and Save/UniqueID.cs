/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/30/2025
    Date Updated: 01/30/2025
    Description: UniqueID system to keep items/enemies persistence between scenes
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    public string uniqueID;

    // Start is called before the first frame update
    void Awake()
    {
        if(string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }
    }
}
