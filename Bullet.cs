using Raylib_CsLo;
using System.Numerics;

namespace SpaceInvaders
{
    /* The `internal class Bullet` is defining a class called `Bullet`. This class represents a bullet
    object in a space invaders game. It has several properties and methods related to the bullet's
    behavior and rendering. */
    internal class Bullet
    {
        public SpriteRenderer spriteRenderer;
        public Transform transform;
        public bool active;
        /* The `public Bullet(Vector2 startPosition, Vector2 direction, int width, int height, float
        speed, Texture image, Color color)` is a constructor for the `Bullet` class. It creates a
        new instance of the `Bullet` class and initializes its properties. */
        public Bullet(Vector2 startPosition, Vector2 direction, int width, int height, float speed, Texture image, Color color)
        {
            transform = new Transform(startPosition, direction, speed);
            spriteRenderer = new SpriteRenderer(width, height, Raylib.RED, image, transform);
        }

        /// <summary>
        /// The Reset function sets the initial position, direction, speed, width, height, and color of
        /// an object.
        /// </summary>
        /// <param name="Vector2">A Vector2 is a data structure that represents a 2-dimensional vector,
        /// typically used to store positions or directions in a 2D space. It has two components, x and
        /// y, which are usually represented as floating-point numbers. In this case, the Vector2
        /// parameters are used to define the</param>
        /// <param name="speed">The speed parameter determines how fast an object will move in a given
        /// direction. It is usually measured in units per second.</param>
        /// <param name="width">The width parameter is an integer that represents the width of an object
        /// or shape.</param>
        /// <param name="height">The height parameter is an integer that represents the height of an
        /// object or element.</param>
        /// <param name="Color">The "Color" parameter is used to specify the color of an object or
        /// element. It is typically represented using the Color class in programming languages, which
        /// provides various methods and properties to manipulate and work with colors.</param>
        public void Reset(Vector2 startPosition, Vector2 direction, float speed, int width, int height, Color color){
            transform.position = startPosition;
            transform.direction = direction;
            transform.speed = speed;
            spriteRenderer.width = width;
            spriteRenderer.height = height;

            active = true;
        }

        /// <summary>
        /// The Update function is a placeholder for code that needs to be executed continuously in a
        /// game or application.
        /// </summary>
        public void Update()
        {
            transform.position += transform.direction * transform.speed * Raylib.GetFrameTime();
        }
        /// <summary>
        /// The Draw function is used to render or display the bullet.
        /// </summary>
        public void Draw()
        {
            spriteRenderer.Draw();
        }
    }
}