
using System.Collections.Generic;

namespace MasterMindResources.Models
{
    public class AttemptRequest
    {
        public int GameId { get; set; }
        public List<string> Attempt { get; set; }
    }
}
