using System;
using System.Collections.Generic;
using System.Text;

namespace NaijaConnect.Core
{
    public class GroupMessage : Message
    {
        public string SenderUsername { get; set; }
        public string SenderId { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }


        public GroupMessage(string username, string senderId, string groupName, string groupId,  string text) : base(text)
        {
            SenderUsername = username;
            SenderId = senderId;
            GroupName = groupName;
            GroupId = groupId;
        }
    }
}
