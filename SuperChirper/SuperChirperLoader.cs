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
        public override void OnLevelLoaded(LoadMode mode)
        {

            chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();
            //chirpPane = Singleton<ChirpPanel>.instance;

            if (chirpPane == null) return;

            // Give intro message (currently doesn't show up)
            ChirpMessage introMessage = new ChirpMessage("SuperChirpy", "Welcome to Super Chirpy!", 12345);
            chirpPane.AddMessage(introMessage, true);

            // Credit to:
            // http://www.reddit.com/r/CitiesSkylinesModding/comments/2ymwxe/example_code_using_the_colossal_ui_in_a_user_mod/
            // https://gist.github.com/reima/9ba51c69f65ae2da7909
            // https://github.com/skymodteam/skymod-chirpymaid

            GameObject clearButtonObject = new GameObject("SuperChirperClearButton", typeof(UIButton));
            GameObject muteButtonObject = new GameObject("SuperChirperMuteButton", typeof(UIButton));

            // Make the buttonObject a child of the uiView.
            clearButtonObject.transform.parent = chirpPane.transform;
            muteButtonObject.transform.parent = chirpPane.transform;

            // Get the button component.
            UIButton clearButton = clearButtonObject.GetComponent<UIButton>();
            UIButton muteButton = muteButtonObject.GetComponent<UIButton>();

            // Set the text to show on the button.
            clearButton.text = "Clear";
            muteButton.text = "Mute";

            // Set the button dimensions. 
            clearButton.width = 50;
            clearButton.height = 20;
            muteButton.width = 50;
            muteButton.height = 20;

            // Style the button to make it look like a menu button.
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

            // Enable sounds.
            clearButton.playAudioEvents = true;
            muteButton.playAudioEvents = true;

            // Place the button.
            clearButton.transformPosition = new Vector3(-1.22f, 1.0f);
            muteButton.transformPosition = new Vector3(-1.37f, 1.0f);

            // Respond to button click.
            clearButton.eventClick += ClearButtonClick;
            muteButton.eventClick += MuteButtonClick;

            SuperChirperMod.ClearButtonInstance = clearButton;
            SuperChirperMod.MuteButtonInstance = muteButton;

        }

        private void ClearButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                // Clear all messages in Chirpy and hide the window
                chirpPane.ClearMessages();
                ChirpPanel.instance.Hide();
                // Give intro message
                ChirpMessage introMessage = new ChirpMessage("SuperChirpy", "Welcome to Super Chirpy!", 12345);
                chirpPane.AddMessage(introMessage, true);
            }
        }

        private void MuteButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                
                if (SuperChirper.IsMuted)
                {
                    // Inform user that chirpy has been unmuted
                    ChirpPanel.instance.AddMessage(new ChirpMessage("SuperChirper","Chirpy now unmuted.",12345), true);

                    // Set chirper to unmuted, adjust button.
                    SuperChirper.IsMuted = false;
                    SuperChirperMod.MuteButtonInstance.width = 50;
                    SuperChirperMod.MuteButtonInstance.text = "Mute";
                } else if (!SuperChirper.IsMuted)
                {
                    // Set chirper to muted, adjust button.
                    SuperChirper.IsMuted = true;
                    SuperChirperMod.MuteButtonInstance.width = 64;
                    SuperChirperMod.MuteButtonInstance.text = "Unmute";
                }
                
            }
        }
    }
}

