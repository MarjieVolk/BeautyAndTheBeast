package ui;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Image;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;
import javax.swing.JFrame;

public class GameFrame extends JFrame {

	private static final long serialVersionUID = -1731679911424005114L;
	
	private static Image bkg = null;
	
	static {
		try {
			bkg = ImageIO.read(new File("art_assets/randomPhoto.jpg"));
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	public GameFrame() {
		super("Beauty and the Beast Game");
		//setVisible(true);
		this.setExtendedState(getExtendedState() | JFrame.MAXIMIZED_BOTH);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	
	@Override
	public void paint(Graphics g) {
		g.setColor(new Color(100, 27, 35));
		g.fillRect(0, 0, this.getWidth(), this.getHeight());
		
		g.drawImage(bkg, 0, 0, this);
		
//		g.setColor(Color.CYAN);
//		g.setFont(new Font("Times New Roman", Font.BOLD, 18));
//		g.drawString("This is our game!", 50, 20);
//		g.fillOval(50, 100, 1300, 700);
	}

}
