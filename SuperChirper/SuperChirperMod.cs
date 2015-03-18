using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

/*
 *  I've tried to give credit throughout to the appropriate people, but here is a list of the work used here:
 *  
 * http://www.reddit.com/r/CitiesSkylinesModding/comments/2ymwxe/example_code_using_the_colossal_ui_in_a_user_mod/ 
 * https://gist.github.com/reima/9ba51c69f65ae2da7909
 * https://github.com/skymodteam/skymod-chirpymaid
 * https://github.com/mabako/reddit-for-city-skylines
 * /u/Zuppis and his ChirpFilter mod: http://steamcommunity.com/sharedfiles/filedetails/?id=407871375
 * https://github.com/mabako/reddit-for-city-skylines/blob/master/RedditSkylines
 * 
 * 
 */
namespace SuperChirper
{
    public class SuperChirperMod : IUserMod
    {
        private static UIButton clearButtonInstance;
        private static UIButton muteButtonInstance;
        private static UIButton filterButtonInstance;

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

        public static UIButton FilterButtonInstance
        {
            get
            {
                return filterButtonInstance;
            }
            set
            {
                filterButtonInstance = value;
            }
        }

        public string Name
        {
            get { return "SuperChirper"; }
        }

        public string Description
        {
            get { return "Adds CLEAR, MUTE and FILTER functionality to Chirpy. Press Alt+C to toggle Chirpy."; }
        }
    }    
}