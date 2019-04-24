using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using System.Windows.Forms;
using System.Media;
using System.Drawing;


namespace Project_Voldemort
{
    class FlightMovement : GeneralTools
    {
        private float amplifier = 1.5f;
        public FlightMovement()
        {
            this.Tick += onTick;
            this.KeyDown += onKeyDown;
            this.KeyUp += onKeyUp;
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                amplifier = 1.5f;
                UI.ShowSubtitle($"slowing down to {amplifier}");
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                amplifier = 100.3f;
                UI.ShowSubtitle($"Speeding up to {amplifier}");
            }

        }

        private void onTick(object sender, EventArgs e)
        {
            if (Toggled)
            {
                if (Game.IsKeyPressed(Keys.W))
                {
                    player.ApplyForce(GameplayCamera.Direction * amplifier);
                }
                if (Game.IsKeyPressed(Keys.A))
                {
                    player.ApplyForce(player.RightVector * -2.5f);
                }
                if (Game.IsKeyPressed(Keys.D))
                {
                    player.ApplyForce(player.RightVector * 2.5f);
                }
                if ((Game.IsKeyPressed(Keys.S)))
                {
                    player.ApplyForce(player.ForwardVector * -amplifier);
                }
                if (Game.IsKeyPressed(Keys.Space))
                {
                    player.ApplyForce(player.UpVector * 0.4f);
                }
            }
            
        }
    }
}
