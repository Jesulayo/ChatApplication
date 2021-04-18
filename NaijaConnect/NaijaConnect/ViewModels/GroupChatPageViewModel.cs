using NaijaConnect.Core;
using NaijaConnect.Models;
using Microsoft.AspNetCore.SignalR.Client;

using NaijaConnect.Models.Model;
using NaijaConnect.Services;
using NaijaConnect.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NaijaConnect.ViewModels
{
    public class GroupChatPageViewModel : ViewModelBase
    {
        private DelegateCommand PreviousPageCommand;
        private DelegateCommand SendCommand;
        
        public DelegateCommand NavigateToPreviousPageCommand => PreviousPageCommand ?? (PreviousPageCommand = new DelegateCommand(PreviousPage));
        public DelegateCommand NavigateToSendCommand => SendCommand ?? (SendCommand = new DelegateCommand(Send));

        
        private Group GroupDetails;
        
        private string groupName;
        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                SetProperty(ref groupName, value);
            }
        }

        private string groupAvatar;
        public string GroupAvatar
        {
            get
            {
                return groupAvatar;
            }
            set
            {
                SetProperty(ref groupAvatar, value);
            }
        }

        private string groupId;
        public string GroupId
        {
            get
            {
                return groupId;
            }
            set
            {
                SetProperty(ref groupId, value);
            }
        }

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                SetProperty(ref message, value);
            }
        }

        
        public ObservableCollection<GroupMessage> Messages { get; private set; } = new ObservableCollection<GroupMessage>();


        public GroupChatPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            
            
        }
        private async void Send()
        {
            if (string.IsNullOrEmpty(Message) || string.IsNullOrWhiteSpace(Message))
            {
                Message = null;
                return;
            }
            if(App.hub.State == HubConnectionState.Disconnected)
            {
                var results = await App.client.GetStringAsync("https://naijaconnectfunction.azurewebsites.net/api/GetInfo");

                var info = JsonConvert.DeserializeObject<ConnectionInfo>(results);

                var connectionBuilder = new HubConnectionBuilder();
                connectionBuilder.WithUrl(info.Url, (Microsoft.AspNetCore.Http.Connections.Client.HttpConnectionOptions obj) =>
                {
                    obj.AccessTokenProvider = () => Task.Run(() => info.AccessToken);
                });

                App.hub = connectionBuilder.Build();
            }

            var comment = new GroupMessage(Settings.UsernameSettings, Settings.KeySettings, GroupName, GroupId, Message.Trim());
            var json = JsonConvert.SerializeObject(comment);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await App.client.PostAsync("https://naijaconnectfunction.azurewebsites.net/api/SendGroupMessage", content);

            Message = string.Empty;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GroupList"))
            {
                GroupDetails = parameters.GetValue<Group>("GroupList");
                GroupName = GroupDetails.Name;
                GroupAvatar = GroupDetails.Avatar;
                GroupId = GroupDetails.Id;

                App.hub.On<object>(nameof(GroupMessage), (message) =>
                {
                    var json = message.ToString();
                    var obj = JsonConvert.DeserializeObject<GroupMessage>(json);
                    if (obj.GroupId == GroupId)
                    {
                        Messages.Add(obj);

                    }

                });

                await App.hub.StartAsync();

            }
            
        }

        private async void PreviousPage()
        {
            await NavigationService.GoBackAsync();
        }

    }
}