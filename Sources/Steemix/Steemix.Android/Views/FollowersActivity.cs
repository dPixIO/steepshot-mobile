using System;
using Android.App;
using Android.OS;
using Com.Lilarcor.Cheeseknife;
using Steemix.Droid.Activity;
using Steemix.Droid.ViewModels;

namespace Steemix.Droid.Views
{
    [Activity]
    public class FollowersActivity : BaseActivity<FollowersViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.lyt_settings);
            Cheeseknife.Inject(this);
        }

      
        [InjectOnClick(Resource.Id.go_back)]
        public void GoBackClick(object sender, EventArgs e)
        {
            Finish();
        }
    }
}