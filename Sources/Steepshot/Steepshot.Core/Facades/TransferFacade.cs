﻿using System;
using System.Threading.Tasks;
using Steepshot.Core.Clients;
using Steepshot.Core.Models.Common;
using Steepshot.Core.Models.Responses;
using Steepshot.Core.Presenters;

namespace Steepshot.Core.Facades
{
    public sealed class TransferFacade
    {
        public UserFriendPresenter UserFriendPresenter { get; }
        public TransferPresenter TransferPresenter { get; }
        public Action OnUserBalanceChanged;
        public Action OnRecipientChanged;

        private BalanceModel _userBalance;
        public BalanceModel UserBalance
        {
            get => _userBalance;
            set
            {
                _userBalance = value;
                OnUserBalanceChanged?.Invoke();
            }
        }

        private UserFriend _recipient;
        public UserFriend Recipient
        {
            get => _recipient;
            set
            {
                _recipient = value;
                OnRecipientChanged?.Invoke();
            }
        }

        public TransferFacade()
        {
            UserFriendPresenter = new UserFriendPresenter();
            TransferPresenter = new TransferPresenter();
        }

        public void SetClient(SteepshotApiClient client)
        {
            UserFriendPresenter.SetClient(client);
            TransferPresenter.SetClient(client);
        }



        public async Task<Exception> TryLoadNextSearchUserAsync(string query) => await UserFriendPresenter.TryLoadNextSearchUserAsync(query).ConfigureAwait(false);
        public async Task<OperationResult<AccountInfoResponse>> TryGetAccountInfoAsync(string login)
        {
            return await TransferPresenter.TryGetAccountInfoAsync(login).ConfigureAwait(false);
        }

        public void TasksCancel()
        {
            TransferPresenter.TasksCancel();
            UserFriendPresenter.TasksCancel();
        }
    }
}
