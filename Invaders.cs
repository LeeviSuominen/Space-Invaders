using Raylib_CsLo;
using System.Globalization;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace SpaceInvaders
{
	class Invaders
    {
		public enum GameState{
			Start,
			Play,
			ScoreScreen,
			OptionScreen,
			PauseScreen,
			DeveloperMenu
		}
		public GameState state;
        int window_width = 900;
        int window_height = 900;

        Player player;
        List<Bullet> bullets;
        List<Enemy> enemies;

		float playerBulletSpeed = 330;
		int bulletWidth;
		int BulletHeight;

		double enemyShootInterval;
		double lastEnemyShootTime;
		float enemyBulletSpeed;
		float enemySpeed;
		float enemySpeedDown;
		float enemyMaxYLine;

		public Texture playerImage;
		List<Texture> enemyImages;
		List<Sound> soundEffects;
		Sound playerShoot;
		Sound playerDie;
		Sound enemyExplode;
		Music menuMusic;
		Texture bulletImage;

		int scoreCounter = 0;

        MainMenu menu = new MainMenu();
		OptionsMenu options = new OptionsMenu();
		PauseMenu pauseMenu = new PauseMenu();


        /// <summary>
        /// The Run function is used for running the program.
        /// </summary>
        public void Run(){
            Init();
            GameLoop();
        }
        /// <summary>
		/// The Init function is used to initialize or set up the program.
		/// </summary>
		public void Init() {
			Raylib.InitWindow(window_width, window_height, "Space Invaders");
            Raylib.SetTargetFPS(30);
			Raylib.SetExitKey(KeyboardKey.KEY_BACKSPACE);
			
			state = GameState.Start;

			// Different enemy images for different rows
			enemyImages = new List<Texture>(4);
			
			enemyImages.Add(Raylib.LoadTexture("data/Images/EnemyPlane.png"));
			enemyImages.Add(Raylib.LoadTexture("data/Images/EnemyPlane2.png"));
			enemyImages.Add(Raylib.LoadTexture("data/Images/EnemyPlane3.png"));
			enemyImages.Add(Raylib.LoadTexture("data/Images/EnemyPlane4.png"));

			bulletImage = Raylib.LoadTexture("data/Images/LaserAmmo.png");

			playerImage = Raylib.LoadTexture("data/Images/PlayerShip.png");

			// Audio for different scenarios in the game
			
			Raylib.InitAudioDevice();
			playerShoot = Raylib.LoadSound("data/Audio/playerShoot.wav");
			playerDie = Raylib.LoadSound("data/Audio/playerDie.wav");
			enemyExplode = Raylib.LoadSound("data/Audio/enemyExplode.wav");
			menuMusic = Raylib.LoadMusicStream("data/Audio/menuMusic.mp3");
			
			menu = new MainMenu();
			menu.StartButtonPressed += OnStartButtonPressed;
			menu.OptionsButtonPressed += OnOptionsButtonPressed;
			options.BackButtonPressed += OnBackButtonPressed;
			pauseMenu.BackToMainMenu += OnMainMenuPressed;
			pauseMenu.BackToGame += OnBackToGame;

			ResetGame();
		}

		void OnStartButtonPressed(object sender, EventArgs args){
			ResetGame();
			state = GameState.Play;
		}

		void OnOptionsButtonPressed(object sender, EventArgs args){
			state = GameState.OptionScreen;
			
		}

		void OnBackButtonPressed(object sender, EventArgs args){
			state = GameState.Start;
		}

		void OnMainMenuPressed(object sender, EventArgs args){
			state = GameState.Start;
		}

		void OnBackToGame(object sender, EventArgs args){
			state = GameState.Play;
		}

		public void SetMusicVolume(){
			Raylib.SetMusicVolume(menuMusic, options.volume);
		}

		/// <summary>
		/// The ResetGame function is used to reset the game to its initial state.
		/// </summary>
		public void ResetGame()
		{
			int playerWidth = 40;
			int playerHeight = 40;
			int enemyWidth = playerWidth;
			int enemyHeight = playerHeight;
            float playerSpeed = 300;
			playerBulletSpeed = 325;
			bulletWidth = 16;
			BulletHeight = 16;
			float enemySpeed = 175;

            Vector2 playerStart = new Vector2(window_width / 2, window_height - 35);
            player = new Player(playerStart, playerSpeed, playerHeight, playerWidth, Raylib.WHITE, playerImage);
            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

			enemyShootInterval = 1.0f;
			lastEnemyShootTime = 5.0f; // Delays the first enemy shot
			enemyBulletSpeed = 225;
			enemySpeed = playerSpeed;
			enemySpeedDown = 10;
			enemyMaxYLine = window_height - playerWidth * 4;

			int rows = 4;
			int columns = 4;
			int startX = 0;
			int startY = playerHeight;
			int currentX = startX;
			int currentY = startY;
			int enemyBetween = playerHeight;

			int enemySize = playerWidth;

			int maxScore = 40;
			int minScore = 10;
			int currentScore = maxScore;

			for (int row = 0; row < rows; row++)
			{
				currentX = startX; // Reset at start of new row

				// Score decreases when goin down
				currentScore = maxScore - row * 10;
				if (currentScore < minScore)
				{
					currentScore = minScore;
				}
				for (int col = 0; col < columns; col++)
				{
					Vector2 enemyStart = new Vector2(currentX, currentY);
					int enemyScore = currentScore;

					Enemy enemy = new Enemy(enemyStart, new Vector2(1, 0), enemySpeed, enemyWidth, enemyHeight, enemyImages[row], enemyScore);

					enemies.Add(enemy);

					currentX += playerWidth + enemyBetween; // Horizontal space between enemies
				}
				currentY += playerHeight + enemyBetween; // Vertical space between enemies
			}
		}

		

        /// <summary>
		/// The GameLoop function is used to control the flow of a game by repeatedly executing a set of
		/// instructions.
		/// </summary>
		public void GameLoop(){
            while (Raylib.WindowShouldClose() == false){
				switch(state)
				{
					case GameState.Start:
					Raylib.UpdateMusicStream(menuMusic);
					Raylib.PlayMusicStream(menuMusic);
					menu.Init();
					menu.MenuUpdate();
					Raylib.BeginDrawing();
					Raylib.ClearBackground(Raylib.BLACK);
					menu.MenuDraw();
					Raylib.EndDrawing();
					break;

					case GameState.Play:
					Raylib.BeginDrawing();
                	Raylib.ClearBackground(Raylib.BLACK);
                	Draw();
                	Update();
					if(Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)){
						state = GameState.PauseScreen;
					}
                	Raylib.EndDrawing();
					break;
					
					case GameState.ScoreScreen:
					ScoreUpdate();
					Raylib.BeginDrawing();
					Raylib.ClearBackground(Raylib.BLACK);
					ScoreDraw();
					Raylib.EndDrawing();
					break;

					case GameState.OptionScreen:
					Raylib.UpdateMusicStream(menuMusic);
					Raylib.PlayMusicStream(menuMusic);
					SetMusicVolume();
					Raylib.BeginDrawing();
					Raylib.ClearBackground(Raylib.BLACK);
					options.Draw();
					Raylib.EndDrawing();
					break;

					case GameState.PauseScreen:
					Raylib.BeginDrawing();
					Raylib.ClearBackground(Raylib.BLACK);
					pauseMenu.Draw();
					Raylib.EndDrawing();
					break;
				}
            }

			Raylib.UnloadSound(playerShoot);
			Raylib.UnloadSound(playerDie);
			Raylib.UnloadSound(enemyExplode);
			Raylib.UnloadMusicStream(menuMusic);
			Raylib.CloseAudioDevice();
        }
        
        /// <summary>
		/// This function keeps a given transform inside a specified area defined by the left, top,
		/// right, and bottom boundaries.
		/// </summary>
		/// <param name="Transform">The Transform component of the game object that you want to keep
		/// inside the specified area.</param>
		/// <param name="SpriteRenderer">The SpriteRenderer component is used to render a sprite on a
		/// GameObject. It contains properties such as the sprite to be rendered, the color of
		/// the sprite, and the sorting order of the sprite in the rendering order.</param>
		/// <param name="left">The left boundary of the area.</param>
		/// <param name="top">The top boundary of the area in which the transform should be kept
		/// inside.</param>
		/// <param name="right">The right boundary of the area in which the transform should be kept
		/// inside.</param>
		/// <param name="bottom">The bottom coordinate of the area to keep the transform inside.</param>
		bool KeepInsideArea(Transform transform, SpriteRenderer spriteRenderer, int left, int top, int right, int bottom){
            float newX = Math.Clamp(transform.position.X, left, right - spriteRenderer.width);
            float newY = Math.Clamp(transform.position.Y, top, bottom - spriteRenderer.height);

            bool xChange = newX != transform.position.X;
            bool yChange = newY != transform.position.Y;

            transform.position.X = newX;
            transform.position.Y = newY;

            return xChange || yChange;
        }

		/// <summary>
		/// This function checks if a given transform is inside a specified area defined by the left, top,
		/// right, and bottom boundaries.
		/// </summary>
		/// <param name="Transform">The Transform component of a game object. It contains information about
		/// the position, rotation, and scale of the object in the scene.</param>
		/// <param name="SpriteRenderer">The SpriteRenderer is a component that renders a sprite on a
		/// GameObject. It is used to display 2D sprites in a scene.</param>
		/// <param name="left">The left boundary of the area.</param>
		/// <param name="top">The top coordinate of the area.</param>
		/// <param name="right">The right boundary of the area.</param>
		/// <param name="bottom">The bottom coordinate of the area.</param>
		bool IsInsideArea(Transform transform, SpriteRenderer spriteRenderer,
			int left, int top, int right, int bottom)
		{
			float x = transform.position.X;
			float r = x + spriteRenderer.width;

			float y = transform.position.Y;
			float b = y + spriteRenderer.height;

			if (x < left || y < top || r > right || b > bottom)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
        /// <summary>
		/// The function "UpdatePlayer" is used to update the player's information or state.
		/// </summary>
		void UpdatePlayer()
		{
			bool playerShoots = player.Update();
			KeepInsideArea(player.transform, player.spriteRenderer,
				0, 0, window_width, window_height);
			if (playerShoots)
			{
				// Create bullet
				CreateBullet(player.transform.position,
					new Vector2(0, -1),
					20, 20, 800, Raylib.RED);
				//Console.WriteLine($"Bullet count: {bullets.Count}");
			}
		}
        /// <summary>
		/// The function "UpdateEnemies" is used to update the state of enemies in a game.
		/// </summary>
		void UpdateEnemies()
		{
			bool changeFormationDirection = false;
			bool canGoDown = true; // Enemies can descent by default
			foreach (Enemy enemy in enemies)
			{
				if (enemy.active)
				{
					enemy.Update();

					bool enemyIn = IsInsideArea(enemy.transform, enemy.spriteRenderer, 0, 0, window_width, window_height);

					if (enemyIn == false)
					{
						changeFormationDirection = true;
					}
					if (enemy.transform.position.Y > enemyMaxYLine)
					{
						canGoDown = false;
					}
				}
			}

			if (changeFormationDirection)
			{
				foreach (Enemy enemy in enemies)
				{
					enemy.transform.direction.X *= -1.0f;
					if (canGoDown)
					{
						enemy.transform.position.Y += enemySpeedDown;
					}
				}
			}

			// Check enemy shooting: has interval passed since last shoot
			double timeNow = Raylib.GetTime();
			if (timeNow - lastEnemyShootTime >= enemyShootInterval)
			{
				// Can shoot!
				Enemy shooter = FindBestEnemyShooter();
				if (shooter != null)
				{
					// Create enemy bullet below shooter
					// Travelling down the screen
					// Create bullet on center of enemy

					// Top left corner + half the size - half the bullet's size
					float x = shooter.transform.position.X + shooter.spriteRenderer.width / 2 - bulletWidth / 2;
					// Below shooter
					float y = shooter.transform.position.Y + shooter.spriteRenderer.height;
					Vector2 bPos = new Vector2(x, y);

					CreateBullet(bPos, new Vector2(0, 1), bulletWidth, BulletHeight, enemyBulletSpeed, Raylib.GREEN);

					lastEnemyShootTime = timeNow;
				}
			}
		}

		/// <summary>
		/// This function finds the best enemy shooter.
		/// </summary>
		Enemy FindBestEnemyShooter()
		{
			/* Pick an enemy who:
			 *  is active
			 *  is closest to player on Y axis
			 *  is within treshold to player on X axis.
			 *  
			 *  In a case where the first row has been partially
			 *  destroyed, the function should select from the rows
			 *  above if they have a good enemy
			 *   
			 *   in this situation the enemy O should be selected:
			 *   X = enemy
			 *   P = player
			 *   
			 *   X X X X
			 *   X O X X
			 *         X
			 *         
			 *     P
			 */

			Enemy best = null;

			// Start from worst possible values
			float bestY = 0.0f;
			float bestXDifference = window_width;

			// start from last enemy, since lowest row is last in the enemies list
			for(int i = enemies.Count-1; i >= 0; i--)
			{
				Enemy candidate = enemies[i];
				if (candidate.active)
				{
					// The enemy must be same or below the current best Y or not a valid shooter
					// has been found yet
					if (candidate.transform.position.Y >= bestY || best == null)
					{
						// Found better Y
						bestY = candidate.transform.position.Y;

						// Absolute value : itseisarvo
						float xDifference = Math.Abs(player.transform.position.X - candidate.transform.position.X);
						if (xDifference < bestXDifference && xDifference < bulletWidth)
						{
							bestXDifference = xDifference;
							best = candidate;
						}
					}
				}
			}

			return best;
		}
        /// <summary>
		/// The function "UpdateBullets" is used to update the state of bullets in a game.
		/// </summary>
		void UpdateBullets()
		{
			foreach (Bullet bullet in bullets)
			{
				if (bullet.active)
				{
					bullet.Update();

					bool isOutside = KeepInsideArea(bullet.transform, bullet.spriteRenderer, 0, 0, window_width, window_height);

					if (isOutside)
					{
						bullet.active = false;
					}
				}
			}
		}
        
        /// <summary>
		/// The function "getRectangle" returns a Rectangle object with the position and dimensions of a
		/// given Transform and SpriteRenderer.
		/// </summary>
		/// <param name="Transform">The Transform parameter represents the position and rotation of a
		/// game object in the scene. It contains properties like position, rotation, and scale.</param>
		/// <param name="SpriteRenderer">The SpriteRenderer is a component that renders a sprite on a
		/// game object. It contains properties such as width and height, which represent the size of
		/// the sprite.</param>
		/// <returns>
		/// The method is returning a Rectangle object.
		/// </returns>
		Rectangle getRectangle(Transform t, SpriteRenderer c)
		{
			Rectangle r = new Rectangle(t.position.X,
				t.position.Y, c.width, c.height);
			return r;
		}
       /// <summary>
	   /// The CheckCollisions function is used to detect and handle collisions between objects in a
	   /// game or simulation.
	   /// </summary>
	    void CheckCollisions()
		{
			Rectangle playerRect = getRectangle(player.transform, player.spriteRenderer);
			foreach (Enemy enemy in enemies)
			{
				if (enemy.active == false)
				{
					continue;
				}
				Rectangle enemyRec = getRectangle(enemy.transform, enemy.spriteRenderer);

				foreach (Bullet bullet in bullets)
				{
					if (bullet.active == false)
					{
						continue;
					}
					Rectangle bulletRec = getRectangle(bullet.transform, bullet.spriteRenderer);

					if (bullet.transform.direction.Y < 0)
					{
						if (Raylib.CheckCollisionRecs(bulletRec, enemyRec))
						{
							// Enemy hit!
							Raylib.PlaySound(enemyExplode);
							Console.WriteLine($"Enemy Hit! Got {enemy.scoreValue} points!");
							scoreCounter += enemy.scoreValue;
							enemy.active = false;
							bullet.active = false;

							int enemiesLeft = CountAliveEnemies();
							if (enemiesLeft == 0)
							{
								// Win game
								state = GameState.ScoreScreen;
							}
							// Do not test the rest of bullets
							break;
						}
					}
					else
					{
						if (Raylib.CheckCollisionRecs(bulletRec, playerRect))
						{
							Raylib.PlaySound(playerDie);
							state = GameState.ScoreScreen;
							player.active = false;
						}
					}
				}
			
			}
		}
        /// <summary>
		/// This function creates a bullet object with the specified position, direction, size, speed,
		/// and color.
		/// </summary>
		/// <param name="Vector2">A 2D vector that represents a position or direction in space. It has
		/// two components, x and y, which are typically represented as floating-point numbers.</param>e
		/// <param name="width">The width of the bullet.</param>
		/// <param name="height">The height parameter represents the height of the bullet.</param>
		/// <param name="speed">The speed parameter determines how fast the bullet will move. It is a
		/// float value, which means it can have decimal places. The higher the value, the faster the
		/// bullet will move.</param>
		/// <param name="Color">The color parameter is used to specify the color of the bullet. It can
		/// be any valid color value, such as a predefined color constant or a custom color
		/// value.</param>
		void CreateBullet(Vector2 pos, Vector2 dir, int width, int height, float speed, Color color)
		{
			bool found = false;
			foreach(Bullet bullet in bullets)
			{
				if (bullet.active == false)
				{
					// Reset this
					Raylib.PlaySound(playerShoot);
					bullet.Reset(pos, dir, speed, width, height, color);
					found = true;
					break;
				}
			}
			// No inactive bullets found!
			if (found == false)
			{
				bullets.Add(new Bullet(pos, dir, width, height, speed, bulletImage, color));
			}
		}

        
        /// <summary>
		/// The Update function is a method that is called repeatedly in a game loop to update the game
		/// state.
		/// </summary>
		void Update(){
            UpdatePlayer();
			UpdateEnemies();
			UpdateBullets();
			CheckCollisions();
        }

        /// <summary>
		/// The Draw function is used to draw the game.
		/// </summary>
		void Draw(){
            player.Draw();
            foreach(Bullet bullet in bullets){
                if(bullet.active){
                bullet.Draw();
                }
            }

            foreach(Enemy enemy in enemies){
                if(enemy.active){
                    enemy.Draw();
                }
            }

			// Draw score
			Raylib.DrawText($"Score: {scoreCounter}", 10, 10, 25, Raylib.WHITE);
        }

		/// <summary>
		/// The function counts the number of alive enemies.
		/// </summary>
		int CountAliveEnemies()
		{
			int alive = 0;
			foreach(Enemy enemy in enemies)
			{
				if(enemy.active)
				{
					alive++;
				}
			}
			return alive;
		}
		
		/// <summary>
		/// The ScoreUpdate function is used to update the score in a C# program.
		/// </summary>
		void ScoreUpdate()
		{
			if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
			{
				ResetGame();
				state = GameState.Play;
			}
		}

		/// <summary>
		/// The ScoreDraw function is used to draw the final score and game over and game won texts.
		/// </summary>
		void ScoreDraw(){
			// Center both lines of text usin Raylib.MeasureText

			string scoreText = $"Final score {scoreCounter}";


			string instructionText = "Game over. Press Enter to play again";
			if (player.active == true)
			{
				instructionText = "You Won! Press Enter to play again";
			}

			int fontSize = 20;
			int sw = Raylib.MeasureText(scoreText, fontSize);
			int iw = Raylib.MeasureText(instructionText, fontSize);

			Raylib.DrawText(scoreText, window_width /2 - sw / 2
				, window_height/2 - 60, fontSize, Raylib.WHITE);

			Raylib.DrawText(instructionText, window_width /2 - iw / 2, 
				window_height/2, fontSize, Raylib.WHITE);
		}
    }
}