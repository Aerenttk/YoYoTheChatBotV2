using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoYoBot
{
    public class Request
    {
        public string UserName { get; set; }
        public int RequestType { get; set; }
        public string RequestArg1 { get; set; }
        public string RequestArg2 { get; set; }
        public string RequestArg3 { get; set; }
        public string RequestArg4 { get; set; }
        public string RequestArg5 { get; set; }

    }
}
