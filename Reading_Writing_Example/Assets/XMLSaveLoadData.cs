using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class XMLSaveLoadData : MonoBehaviour
{
    #region variables

    string _fileName, _fileLocation;
    string _nameData, _healthData, _locationData, _playerName;
    public GameObject playerObject, otherObject;
    public Player player, other;
    string writeText;
    #endregion

    void Start()
    {
        // SET the data path we will access later
        _fileLocation = Application.dataPath;
        // SET the file name we will access later
        _fileName = "SaveXMLData.xml";
        // SET the components of our objects
        player = playerObject.GetComponent<Player>();
        other = otherObject.GetComponent<Player>();
    }

    void OnGUI()
    {
        // SET the Screen Width and Height variables
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;
        //Create the Save button
        if (GUI.Button(new Rect(scrW * 2.5f, scrH * 1.5f, scrW * 2, scrH), "Save Data"))
        {
            //SET the name, health and location as strings to write in our XML document
            _nameData = playerObject.name;
            _healthData = player.health.ToString(); ;
            _locationData = player.transform.position.ToString();
            //Serialize the data into the correct format (Dividing data with the '|' char
            writeText = SerializeData(_nameData + "|" + _healthData + "|" + _locationData);
            //Create our XML document
            CreateXML();
            Debug.Log("File with player name Saved");
        }
        //Create the load button
        if (GUI.Button(new Rect(scrW * 11.5f, scrH * 1.5f, scrW * 2, scrH), "Load Data"))
        {
            //Load our XML file
            LoadXML();
            //IF data exists
            if (_nameData != null)
            {
                //SET the 2nd objects name, health and location equal to the saved data
                otherObject.name = _nameData;
                other.health = float.Parse(_healthData);
                other.transform.position = StringToVector3(_locationData);
            }
            Debug.Log("Loaded file");
        }
    }

    void CreateXML()
    {
        //Set our serializer and writer
        XmlSerializer ser = new XmlSerializer(typeof(string));
        StreamWriter writer;
        //SET ou file location and name
        FileInfo info = new FileInfo(_fileLocation + "\\" + _fileName);
        //if no info exists
        if (!info.Exists)
        {
            //create the text of the document
            writer = info.CreateText();
        }
        else
        {
            //Delete the previous entry
            info.Delete();
            //create a new one
            writer = info.CreateText();
        }
        //Write the text
        writer.Write(writeText);
        //Serialize data and close the stream
        //ser.Serialize(writer, _nameData + "|" + _healthData + "|" + _locationData);
        writer.Close();

        Debug.Log("File Written");
        //Set name data to nothing to prevent errors
        _nameData = "";
    }
    void LoadXML()
    {
        //SET our StreamReader at file Location as File name
        StreamReader reader = File.OpenText(_fileLocation + "\\" + _fileName);
        // SET the info we need
        string _info = reader.ReadToEnd();
        //Deserialize it
        writeText = (string)DeSerializeData(_info);
        // SET the array and split it by the dividing character we are using
        string[] _infoArray = writeText.Split('|');
        //Closer the reader
        reader.Close();
        //Split the data into it's relevant fields after dividing into the array
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

    string SerializeData(string data) // formats
    {
        // SET the string we will be returning
        string xmlToString = null;
        //Create a new Memory Stream
        MemoryStream memoryStream = new MemoryStream();
        //SET our serializer type
        XmlSerializer xs = new XmlSerializer(typeof(string));
        //Set the Text writer to wite our memory stream in UTF8 format
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //Serialize our data
        xs.Serialize(xmlTextWriter, data);
        //Set our memoryStream
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        //Create a new encoding variable
        UTF8Encoding encoding = new UTF8Encoding();
        //SET an array of bytes using Memory Stream
        byte[] characters = memoryStream.ToArray();
        //Set our string as the text set by the bytes array
        xmlToString = encoding.GetString(characters);
        return xmlToString;
    }
    object DeSerializeData(string data)
    {
        //SET our serializer type
        XmlSerializer xs = new XmlSerializer(typeof(string));
        //SET the memory stream variable to be our data
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(data));
        //Set the text writer stream and encoding format
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //Return the deserialized stream
        return xs.Deserialize(memoryStream);
    }
    byte[] StringToUTF8ByteArray(string pXmlString)
    {
    //SET the encoding to UTF8
        UTF8Encoding encoding = new UTF8Encoding();
        //make an array of bytes using our string
        byte[] byteArray = encoding.GetBytes(pXmlString);
        //return the array 
        return byteArray;
    }
}
