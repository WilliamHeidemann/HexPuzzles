﻿using System;
using GamePlay;

namespace ScriptableObjectClasses
{
    [Serializable]
    public struct AxialHex
    {
        public TileType tileType;
        public int Q;
        public int R;
        public int S => -Q-R;
        public int teleportQ;
        public int teleportR;
        public bool switchOn;
    }
}