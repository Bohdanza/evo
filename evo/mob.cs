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
    public class Mob
    {
        public virtual int X { get; protected set; }
        public virtual int Y { get; protected set; }
    
        public virtual int Speed { get; protected set; }
        public virtual int Strength { get; protected set; }
        public virtual int FoodToGrow { get; protected set; }
        public virtual int CurrentFood { get; protected set; } = 0;
        private Texture2D MobTexture { get; set; }
        public bool Alive = true;

        public Mob(ContentManager contentManager, int x, int y, int speed, int strength, int foodToGrow)
        {
            X = x;
            Y = y;

            Speed = speed;

            Strength = strength;

            FoodToGrow = foodToGrow;

            MobTexture = contentManager.Load<Texture2D>("mob_texture");
        }

        public virtual void Update(GameWorld gameWorld, ContentManager contentManager)
        {
            var rnd = new Random();

            X += rnd.Next(-1, 2);
            Y += rnd.Next(-1, 2);

            X = Math.Max(X, 0);
            X = Math.Min(X, 200);
            
            Y = Math.Max(Y, 0);
            Y = Math.Min(Y, 200);

            Mob clst = gameWorld.GetClosestMob(this);

            if (GameWorld.GetDist(X, Y, clst.X, clst.Y) <= 2)
            {
                int sp = Math.Abs(Speed - clst.Speed);
                int pp = Math.Abs(Strength - clst.Strength);
                int fp = Math.Abs(FoodToGrow - clst.FoodToGrow);
                
                if ((sp + pp + fp) / 3 <= 1)
                {
                    if (CurrentFood >= FoodToGrow)
                    {
                        CurrentFood -= FoodToGrow;

                        gameWorld.Mobs.Add(new Mob(contentManager, X, Y, Math.Min(255, Math.Max(1, (Speed + clst.Speed) + rnd.Next(-1, 3))), Math.Min(255, Math.Max(1, (Strength + clst.Strength) + rnd.Next(-1, 5))), Math.Min(255, Math.Max(1, (FoodToGrow + clst.FoodToGrow) / 2 + rnd.Next(-2, 1)))));
                    }
                }
                else
                {
                    if (Strength > clst.Strength)
                    {
                        clst.Alive = false;

                        FoodToGrow += clst.CurrentFood + 1;
                    }
                    else if (Strength < clst.Strength)
                    {
                        Alive = false;

                        clst.CurrentFood += CurrentFood + 1;
                    }
                    else
                    {
                        Alive = false;

                        clst.Alive = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(MobTexture, new Vector2(x, y), new Color(Speed*3, Strength*3, FoodToGrow*3));
        }
    }
}