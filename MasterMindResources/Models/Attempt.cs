using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindResources.Models
{
	public class Attempt
	{
		public Attempt()
		{

		}

        public int AttemptId { get; set; }
        public AttemptType AttemptType { get; set; }
        public int GameId { get; set; }

        public string ValueOne { get; set; }
		public string ValueTwo { get; set; }
		public string ValueThree { get; set; }
		public string ValueFour { get; set; }
		public string Hints { get; set; }

        public List<string> ValuesList { get { return new List<string> { ValueOne, ValueTwo, ValueThree, ValueFour }; } }
    }
}
