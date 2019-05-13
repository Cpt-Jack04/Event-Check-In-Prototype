using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    private string savePath = "";  // String for the path of the json file.

    [SerializeField] private TextMeshProUGUI nameField = null;          // Entry field for the name.
    [SerializeField] private TextMeshProUGUI employeeIDField = null;    // Entry field for the employee ID.

    private void Awake()
    {
        savePath = Application.dataPath + "/saveFile.json";
    }

    /// <summary> Information pulled from the entry field. </summary>
    public void SubmittedInfo()
    {
        EnteredData newData = new EnteredData(nameField.text, employeeIDField.text);    // Saves the data entered to a data container.

        string newJson = JsonUtility.ToJson(newData);   // Converts data to json formatted strings.
        string oldJson = "";                            // Variable to store old json data.

        try
        {
            oldJson = File.ReadAllText(savePath);   // Loads previosuly saved data if any.
        }
        catch (FileNotFoundException)   // If no saved data found...
        {
            Debug.Log("saveFile.json not found.");
        }
        if (oldJson != "")
            oldJson += "\n";

        File.WriteAllText(savePath, oldJson + newJson); // Saves data string to json file.

        GetComponent<AppManager>().BeginAsyncLoading(); // Begins reloading scene.
    }

    private void OnApplicationQuit()
    {
        // Send json data to firebase.

        if (File.Exists(savePath))
            File.Delete(savePath);              // Deletes the json file.
        if (File.Exists(savePath + ".meta"))
            File.Delete(savePath + ".meta");    // Deletes the json meta data file.
    }

    [Serializable]
    private class EnteredData
    {
        public string dateTime;         // Date and time that it was submitted.
        public string eventAttended;    // Event that was attended.
        public string employeeName;     // Name of the person.
        public string employeeID;       // ID number of the person attending.

        /// <summary> Data the user entered. </summary>
        /// <param name="userName"> Name of the user. </param>
        /// <param name="userID"> ID number of the user. </param>
        public EnteredData(string userName, string userID)
        {
            dateTime = DateTime.Now.ToString();
            eventAttended = AppManager.currentEventName;
            employeeName = userName;
            employeeID = userID;
        }
    }
}