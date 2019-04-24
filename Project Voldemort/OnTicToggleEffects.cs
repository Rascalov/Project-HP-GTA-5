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
    class OnTicToggleEffects : GeneralTools
    {
        private List<int> particleLijst = new List<int>();
        public OnTicToggleEffects()
        {
            this.Tick += onTick;
            this.KeyUp += onKeyUp;
            this.KeyDown += onKeyDown;
        }
        private void onTick(object sender, EventArgs e)
        {
            if (Toggled)
            {
                LoadPTFX("scr_agencyheistb");
                PlayParticlefx("scr_agencyheistb", "scr_agency3b_elec_box", player.Position + player.ForwardVector, 2, particleLijst);
                player.ApplyForce(player.UpVector * 0.01f);
                if (particleLijst.Count > 2)
                {
                    Function.Call(Hash.REMOVE_PARTICLE_FX, particleLijst[1], 0);
                    UI.Notify(particleLijst.Remove(1).ToString());
                }
                
            }
            Wait(10);

        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
          
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {

        }

    }
}
