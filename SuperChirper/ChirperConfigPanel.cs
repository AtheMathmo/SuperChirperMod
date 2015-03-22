using System;
using System.Collections.Generic;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace SuperChirper
{
    public class ChirperConfigPanel : UIPanel
    {
        private ChirpPanel chirpPane;

        public override void Start()
        {
            chirpPane = GameObject.Find("ChirperPanel").GetComponent<ChirpPanel>();

            if (chirpPane == null) return;

            // Set visuals for panel
            this.backgroundSprite = "ChirperBubble";
            this.color = new Color32(122, 132, 138, 255);
            this.width = 300;
            this.height = 200;
            this.transformPosition = new Vector3(-1.0f, 0.9f);

            // Allow automated layout
            this.autoLayoutDirection = LayoutDirection.Vertical;
            this.autoLayoutStart = LayoutStart.TopLeft;
            this.autoLayoutPadding = new RectOffset(10, 10, 0, 10);
            this.autoLayout = true;

            // Add drag handle for panel
            UIDragHandle dragHandle = this.AddUIComponent<UIDragHandle>();
            // Add title to drag handle
            UILabel titleLabel = dragHandle.AddUIComponent<UILabel>();
            titleLabel.text = "Options";
            titleLabel.textScale = 1.5f;
            titleLabel.textColor = new Color32(36, 202, 255, 255);
            titleLabel.textAlignment = UIHorizontalAlignment.Center;

            UIButton muteButton = AddNewButton("Mute");
            UIButton filterButton = AddNewButton("Filters: OFF");
            UIButton hashTagsButton = AddNewButton("HashTags: ON");

            // Defaults to ON if ChirpFilter is active.
            /*
            if (SuperChirper.HasFilters)
                filterButton.text = "Filters: ON";
             */

            muteButton.eventClick += MuteButtonClick;
            filterButton.eventClick += FilterButtonClick;
            hashTagsButton.eventClick += HashTagsButtonClick;

            SuperChirperMod.MuteButtonInstance = muteButton;
            SuperChirperMod.FilterButtonInstance = filterButton;
            SuperChirperMod.HashTagsButtonInstance = hashTagsButton;

            // Default to hidden
            this.Hide();

            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SuperChirper] ConfigPanel built.");
        }

        protected override void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
        {
            base.OnResolutionChanged(previousResolution, currentResolution);
        }

        #region "Button installation"
        private UIButton AddNewButton(string buttonText)
        {
            UIButton newButton = this.AddUIComponent<UIButton>();

            SetDefaultButton(newButton, buttonText);

            return newButton;
        }

        private void SetDefaultButton(UIButton button, string buttonText)
        {
            button.text = buttonText;
            button.width = this.width-this.autoLayoutPadding.left*2;
            button.height = 25;

            button.normalBgSprite = "ButtonMenu";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.focusedBgSprite = "ButtonMenuFocused";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.textColor = new Color32(255, 255, 255, 255);
            button.disabledTextColor = new Color32(7, 7, 7, 255);
            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.focusedTextColor = new Color32(255, 255, 255, 255);
            button.pressedTextColor = new Color32(30, 30, 44, 255);
        }
        #endregion

        #region "Button Clicks"
        private void LabelClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                this.Hide();
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
                    chirpPane.m_NotificationSound = SuperChirperLoader.MessageSound;

                    // Inform user that chirpy has been unmuted
                    chirpPane.AddMessage(new ChirpMessage("SuperChirper", "Chirpy now unmuted.", 12345), true);

                    // Adjust button.                    
                    SuperChirperMod.MuteButtonInstance.text = "Mute";
                }
                else if (!SuperChirper.IsMuted)
                {
                    // Set chirper to muted, update sounds.
                    SuperChirper.IsMuted = true;
                    chirpPane.m_NotificationSound = null;
                    chirpPane.ClearMessages();

                    // Adjust button.
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

        private void HashTagsButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (eventParam.buttons == UIMouseButton.Left && ChirpPanel.instance != null)
            {
                if (SuperChirper.IsHashTagged)
                    {
                        SuperChirper.IsHashTagged = false;
                        chirpPane.AddMessage(new ChirpMessage("SuperChirper", "HashTags OFF.", 12345), true);
                        SuperChirperMod.HashTagsButtonInstance.text = "HashTags: OFF";
                    }
                else if (!SuperChirper.IsHashTagged)
                    {
                        SuperChirper.IsHashTagged = true;
                        chirpPane.AddMessage(new ChirpMessage("SuperChirper", "HashTags ON.", 12345), true);
                        SuperChirperMod.HashTagsButtonInstance.text = "HashTags: ON";
                    }
            }
        }
        #endregion

    }
}
