package state;

import java.io.Serializable;
import java.util.HashMap;


public class GameData implements Serializable {

	private static final long serialVersionUID = 1160271752038929631L;
	public static final int INVENTORY_SIZE = 10;

	private HashMap<String, Room> rooms;
	private String currentRoom;
	private Item[] inventory;
	private Time currentTime;
	private int day;

	public void advanceTime() {
		Time t = currentTime.getNext();
		if (t == null) {
			throw new IllegalStateException(
					"Cannot advance time when it is already night.  To proceed to the next day, call nextDay()");
		}
		currentTime = t;
	}

	public void nextDay() {
		currentTime = Time.MORNING;
		day++;
	}
	
	public Time getCurrentTime() {
		return currentTime;
	}
	
	public int getDay() {
		return day;
	}
	
	public Room getRoom(String roomName) {
		return rooms.get(roomName);
	}
	
	public Room getCurrentRoom() {
		return rooms.get(currentRoom);
	}
	
	public String getCurrentRoomName() {
		return currentRoom;
	}
	
	public void moveToRoom(String roomName) {
		currentRoom = roomName;
	}
	
	public Item getInventoryItem(int index) {
		return inventory[index];
	}
	
	public int addInventoryItem(Item item) {
		for (int i = 0; i < inventory.length; i++) {
			if (inventory[i] == null) {
				inventory[i] = item;
				return i;
			}
		}
		
		throw new IllegalStateException("Cannot add item: inventory is full");
	}
	
	public Item removeInventoryItem(int index) {
		Item i = inventory[index];
		inventory[index] = null;
		return i;
	}
	
	public boolean removeInventoryItem(Item item) {
		for (int i = 0; i < inventory.length; i++) {
			if (inventory[i].equals(item)) {
				inventory[i] = null;
				return true;
			}
		}
		
		return false;
	}
}
