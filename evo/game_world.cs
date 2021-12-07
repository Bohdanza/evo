using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;

namespace evo
{
    public class GameWorld
    {
        public List<Mob> Mobs { get; protected set; }
        
        public GameWorld(ContentManager contentManager)
        {
            Mobs = new List<Mob>();

            var rnd = new Random();

            int mobsCount = rnd.Next(100, 150);

            for (int i = 0; i < mobsCount; i++)
            {
                Mobs.Add(new Mob(contentManager, rnd.Next(50, 150), rnd.Next(50, 150), rnd.Next(2), rnd.Next(1, 10), 1));
            }    
        }

        public void Update(ContentManager contentManager)
        {
            int l = 1;

            for (int i = 0; i < Mobs.Count; i += l)
            {
                l = 1;

                Mobs[i].Update(this, contentManager);

                if(!Mobs[i].Alive)
                {
                    l = 0;

                    Mobs.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var currentMob in Mobs)
            {
                currentMob.Draw(spriteBatch, currentMob.X * 3, currentMob.Y * 3);
            }
        }

        public Mob GetClosestMob(Mob MobToGet)
        {
            double dist = 1e9;
            Mob closest = null;

            foreach(var currentMob in Mobs)
            {
                double tmpd = GetDist(MobToGet.X, MobToGet.Y, currentMob.X, currentMob.Y);

                if (tmpd <= dist)
                {
                    closest = currentMob;
                }
            }

            return closest;
        }

        public static double GetDist(double x1, double y1, double x2, double y2)
        {
            double xd = x1 - x2;
            double yd = y1 - y2;

            return Math.Sqrt(xd * xd + yd * yd);
        }
    }
}