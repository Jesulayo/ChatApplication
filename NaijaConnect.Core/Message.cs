using System;
using System.Collections.Generic;
using System.Text;

namespace NaijaConnect.Core
{
    public class Message
    {
        public string TypeInfo { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public Message() { }
        public Message(string text)
        {
            TypeInfo = GetType().Name;
            Text = text;
            Timestamp = DateTimeOffset.Now;
        }
    }
}
