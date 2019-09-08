﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IrrKlang;

namespace Game3
{
    public class Map
    {
 public enum orientation
        {
            front,
            back,
            left,
            right,
up,
down,
        }
        public orientation o;
        Random random = new Random();
        public ISoundEngine engine = new ISoundEngine();
        public Player Player
        {
            get { return player; }
        }
        Player player;
        public Dictionary<string, string> tiles = new Dictionary<string, string>();
        public List<Stairs> staircases = new List<Stairs>();
        public List<Doors> door = new List<Doors>();
        int mapName;
        public Map(int mapname)
        {
            this.mapName = mapname;
        }

        public void Drawmap()
        {
            if (mapName == 1)
            {
                spawn_tile(30, 35, 88, 93, 0, 0, "rocks");
                spawn_tile(30, 35, 97, 100, 2, 2, "tile");
                spawn_staircases(30, 35, 94, 96, 0, 1, orientation.front, "rocks");
                spawn_staircases(90, 100, 90, 100, 0, 1, orientation.front, "rocks");
                spawn_door(5, 5, 0, "sounds/door1.wav", "sounds/dooropen.wav", "sounds/doorclose.wav", false);
                spawn_door(10, 5, 0, "sounds/door1.wav", "sounds/dooropen.wav", "sounds/doorclose.wav", true);
                spawn_player(35, 89, 0);
            }
        }

    public void Update(KeyboardState keystate, GameTime gameTime)
        {
            player.Update(keystate, gameTime);
            engine.SetListenerPosition(player.me.X, player.me.Y, player.me.Z, 0, 0, 1);
            updateDoors(gameTime);
            updateStairs(gameTime);
        }


        public void updateDoors(GameTime gameTime)
        {
            for(int i=0; i<door.Count(); i++)
            {
                door[i].Update(gameTime);
            }
        }

        public void updateStairs(GameTime gameTime)
        {
            for(int i=0; i<staircases.Count(); i++)
            {
                staircases[i].Update(gameTime);
            }
        }

        public void spawn_player(float x, float y, float z)
        {
            player = new Player(this);
            player.me.X = x;
            player.me.Y = y;
            player.me.Z = z;
        }

        public void spawn_door(int dx, int dy, int dz, string s1 = "sounds/door1.wav", string s2 = "sounds/dooropen.wav", string s3 = "sounds/doorclose.wav", bool isopen = false)
        {
            door.Add(new Doors(this, dx, dy, dz, s1, s2, s3, isopen));
        }

        public void spawn_staircases(int minsx, int maxsx, int minsy, int maxsy, int sz, int sheigth, orientation o, string stile)
        {
            staircases.Add(new Stairs(this, minsx, maxsx, minsy, maxsy, sz, sheigth, o, stile));
            spawn_tileWithSlope(minsx, maxsx, minsy, maxsy, sz, sheigth, o, stile);
        }

        public void spawn_tile(int minx, int maxx, int miny, int maxy, int minz, int maxz, string tile)
        {
for(int x=minx; x<=maxx; x++)
            {
                for(int y=miny; y<=maxy; y++)
                {
                    for(int z=minz; z<=maxz; z++)
                    {
                        tiles.Add(x+":"+y+":"+z, tile);
                    }
                }
            }
        }

public void spawn_tileWithSlope(int minx, int maxx, int miny, int maxy, int minz, int stepHeight, orientation o, string tile)
        {
            int maxz = 0;
            int basez = minz;
if(o==orientation.left||o==orientation.right)
            {
                maxz = maxx - minx;
            }
            else
            {
                maxz = maxy - miny;
            }
            maxz = Math.Abs(maxz * stepHeight);
            int factor = stepHeight;
if(o==orientation.left||o==orientation.back)
            {
                factor = -1 * factor;
                basez = maxz;
            }
if(o==orientation.left || o==orientation.right)
            {
                for(int x=minx; x<=maxx; x++)
                {
                    for(int y=miny; y<=maxy; y++)
                    {
                        for(int z=basez; z<=maxz; z++)
                        {
                            tiles.Add(x + ":" + y + ":" + z, tile);
                        }
                    }
                    basez += factor;
                }
            }
            else
            {
for(int y=miny; y<=maxy; y++)
                {
                    for(int x=minx; x<=maxx; x++)
                    {
                        for(int z=basez; z<=maxz; z++)
                        {
                            tiles.Add(x + ":" + y + ":" + z, tile);
                        }
                    }
                    basez += factor;
                }
            }
        }

        public string gmt()
        {
            if (tiles.ContainsKey(player.me.X + ":" + player.me.Y + ":" + player.me.Z))
                {
                string outval;
                tiles.TryGetValue(player.me.X + ":" + player.me.Y + ":" + player.me.Z, out outval);
                return outval;
            }
                    return "";
        }


        public string get_tile_at(float x, float y, float z)
        {
            string o;
            tiles.TryGetValue(x + ":" + y+":"+z, out o);
            return o;
        }



        public  void playstep()
        {
            if (gmt().IndexOf("wall",0)>-1)
            {
                engine.Play2D("sounds/" + gmt() + ".wav");
                bounce();
            }
            else
            {
                engine.Play2D("sounds/" + get_tile_at(player.me.X, player.me.Y, player.me.Z) + "step" + random.Next(1, 5) + ".wav");
            }
        }




        public  void bounce()
        {
            if (player.orientation==Player.playerOrientation.Front)
            {
                player.me.Y += -1;
            }
            else if (player.orientation == Player.playerOrientation.Up)
            {
                player.me.Z += -1;
            }
            else if (player.orientation==Player.playerOrientation.Left)
            {
                player.me.X += 1;
            }
            else if (player.orientation==Player.playerOrientation.Right)
            {
                player.me.X += -1;
            }
            else if (player.orientation==Player.playerOrientation.Back)
            {
                player.me.Y += 1;
            }
            else if (player.orientation == Player.playerOrientation.Down)
            {
                player.me.Z += 1;
            }
        }

    }
}
