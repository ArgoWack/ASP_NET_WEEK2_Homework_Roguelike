﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model
{
    public class RoomToDiscover
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string EnteringDirection { get; set; }
        public HashSet<string> BlockedDirections { get; set; }

        public RoomToDiscover(int x, int y, string enteringDirection)
        {
            X = x;
            Y = y;
            EnteringDirection = enteringDirection;
            BlockedDirections = new HashSet<string>();

        }
        //for serialization
        public RoomToDiscover() { }
    }
}