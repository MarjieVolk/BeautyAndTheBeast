package ui;

import java.awt.Image;
import java.awt.Point;
import java.util.Collection;
import java.util.Iterator;
import java.util.LinkedList;

import ui.Screen.Overlay;

public class Screen implements Iterable<Overlay> {

	private Image background;
	private LinkedList<Overlay> overlays;

	public Screen(Image background, Collection<Overlay> overlays) {
		this.background = background;
		this.overlays = new LinkedList<Overlay>(overlays);
	}

	public Image getBackground() {
		return background;
	}

	public void removeOverlay(Overlay o) {
		overlays.remove(o);
	}

	public void addOverlay(Overlay o) {
		overlays.add(o);
	}

	@Override
	public Iterator<Overlay> iterator() {
		return new Iterator<Overlay>() {

			private int i = 0;
			private final Overlay[] ols = overlays.toArray(new Overlay[overlays
					.size()]);

			@Override
			public boolean hasNext() {
				return i < ols.length;
			}

			@Override
			public Overlay next() {
				if (!hasNext()) {
					throw new IllegalStateException();
				}

				i++;
				return ols[i - 1];
			}

			@Override
			public void remove() {
				//NOTE: does not do anything
			}

		};
	}

	public static class Overlay {

		private Image image;
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
		public Overlay(Image image, int x, int y) {
			this.image = image;
			this.x = x;
			this.y = y;
		}

		public Image getImage() {
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
