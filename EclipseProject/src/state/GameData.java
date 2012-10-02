package state;

import java.io.Serializable;
import java.util.HashMap;

import state.enums.Item;
import state.enums.Room;
import state.enums.Time;


public class GameData implements Serializable {

	private static final long serialVersionUID = 1160271752038929631L;
	public static final int INVENTORY_SIZE = 10;

	private HashMap<Room, RoomData> rooms;
	private Room currentRoom;
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
	
	public RoomData getRoomData(Room room) {
		return rooms.get(room);
	}
	
	public RoomData getCurrentRoomData() {
		return rooms.get(currentRoom);
	}
	
	public Room getCurrentRoom() {
		return currentRoom;
	}
	
	public void moveToRoom(Room room) {
		currentRoom = room;
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
