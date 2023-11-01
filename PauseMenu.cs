using System.Numerics;
using Raylib_CsLo;

namespace SpaceInvaders
{
    internal class PauseMenu
    {
        int screenWidth = 900;
        int screenHeight = 900;
        int height = 100;
        int width = 160;
        public event EventHandler BackToGame;
        public event EventHandler BackToMainMenu;
        public void Draw(){
            int center_X = screenWidth / 2 - width / 2;
            int center_Y = screenHeight / 2 - height / 2;

            int font = 40;
            string pauseText = "Game Paused";
            string escText = "Press ESC to continue";

            int pt = Raylib.MeasureText(pauseText, font);

            int et = Raylib.MeasureText(escText, font - 20);

            Raylib.DrawText(pauseText, screenWidth / 2 - pt / 2, screenHeight / 8, 40, Raylib.WHITE);

            if(Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)){
                BackToGame.Invoke(this, EventArgs.Empty);
            }

            Raylib.DrawText(escText, screenWidth / 2 - et / 2, screenHeight / 2, 20, Raylib.WHITE);

            if(RayGui.GuiButton(new Rectangle(center_X, center_Y + 300, width, height), "Back To Main Menu"))
            {
                BackToMainMenu.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
