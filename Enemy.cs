using Raylib_CsLo;
using System.Numerics;
using System.ComponentModel;

namespace SpaceInvaders
{
    /* The `internal class Enemy` is defining a class called `Enemy` that is only accessible within the
    current assembly. This class represents an enemy object in a game and contains properties and
    methods related to the enemy's behavior and appearance. */
    internal class Enemy
    {
        public Transform transform;
        public SpriteRenderer spriteRenderer;
        public bool active;
        public int scoreValue;

        public Enemy(Vector2 position, Vector2 direction, float speed, int width, int height, Texture image, int score)
        {
            transform = new Transform(position, direction, speed);
            spriteRenderer = new SpriteRenderer(50, 50, Raylib.RED, image, transform);
            active = true;
            scoreValue = score;
        }

        internal void Update(){
            if(active){
                float deltaTime = Raylib.GetFrameTime();
                transform.position += transform.direction * transform.speed * deltaTime;
            }
        }

        internal void Draw()
        {
            if (active)
            {
                spriteRenderer.Draw();
            }
        }
    }
}