using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Collision
    {
        public bool CheckCollision(Rectangle rec1, Rectangle rec2)
        {

            if (Raylib.CheckCollisionRecs(rec1, rec2))
            {
                return true;
            }
            return false;
        }
    }
}
