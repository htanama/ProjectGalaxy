/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/30/2025
    Date Updated: 02/01/2025
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

    //store positions in each scene, used when moving from scene to scene
    public List<ScenePositionData> scenePositions = new List<ScenePositionData>();
    public List<string> destroyedObjects = new List<String>();    //keep track of destroyed objects
    public List<CheckpointData> lastCheckpointPositions = new List<CheckpointData>();   //store checkpoints and know which to use
    public List<string> completedObjectives = new List<string>();   //to store completed objectives
    public List<InventorySlotData> inventorySlots = new List<InventorySlotData>();      //to save inventory

    public int energyCellsCollected = 0;  //keep track of collected cells
    public int shardsCollected = 0;       //keep track of collected shards

    //store newest checkpoint
    public void SaveCheckpoint(string sceneName, Vector3 position)
    {
        CheckpointData existing = lastCheckpointPositions.Find(cp => cp.sceneName == sceneName);
        if (existing != null)
        {
            existing.position = position;   //overwrite newest checkpoint if another exists
        }
        else
        {
            lastCheckpointPositions.Add(new CheckpointData(sceneName, position));       //add new checkpoint to scene if one does not already exist
        }
    }

    //get newest checkpoint
    public Vector3 GetCheckpointPosition(string sceneName)
    {
        CheckpointData exising = lastCheckpointPositions.Find(cp =>cp.sceneName == sceneName);
        return exising != null ? exising.position : Vector3.zero;   //returns (0,0,0) if no position found
    }

    //used when goin from scene to scene NOT respawning
    public void SaveSceneTransitionPosition(string sceneName, Vector3 position)
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

    //get position to place player in scene correctly when entering from another scene
    public Vector3 GetSceneTransitionPosition(string sceneName)
    {
        ScenePositionData existing = scenePositions.Find(sp => sp.sceneName == sceneName);
        return existing != null ? existing.position : Vector3.zero;
    }

    //returns the objective if completed
    public bool IsObjectiveCompleted(string objectiveID)
    {
        return completedObjectives.Contains(objectiveID);
    }

    //set the objective as complete
    public void MarkObjectiveAsCompleted(string objectiveID)
    {
        if(!completedObjectives.Contains(objectiveID))
        {
            completedObjectives.Add(objectiveID);
        }
    }
}

[Serializable]
public class CheckpointData
{
    public string sceneName;
    public Vector3 position;

    public CheckpointData(string sceneName, Vector3 position)
    {
        this.sceneName = sceneName;
        this.position = position;
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