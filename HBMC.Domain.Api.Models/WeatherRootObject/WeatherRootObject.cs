using System;
using System.Collections.Generic;
using System.Text;

namespace HBMC.Domain.Api.Models
{
	public class WeatherRootObject
	{
		public string Id { get; set; }
		public Wind Wind { get; set; }
		public List<Weather> Weather {get;set;}
		public string Name { get; set; }
		public int Cos { get; set; }
		public string DT { get; set; }
		public string @base { get; set;}
		public Coord Coord { get; set; }
		public Main Main { get; set; }
		public Clouds Clouds { get; set; }
		public Sys Sys { get; set; }
		public int Visibility { get; set; }
		public int TimeZone { get; set; }
	}

		
}
