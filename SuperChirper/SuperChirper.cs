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
        private IChirper thisChirper;

        private Dictionary<ChirpMessage, bool> messageFilterMap;
        private List<ChirpMessage> hashTaggedMessages;
        private Dictionary<ChirpMessage, IChirperMessage> messageMap;

        private bool userOpened = false;

        private static bool isMuted = false;
        private static bool isFiltered = false;
        private static bool hasFilters = false;
        private static bool isHashTagged = true;

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

        public static bool IsHashTagged
        {
            get
            {
                return isHashTagged;
            }
            set
            {
                isHashTagged = value;
            }
        }




        //Thread: Main
        public override void OnCreated(IChirper chirper)
        {
            try
            {
                chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();
                messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();

                thisChirper = chirper;

                messageFilterMap = new Dictionary<ChirpMessage, bool>();
                hashTaggedMessages = new List<ChirpMessage>();
                messageMap = new Dictionary<ChirpMessage, IChirperMessage>();

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

                // Remove hashtags
                if (!isHashTagged)
                {
                    RemoveHashtags();
                }

                 
            }

            // Detect keypress - Alt+C
            if (Event.current.alt && Input.GetKeyDown(KeyCode.C))
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Chirpy toggled.");

                // Toggle chirpy
                chirpPane.gameObject.SetActive(!chirpPane.gameObject.activeSelf);
                if (isMuted)
                {
                    newMsgIn = true;
                }


                /*
                 * TODO:
                 * Stop messages not clearing when we reactivate after muting
                 * Stop hashtags coming back when we toggle active again - why is this even happening?!
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
                ChirpMessage storedMessage = new ChirpMessage(message.senderName, message.text, message.senderID);

                if (cm != null)
                {
                    // Check if message is garbage
                    bool filter = ChirpFilter.FilterMessage(cm.m_messageID);

                    // Check if we should make noise
                    chirpPane.m_NotificationSound = ((isFiltered && filter) ? null : SuperChirperLoader.MessageSound);

                    // TODO: Change to ChirpMessage in dictionary, to make compatible with hashtag removal.
                    messageFilterMap.Add(storedMessage, filter);
                }
                else
                {
                    // Default to unfiltered messages.
                    messageFilterMap.Add(storedMessage, false);
                }

                hashTaggedMessages.Add(storedMessage);
                messageMap.Add(storedMessage, message);

            }

        }

        // Custom method to delete a single message - uses ChirpMessages.
        private void DeleteMessage(ChirpMessage message)
        {
            // Get container for chirps
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;

            for (int i = 0; i < container.childCount; ++i)
            {
                if (container.GetChild(i).GetComponentInChildren<UILabel>().text.Equals(message.GetText()))
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Deleted Message:" + message.text);
                    // Remove both visual and internal message.
                    UITemplateManager.RemoveInstance("ChirpTemplate", container.GetChild(i).GetComponent<UIPanel>());
                    IChirperMessage delMessage;
                    messageMap.TryGetValue(message, out delMessage);
                    messageManager.DeleteMessage(delMessage);

                    if (!userOpened)
                        chirpPane.Collapse();
                    return;
                }
            }
        }

        // Custom method to filter all messages
        private void FilterMessages()
        {
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;

            foreach (ChirpMessage message in messageFilterMap.Keys)
            {
                // Check if we flagged it as garbage.
                bool filtered;
                messageFilterMap.TryGetValue(message, out filtered);

                if (filtered)
                {
                    // Remove all instances of the message.                    
                    try
                    {
                        DeleteMessage(message);
                        messageFilterMap.Remove(message);

                        if (hashTaggedMessages.Contains(message))
                            hashTaggedMessages.Remove(message);
                    }
                    catch (Exception e)
                    {
                        DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Error: " + e.Message);
                    }

                }
            }
            
        }

        private void RemoveHashtags()
        {
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;

            foreach (ChirpMessage message in hashTaggedMessages)
            {
                string newMessageText = ChirpFilter.DeHashTagMessage(message);
                DeleteMessage(message);

                ChirpMessage newMessage = new ChirpMessage(message.senderName,newMessageText,message.senderID);
                chirpPane.AddMessage(newMessage, true);

                // Clean up
                bool filtered;
                messageFilterMap.TryGetValue(message, out filtered);
                IChirperMessage managerMessage;
                messageMap.TryGetValue(message, out managerMessage);
                
                if (messageFilterMap.ContainsKey(message))
                {
                    messageFilterMap.Remove(message);
                    messageFilterMap.Add(newMessage, filtered);
                }

                if (messageMap.ContainsKey(message))
                {
                    messageMap.Remove(message);
                    messageMap.Add(newMessage, managerMessage);
                }
                

                hashTaggedMessages.Remove(message);
            }
        }
    }
}