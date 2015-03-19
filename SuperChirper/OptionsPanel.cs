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
            this.AddUIComponent(typeof(UIDragHandle));

            this.backgroundSprite = "GenericPanelLight";
            this.color = new Color32(120, 120, 255, 200);
            this.width = 300;
            this.height = 200;

            // Default to hidden
            this.Hide();
        }

        protected override void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
        {
            base.OnResolutionChanged(previousResolution, currentResolution);
        }
    }
}
