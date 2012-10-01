package state;

import java.io.Serializable;
import java.util.HashMap;

public abstract class Room implements Serializable {

	private static final long serialVersionUID = -7501007216422403699L;

	private HashMap<String, Serializable> values;
	
	protected Serializable getValue(String key) {
		return values.get(key);
	}
	
	protected void putValue(String key, Serializable value) {
		values.put(key, value);
	}
}
