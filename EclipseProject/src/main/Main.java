package main;

import java.awt.Image;
import java.io.IOException;
import java.util.LinkedList;

import ui.ImagePool;
import ui.Screen;
import ui.Screen.Overlay;
import ui.Window;

public class Main {

	public static void main(String[] args) throws IOException {
		ImagePool imgs = new ImagePool();
		Image bkg = imgs.getImage("randomPhoto.jpg");
		
		LinkedList<Overlay> overlays = new LinkedList<Overlay>();
		overlays.add(new Overlay(imgs.getImage("Smiley.png"), 30, 30));
		
		Screen s = new Screen(bkg, overlays);
		
		new Window(s).setVisible(true);
	}
}
