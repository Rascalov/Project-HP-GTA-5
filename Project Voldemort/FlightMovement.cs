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
            if (Toggled && e.KeyCode == Keys.ShiftKey)
            {
                amplifier = 5.5f;
                UI.ShowSubtitle($"slowing down to {amplifier}");
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (Toggled && e.KeyCode == Keys.ShiftKey)
            {
                amplifier = 300;
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
                    player.ApplyForce(player.RightVector * -amplifier);
                }
                if (Game.IsKeyPressed(Keys.D))
                {
                    player.ApplyForce(player.RightVector * amplifier);
                }
                if ((Game.IsKeyPressed(Keys.S)))
                {
                    player.ApplyForce(GameplayCamera.Direction * -amplifier);
                }
                if (Game.IsKeyPressed(Keys.Space))
                {
                    if (Game.IsKeyPressed(Keys.ShiftKey))
                    {
                        player.ApplyForce(player.UpVector * amplifier);
                    }
                    player.ApplyForce(player.UpVector * 1.4f);
                }
                if (Game.IsKeyPressed(Keys.ControlKey))
                {
                    if (Game.IsKeyPressed(Keys.ShiftKey))
                    {
                        player.ApplyForce(player.UpVector * -amplifier);
                    }
                    player.ApplyForce(player.UpVector * -1.4f);
                }
                if (player.IsFalling)
                {
                    // het heeft niks te maken met animatie, hij wordt gewoon naar de grond getrokken. 
                    // ik denk dat de gravity gewoon weer aan wordt gezet met flikker animatie en zo. 

                    //UI.ShowSubtitle(player.HeightAboveGround.ToString());
                    // stop anim, apply force, turn collision off brief
                    Function.Call(Hash.STOP_CURRENT_PLAYING_AMBIENT_SPEECH, player);
                    player.Task.ClearAll();
                    player.ApplyForce(player.UpVector * 1.4f);
                    player.HasCollision = false;
                    player.HasCollision = true;



                    //player.FreezePosition = true;
                     player.Task.ClearAllImmediately();
                    //Wait(400);
                    //player.FreezePosition = false;
                }
            }
            
        }
    }
}
