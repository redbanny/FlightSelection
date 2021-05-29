using System;
using System.Collections.Generic;
using System.Linq;

namespace Gridnine.FlightCodingTest
{
    public class FlightBuilder
    {
        private DateTime _threeDaysFromNow;

        public FlightBuilder()
        {
            _threeDaysFromNow = DateTime.Now.AddDays(3);
        }

        public IList<Flight> GetFlights()
        {
            return new List<Flight>
			           {
                           //A normal flight with two hour duration
			               CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2)),

                           //A normal multi segment flight
			               CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(3), _threeDaysFromNow.AddHours(5)),
                           
                           //A flight departing in the past
                           CreateFlight(_threeDaysFromNow.AddDays(-6), _threeDaysFromNow),

                           //A flight that departs before it arrives
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(-6)),

                           //A flight with more than two hours ground time
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(5), _threeDaysFromNow.AddHours(6)),

                            //Another flight with more than two hours ground time
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(3), _threeDaysFromNow.AddHours(4), _threeDaysFromNow.AddHours(6), _threeDaysFromNow.AddHours(7))
			           };
        }

        private static Flight CreateFlight(params DateTime[] dates)
        {
            if (dates.Length % 2 != 0) throw new ArgumentException("You must pass an even number of dates,", "dates");

            var departureDates = dates.Where((date, index) => index % 2 == 0);
            var arrivalDates = dates.Where((date, index) => index % 2 == 1);

            var segments = departureDates.Zip(arrivalDates,
                                              (departureDate, arrivalDate) =>
                                              new Segment { DepartureDate = departureDate, ArrivalDate = arrivalDate }).ToList();

            return new Flight { Segments = segments };
        }
    }

    public class Flight
    {
        public IList<Segment> Segments { get; set; }
    }

    public class Segment
    {
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
    }

    public class FlightSelect
    {
        private IList<Flight> listik;
        private ISelection delegat;

        public IList<Flight> Listik { get => listik; set => listik = value; }
        public ISelection Delegat { get => delegat; set => delegat = value; }

        public FlightSelect(IList<Flight> listik, ISelection delegat)
        {
            Listik = listik;
            Delegat = delegat;
        }

        public List<Flight> MakeSelection()
        {
            return Listik.Where(f => Delegat.Compare(f)).ToList<Flight>();
        }
    }
    public interface ISelection
    {
        bool Compare(Flight flight);
    }

    public class FirstSelect : ISelection
    {
        public bool Compare(Flight flight)
        {
            int i = 0;
            while (i < flight.Segments.Count)
            {
                if (!(flight.Segments[i].DepartureDate >= DateTime.Now))
                {
                    return false;
                }
                i++;
            }
            return true;
        }
    }

    public class SecondSelect : ISelection
    {
        public bool Compare(Flight flight)
        {
            int i = 0;
            while (i < flight.Segments.Count)
            {
                if (!(flight.Segments[i].ArrivalDate >= flight.Segments[i].DepartureDate))
                {
                    return false;
                }
                i++;
            }
            return true;
        }
    }

    public class ThirdSelect : ISelection
    {
        public bool Compare(Flight flight)
        {
            double airTime = 0, groundTime = 0;
            for (int j = 1; j < flight.Segments.Count; j++)
            {
                airTime += flight.Segments[j - 1].ArrivalDate.Subtract(flight.Segments[j - 1].DepartureDate).TotalHours;
                groundTime += flight.Segments[j].DepartureDate.Subtract(flight.Segments[j - 1].ArrivalDate).TotalHours;
            }
            return  (airTime - groundTime) < 2;
        }
    }
}

