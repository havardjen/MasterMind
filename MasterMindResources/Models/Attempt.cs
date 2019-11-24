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

		public string ValueOne { get; set; }
		public string ValueTwo { get; set; }
		public string ValueThree { get; set; }
		public string ValueFour { get; set; }
		public string Hints { get; set; }
	}
}
