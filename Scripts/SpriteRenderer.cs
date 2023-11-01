using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class SpriteRenderer
    {
        public Vector2 position;
        Vector2 drawOffset;

        float scale;
        
        public int width;
        public int height;

        public Color color;
        
        Texture sprite;
        public Transform transform_ref;
        Rectangle sourceRec;

        public SpriteRenderer(int width, int height, Color color, Texture sprite, Transform transform)
        {
            this.width = width;
            this.height = height;
            this.color = color;
            this.sprite = sprite;
            transform_ref = transform;
            
            sourceRec = GetRectangle();
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(position.X, position.Y, width, height);
        }

        public void Draw()
        {
            //Raylib.DrawRectangleRec(GetRectangle(), color);
            Raylib.DrawTextureEx(sprite, transform_ref.position, 0.0f, 0.5f, Raylib.WHITE);
            //Raylib.DrawTexturePro(sprite, sourceRec, GetRectangle(), new Vector2(0.0f, 0.0f), 0.0f, color);
			//Raylib.DrawRectangleLines((int)transform_ref.position.X, (int)transform_ref.position.Y, (int)width, (int)height, color);
        }
    }
}

