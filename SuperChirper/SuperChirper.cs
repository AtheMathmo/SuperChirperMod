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
        public static IChirper accessChirper;
        MessageManager messageManager;
        

        private static bool isMuted = false;

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


        //Thread: Main
        public override void OnCreated(IChirper chirper)
        {
            if (accessChirper == null)
                accessChirper = chirper;

            ChirpPanel.instance.AddMessage(new ChirpMessage("SuperChirper", "Welcome to SuperChirper!", 12345), true);

            messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();

            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] Initialised modification");
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
                //ChirpPanel.instance.ClearMessages();
            } else if (!isMuted)
            {

            }
        }

        //Thread: Main
        public override void OnNewMessage(IChirperMessage message)
        {
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
            for (int i = 0; i < ChirpPanel.instance.transform.childCount; i++)
            {
                if (message.text.Equals(ChirpPanel.instance.transform.GetChild(i).GetComponentInChildren<UILabel>().text))
                {
                    UITemplateManager.RemoveInstance("ChirpTemplate", ChirpPanel.instance.transform.GetChild(i).GetComponent<UIPanel>());
                    messageManager.DeleteMessage(message);
                    return;
                }
            }
        }

    }
    
}