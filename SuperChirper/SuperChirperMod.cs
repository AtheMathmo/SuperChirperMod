using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace SuperChirper
{
    public class SuperChirperMod : IUserMod
    {
        private static UIButton clearButtonInstance;
        private static UIButton muteButtonInstance;

        public static UIButton ClearButtonInstance
        {
            get
            {
                return clearButtonInstance;
            }
            set
            {
                clearButtonInstance = value;
            }
        }

        public static UIButton MuteButtonInstance
        {
            get
            {
                return muteButtonInstance;
            }
            set
            {
                muteButtonInstance = value;
            }
        }

        public string Name
        {
            get { return "SuperChirper"; }
        }

        public string Description
        {
            get { return "Makes Chirpy beautiful."; }
        }
    }    
}