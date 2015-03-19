using System;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace SuperChirper
{

     public class SuperChirperLoader : LoadingExtensionBase
    {
         private static ChirpPanel chirpPane;
         private MessageManager messageManager;
         private GameObject optionsPanel;

         private static AudioClip messageSound = null; 

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

        public override void OnLevelLoaded(LoadMode mode)
        {
            chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();
            messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();

            if (chirpPane == null) return;

            messageSound = chirpPane.m_NotificationSound;

            #region "ChirpFilters Mod"
            // We check if ChirpFilters mod is installed - if it is we don't worry about handling our own filters.
            GameObject chirperFilter = GameObject.Find("ChirperFilterModule");
            if (chirperFilter != null)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] ChirpFilters located.");
                SuperChirper.HasFilters = true;
            }
            else if (chirperFilter == null)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] ChirpFilters NOT located.");
                SuperChirper.HasFilters = false;
            }
            #endregion

            #region "NewGame"
            // Give intro message (only shows up on new level)
            ChirpMessage introMessage = new ChirpMessage("SuperChirpy", "Welcome to Super Chirpy! Press Alt+C to toggle Chirpy, use buttons above to MUTE and CLEAR.", 12345);
            // Get rid of default message
            chirpPane.ClearMessages();
            chirpPane.AddMessage(introMessage);
            #endregion

            

            GameObject clearButtonObject = new GameObject("SuperChirperClearButton", typeof(UIButton));
            GameObject muteButtonObject = new GameObject("SuperChirperMuteButton", typeof(UIButton));
            GameObject filterButtonObject = new GameObject("SuperChirperFilterButton", typeof(UIButton));
            GameObject optionsButtonObject = new GameObject("SuperChirperOptionsButton", typeof(UIButton));
            optionsPanel = new GameObject("SuperChirperOptionsPanel", typeof(OptionsPanel));


            UIView.GetAView().AttachUIComponent(optionsPanel);

            // Make the Objects a child of the uiView.
            clearButtonObject.transform.parent = chirpPane.transform;
            muteButtonObject.transform.parent = chirpPane.transform;
            filterButtonObject.transform.parent = chirpPane.transform;
            optionsButtonObject.transform.parent = chirpPane.transform;

            // Get the button component.
            UIButton clearButton = clearButtonObject.GetComponent<UIButton>();
            UIButton muteButton = muteButtonObject.GetComponent<UIButton>();
            UIButton filterButton = filterButtonObject.GetComponent<UIButton>();
            UIButton optionsButton = optionsButtonObject.GetComponent<UIButton>();

            // Set the text to show on the button.
            clearButton.text = "Clear";
            muteButton.text = "Mute";
            
            // Defaults to on if ChirpFilter is active.
            if (SuperChirper.HasFilters)
                filterButton.text = "Filters: ON";
            else
                filterButton.text = "Filters: OFF";

            optionsButton.text = "Options";

            // Set the button dimensions. 
            clearButton.width = 50;
            clearButton.height = 20;
            muteButton.width = 50;
            muteButton.height = 20;
            filterButton.width = 90;
            filterButton.height = 20;
            optionsButton.width = 60;
            optionsButton.height = 20;

            // Style the buttons to make them look like a menu button.
            clearButton.normalBgSprite = "ButtonMenu";
            clearButton.disabledBgSprite = "ButtonMenuDisabled";
            clearButton.hoveredBgSprite = "ButtonMenuHovered";
            clearButton.focusedBgSprite = "ButtonMenuFocused";
            clearButton.pressedBgSprite = "ButtonMenuPressed";
            clearButton.textColor = new Color32(255, 255, 255, 255);
            clearButton.disabledTextColor = new Color32(7, 7, 7, 255);
            clearButton.hoveredTextColor = new Color32(7, 132, 255, 255);
            clearButton.focusedTextColor = new Color32(255, 255, 255, 255);
            clearButton.pressedTextColor = new Color32(30, 30, 44, 255);

            muteButton.normalBgSprite = "ButtonMenu";
            muteButton.disabledBgSprite = "ButtonMenuDisabled";
            muteButton.hoveredBgSprite = "ButtonMenuHovered";
            muteButton.focusedBgSprite = "ButtonMenuFocused";
            muteButton.pressedBgSprite = "ButtonMenuPressed";
            muteButton.textColor = new Color32(255, 255, 255, 255);
            muteButton.disabledTextColor = new Color32(7, 7, 7, 255);
            muteButton.hoveredTextColor = new Color32(7, 132, 255, 255);
            muteButton.focusedTextColor = new Color32(255, 255, 255, 255);
            muteButton.pressedTextColor = new Color32(30, 30, 44, 255);

            
            filterButton.normalBgSprite = "ButtonMenu";
            filterButton.disabledBgSprite = "ButtonMenuDisabled";
            filterButton.hoveredBgSprite = "ButtonMenuHovered";
            filterButton.focusedBgSprite = "ButtonMenuFocused";
            filterButton.pressedBgSprite = "ButtonMenuPressed";
            filterButton.textColor = new Color32(255, 255, 255, 255);
            filterButton.disabledTextColor = new Color32(7, 7, 7, 255);
            filterButton.hoveredTextColor = new Color32(7, 132, 255, 255);
            filterButton.focusedTextColor = new Color32(255, 255, 255, 255);
            filterButton.pressedTextColor = new Color32(30, 30, 44, 255);

            optionsButton.normalBgSprite = "ButtonMenu";
            optionsButton.disabledBgSprite = "ButtonMenuDisabled";
            optionsButton.hoveredBgSprite = "ButtonMenuHovered";
            optionsButton.focusedBgSprite = "ButtonMenuFocused";
            optionsButton.pressedBgSprite = "ButtonMenuPressed";
            optionsButton.textColor = new Color32(255, 255, 255, 255);
            optionsButton.disabledTextColor = new Color32(7, 7, 7, 255);
            optionsButton.hoveredTextColor = new Color32(7, 132, 255, 255);
            optionsButton.focusedTextColor = new Color32(255, 255, 255, 255);
            optionsButton.pressedTextColor = new Color32(30, 30, 44, 255);

            // Enable sounds.
            clearButton.playAudioEvents = true;
            muteButton.playAudioEvents = true;
            filterButton.playAudioEvents = true;
            optionsButton.playAudioEvents = true;

            // Place the button.
            clearButton.transformPosition = new Vector3(-1.22f, 1.0f);
            muteButton.transformPosition = new Vector3(-1.37f, 1.0f);
            filterButton.transformPosition = new Vector3(-1.57f, 1.0f);
            optionsButton.transformPosition = new Vector3(-1.72f, 1.0f);

            // Respond to button click.
            clearButton.eventClick += ClearButtonClick;
            muteButton.eventClick += MuteButtonClick;
            filterButton.eventClick += FilterButtonClick;
            optionsButton.eventClick += OptionsButtonClick;

            SuperChirperMod.ClearButtonInstance = clearButton;
            SuperChirperMod.MuteButtonInstance = muteButton;
            SuperChirperMod.FilterButtonInstance = filterButton;
            SuperChirperMod.OptionsButtonInstance = optionsButton;

        }

        private void ClearButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                // Clear all messages in Chirpy and hide the window
                chirpPane.ClearMessages();
                chirpPane.Collapse();

            }
        }

        private void MuteButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                
                if (SuperChirper.IsMuted)
                {
                    // Unmute the chirper, let it make noise.
                    SuperChirper.IsMuted = false;
                    chirpPane.m_NotificationSound = messageSound;

                    // Inform user that chirpy has been unmuted
                    chirpPane.AddMessage(new ChirpMessage("SuperChirper","Chirpy now unmuted.",12345), true);

                    // Adjust button.                    
                    SuperChirperMod.MuteButtonInstance.width = 50;
                    SuperChirperMod.MuteButtonInstance.text = "Mute";
                } 
                else if (!SuperChirper.IsMuted)
                {
                    // Set chirper to muted, update sounds, adjust button.
                    SuperChirper.IsMuted = true;
                    chirpPane.m_NotificationSound = null;

                    SuperChirperMod.MuteButtonInstance.width = 64;
                    SuperChirperMod.MuteButtonInstance.text = "Unmute";
                }
                
            }
        }

        private void FilterButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                if (!SuperChirper.HasFilters)
                {
                    if (SuperChirper.IsFiltered)
                    {
                        SuperChirper.IsFiltered = false;
                        chirpPane.AddMessage(new ChirpMessage("SuperChirper", "Filters removed.", 12345), true);
                        SuperChirperMod.FilterButtonInstance.text = "Filters: OFF";
                    }
                    else if (!SuperChirper.IsFiltered)
                    {
                        SuperChirper.IsFiltered = true;
                        chirpPane.AddMessage(new ChirpMessage("SuperChirper", "Filters applied.", 12345), true);
                        SuperChirperMod.FilterButtonInstance.text = "Filters: ON";
                    }
                }
                else
                {
                    chirpPane.AddMessage(new ChirpMessage("SuperChirper", "ChirpFilters by Zuppi detected. Please disable if you want to toggle filters.", 12345), true);
                }
                

            }
        }

        private void OptionsButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                OptionsPanel panelComponent = optionsPanel.GetComponent<OptionsPanel>();
                if (panelComponent.isVisible)
                {
                    panelComponent.Hide();
                }
                else 
                {
                    panelComponent.Show();
                }
                    
            }
        }

    }
}

