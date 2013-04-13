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
		private static readonly String posKeyX = "CORE.position.x";
		private static readonly String posKeyY = "CORE.position.y";
		private static readonly String posKeyZ = "CORE.position.z";
		
		private static readonly String rotKeyX = "CORE.rotation.x";
		private static readonly String rotKeyY = "CORE.rotation.y";
		private static readonly String rotKeyZ = "CORE.rotation.z";
		private static readonly String rotKeyW = "CORE.rotation.w";
		
		public static readonly String invKey = "CORE.inventory";
		
		public static readonly int INVENTORY_SIZE = 8;
		
		private static readonly String saveDir = Application.dataPath + "\\saves";
		private static readonly String lastPlayedSaveFile = saveDir + "\\lastPlayed.txt";
		private static readonly String extension = ".xml";
		
		private static String filePath = getFilePathForName("default");
		private static GameState instance = new GameState();
		#endregion
		
		#region constructors and fields
		private SerializableDictionary<String, object> data;
		private List<GameStateListener> listeners = new List<GameStateListener>();
		
		private GameState () : this (new SerializableDictionary<String, object>()) {}
		
		private GameState (SerializableDictionary<String, object> data) {
			this.data = data;
			initializeStartState(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
		}
		#endregion
		
		#region methods
		public bool has(String key) {
			return data.ContainsKey(key);
		}
		
		public void put(String key, object val) {
			object oldVal = null;
			
			if (has(key)) {
				oldVal = data[key];
				data[key] = val;
			} else {
				data.Add(key, val);
			}
			
			foreach (GameStateListener gsl in listeners) {
				gsl.stateChanged(key, oldVal, val);
			}
		}
		
		public object get(String key) {
			return data[key];
		}
		
		public void setCameraPosition(Vector3 position) {
			put(posKeyX, position.x);
			put(posKeyY, position.y);
			put(posKeyZ, position.z);
		}
		
		public Vector3 getCameraPosition() {
			if (!has(posKeyX)) {
				throw new Exception("Camera Position not initialized");
			}
			return new Vector3((float) get(posKeyX), (float) get(posKeyY), (float) get(posKeyZ));
		}
		
		public void setCameraRotation(Quaternion rotation) {
			put(rotKeyX, rotation.x);
			put(rotKeyY, rotation.y);
			put(rotKeyZ, rotation.z);
			put(rotKeyW, rotation.w);
		}
		
		public Quaternion getCameraRotation() {
			if (!has(rotKeyX)) {
				throw new Exception("Camera Rotaiton not initialized");
			}
			return new Quaternion((float) get(rotKeyX), (float) get(rotKeyY),
				(float) get(rotKeyZ), (float) get(rotKeyW));
		}
		
		public int addItem(String itemId) {
			String[] inv = getInventory();
			
			for (int i = 0; i < inv.Length; i++) {
				if (inv[i] == null) {
					inv[i] = itemId;
					setInventory(inv);
					return i;
				}
			}
			
			throw new Exception("Item cannot be added to inventory - Inventory is full");
		}
		
		public bool removeItem(String itemId) {
			String[] inv = getInventory();
			
			for (int i = 0; i < inv.Length; i++) {
				if (itemId.Equals(inv[i])) {
					inv[i] = null;
					setInventory(inv);
					return true;
				}
			}
			
			return false;
		}
		
		public void setInventory(String[] inv) {
			String[] oldInv = getInventory();
			
			for (int i = 0; i < INVENTORY_SIZE; i++) {
				put(invKey + "." + i, inv[i]);
			}
			
			foreach (GameStateListener gsl in listeners) {
				gsl.stateChanged(invKey, oldInv, inv);
			}
		}
		
		public String[] getInventory() {			
			String[] inv = new String[INVENTORY_SIZE];
			for (int i = 0; i < INVENTORY_SIZE; i++) {
				inv[i] = (String) get(invKey + "." + i);
			}
			
			return inv;
		}
		
		public void addListener(GameStateListener listener) {
			listeners.Add(listener);
		}
		
		private void initializeStartState(Vector3 cameraPosition, Quaternion cameraRotation) {
			// Define new game start state			
			//setCameraPosition(cameraPosition);
			//setCameraRotation(cameraRotation);
			
			for (int i = 0; i < INVENTORY_SIZE; i++) {
				put(invKey + "." + i, null);
			}
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
			
			memoryStream.Close();
			xmlTextWriter.Close();
			
			return XmlizedString; 
		} 
 
		// Convert serialized xml string back into object
		private static object DeserializeObject(string pXmlizedString) { 
			XmlSerializer xs = new XmlSerializer(typeof(SerializableDictionary<String, object>)); 
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
			
			object o = xs.Deserialize(memoryStream);
			
			memoryStream.Close();
			
			return o;
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

