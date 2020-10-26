using System;
using System.Collections.Generic;
using System.Text;

namespace HBMC.Domain.Api.Models
{
    public class Schedule
    {
		public string Id { get; set; }
		public DateTime ArrivalTime { get; set; }
		public DateTime DepartTime { get; set; }
		public string PortId { get; set; }
		public string BoatId { get; set; }
		public string ShipId { get; set; }
	}
}
