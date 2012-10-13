package knathos;

import java.util.Collection;
import java.util.HashMap;

import knathos.Location.Direction;
import knathos.Location.Transition;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureAtlas;
import com.badlogic.gdx.graphics.g2d.TextureRegion;

public class Room implements Screen {

	private static final double TURN_MARGIN = 0.18;

	private BeautyGame game;
	private TextureAtlas atlas;
	private HashMap<String, TextureRegion> textures = new HashMap<String, TextureRegion>();

	private Location loc = null;
	private Direction dir = Direction.NORTH;

	// private OrthographicCamera camera;
	private SpriteBatch batch;

	private int width = Gdx.graphics.getWidth();
	private int height = Gdx.graphics.getHeight();

	public Room(BeautyGame game, String textureFile) {
		this.game = game;
		atlas = new TextureAtlas(textureFile);

		// camera = new OrthographicCamera(1, h / w);
		batch = new SpriteBatch();
	}

	public TextureRegion getTexture(String name) {
		TextureRegion t = textures.get(name);
		if (t == null) {
			t = atlas.findRegion(name);
			textures.put(name, t);
		}
		return t;
	}

	public void setLocation(Location l) {
		loc = l;
	}

	public void setDirection(Direction d) {
		dir = d;
	}

	@Override
	public void render(float delta) {
		// Update location if clicked
		update: if (Gdx.input.justTouched()) {
			int x = Gdx.input.getX() * width / Gdx.graphics.getWidth();
			int y = Gdx.input.getY() * height / Gdx.graphics.getHeight();
			int margin = (int) (width * TURN_MARGIN);

//			System.out.println("Clicked (" + x + ", " + y + ")\n\t Width: "
//					+ width + ", Margin: " + margin);

			// Check for transitions first
			Collection<Transition> transitions = loc.getTransitions(dir);
			for (Transition t : transitions) {
				if (t.clickArea().contains(x, y)) {
					loc = t.newLocation();
					break update;
				}
			}

			// If no transitions apply, check if click is within the turn margin
			if (x < margin) {
				// Turn left
				dir = loc.getLeftOf(dir);
			} else if (x > width - margin) {
				// Turn right
				dir = loc.getRightOf(dir);
			}
		}

		// Display image
		TextureRegion background = loc.getImage(dir);
		Gdx.gl.glClearColor(1, 1, 1, 1);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);

		// batch.setProjectionMatrix(camera.combined);
		batch.begin();
		batch.draw(background, 0, 0, width, height);
		batch.end();
	}

	@Override
	public void resize(int width, int height) {
		// this.width = Gdx.graphics.getWidth();
		// this.height = Gdx.graphics.getHeight();
	}

	@Override
	public void show() {
		// TODO Auto-generated method stub

	}

	@Override
	public void hide() {
		// TODO Auto-generated method stub

	}

	@Override
	public void pause() {
		// TODO Auto-generated method stub

	}

	@Override
	public void resume() {
		// TODO Auto-generated method stub

	}

	@Override
	public void dispose() {
		batch.dispose();
		atlas.dispose();
	}

}
