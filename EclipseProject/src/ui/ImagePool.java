package ui;

import java.awt.Image;
import java.io.File;
import java.io.IOException;
import java.util.HashMap;

import javax.imageio.ImageIO;

public class ImagePool {

	private HashMap<String, Image> loadedImages = new HashMap<String, Image>();

	/**
	 * Given a filename String, loads and returns the associated image. The file
	 * is assumed to be in the project folder art_assets.  
	 * 
	 * @param filename
	 * @return The loaded Image
	 * @throws IOException
	 */
	public Image getImage(String filename) throws IOException {
		Image i = loadedImages.get(filename);
		if (i == null) {
			i = ImageIO.read(new File("art_assets/" + filename));
			loadedImages.put(filename, i);
		}
		
		return i;
	}
	
}
