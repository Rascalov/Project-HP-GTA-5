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
                        entity.ApplyForce(player.ForwardVector * 7, player.RightVector * 3);
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
            if (e.KeyCode == Keys.U)
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
            player.HasCollision = false;
            player.IsVisible = false;
            player.Task.PlayAnimation("veh@bike@police@front@base", "sit_balance_fwd", 2, 1999999999, (AnimationFlags)39);
            
        }
        private void FlyDisable()
        {
            UI.ShowSubtitle("Flying disabled");
            player.HasGravity = true;
            Toggled = false;
            player.HasCollision = true;

            player.IsVisible = true;
            player.Task.ClearAnimation("veh@bike@police@front@base", "sit_balance_fwd");
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
           
        }

    }

    /*
    #region MovementInToggledMode
    class ForwardForce : GeneralTools
    {
        public ForwardForce()
        {
            this.Tick += onTick;
            
        }
        private void onTick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.W) && Toggled)
            {
                player.ApplyForce(player.ForwardVector * 4.5f);
            }
        }
    }
    class LeftForce : GeneralTools
    {
        public LeftForce()
        {
            this.Tick += onTick;
        }
        private void onTick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.A) && Toggled)
            {
                player.ApplyForce(player.RightVector * -2.5f);
            }
        }
        
    }
    class RightForce : GeneralTools
    {

    }
    class UpForce : GeneralTools
    {

    }
    class DownForce : GeneralTools
    {

    }
    #endregion
    */
    
}
