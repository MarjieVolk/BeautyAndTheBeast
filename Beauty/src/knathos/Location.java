package knathos;

import java.io.File;
import java.util.Collection;
import java.util.HashMap;
import java.util.LinkedList;

import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.Rectangle;

public class Location {

	public static enum Direction {
		NORTH, EAST, SOUTH, WEST;

		public Direction turnLeft() {
			switch (this) {
			case NORTH:
				return WEST;
			case EAST:
				return NORTH;
			case SOUTH:
				return EAST;
			case WEST:
				return SOUTH;
			default:
				throw new IllegalStateException("Direction " + this.toString()
						+ " not recognized");
			}
		}

		public Direction turnRight() {
			switch (this) {
			case NORTH:
				return EAST;
			case EAST:
				return SOUTH;
			case SOUTH:
				return WEST;
			case WEST:
				return NORTH;
			default:
				throw new IllegalStateException("Direction " + this.toString()
						+ " not recognized");
			}
		}
	}

	public class Transition {

		private Rectangle clickArea;
		private Location toLoc;

		public Transition(Rectangle clickArea, Location newLocation) {
			this.clickArea = clickArea;
			this.toLoc = newLocation;
		}

		public Rectangle clickArea() {
			return clickArea;
		}

		public Location newLocation() {
			return toLoc;
		}
	}

	private TextureRegion[] imgs;
	private String id;
	private HashMap<Direction, LinkedList<Transition>> transitions = new HashMap<Direction, LinkedList<Transition>>();

	public Location(TextureRegion north, TextureRegion south,
			TextureRegion east, TextureRegion west, String id) {
		imgs = new TextureRegion[] { north, east, south, west };
		this.id = id;
	}

	/**
	 * Creates a location, assuming that the images associated with this
	 * location are children of the directory "imageDir" and that they are each
	 * named "east", "west", "north", or "south" according to their direction.
	 * 
	 * @param imageDir
	 *            The relative directory in which this location's images are
	 *            located
	 * @param fileType
	 *            The file type extension of all of the images (ex: ".jpg")
	 */
	public Location(Room room, String imageDir) {
		this(room.getTexture(imageDir + "/north"), room
				.getTexture(imageDir + "/south"), room
				.getTexture(imageDir + "/east"), room
				.getTexture(imageDir + "/west"), imageDir);
	}
	
	public String toString() {
		return id;
	}

	public void addTransition(Direction directionFacing, Rectangle clickArea,
			Location newLocation) {
		LinkedList<Transition> t = transitions.get(directionFacing);
		if (t == null) {
			t = new LinkedList<Transition>();
			transitions.put(directionFacing, t);
		}

		t.add(new Transition(clickArea, newLocation));
	}

	public Collection<Transition> getTransitions(Direction dir) {
		LinkedList<Transition> t = transitions.get(dir);
		if (t == null) {
			t = new LinkedList<Transition>();
			transitions.put(dir, t);
		}
		return t;
	}

	public TextureRegion getImage(Direction dir) {
		return imgs[dir.ordinal()];
	}

	public boolean hasDirection(Direction dir) {
		return getImage(dir) != null;
	}

	public Direction getLeftOf(Direction dir) {
		Direction newDir = dir.turnLeft();
		if (!hasDirection(newDir))
			newDir = newDir.turnLeft();

		if (hasDirection(newDir))
			return newDir;
		else
			return dir;
	}

	public Direction getRightOf(Direction dir) {
		Direction newDir = dir.turnRight();
		if (!hasDirection(newDir))
			newDir = newDir.turnRight();

		if (hasDirection(newDir))
			return newDir;
		else
			return dir;
	}
}
