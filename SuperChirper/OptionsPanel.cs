using System;
using System.Collections.Generic;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace SuperChirper
{
    class OptionsPanel : UIPanel
    {
        
        public override void Start()
        {
            // Set visuals for panel
            this.backgroundSprite = "GenericPanel";
            this.color = new Color32(120, 120, 255, 200);
            this.width = 300;
            this.height = 200;

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
            titleLabel.textColor = new Color32(255, 145, 100, 255);
            titleLabel.textAlignment = UIHorizontalAlignment.Center;
           
            UIButton testButton = this.AddUIComponent<UIButton> ();
            testButton.text = "Test";
            testButton.width = 100;
            testButton.height = 20;

            testButton.normalBgSprite = "ButtonMenu";
            testButton.disabledBgSprite = "ButtonMenuDisabled";
            testButton.hoveredBgSprite = "ButtonMenuHovered";
            testButton.focusedBgSprite = "ButtonMenuFocused";
            testButton.pressedBgSprite = "ButtonMenuPressed";
            testButton.textColor = new Color32(255, 255, 255, 255);
            testButton.disabledTextColor = new Color32(7, 7, 7, 255);
            testButton.hoveredTextColor = new Color32(7, 132, 255, 255);
            testButton.focusedTextColor = new Color32(255, 255, 255, 255);
            testButton.pressedTextColor = new Color32(30, 30, 44, 255);

            UICheckBox checkBox = this.AddUIComponent<UICheckBox>();             

            // Default to hidden
            this.Hide();
        }

        protected override void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
        {
            base.OnResolutionChanged(previousResolution, currentResolution);
        }
    }
}
