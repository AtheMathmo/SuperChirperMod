using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace SuperChirper
{
    class ChirpFilter
    {
        // CREDIT TO:
        // https://github.com/mabako/reddit-for-city-skylines/blob/master/RedditSkylines
        public static bool FilterMessage(string input)
        {

            switch (input)
            {
                // Handles ID's of all nonsense chirps.
                case LocaleID.CHIRP_ASSISTIVE_TECHNOLOGIES:
                case LocaleID.CHIRP_ATTRACTIVE_CITY:
                case LocaleID.CHIRP_CHEAP_FLOWERS:
                case LocaleID.CHIRP_DAYCARE_SERVICE:
                case LocaleID.CHIRP_HAPPY_PEOPLE:
                case LocaleID.CHIRP_HIGH_TECH_LEVEL:
                case LocaleID.CHIRP_LOW_CRIME:
                case LocaleID.CHIRP_NEW_FIRE_STATION:
                case LocaleID.CHIRP_NEW_HOSPITAL:
                case LocaleID.CHIRP_NEW_MAP_TILE:
                case LocaleID.CHIRP_NEW_MONUMENT:
                case LocaleID.CHIRP_NEW_PARK:
                case LocaleID.CHIRP_NEW_PLAZA:
                case LocaleID.CHIRP_NEW_POLICE_HQ:
                case LocaleID.CHIRP_NEW_TILE_PLACED:
                case LocaleID.CHIRP_NEW_UNIVERSITY:
                case LocaleID.CHIRP_NEW_WIND_OR_SOLAR_PLANT:
                case LocaleID.CHIRP_ORGANIC_FARMING:
                case LocaleID.CHIRP_POLICY:
                case LocaleID.CHIRP_PUBLIC_TRANSPORT_EFFICIENCY:
                case LocaleID.CHIRP_RANDOM:
                case LocaleID.CHIRP_STUDENT_LODGING:
                    return true;
                default:
                    return false;
            }
        }

        public static string DeHashTagMessage(IChirperMessage inputMessage)
        {
            string messageText = inputMessage.text;

            string[] words = messageText.Split(' ');
            List<string> newMessage = new List<string>();

            for (int i = 0; i < words.Length; i++ )
            {
                if (words[i].ToCharArray()[0].Equals(Char.Parse("#")))
                {
                    // Keep only if not an end word.
                    if (!CheckEndHashTagWord(i, words))
                    {
                        // Remove hashtag
                        newMessage.Add(words[i].Substring(1, words[i].Length-1));

                    }

                }
                else
                {
                    newMessage.Add(words[i]);
                }
            }

            string outputMessage = String.Join(" ", newMessage.ToArray());
            return outputMessage;
        }

        private static bool CheckEndHashTagWord(int indexCheck, string[] words)
        {
            // Check if punctuation comes after hashtagged word. Return true if not (i.e. it is an end word).
            for (int j = indexCheck; j < words.Length; j++ )
            {
                char[] word = words[j].ToCharArray();
                if (Char.IsPunctuation(word[word.Length-1]))
                {
                    return false;
                }
            }
            // Start at indexCheck - incase indexCheck == 0.
            for (int j = indexCheck; j >= 0; j--)
            {
                char[] word = words[j].ToCharArray();

                if (Char.IsPunctuation(word[word.Length-1]))
                {
                    // Break out if we find punctuation before a capital letter.
                    break;
                }

                if (Char.IsUpper(word[0]))
                {
                    // Found an uppercase - so the word lies in a sentence.
                    return false;
                }
            }

            return true;
        }

    }
}
