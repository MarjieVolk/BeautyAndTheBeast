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
		
		private static readonly String filePath = Application.dataPath + "\\Bab.xml";
		private static GameState instance = loadState();
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
		public static GameState getInstance() {
			return instance;
		}
		
		public static void saveState() {
      		FileInfo t = new FileInfo(filePath); 
      		if(t.Exists) {
         		t.Delete();
      		}
         	StreamWriter writer = t.CreateText(); 
      		writer.Write(SerializeObject(getInstance().data)); 
      		writer.Close();
		}
		
		private static GameState loadState() {
			GameState state = null;
			if (new FileInfo(filePath).Exists) {
				StreamReader r = File.OpenText(filePath); 
	      		String info = r.ReadToEnd(); 
	      		r.Close();
				SerializableDictionary<String, object> loadedData =
					(SerializableDictionary<String, object>) DeserializeObject(info);
				if (loadedData != null) {
					state = new GameState(loadedData);
				}
			}
			
			if (state == null) {
				state = new GameState();
			}
			return state;
		}
		
		// Convert object to serialized xml string
   		private static string SerializeObject(object pObject) { 
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

