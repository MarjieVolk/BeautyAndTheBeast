using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace AssemblyCSharp
{
	[Serializable]
	public class GameState
	{
		#region static constants
		private static readonly String roomKey = "CORE.room";
		private static readonly String locKey = "CORE.location";
		private static readonly String dirKey = "CORE.direction";
		
		private static readonly String saveDir = Application.dataPath + "\\saves";
		private static readonly String lastPlayedSaveFile = saveDir + "\\lastPlayed.txt";
		private static readonly String extension = ".xml";
		
		private static String filePath;
		private static GameState instance;
		#endregion
		
		#region constructors and fields
		private SerializableDictionary<String, object> data;
		
		private GameState () {
			data = new SerializableDictionary<String, object>();
		}
		
		private GameState (SerializableDictionary<String, object> data) {
			this.data = data;
		}
		#endregion
		
		#region methods
		public bool has(String key) {
			return data.ContainsKey(key);
		}
		
		public void put(String key, object val) {
			if (has(key)) {
				data[key] = val;
			} else {
				data.Add(key, val);
			}
		}
		
		public object get(String key) {
			return data[key];
		}
		
		public void setRoom(RoomData r) {
			data[roomKey] = RoomToStringMapping.getName(r);
		}
		
		public RoomData getRoom() {
			if (!has(roomKey)) {
				initializeStartState();
			}
			return RoomToStringMapping.getRoom((String) data[roomKey]);
		}
		
		public void setLocation(String locName) {
			data[locKey] = locName;
		}
		
		public String getLocation() {
			if (!has(locKey)) {
				initializeStartState();
			}
			return (String) data[locKey];
		}
		
		public void setDirection(Direction d) {
			data[dirKey] = (int) d;
		}
		
		public Direction getDirection() {
			if (!has(dirKey)) {
				initializeStartState();
			}
			return (Direction) data[dirKey];
		}
		
		private void initializeStartState() {
			// Define new game start state
			data.Add(locKey, "bottomRight");
			data.Add(dirKey, (int) Direction.NORTH);
			data.Add(roomKey, "Test");
		}
		#endregion
		
		#region static methods
		//static constructor
		static GameState() {
			System.IO.Directory.CreateDirectory(saveDir);
		}
		
		public static GameState getInstance() {
			return instance;
		}
		
		public static void saveCurrentGame() {
      		FileInfo t = new FileInfo(filePath); 
      		if(t.Exists) {
         		t.Delete();
      		}
         	StreamWriter writer = t.CreateText(); 
      		writer.Write(SerializeObject(getInstance().data)); 
      		writer.Close();
		}
		
		public static void startNewGame(String gameName) {
			filePath = getFilePathForName(gameName);
			setLastPlayedGame(gameName);
			instance = new GameState();
		}
		
		public static void loadLastPlayedGame() {
			loadGame(getLastPlayedGame());
		}
		
		public static void loadGame(String gameName) {
			setLastPlayedGame(gameName);
			
			instance = null;
			filePath = getFilePathForName(gameName);
			if (new FileInfo(filePath).Exists) {
				StreamReader r = File.OpenText(filePath); 
	      		String info = r.ReadToEnd(); 
	      		r.Close();
				SerializableDictionary<String, object> loadedData =
					(SerializableDictionary<String, object>) DeserializeObject(info);
				if (loadedData != null) {
					instance = new GameState(loadedData);
				}
			}
			
			if (instance == null) {
				instance = new GameState();
			}
		}
		
		public static Boolean hasLastPlayedFile() {
			return new FileInfo(lastPlayedSaveFile).Exists;
		}
		
		public static string[] getSavedGames() {
			string[] fullNames = Directory.GetFiles(saveDir, "*" + extension);
			string[] simpleNames = new string[fullNames.Length];
			for (int i = 0; i < fullNames.Length; i++) {
				string nameWithExtension = new FileInfo(fullNames[i]).Name;
				simpleNames[i] = Path.GetFileNameWithoutExtension(nameWithExtension);
			}
			return simpleNames;
		}
		
		private static String getFilePathForName(String gameName) {
			return saveDir + "\\" + gameName + extension;
		}
		
		private static String getLastPlayedGame() {
			FileInfo file = new FileInfo(lastPlayedSaveFile);
			if (!file.Exists) {
				throw new ArgumentException("There is no most recently played save");
			}
			
			StreamReader r = File.OpenText(lastPlayedSaveFile);
			String s = r.ReadToEnd();
			r.Close();
			
			return s;
		}
		
		private static void setLastPlayedGame(String name) {
			FileInfo t = new FileInfo(lastPlayedSaveFile); 
      		if(t.Exists) {
         		t.Delete();
      		}
			StreamWriter writer = t.CreateText(); 
      		writer.Write(name); 
      		writer.Close();
		}
		
		// Convert object to serialized xml string
   		private static String SerializeObject(object pObject) { 
      		string XmlizedString = null; 
      		MemoryStream memoryStream = new MemoryStream(); 
     		XmlSerializer xs = new XmlSerializer(typeof(SerializableDictionary<String, object>)); 
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
			xs.Serialize(xmlTextWriter, pObject); 
			memoryStream = (MemoryStream) xmlTextWriter.BaseStream; 
			XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
			return XmlizedString; 
		} 
 
		// Convert serialized xml string back into object
		private static object DeserializeObject(string pXmlizedString) { 
			XmlSerializer xs = new XmlSerializer(typeof(SerializableDictionary<String, object>)); 
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
			return xs.Deserialize(memoryStream); 
		}
		
		private static string UTF8ByteArrayToString(byte[] characters) {      
			UTF8Encoding encoding = new UTF8Encoding(); 
			string constructedString = encoding.GetString(characters); 
			return (constructedString); 
		} 
 
		private static byte[] StringToUTF8ByteArray(string pXmlString) { 
			UTF8Encoding encoding = new UTF8Encoding(); 
			byte[] byteArray = encoding.GetBytes(pXmlString); 
			return byteArray; 
		}
		#endregion
	}
}

