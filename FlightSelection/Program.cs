using Gridnine.FlightCodingTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSelection
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Segment> listForSelection = new List<Segment>();
            FlightBuilder flightBuilder = new FlightBuilder();
            var flightList = flightBuilder.GetFlights();
            int i = 0;
            while (i < flightList.Count)
            {
                int j = 0;
                while (j < flightList[i].Segments.Count)
                {
                    listForSelection.Add(flightList[i].Segments[j]);
                    j++;
                }
                i++;
            }
            FirstSelect firstSelect = new FirstSelect();
            FlightSelect flightSelect = new FlightSelect(flightList, firstSelect);
            var result = flightSelect.MakeSelection();
            Console.WriteLine("1. Исключаем вылеты до текущего момента времени");
            i = 0;
            while (i < result.Count)
            {
                int j = 0;
                while (j < result[i].Segments.Count)
                {
                    Console.WriteLine($"{result[i].Segments[j].DepartureDate} => {result[i].Segments[j].ArrivalDate}");
                    j++;
                }                
                i++;
            }

            Console.WriteLine("2. Исключаем сегменты с датой прилёта раньше даты вылета");
            i = 0;
            SecondSelect secondSelect = new SecondSelect();
            flightSelect = new FlightSelect(flightList, secondSelect);
            result = flightSelect.MakeSelection();
            while (i < result.Count)
            {
                int j = 0;
                while (j < result[i].Segments.Count)
                {
                    Console.WriteLine($"{result[i].Segments[j].DepartureDate} => {result[i].Segments[j].ArrivalDate}");
                    j++;
                }                
                i++;
            }
            Console.WriteLine("3. Исключаем полеты с проведенным на земле временем более 2 часов");
            i = 0;
            ThirdSelect thirdSelect = new ThirdSelect();
            flightSelect = new FlightSelect(flightList, thirdSelect);
            result = flightSelect.MakeSelection();
            while (i < result.Count)
            {
                int j = 0;
                while (j < result[i].Segments.Count)
                {
                    Console.WriteLine($"{result[i].Segments[j].DepartureDate} => {result[i].Segments[j].ArrivalDate}");
                    j++;
                }
                i++;
            }
        }
    }
}
