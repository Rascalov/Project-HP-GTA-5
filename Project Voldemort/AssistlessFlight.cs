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

        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                World.AddExplosion(Game.Player.Character.Position, ExplosionType.Bike, 1f, 1f);
                
            }
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
        }

    }
}
