using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Content.PM;
using System.Threading.Tasks;

namespace Steemix.Android.Activity
{
    [Activity(Label = "SteepShot", MainLauncher=true,Icon = "@mipmap/ic_launcher",ScreenOrientation = ScreenOrientation.Portrait)]
	public class FeedActivity : BaseActivity<FeedViewModel>, View.IOnScrollChangeListener
	{
        RecyclerView FeedList;
        ProgressBar Bar;
        Adapter.FeedAdapter FeedAdapter;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.lyt_feed);

            FeedList = FindViewById<RecyclerView>(Resource.Id.feed_list);
            Bar = FindViewById<ProgressBar>(Resource.Id.loading_spinner);
            FeedList.SetLayoutManager(new LinearLayoutManager(this));
            var follow = FindViewById<TextView>(Resource.Id.Title);
            follow.Clickable = true;

            follow.Click += (sender, e) => {
                StartActivity(typeof(SignInActivity));
            };

			FeedAdapter = new Adapter.FeedAdapter(this, ViewModel.Posts);
            FeedList.SetAdapter(FeedAdapter);


			FeedList.SetOnScrollChangeListener(this);
        }

		protected override void OnResume()
		{
			base.OnResume();
			FeedAdapter.NotifyDataSetChanged();
			ViewModel.Posts.CollectionChanged += Posts_CollectionChanged;
		}

		protected override void OnPause()
		{
			ViewModel.Posts.CollectionChanged -= Posts_CollectionChanged;
			base.OnPause();
		}

		void Posts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RunOnUiThread(() =>
			{
				if (Bar.Visibility == ViewStates.Visible)
					Bar.Visibility = ViewStates.Gone;
				FeedAdapter.NotifyDataSetChanged();
			});
		}

        int prevPos=0;
        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
           int pos = ((LinearLayoutManager)FeedList.GetLayoutManager()).FindLastCompletelyVisibleItemPosition();
            if (pos > prevPos && pos != prevPos)
            {
                if (pos == FeedList.GetAdapter().ItemCount - 1)
                {
                    if (pos < FeedAdapter.ItemCount)
                    {
						Task.Run(() =>ViewModel.GetTopPosts(FeedAdapter.GetItem(FeedAdapter.ItemCount - 1).Url, 20));
                        prevPos = pos;
                    }
                }
            }
        }
    }
}