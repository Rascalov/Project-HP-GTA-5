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
    public class Spells : GeneralTools
    {
        private List<int> LoopedAnimaties = new List<int>();
        private List<int> vuurlijst = new List<int>();
        private int decide = 1;
        private int keeps;
        public Spells()
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
            #region Finished(Per definitie)     
            if (e.KeyCode == Keys.X && !player.IsInVehicle()) { Nyeaaaaah(); } // Scream baby
            if (e.KeyCode == Keys.E && Game.Player.IsAiming && player.Weapons.Current.Hash == WeaponHash.FlareGun && !player.IsInVehicle())
            {
                ExplosionBeam();
            }
            if (e.KeyCode == Keys.Z) { ExplosionOfMadness(); }
            if (e.KeyCode == Keys.B) { AvadaKedavra(); }
            if (Toggled  && e.KeyCode == Keys.E) { FlightExplosionBeam(); }
            #endregion
            // B E X Z, in huidig gebruik

            if (e.KeyCode == Keys.J)
            {
                //  LaserBeam();
            }
            if (e.KeyCode == Keys.F && Game.Player.IsAiming && player.Weapons.Current.Hash == WeaponHash.FlareGun)
            {
                PushBackwave();
                //TestingObjects();
            }

        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {

        }
        private void FireStuff()
        {
            
            // doel : voorwaarts veld van vuur met voldemort firespit sound
            Vector3 camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
            Vector3 camRot = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT);
            float retz = camRot.Z * 0.0174532924F;
            float retx = camRot.X * 0.0174532924F;
            float absx = (float)Math.Abs(Math.Cos(retx));
            Vector3 camStuff = new Vector3((float)Math.Sin(retz) * absx * -1, (float)Math.Cos(retz) * absx, (float)Math.Sin(retx));


            //ResetFires();
            Vector3 Rpos = new Vector3(0, 0, 0);
            for (int r = 5; r < 40; r += 2)
            {
                Rpos = camPos + (camStuff * r);
                float z = World.GetGroundHeight(Rpos);
                for (float k = Rpos.X - 2.5f; k < Rpos.X + 2.5f; k++)
                {
                    World.AddExplosion(new Vector3(k, Rpos.Y, z), ExplosionType.Molotov1, 0.1f, 0.0f, false, false);
                    // int i = Function.Call<int>(Hash.START_SCRIPT_FIRE, k, Rpos.Y, z, 25, false);
                    //vuurlijst.Add(i);
                    //UI.ShowSubtitle(vuurlijst.Count.ToString());
                    Wait(5);
                }
            }

            // Heading hoger dan 180 betekent de y waardes moeten met + 
            // Heading lager dan 180 betekent de y waardes moeten met -



            //for (float r = ; r < ; r += )
            //{
            //    Vector3 Rpos = new Vector3(0, 0, 0);
            //    for (float k = x - 5; k < x + 5; k++)
            //    {
            //        int i = Function.Call<int>(Hash.START_SCRIPT_FIRE, k, r, z, 25, false);
            //        vuurlijst.Add(i);
            //        UI.ShowSubtitle(vuurlijst.Count.ToString());
            //        Wait(5);
            //    }
            //}
        }
        private void ResetFires()
        {
            foreach (int item in vuurlijst)
            {
                Function.Call(Hash.REMOVE_SCRIPT_FIRE, item);
            }
        }

        #region SoundPlayer Usage
        
        #endregion
        #region Spells
        private void Nyeaaaaah() //  ShockWave/Explosion
        {
            // scr_xm_submarine_surface_explosion from scr_xm_submarine
            // exp_water from core
            player.IsInvincible = true;
            player.CanRagdoll = false;
            string ptfx = "core";
            LoadPTFX(ptfx);
            player.Task.PlayAnimation("rcmcollect_paperleadinout@", "meditiate_idle", 3, 3500, AnimationFlags.Loop); //charge anim
            PlaySound("Charging.wav"); // charge sound
            Wait(3500); // Hold untill Sound is played (around 3.5 seconds)   
            PlayParticlefx(ptfx, "exp_water", player.Position, 3.0); // scr_xm_submarine_surface_explosion
            World.AddOwnedExplosion(player, Game.Player.Character.Position, (ExplosionType)39, 10.0f, 1f, true, false); // Modified Snowball (exType 39) explosion
            player.Task.PlayAnimation("random@arrests", "kneeling_arrest_get_up");
            PlaySound("VoldemortScream.wav"); // Nyeaaaaah
            for (int i = 0; i < 2; i++)
            {
                Wait(400);
                PlayParticlefx(ptfx, "exp_water", new Vector3(player.Position.X, player.Position.Y, player.Position.Z + 2), 20.0);
                World.AddOwnedExplosion(player, Game.Player.Character.Position, (ExplosionType)39, 0.5f, 1f, true, false); // Modified Snowball (exType 39) explosion
            }
            Wait(4300); //

            player.IsInvincible = false;
            player.CanRagdoll = true;

            // ToDo: Charge and release animation, ShockWave fx
        }
        private void ExplosionBeam() // straight from face explosions with increased radius per explosion
        {
            // blood_stungun from core
            // scr_trev1_trailer_boosh from scr_trevor1
            // ent_amb_elec_crackle_sp from core
            Vector3 cam = GameplayCamera.Direction;
            Vector3 camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
            Vector3 Rpos = new Vector3(0, 0, 0);
            PlaySound("Grunt" + decide + ".wav");
            string ptfx = "scr_trevor1";
            LoadPTFX(ptfx);
            float max = 60;
            if (Toggled)
            {
                max = 120;
            }
            for (float r = 8; r < max; r += 3)
            {
                Rpos = camPos + (cam * r);
                World.AddOwnedExplosion(player, Rpos, ExplosionType.Plane, 1.2f, 0.2f, false, true);
                PlayParticlefx(ptfx, "scr_trev1_trailer_boosh", Rpos, 3.5);
                Wait(4);
            }
            Wait(200);
            World.AddOwnedExplosion(player, Rpos, ExplosionType.Plane, 1.4f, 1.2f, true, false);
            switch (decide) // switch between grunt audios
            {
                case 1:
                    decide = 2;
                    break;
                case 2:
                    decide = 3;
                    break;
                case 3:
                    decide = 1;
                    break;
            }

        }
        private void FlightExplosionBeam() // straight from face explosions with increased radius per explosion
        {
            // blood_stungun from core
            // scr_trev1_trailer_boosh from scr_trevor1
            // ent_amb_elec_crackle_sp from core
            Vector3 cam = GameplayCamera.Direction;
            Vector3 camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
            Vector3 Rpos = new Vector3(0, 0, 0);
            PlaySound("Grunt" + decide + ".wav");
            string ptfx = "scr_trevor1";
            double scale = 3.5;
            LoadPTFX(ptfx);
            for (float r = 8; r < 120; r += 3)
            {
                Rpos = camPos + (cam * r);
                World.AddOwnedExplosion(player, Rpos, ExplosionType.Barrel, 1.2f, 0.2f, false, true);
                PlayParticlefx(ptfx, "scr_trev1_trailer_boosh", Rpos, scale);
                scale += 2;
                Wait(5);
            }
            Wait(50);
            World.AddOwnedExplosion(player, Rpos, ExplosionType.Plane, 1.4f, 1.2f, true, false);
            switch (decide) // switch between grunt audios
            {
                case 1:
                    decide = 2;
                    break;
                case 2:
                    decide = 3;
                    break;
                case 3:
                    decide = 1;
                    break;
            }

        }
        private void AvadaKedavra()
        {
            // van een tutorial op GTA5Mods.com, pakt de camangle denk ik
            //get Aim Postion
            Vector3 camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
            RaycastResult ray = World.Raycast(camPos, camPos + GameplayCamera.Direction * 9000, IntersectOptions.Peds1);
            if (ray.DitHitEntity)
            {
                string ptfx = "scr_trevor1";
                LoadPTFX(ptfx);
                Ped p = World.GetClosestPed(ray.HitCoords, 4);
                player.Task.PlayAnimation("guard_reactions", "1hand_aiming_cycle");
                PlaySound("AvadaKedavra1.wav");
                Wait(700);

                //Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_xmas_firework_burst_ring_green",
                //ray.HitCoords.X, ray.HitCoords.Y, ray.HitCoords.Z, 0.0, 0.0, 0.0, 3.0, 0, 0, 0);
                Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, ptfx);

                Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_trev1_trailer_boosh",
                   player.Weapons.CurrentWeaponObject.Position.X, player.Weapons.CurrentWeaponObject.Position.Y, player.Weapons.CurrentWeaponObject.Position.Z, 0.0, 0.0, 0.0, 0.4, 0, 0, 0);
                Wait(200);
                Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, ptfx);
                Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, "scr_trev1_trailer_boosh",
                    p.Position.X, p.Position.Y, p.Position.Z, 0.0, 0.0, 0.0, 2.0, 0, 0, 0);
                Wait(0);
                Function.Call(Hash.SET_PED_TO_RAGDOLL, p, 2000, 2000, 0, false, false, false);
                p.ApplyForceRelative(player.ForwardVector * 1.5f, player.ForwardVector * 1.5f);
                //p.ApplyForce(new Vector3(2,18, 2) + player.ForwardVector * 2);
                Wait(0);
                p.Kill();
            }
            // Target, if ped, kill. misschien met een particle effect
            // Soundeffect
        }
        #endregion

        void ExplosionOfMadness()
        {
            // Circle explosions
            // animation used: 1hand_aim_med_sweep
            int radius = 8;
            Vector3 loc = player.Position;
            float playerX = player.Position.X;
            float playerY = player.Position.Y;
            float zValue = player.Position.Z;
            // de complete radius
            float radiusReal = radius * radius;
            float Xloc = 0, Yloc = 0;

            // Deze loop ittereert per radius, begin bij 4, de player moet niet geraakt worden door de explosie. 
            PlaySound("VeryAngryGrunt.wav");
            player.Task.PlayAnimation("guard_reactions", "1hand_aim_med_sweep", 4, 4200, AnimationFlags.Loop);
            Wait(0);
            LoadPTFX("scr_xm_submarine");
            for (int r = 8; r < radiusReal; r += 9)
            {
                for (int d = 0; d < 360; d += 18)
                {
                    double angle = d * Math.PI / 180;
                    Xloc = (float)(playerX + r * Math.Cos(angle));
                    Yloc = (float)(playerY + r * Math.Sin(angle));
                    Vector3 Rpos = new Vector3(Xloc, Yloc, zValue);
                    PlayParticlefx("scr_xm_submarine", "scr_xm_submarine_explosion", Rpos, 5.0);
                    World.AddOwnedExplosion(player, Rpos, ExplosionType.Plane, 4.0f, 0.3f, true, true);
                    Wait(25);
                }
                Wait(25);
            }
            
        }

        void LaserBeam()
        {
            player.Task.PlayAnimation("guard_reactions", "1hand_aiming_cycle", 8, 12800, (AnimationFlags)34);
            PlaySound("scripts/audioVoldemort/InitialLaserScream.wav");
            Wait(4555);
            PlaySound("scripts/audioVoldemort/LaserLoop.wav");
        }
        void PushBackwave()
        {
            Vector3 camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
            Vector3 camRot = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT);
            float retz = camRot.Z * 0.0174532924F;
            float retx = camRot.X * 0.0174532924F;
            float absx = (float)Math.Abs(Math.Cos(retx));
            Vector3 camStuff = new Vector3((float)Math.Sin(retz) * absx * -1, (float)Math.Cos(retz) * absx, (float)Math.Sin(retx));
            Vector3 Rpos;
            //* scr_stunts
            //* scr_stunts_shotburst
            // scr_rcpaparazzo1 scr_rcpap1_camera
            string ptfx = "scr_rcpaparazzo1";
            LoadPTFX(ptfx);
            PlaySound("NEEE.wav");
            Wait(350);
            World.AddExplosion(player.Position, ExplosionType.Barrel, 0.0f, 1.0f, false, true);
            for (int r = 5; r < 70; r += 4)
            {
                Rpos = camPos + (camStuff * r);
                

                foreach (Entity entity in World.GetNearbyEntities(Rpos, 10))
                {
                    if (entity != player)
                    {
                        //Function.Call(Hash.SET_PED_TO_RAGDOLL, entity, 2000, 2000, 0, false, false, false);
                        if (entity is Ped)
                        {
                            Function.Call(Hash.SET_PED_TO_RAGDOLL, entity, 2000, 2000, 0, false, false, false);
                        }
                        else if (entity is Prop)
                        {
                            World.AddExplosion(entity.Position, ExplosionType.ProxMine, 0.1f, 0.0f, false, true);
                        }

                        entity.ApplyForce(player.ForwardVector * 100, player.ForwardVector * 50);

                    }
                }

                Wait(50);
            }


        }
        void TestingObjects()
        {
            //862871082 = Traffic light groot
            //589548997 = player prop
            //576500243  = player prop  
            //-1063472968 = grote (ik dacht verwoestbare) lantaarnpaal
            // 1043035044
            //589548997 = traffic light pedestrian klein
            // -9941100897 = military light
            // -21288878 = barred fences
            foreach (Prop p in World.GetNearbyProps(player.Position, 2))
            {
                UI.Notify(p.Model.Hash.ToString());
                //if (p.Model.Hash == 576500243)
                //{
                //   World.AddExplosion(p.Position, ExplosionType.GasTank, 2f, 0.0f, false, false);
                //}
                //else
                //{
                //    p.ApplyForceRelative(new Vector3(0, 50, -100), new Vector3(0, 100, 20), ForceType.MaxForceRot2);
                //}
            }
        }
        void KILL()
        {
            Vector3 camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
            Vector3 camRot = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT);
            float retz = camRot.Z * 0.0174532924F;
            float retx = camRot.X * 0.0174532924F;
            float absx = (float)Math.Abs(Math.Cos(retx));
            Vector3 camStuff = new Vector3((float)Math.Sin(retz) * absx * -1, (float)Math.Cos(retz) * absx, (float)Math.Sin(retx));

            RaycastResult ray = World.Raycast(camPos, camPos + camStuff * 1000, IntersectOptions.Everything);
            ////if (ray.DitHitEntity)
            ////{
            ////    Ped p = World.GetClosestPed(ray.HitCoords, 4);

            ////    p.Weapons.Give(WeaponHash.Bat, 1, true, true);
            ////    p.Task.FightAgainst(World.GetClosestPed(p.Position,20));

            ////}
            //foreach (Prop p in World.GetAllProps())
            //{
            //    p.HasGravity = false;
            //}
            Color c = Color.Black;
            Vector3 plp = player.Position;
            int count = 10000;
            for (int i = 0; i < count; i++)
            {

                // DrawBox(player.Position, camPos + player.ForwardVector + (camStuff * 10), c);
                DrawLine(player.Weapons.CurrentWeaponObject.Position, player.Weapons.CurrentWeaponObject.ForwardVector + (camStuff * 10), c);
                camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
                camRot = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT);
                retz = camRot.Z * 0.0174532924F;
                retx = camRot.X * 0.0174532924F;
                absx = (float)Math.Abs(Math.Cos(retx));
                camStuff = new Vector3((float)Math.Sin(retz) * absx * -1, (float)Math.Cos(retz) * absx, (float)Math.Sin(retx));
                ray = World.Raycast(camPos, camPos + camStuff * 1000, IntersectOptions.Map);
                Wait(10);


            }




        }

        public void DrawLine(Vector3 from, Vector3 to, Color col)
        {

            Function.Call(Hash.DRAW_LINE, from.X, from.Y, from.Z, to.X, to.Y, to.Z, col.R, col.G, col.B, col.A);
        }
        public void DrawBox(Vector3 a, Vector3 b, Color col)
        {
            Function.Call(Hash.DRAW_BOX, a.X, a.Y, a.Z, b.X, b.Y, b.Z, col.R, col.G, col.B, col.A);
        }
    }
}
