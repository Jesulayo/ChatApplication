using Microsoft.AspNetCore.SignalR.Client;
using NaijaConnect.Core;
using NaijaConnect.Models;
using NaijaConnect.Models.Model;
using NaijaConnect.Utils;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NaijaConnect.ViewModels
{
    public class GroupChatListPageViewModel : ViewModelBase
    {
        private DelegateCommand GroupChatSelectionCommand;
        private DelegateCommand NewGroupChatPageCommand;
        private DelegateCommand ProfileCommand;


        public DelegateCommand NavigateGroupChatSelectionCommand => GroupChatSelectionCommand ?? (GroupChatSelectionCommand = new DelegateCommand(ExecuteNavigateToGroupChatSelectionCommand));
        public DelegateCommand NavigateNewGroupChatPageCommand => NewGroupChatPageCommand ?? (NewGroupChatPageCommand = new DelegateCommand(ExecuteNavigateToNewGroupChatPageCommand));
        public DelegateCommand NavigateProfileCommand => ProfileCommand ?? (ProfileCommand = new DelegateCommand(ExecuteNavigateToProfileCommand));





        public ObservableCollection<Group> MyGroupChat { get => GetGroupChats(); }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                SetProperty(ref username, value);
            }
        }

        private string avatar;
        public string Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                SetProperty(ref avatar, value);
            }
        }

        private Group groupChatSelection;

        public Group GroupChatSelection
        {
            get
            {
                return groupChatSelection;
            }
            set
            {
                SetProperty(ref groupChatSelection, value);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                SetProperty(ref isBusy, value);
            }
        }

        public GroupChatListPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Username = Settings.UsernameSettings;
            Avatar = Settings.AvatarSettings;
        }

        private ObservableCollection<Group> GetGroupChats()
        {
            
            var groupList = new ObservableCollection<Group>();

            groupList.Add(new Group
            {
                Id = "xamarin",
                Name = "Xamarin Group",
                Avatar = "group_chat_picture.png"
            });
            groupList.Add(new Group
            {
                Id = "asp",
                Name = "ASP.NET",
                Avatar = "group_chat_picture.png"
                
            });
            groupList.Add(new Group
            {
                Id = "c#",
                Name = "C#",
                Avatar = "group_chat_picture.png"
            });

            return groupList;
        }

        private async void ExecuteNavigateToGroupChatSelectionCommand()
        {
            if (GroupChatSelection != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("GroupList", GroupChatSelection);
                await NavigationService.NavigateAsync("GroupChatPage", parameters);
                GroupChatSelection = null;
                
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            var result = await App.client.GetStringAsync("https://naijaconnectfunction.azurewebsites.net/api/GetInfo");

            var info = JsonConvert.DeserializeObject<ConnectionInfo>(result);

            var connectionBuilder = new HubConnectionBuilder();
            connectionBuilder.WithUrl(info.Url, (Microsoft.AspNetCore.Http.Connections.Client.HttpConnectionOptions obj) =>
            {
                obj.AccessTokenProvider = () => Task.Run(() => info.AccessToken);
            });

            App.hub = connectionBuilder.Build();
            

        }

        

        private void ExecuteNavigateToNewGroupChatPageCommand()
        {
            NavigationService.NavigateAsync("NewGroupChatPage");

        }

        private void ExecuteNavigateToProfileCommand()
        {
            NavigationService.NavigateAsync("ProfilePage");
        }
    }
}
