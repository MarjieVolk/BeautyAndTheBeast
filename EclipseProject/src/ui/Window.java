package ui;

import java.awt.Color;
import java.awt.Graphics;

import javax.swing.JFrame;

import ui.Screen.Overlay;

public class Window extends JFrame {

	private static final long serialVersionUID = -1731679911424005114L;
	
	private Screen data;
	
	public Window(Screen data) {
		super("Beauty and the Beast Game");
		this.setExtendedState(getExtendedState() | JFrame.MAXIMIZED_BOTH);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setUndecorated(true);
		this.data = data;
	}
	
	public void setData(Screen s) {
		this.data = s;
	}
	
	@Override
	public void paint(Graphics g) {
		g.setColor(new Color(100, 27, 35));
		g.fillRect(0, 0, this.getWidth(), this.getHeight());
		
		g.drawImage(data.getBackground(), 0, 0, this);
		for (Overlay o : data) {
			g.drawImage(o.getImage(), o.getX(), o.getY(), this);
		}
	}

}
