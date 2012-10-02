package main;

import java.awt.Frame;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.IOException;
import java.util.LinkedList;

import state.interactionpoints.InteractionPoint;
import ui.Screen;
import ui.Screen.Overlay;
import ui.Window;

public class Main {

	public static void main(String[] args) throws IOException {		
		LinkedList<Overlay> overlays = new LinkedList<Overlay>();
		overlays.add(new Overlay("Smiley.png", 30, 30));
		
		LinkedList<InteractionPoint> ips = new LinkedList<InteractionPoint>();
		InteractionPoint p = new InteractionPoint(new Rectangle(0, 0, 10, 10));
		p.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent event) {
				System.out.println("Closing");
				Frame[] frames = Frame.getFrames();
				for (Frame f : frames) {
					f.dispose();
				}
			}
			
		});
		ips.add(p);
		
		Screen s = new Screen("randomPhoto.jpg", overlays, ips);
		
		new Window(s).setVisible(true);
	}
}
