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


namespace Project_Voldemort
{
    class AssistlessFlight : GeneralTools
    {
        
        public AssistlessFlight()
        {
            this.Tick += onTick;
            this.KeyUp += onKeyUp;
            this.KeyDown += onKeyDown;
        }
        private void onTick(object sender, EventArgs e)
        {
            if (Toggled)
            {
                foreach (Entity entity in World.GetNearbyEntities(player.Position + player.ForwardVector, 5))
                {
                    if (entity != player)
                    {
                        if (entity is Ped)
                        {
                            Function.Call(Hash.SET_PED_TO_RAGDOLL, entity, 2000, 2000, 0, false, false, false);
                        }
                        entity.ApplyForce(player.ForwardVector * 7, player.RightVector * 2);
                    }
                }
            }
            
        }
        private void onKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Toggled)
            {
                if (e.KeyCode == Keys.W)
                {
                    player.FreezePosition = true;
                    player.FreezePosition = false;
                    player.ApplyForce(GameplayCamera.Direction * 4);
                }
            }
            if (e.KeyCode == Keys.G)
            {
                if (!Toggled)
                {
                    FlyEnable();
                }
                else
                {
                    FlyDisable();
                }
            }
        }
        private void FlyEnable()
        {
            UI.ShowSubtitle("Flying enabled");
            player.HasGravity = false;
            Toggled = true;
            player.CanRagdoll = false;
            LoadPTFX("core");
            PlayParticlefx("core", "ent_ray_heli_aprtmnt_exp", player.Position, 3.0);
            player.HasCollision = true; // turned
            player.IsVisible = false; // turned
            PlaySound("Flight.wav");
            
          //  player.Task.PlayAnimation("veh@bike@police@front@base", "sit_balance_fwd", 2, 1999999999, AnimationFlags.Loop);
            
        }
        private void FlyDisable()
        {
            UI.ShowSubtitle("Flying disabled");
            player.HasGravity = true;
            Toggled = false;
            player.HasCollision = true;
            LoadPTFX("core");
            PlayParticlefx("core", "ent_ray_heli_aprtmnt_exp", player.Position, 3.0);
            player.CanRagdoll = true;
            player.IsVisible = true;
            PlaySound("Grounding.wav");
            player.FreezePosition = true;
            Wait(40);
            player.FreezePosition = false;
            
            //  player.Task.ClearAnimation("veh@bike@police@front@base", "sit_balance_fwd");
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
           
        }

    }    
}
