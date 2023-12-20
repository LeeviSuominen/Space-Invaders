using Raylib_CsLo;
using System.Numerics;

namespace SpaceInvaders
{
    internal class MainMenu
    {
        int button_width = 240;
        int button_height = 60;
        int window_width = 900;
        int window_height = 900;

        List<Vector2> startScreenStars;

        public event EventHandler StartButtonPressed;
        public event EventHandler OptionsButtonPressed;
        
		/// <summary>
		/// The Init function initializes a list of Vector2 objects with random x and y coordinates within
		/// specified ranges used for the startscreen stars.
		/// </summary>
		public void Init(){
            Random random = new Random();
			startScreenStars = new List<Vector2>(20);
			for(int i = 0; i < startScreenStars.Capacity; i++)
			{
				startScreenStars.Add(new Vector2(random.Next(0, window_width), random.Next(-window_height, -1)));
			}
        }


/// <summary>
/// The `MenuUpdate` function is responsible for updating the positions of the start screen stars.
/// </summary>
        public void MenuUpdate()
		{
            for (int i = 0; i < startScreenStars.Count; i++)
			{
				Vector2 s = startScreenStars[i];
				s.Y = s.Y + 40 * Raylib.GetFrameTime();
				s.Y = s.Y % window_height;
				startScreenStars[i] = new Vector2(s.X, s.Y);
			}
        }

		/// <summary>
		/// The MenuDraw function is used to initiate the drawing process.
		/// </summary>
		public void MenuDraw()
		{
			Raylib.DrawCircleGradient(window_width / 2, window_height / 2 -100, window_height * 3.0f, Raylib.BLACK, Raylib.BLUE);

			// Draw stars
			for(int i = 0; i < startScreenStars.Count; i++)
			{
				Raylib.DrawCircleGradient((int)startScreenStars[i].X, (int)startScreenStars[i].Y + 900, 4.0f, Raylib.WHITE, Raylib.RED);
			}

			Color[] colors = { Raylib.LIGHTGRAY, Raylib.GRAY, Raylib.DARKGRAY, Raylib.RED };
			string gameName1 = "SPACE";
			string gameName2 = "INVADERS";
			int fontSize = 60;
			for (int i = 0; i < colors.Length; i++)
			{
				double change = Math.Sin(Raylib.GetTime() + i) * 4.0;
				double y = 120 + change * 14.0;

				DrawTextCentered(gameName1, (int)(y), fontSize, colors[i]);
				DrawTextCentered(gameName2, (int)(y + 60), fontSize, colors[i]);
				Raylib.DrawText("Movement: A and D", 1, 1, 25, Raylib.WHITE);
				Raylib.DrawText("Shoot: Space", 1, 25, 25, Raylib.WHITE);
				Raylib.DrawText("Pause Game: ESC", 1, 50, 25, Raylib.WHITE);
				Raylib.DrawText("Exit Game: BACKSPACE", 1, 75, 25, Raylib.WHITE);
			}
			
			int center_X = window_width / 2 - button_width / 2;
			int center_Y = window_height / 2 - button_height / 2;
			if(RayGui.GuiButton(new Rectangle(center_X, center_Y, button_width, button_height), "Start game")){
				StartButtonPressed.Invoke(this, EventArgs.Empty);
			}

			if(RayGui.GuiButton(new Rectangle(center_X, center_Y + 100, button_width, button_height), "Settings")){
				OptionsButtonPressed.Invoke(this, EventArgs.Empty);
			}
		}

        /// <summary>
		/// The function draws text centered on the screen at a specified y-coordinate, with a specified font
		/// size and color.
		/// </summary>
		/// <param name="text">The text that you want to draw on the screen.</param>
		/// <param name="y">The y parameter represents the vertical position at which the text will be drawn
		/// on the screen or canvas.</param>
		/// <param name="fontSize">The fontSize parameter is an integer that represents the size of the font
		/// to be used when drawing the text.</param>
		/// <param name="Color">The "Color" parameter refers to the color of the text that will be drawn on
		/// the screen. It can be any valid color value, such as a predefined color name (e.g., "Red", "Blue",
		/// "Green") or a custom color value (e.g., RGB or hexadecimal value</param>
		void DrawTextCentered(string text, int y, int fontSize, Color color)
		{
			int sw = Raylib.MeasureText(text, fontSize);

			Raylib.DrawText(text, window_width /2 - sw / 2
				, y, fontSize, color);
		}
    }

}

