using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
public class SaveLoadData : MonoBehaviour
{
    #region variables

    string _fileName, _fileLocation;
    string _nameData, _healthData, _locationData, _playerName;
    public GameObject playerObject, otherObject;
    public Player player, other;

    #endregion

    void Start()
    {
        _fileLocation = Application.dataPath;
        _fileName = "SaveData.xml";
        player = playerObject.GetComponent<Player>();
        other = otherObject.GetComponent<Player>();
    }

    void OnGUI()
    {
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;

        if (GUI.Button(new Rect(scrW * 2.5f, scrH * 1.5f, scrW * 2, scrH), "Save Data"))
        {
            _nameData = playerObject.name;
            _healthData = player.health.ToString(); ;
            _locationData = player.transform.position.ToString(); ;
            CreateXML();
            Debug.Log("File with player name Saved");
        }

        if (GUI.Button(new Rect(scrW * 11.5f, scrH * 1.5f, scrW * 2, scrH), " Load Data"))
        {
            LoadXML();
            if (_nameData != null)
            {
                otherObject.name = _nameData;
                other.health = float.Parse(_healthData);
                other.transform.position = StringToVector3(_locationData);
            }
            Debug.Log("Loaded file");
        }
    }

    void CreateXML()
    {
        
        StreamWriter writer;
        FileInfo info = new FileInfo(_fileLocation + "\\" + _fileName);
        if (!info.Exists)
        {
            writer = info.CreateText();
        }
        else
        {
            info.Delete();
            writer = info.CreateText();
        }
        writer.Write(_nameData + "|" + _healthData + "|" + _locationData);
        writer.Close();

        Debug.Log("File Written");
        _nameData = "";
    }
    void LoadXML()
    {
        StreamReader reader = File.OpenText(_fileLocation + "\\" + _fileName);
        string _info = reader.ReadToEnd();
        string[] _infoArray = _info.Split('|');
        reader.Close();
        _nameData = _infoArray[0];
        _healthData = _infoArray[1];
        _locationData = _infoArray[2];
        Debug.Log("Read File");
    }
    Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
