using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTestApp
{
    public class Profile
    {
        public string GameAddress { get; set; }
        public int GamePort { get; set; }
        public string GameName { get; set; }
        public string GameDisplay => $"{GameAddress}:{GamePort}";
        public string CustomDescription { get; set; }
        public DateTime LastPlayed { get; set; }
    }
}
