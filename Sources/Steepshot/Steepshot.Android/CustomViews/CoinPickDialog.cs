﻿using System;
using System.Collections;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Steepshot.Core.Localization;
using Steepshot.Core.Models.Requests;
using Steepshot.Core.Utils;
using Steepshot.Utils;

namespace Steepshot.CustomViews
{
    public class CoinPickDialog : BottomSheetDialog
    {
        public Action<CurrencyType> CoinSelected;
        private readonly IList _displayCoins;
        private readonly List<CurrencyType> _coins;
        private WheelPicker _wheelPicker;
        private int _selectedPosition;

        private CoinPickDialog(Context context) : base(context) { }

        public CoinPickDialog(Context context, List<CurrencyType> data) : this(context)
        {
            _displayCoins = new List<string>();
            data.ForEach(x => _displayCoins.Add(x.ToString().ToUpper()));
            _coins = data;
        }

        public void Show(int selected)
        {
            _selectedPosition = selected;
            Show();
        }

        public override void Show()
        {
            using (var dialogView = LayoutInflater.From(Context).Inflate(Resource.Layout.lyt_coin_pick, null))
            {
                dialogView.SetMinimumWidth((int)(Style.ScreenWidth * 0.8));

                var dialogTitle = dialogView.FindViewById<TextView>(Resource.Id.dialog_title);
                dialogTitle.Typeface = Style.Semibold;
                dialogTitle.Text = AppSettings.LocalizationManager.GetText(LocalizationKeys.SelectToken);

                _wheelPicker = dialogView.FindViewById<WheelPicker>(Resource.Id.coin_picker);
                _wheelPicker.Items = _displayCoins;
                _wheelPicker.ItemSelected += ItemSelected;
                _wheelPicker.Select(_selectedPosition);

                var selectBtn = dialogView.FindViewById<Button>(Resource.Id.select_btn);
                var cancelBtn = dialogView.FindViewById<Button>(Resource.Id.cacncel_btn);

                selectBtn.Text = AppSettings.LocalizationManager.GetText(LocalizationKeys.Select);
                cancelBtn.Text = AppSettings.LocalizationManager.GetText(LocalizationKeys.Cancel);

                selectBtn.Click += SelectBtnOnClick;
                cancelBtn.Click += CancelBtnOnClick;

                SetContentView(dialogView);
                Window.FindViewById(Resource.Id.design_bottom_sheet).SetBackgroundColor(Color.Transparent);
                var dialogPadding = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 10, Context.Resources.DisplayMetrics);
                Window.DecorView.SetPadding(dialogPadding, dialogPadding, dialogPadding, dialogPadding);
                base.Show();

                var bottomSheet = FindViewById<FrameLayout>(Resource.Id.design_bottom_sheet);
                var behavior = BottomSheetBehavior.From(bottomSheet);
                behavior.State = BottomSheetBehavior.StateExpanded;
                behavior.SetBottomSheetCallback(new CustomBottomSheetCallback());
            }
        }

        private void ItemSelected(int pos)
        {
            _selectedPosition = pos;
        }

        private void CancelBtnOnClick(object sender, EventArgs e)
        {
            _wheelPicker.ItemSelected -= ItemSelected;
            Cancel();
        }

        private void SelectBtnOnClick(object sender, EventArgs e)
        {
            CoinSelected?.Invoke(_coins[_selectedPosition]);
            CancelBtnOnClick(null, null);
        }
    }
}