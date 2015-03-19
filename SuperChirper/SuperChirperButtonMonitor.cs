using System;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace SuperChirper
{

    public class SuperChirperButtonMonitor : ThreadingExtensionBase
    {
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            try
            {
                if (ChirpPanel.instance != null && SuperChirperMod.ClearButtonInstance != null)
                {
                    if (SuperChirperMod.ClearButtonInstance.isVisible && !ChirpPanel.instance.isShowing)
                    {
                        SuperChirperMod.ClearButtonInstance.Hide();
                        SuperChirperMod.MuteButtonInstance.Hide();
                        SuperChirperMod.FilterButtonInstance.Hide();
                        SuperChirperMod.OptionsButtonInstance.Hide();
                    }
                    else if (!SuperChirperMod.ClearButtonInstance.isVisible && ChirpPanel.instance.isShowing)
                    {
                        SuperChirperMod.ClearButtonInstance.Show();
                        SuperChirperMod.MuteButtonInstance.Show();
                        SuperChirperMod.FilterButtonInstance.Show();
                        SuperChirperMod.OptionsButtonInstance.Show();
                    }
                }
            }
            catch
            {
                // Gulp.
            }

            base.OnUpdate(realTimeDelta, simulationTimeDelta);
        }
    }
}
