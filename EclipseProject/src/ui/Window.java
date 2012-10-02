package ui;

import java.awt.Color;
import java.awt.Graphics;
import java.io.IOException;

import javax.swing.JFrame;

import state.interactionpoints.InteractionPoint;
import ui.Screen.Overlay;

public class Window extends JFrame {

	private static final long serialVersionUID = -1731679911424005114L;

	private Screen data;
	private ImagePool imgs = new ImagePool();

	public Window(Screen data) {
		super("Beauty and the Beast Game");
		this.setExtendedState(getExtendedState() | JFrame.MAXIMIZED_BOTH);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setUndecorated(true);
		setData(data);
	}

	public void setData(Screen s) {
		if (data != null) {
			for (InteractionPoint p : data.getInteractionPoints()) {
				this.remove(p);
			}
		}
		this.data = s;
		for (InteractionPoint p : data.getInteractionPoints()) {
			this.add(p);
		}
		repaint();
	}

	@Override
	public void paint(Graphics g) {
		g.setColor(new Color(100, 27, 35));
		g.fillRect(0, 0, this.getWidth(), this.getHeight());

		try {
			g.drawImage(imgs.getImage(data.getBackground()), 0, 0, this);
		} catch (IOException e) {
			e.printStackTrace();
		}

		for (Overlay o : data.getOverlays()) {
			try {
				g.drawImage(imgs.getImage(o.getImage()), o.getX(), o.getY(),
						this);
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
	}

}
