using System.Numerics;
using Raylib_CsLo;

namespace SpaceInvaders
{
    /* The `internal class Player` is defining a class called `Player` that is only accessible within
    the current assembly. It is used to represent a player in a game. */
    internal class Player
    {        
        public SpriteRenderer spriteRenderer;
        public Transform transform {get; private set;}

        double shootInterval = 0.3;
        double lastShootTime;

        public bool active;

       /* The `public Player(Vector2 position, float speed, int height, int width, Color color, Texture
       image)` is a constructor for the `Player` class. It is used to create a new instance of the
       `Player` class with the specified parameters. */
        public Player(Vector2 position, float speed, int height, int width, Color color, Texture image){
            transform = new Transform(position, new Vector2(0,0), speed);
            spriteRenderer = new SpriteRenderer(width, height, color, image, transform);

            lastShootTime =- shootInterval;
            active = true;
        }

        
        /// <summary>
        /// The Update function returns a boolean value.
        /// </summary>
        public bool Update(){
            float deltaTime = Raylib.GetFrameTime();
            if(Raylib.IsKeyDown(KeyboardKey.KEY_A)){
                transform.position.X -= transform.speed * deltaTime;
            }

            else if (Raylib.IsKeyDown(KeyboardKey.KEY_D)){
                transform.position.X += transform.speed * deltaTime;
                }
            
                bool shoot = false;
            
            if(Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                double timeNow = Raylib.GetTime();
                double timeSinceLastShot = timeNow - lastShootTime;
                if(timeSinceLastShot >= shootInterval)
                {
                    lastShootTime = timeNow;
                    shoot = true;
                    return true;
                }

            }
            return shoot;

        }
        public void Draw(){
            spriteRenderer.Draw();
        }
    }
}