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

               

        private static bool isMuted = false;
        // Currently not used.
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

        


        //Thread: Main
        public override void OnCreated(IChirper chirper)
        {
            try
            {
                chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();
                messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();

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
            /*
            if (isMuted)
            {
                // Collapse and clear only when a new message is received. (Otherwise can not open once muted.)
                if (newMsgIn)
                {
                    
                    chirpPane.ClearMessages();
                    chirpPane.Collapse();
                    newMsgIn = false;
                }
            } 
            else if (!isMuted)
            {
                // Room for further implementation in the future. (Potentially filtering etc.)
            }
             * 
             */


        }

        //Thread: Main
        public override void OnNewMessage(IChirperMessage message)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Message received");
            newMsgIn = true;

            // Ideally we want to filter at this stage, but currently the DeleteMessage doesn't work.
            if (isMuted)
            {
                DeleteMessage(message);
            }
            else if (!isMuted)
            {

            }
        }

        // Custom method to delete messages (currently not used)
        private void DeleteMessage(IChirperMessage message)
        {
            Transform container = chirpPane.transform.FindChild("Chirps").FindChild("Clipper").FindChild("Container").gameObject.transform;
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Total Children:" + container.childCount);

            for (int i = 0; i < container.childCount; ++i)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Delete Message:" + message.text);
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Contained Message:" + container.GetChild(i).GetComponentInChildren<UILabel>().text);
                if (container.GetChild(i).GetComponentInChildren<UILabel>().text.Equals(message.text))
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