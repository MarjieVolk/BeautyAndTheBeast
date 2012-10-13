package knathos;

import knathos.Location.Direction;

import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.math.Rectangle;

public class BeautyGame extends Game {

	@Override
	public void create() {
		int width = Gdx.graphics.getWidth();
		int height = Gdx.graphics.getHeight();

		// String[] folders = { "center_back", "center_windows", "left_back",
		// "left_windows", "right_windows", "right_back" };

		Room r = new Room(this, "diningRoom.txt");

		Location rb = new Location(r, "right_back");
		Location cb = new Location(r, "center_back");
		Location lb = new Location(r, "left_back");
		Location rw = new Location(r, "right_windows");
		Location cw = new Location(r, "center_windows");
		Location lw = new Location(r, "left_windows");
		
		Rectangle clickArea = new Rectangle(width / 4, height / 4,
				width / 2, height / 2);
		
		rb.addTransition(Direction.WEST, clickArea, cb);
		rb.addTransition(Direction.NORTH, clickArea, rw);
		
		cb.addTransition(Direction.WEST, clickArea, lb);
		cb.addTransition(Direction.EAST, clickArea, rb);
		
		lb.addTransition(Direction.NORTH, clickArea, lw);
		lb.addTransition(Direction.EAST, clickArea, cb);
		
		rw.addTransition(Direction.SOUTH, clickArea, rb);
		rw.addTransition(Direction.WEST, clickArea, cw);
		
		cw.addTransition(Direction.WEST, clickArea, lw);
		cw.addTransition(Direction.EAST, clickArea, rw);
		
		lw.addTransition(Direction.SOUTH, clickArea, lb);
		lw.addTransition(Direction.EAST, clickArea, cw);

		// Location l = null;
		// for (String f : folders) {
		// l = new Location(r, f);
		// }
		r.setLocation(rb);
		r.setDirection(Direction.WEST);

		this.setScreen(r);
	}
}
