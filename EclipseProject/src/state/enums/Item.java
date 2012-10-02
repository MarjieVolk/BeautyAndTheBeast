package state.enums;

import java.io.Serializable;

public enum Item implements Serializable {

	// TODO: list all items in game, with necessary information
	// (ie, inventory image filename)
	;
	
	private Item(String inventoryImage) {
		this.inventoryImage = inventoryImage;
	}
	
	private String inventoryImage;
	
	public String getInventoryImageFilename() {
		return inventoryImage;
	}
}
