/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 01/28/2025
    Description: Class to display the objectives on the UI
 */
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectiveText;

    //defining objectives directly in the script as a Dictionary
    private Dictionary<int, string> objectives = new Dictionary<int, string>
    {
        { 0, "Arm yourself" },
        { 1, "Exit the ship" },
        { 2, "Defend yourself" },
        { 3, "Pick up the Energy Cell" },
        { 4, "Explore the planet for more cells" },
        { 5, "Search the areas for a cell" },
        { 6, "Cells collected 2/3" },
        { 7, "All cells collected. Return to the ship." },
        { 8, "Survive." }
    };

    private Queue<int> objectivesQueue = new Queue<int>(); //queue to store the IDs of objectives

    private void Start()
    {
        //add the objectives to queue (in order)
        foreach (var id in objectives.Keys)
        {
            objectivesQueue.Enqueue(id);
        }

        UpdateObjectiveText();
    }

    //completes the current objective and moves to the next
    public void CompleteObjective()
    {
        if (objectivesQueue.Count > 0)
        {
            objectivesQueue.Dequeue(); //remove the current objective
            UpdateObjectiveText();    //update the UI text
        }
        else
        {
            //Debug.Log("All objectives completed!");
        }
    }

    //updates the UI text with the current objective
    private void UpdateObjectiveText()
    {
        if (objectivesQueue.Count > 0)
        {
            int currentObjectiveId = objectivesQueue.Peek();
            objectiveText.text = objectives[currentObjectiveId];
        }
        else
        {
            objectiveText.text = "Not bad, insert the cells into the ship and skedaddle!";
        }
    }

    //add a new objective directly in code
    public void AddObjective(string newObjective)
    {
        int newId = objectives.Count;           //use the next available ID
        objectives.Add(newId, newObjective);
        objectivesQueue.Enqueue(newId);
        UpdateObjectiveText();
    }
}
