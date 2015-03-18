using System;
using System.Collections.Generic;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;


namespace SuperChirper
{
    public class SuperChirper : ChirperExtensionBase
    {
        // Chirper instance to adopt the game instance.
        private static ChirpPanel chirpPane;
        private static MessageManager messageManager;

        private Dictionary<IChirperMessage, bool> messageFilterMap;

        private bool userOpened = false;

        private static bool isMuted = false;
        private static bool isFiltered = false;
        private static bool hasFilters = false;

        private bool newMsgIn = false;

        public static bool IsMuted
        {
            get
            {
                return isMuted;
            }
            set
            {
                isMuted = value;
            }
        }

        public static bool IsFiltered
        {
            get
            {
                return isFiltered;
            }
            set
            {
                isFiltered = value;
            }
        }

        public static bool HasFilters
        {
            get
            {
                return hasFilters;
            }
            set
            {
                hasFilters = value;
            }
        }




        //Thread: Main
        public override void OnCreated(IChirper chirper)
        {
            try
            {
                chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();
                messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();
                messageFilterMap = new Dictionary<IChirperMessage, bool>();            

                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Initialised modification.");
            }
            catch
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Initialisation failed.");
            }

        }

        //Thread: Main
        public override void OnReleased()
        {
        }

        //Thread: Main
        public override void OnUpdate()
        {
            if (isMuted)
            {
                // Collapse and clear only when a new message is received. (Otherwise can not open once muted.)
                if (newMsgIn)
                {
                    chirpPane.ClearMessages();
                    messageFilterMap.Clear();
                    // Check if user had window open before.
                    if (!userOpened)
                        chirpPane.Collapse();
                    newMsgIn = false;
                }
            }
            else if (!isMuted)
            {
                // Room for further implementation in the future. (Potentially filtering etc.)
                
                if (isFiltered && !hasFilters)
                {
                    FilterMessages();
                }
                 
            }

            // Detect keypress - Alt+C
            if (Event.current.alt && Input.GetKeyDown(KeyCode.C))
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Chirpy toggled.");

                // Toggle chirpy
                chirpPane.gameObject.SetActive(!chirpPane.gameObject.activeSelf);

                /*
                 * TODO:
                 * Stop messages not clearing when we reactivate after muting
                 */

            }


        }

        //Thread: Main
        public override void OnNewMessage(IChirperMessage message)
        {
            // To make mute collapsing work better.
            userOpened = chirpPane.isShowing;
            newMsgIn = true;

            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Message received");

            if (!isMuted)
            {
                // Cast message and check whether it should be filtered.
                CitizenMessage cm = message as CitizenMessage;

                if (cm != null)
                {
                    // Check if message is garbage
                    bool filter = ChirpFilter.FilterMessage(cm.m_messageID);

                    // Check if we should make noise
                    chirpPane.m_NotificationSound = (filter ? null : SuperChirperLoader.MessageSound);

                    messageFilterMap.Add(message, filter);
                }
                else
                {
                    // Default to unfiltered messages.
                    messageFilterMap.Add(message, false);
                }

            }
        }



        // Custom method to delete messages (currently not used)
        private void DeleteMessage(IChirperMessage message)
        {
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;

            for (int i = 0; i < container.childCount; ++i)
            {
                if (container.GetChild(i).GetComponentInChildren<UILabel>().text.Equals(message.text))
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Deleted Message:" + message.text);
                    UITemplateManager.RemoveInstance("ChirpTemplate", container.GetChild(i).GetComponent<UIPanel>());
                    messageManager.DeleteMessage(message);
                    if (!userOpened)
                        chirpPane.Collapse();
                    return;
                }
            }
        }

        // Custom method to filter all messages (currently not used)
        private void FilterMessages()
        {
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;

            foreach (IChirperMessage message in messageFilterMap.Keys)
            {
                bool filtered;
                messageFilterMap.TryGetValue(message, out filtered);

                if (filtered)
                {
                    DeleteMessage(message);
                    messageFilterMap.Remove(message);
                }
            }
            
        }
    }
}