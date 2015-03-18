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

        private static Component chirpFilter_FilterModule;
        private Dictionary<IChirperMessage, bool> messageFilterMap;

        private bool userOpened = false;

        private static bool isMuted = false;
        private static bool isFiltered = false;
        private static bool hasFilters = false;

        private bool newMsgIn = false;

        public static Component ChirpFilter_FilterModule
        {
            get
            {
                return chirpFilter_FilterModule;
            }
            set
            {
                chirpFilter_FilterModule = value;
            }

        }
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
                    // Check if user had window open before.
                    if (!userOpened)
                        chirpPane.Collapse();
                    newMsgIn = false;
                }
            }
            else if (!isMuted)
            {
                // Room for further implementation in the future. (Potentially filtering etc.)
                /*
                 * Currently not implemented.
                if (isFiltered)
                {
                    FilterMessages();
                }
                 */

            }

            // Detect keypress - Alt+C
            if (Event.current.alt && Input.GetKeyDown(KeyCode.C))
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Alt+C detected.");

                // Toggle chirpy
                chirpPane.gameObject.SetActive(!chirpPane.gameObject.activeSelf);

            }


        }

        //Thread: Main
        public override void OnNewMessage(IChirperMessage message)
        {
            userOpened = chirpPane.isShowing;

            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Message received");
            newMsgIn = true;

            if (!isMuted)
            {
                bool filter = false;
                if (chirpFilter_FilterModule is IFormattable)
                {
                    IFormattable formattable = chirpFilter_FilterModule as IFormattable;
                    string text = formattable.ToString(message.text, null);
                    if (text == "true")
                    {
                        filter = true;
                    }
                    else if (text == "false")
                    {
                        filter = false;
                    }
                }
                messageFilterMap.Add(message, filter);
            }
        }



        // Custom method to delete messages (currently not used)
        private void DeleteMessage(IChirperMessage message)
        {
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;
            
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Total Children:" + container.childCount);

            for (int i = 0; i < container.childCount; ++i)
            {
                if (container.GetChild(i).GetComponentInChildren<UILabel>().text.Equals(message.text))
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Deleted Message:" + message.text);
                    UITemplateManager.RemoveInstance("ChirpTemplate", container.GetChild(i).GetComponent<UIPanel>());
                    messageManager.DeleteMessage(message);
                    chirpPane.Collapse();
                    return;
                }
            }
        }
        // Custom method to filter all messages (currently not used)
        private void FilterMessages()
        {
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Total Children:" + container.childCount);

            for (int i = 0; i < container.childCount; ++i)
            {


            }
        }
    }
}