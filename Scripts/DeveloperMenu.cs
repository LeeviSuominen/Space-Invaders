using Raylib_CsLo;

namespace SpaceInvaders
{
    internal class DeveloperMenu
    {
        public event EventHandler BackFromDeveloper;
        public event EventHandler DeveloperResetGame;
        /// <summary>
        /// The Draw function is used to render or display the developer menu.
        /// </summary>
        public void Draw()
        {
            int screenHeight = Raylib.GetScreenHeight();
            int screenWidth = Raylib.GetScreenWidth();
            int font = 40;
            int height = 100;
            int width = 120;
            int posX = screenWidth / 2 - width / 2;

            int titleText = Raylib.MeasureText("Developer Menu", font);
            int textPosX = (screenWidth - titleText) / 2;
            Raylib.DrawText("Developer Menu", textPosX, screenHeight / 7, font, Raylib.WHITE);

            if(RayGui.GuiButton(new Rectangle(posX, screenHeight - 150, width, height), "Back")){
                BackFromDeveloper.Invoke(this, EventArgs.Empty);
            }

            else if (RayGui.GuiButton(new Rectangle(posX - 20, screenHeight / 3, width + 50, height + 30), "Reset Game")){
                DeveloperResetGame.Invoke(this, EventArgs.Empty);
            }
        }

    }
}