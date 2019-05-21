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

namespace Project_Voldemort
{
    public class GeneralTools : Script
    {
        protected Ped player = Game.Player.Character;
        public static bool Toggled { get; set; }
        public void PlayParticlefx(string dictionaryName, string ptfxName, Vector3 position, double scale)
        {
            Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, dictionaryName);
            Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, ptfxName,
            position.X, position.Y, position.Z, 0.0, 0.0, 0.0, scale, 0, 0, 0);
        }
        public void PlayParticlefx(string dictionaryName, string ptfxName, Vector3 position, double scale, List<int> particleList)
        {
            Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, dictionaryName);
            particleList.Add(Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, ptfxName,
            position.X, position.Y, position.Z, 0.0, 0.0, 0.0, scale, 0, 0, 0));
        }
        
        public void LoadPTFX(string naam)
        {
            int d = 0;
            if (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, naam))
            {
                while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, naam) && d < 2000) //scr_oddjobtraffickingair
                {
                    Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, naam);
                    d++;
                    Wait(0);
                }
            }
        }// Loads a ptfx until the game confirms it to be set in the call.
        public void PlaySound(string FileName)
        {
            SoundPlayer player = new SoundPlayer($"scripts/audioVoldemort/{FileName}");
            player.Play();
        }// Plays a sound seperate from the game.
    }
}
