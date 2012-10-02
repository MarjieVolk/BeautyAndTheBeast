package ui;

import java.util.Collection;

import state.interactionpoints.InteractionPoint;

public class Screen {

	private String background;
	private Collection<Overlay> overlays;
	private Collection<InteractionPoint> ips;

	public Screen(String background, Collection<Overlay> overlays,
			Collection<InteractionPoint> interactionPoints) {
		this.background = background;
		this.overlays = overlays;
		this.ips = interactionPoints;
	}

	public String getBackground() {
		return background;
	}

	public Overlay[] getOverlays() {
		return overlays.toArray(new Overlay[overlays.size()]);
	}
	
	public InteractionPoint[] getInteractionPoints() {
		return ips.toArray(new InteractionPoint[ips.size()]);
	}

	public static class Overlay {

		private String image;
		private int x;
		private int y;

		/**
		 * Constructs a new Overlay object with the given image and location
		 * 
		 * @param image
		 *            The overlay image
		 * @param location
		 *            The top left coordinate of this overlay, with respect to
		 *            the background image
		 */
		public Overlay(String image, int x, int y) {
			this.image = image;
			this.x = x;
			this.y = y;
		}

		public String getImage() {
			return image;
		}

		public int getX() {
			return x;
		}

		public int getY() {
			return y;
		}

		public boolean equals(Object other) {
			if (!(other instanceof Overlay)) {
				return false;
			}

			Overlay oth = (Overlay) other;
			return image.equals(oth.image) && x == oth.x && y == oth.y;
		}
	}
}
