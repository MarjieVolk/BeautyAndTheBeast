package state.interactionpoints;

import java.awt.Point;
import java.awt.Rectangle;
import java.awt.Shape;

public abstract class InteractionPoint {

	private Shape interactionArea;
	
	public boolean contains(Point p) {
		return interactionArea.contains(p);
	}
	
	public Rectangle getBounds() {
		return interactionArea.getBounds();
	}
	
	public abstract void mousePressed(Point p);
	
	public abstract void mouseReleased(Point p);
	
	public abstract void mouseMoved(Point p);
}
