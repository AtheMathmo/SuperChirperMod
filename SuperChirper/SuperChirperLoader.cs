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

        // For development
        private void DestroyPanel()
        {
            if (optionsPanel != null)
            {
                GameObject.Destroy(optionsPanel);
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();
            messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();

            // For development
            DestroyPanel();

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
            GameObject optionsButtonObject = new GameObject("SuperChirperOptionsButton", typeof(UIButton));
            optionsPanel = new GameObject("SuperChirperOptionsPanel", typeof(OptionsPanel));

            UIView.GetAView().AttachUIComponent(optionsPanel);

            // Make the Objects a child of the uiView.
            clearButtonObject.transform.parent = chirpPane.transform;
            optionsButtonObject.transform.parent = chirpPane.transform;

            // Get the button component.
            UIButton clearButton = clearButtonObject.GetComponent<UIButton>();
            UIButton optionsButton = optionsButtonObject.GetComponent<UIButton>();

            // Set the text to show on the button.
            clearButton.text = "Clear";
            optionsButton.text = "Options";

            // Set the button dimensions. 
            clearButton.width = 50;
            clearButton.height = 20;
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
            optionsButton.playAudioEvents = true;

            // Place the button.
            clearButton.transformPosition = new Vector3(-1.22f, 1.0f);
            optionsButton.transformPosition = new Vector3(-1.62f, 1.0f);

            // Respond to button click.
            clearButton.eventClick += ClearButtonClick;
            optionsButton.eventClick += OptionsButtonClick;

            SuperChirperMod.ClearButtonInstance = clearButton;

            SuperChirperMod.OptionsButtonInstance = optionsButton;
            SuperChirperMod.OptionsPanelInstance = optionsPanel.GetComponent<OptionsPanel>();

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

        private void OptionsButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {

                if (SuperChirperMod.OptionsPanelInstance.isVisible)
                {
                    SuperChirperMod.OptionsPanelInstance.Hide();
                }
                else 
                {
                    SuperChirperMod.OptionsPanelInstance.Show();
                }
                    
            }
        }

    }
}

