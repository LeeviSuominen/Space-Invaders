﻿using System.Numerics;
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
        public event EventHandler PauseResetGame;
        private readonly Invaders invaders;

        public PauseMenu(Invaders invaders = null)
        {
            this.invaders = invaders;
        }

        public void Draw(Invaders invaders)
        {
            int center_X = screenWidth / 2 - width / 2;
            int center_Y = screenHeight / 2 - height / 2;

            int enemiesKilled = invaders.EnemiesKilled;

            int font = 40;
            string pauseText = "Game Paused";
            string escText = "Press ESC to continue";

            int pt = Raylib.MeasureText(pauseText, font);

            int et = Raylib.MeasureText(escText, font - 20);

            Raylib.DrawText(pauseText, screenWidth / 2 - pt / 2, screenHeight / 25, 40, Raylib.WHITE);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
            {
                BackToGame.Invoke(this, EventArgs.Empty);
            }

            Raylib.DrawText(escText, screenWidth / 2 - et / 2, screenHeight / 8, 20, Raylib.WHITE);

            if (RayGui.GuiButton(new Rectangle(center_X + 225, center_Y + 300, width + 50, height + 30), "Back To Main Menu"))
            {
                BackToMainMenu.Invoke(this, EventArgs.Empty);
            }

            Raylib.DrawText("Enemies killed: " + enemiesKilled, 75, 275, 25, Raylib.WHITE);

            if (RayGui.GuiButton(new Rectangle(75, center_Y + 300, width + 50, height + 30), "Reset Game"))
            {
                PauseResetGame.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
