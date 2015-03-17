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

        private static AudioClip messageSound = null;        

        private static bool isMuted = false;
        private static bool isFiltered = false;
        
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

        public static AudioClip MessageSound
        {
            get
            {
                return messageSound;
            }
            set
            {
                messageSound = value;
            }
        }


        //Thread: Main
        public override void OnCreated(IChirper chirper)
        {
            try
            {
                chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();
                messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();

                messageSound = chirpPane.m_NotificationSound;

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
            chirpPane.m_NotificationSound = messageSound;
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
                    chirpPane.Collapse();
                    newMsgIn = false;
                }
            } else if (!isMuted)
            {

            }
            
            
             
        }

        //Thread: Main
        public override void OnNewMessage(IChirperMessage message)
        {
            /*
             * Checks if message is garbage, if so removes it.
            CitizenMessage cm = message as CitizenMessage;

            if (cm == null)
            {
                DeleteMessage(message);
            }
            */

            newMsgIn = true;
            chirpPane.m_NotificationSound = messageSound;
            if (isMuted)
            {
                //DeleteMessage(message);
            }
            else if (!isMuted)
            {

            }
        }

        // Custom method to delete messages
        private void DeleteMessage(IChirperMessage message)
        {
            var container = ChirpPanel.instance.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;
            for (int i = 0; i < ChirpPanel.instance.transform.childCount; i++)
            {
                if (message.text.Equals(container.GetChild(i).GetComponentInChildren<UILabel>().text))
                {
                    UITemplateManager.RemoveInstance("ChirpTemplate", container.GetChild(i).GetComponent<UIPanel>());
                    messageManager.DeleteMessage(message);
                    chirpPane.Collapse();
                    return;
                }
            }
        }

    }
    
}