using System;
using System.Collections.Generic;
using System.Text;

namespace HBMC.Domain.Api.Models
{
	public class WeatherRootObject
	{
		public int id { get; set; }
		public Wind wind { get; set; }
		public List<Weather> weather { get;set;}
		public string name { get; set; }
		public int cod { get; set; }
		public int dt { get; set; }
		public string @base { get; set;}
		public Coord coord { get; set; }
		public Main main { get; set; }
		public Clouds clouds { get; set; }
		public Sys sys { get; set; }
		public int visibility { get; set; }
		public int timezone { get; set; }
	}

		
}
