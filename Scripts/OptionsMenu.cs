using System.Numerics;
using Raylib_CsLo;

namespace SpaceInvaders
{
    internal class OptionsMenu
    {
        int screenWidth = 900;
        int screenHeight = 900;
        int height = 100;
        int width = 120;

        public float volume = 1.0f;

        public event EventHandler BackButtonPressed;

      /// <summary>
      /// The Draw function is used to draw the optionsmenu.
      /// </summary>
        public void Draw(){
            int posX = screenWidth / 2 - width / 2;

            string settingsText = "Settings";
            string volumeText = "Volume: ";
            int font = 50;

            int st = Raylib.MeasureText(settingsText, font);
            //RayGui.GuiLabel(new Rectangle(titlePosX, titlePosY, width, height ), "Settings");
            Raylib.DrawText(settingsText, screenWidth / 2 - st / 2, screenHeight / 8, 50, Raylib.WHITE);
            Raylib.DrawText(volumeText, 20, screenHeight - 650, 20, Raylib.WHITE);
            volume = RayGui.GuiSlider(new Rectangle(125, screenHeight - 650, width * 2, height / 4), "Min", "Max", volume, 0.0f, 1.0f);

            if(RayGui.GuiButton(new Rectangle(posX, screenHeight - 150, width, height), "Back")){
                BackButtonPressed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
