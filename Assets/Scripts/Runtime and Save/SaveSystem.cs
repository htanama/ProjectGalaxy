/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/30/2025
    Date Updated: 01/31/2025
    Description: Handles saving to/from a file
 */
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static string GetSavePath(int slot) => 
        Path.Combine(Application.persistentDataPath, $"SaveSlot_{slot}.json");

    //saving to a json file
    public static void SaveGame(SaveData data, int slot)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(slot), json);
    }

    public static SaveData LoadGame(int slot)
    {
        string path = GetSavePath(slot);

        //create new save if one does not exist
        if(!File.Exists(path))
        {
            return new SaveData();
        }

        string json = File.ReadAllText(path);

        if (string.IsNullOrEmpty(json))
        {
            return new SaveData();      //to prevent crashes if empty
        }

        try
        {
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.Exception e)
        {
            return new SaveData();      //prevent crash if file is corrupt
        }

        return null;    //no save found
    }

    public static void DeleteSave(int slot)
    {
        string path = GetSavePath(slot);
        if(File.Exists(path))
        {
            File.Delete(path);      //delete selected save slot
        }
    }
}
