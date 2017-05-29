using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using static Android.Views.Animations.Animation;

namespace IsItRaining.Includes
{
    public class AnimationListener : Java.Lang.Object, IAnimationListener
    {
        MainActivity activity;
        LinearLayout linearLayout;

        public AnimationListener(MainActivity act, LinearLayout ll)
        {
            activity = act;
            linearLayout = ll;
        }



        public void Dispose()
        {
            
        }

        public void OnAnimationEnd(Animation animation)
        {
            //Animation fadeOut = AnimationUtils.LoadAnimation(activity.ApplicationContext, Resource.Animation.fade_out);
            //linearLayout.StartAnimation(fadeOut);
        }

        public void OnAnimationRepeat(Animation animation)
        {
            
        }

        public void OnAnimationStart(Animation animation)
        {
            
        }
    }
}