package state.enums;

import java.io.Serializable;

public enum Time implements Serializable {

	NIGHT(null), EVENING(NIGHT), NOON(EVENING), MORNING(NOON);
	
	private Time next;
	
	private Time(Time next) {
		this.next = next;
	}
	
	public Time getNext() {
		return next;
	}
}
