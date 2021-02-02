using GlobalAccelerex.API.Data;
using GlobalAccelerex.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccelerex.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRestaurantOpeningHourService
    {
        Response<List<string>> ProcessRestaurantOpeningHours(RestaurantOpeningHour data);
    }

    /// <summary>
    /// 
    /// </summary>
    public class RestaurantOpeningHourService : IRestaurantOpeningHourService
    {
        public static string CLOSED_SUFFIX = ": Closed";
        public static string NO_DATA_SUFFIX = ": No data provided";
        public static string MALFORMED_DATA = "Malformed data";
        public static string COLON = ": ";

        /// <summary>
        /// 
        /// </summary>
        public RestaurantOpeningHourService()
        {

        }

        /// <summary>
        /// Process restaur
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Response<List<string>> ProcessRestaurantOpeningHours(RestaurantOpeningHour data)
        {
            var response = Response<List<string>>.Failed(string.Empty);
            try
            {
                List<string> responseString = new List<string>();

                responseString.Add(ConvertRawOpeningHourToResponse(nameof(data.Monday), data.Monday));
                responseString.Add(ConvertRawOpeningHourToResponse(nameof(data.Tuesday), data.Tuesday));
                responseString.Add(ConvertRawOpeningHourToResponse(nameof(data.Wednesday), data.Wednesday));
                responseString.Add(ConvertRawOpeningHourToResponse(nameof(data.Thursday), data.Thursday));
                responseString.Add(ConvertRawOpeningHourToResponse(nameof(data.Friday), data.Friday));
                responseString.Add(ConvertRawOpeningHourToResponse(nameof(data.Saturday), data.Saturday));
                responseString.Add(ConvertRawOpeningHourToResponse(nameof(data.Sunday), data.Sunday));

                response = Response<List<string>>.Success(responseString);
            }
            catch (Exception ex)
            {
                response = Response<List<string>>.Failed("Failed to process restaurant data");
            }

            return response;
        }

        private static string ConvertRawOpeningHourToResponse(string day, Day[] times)
        {
            if (times == null)
            {
                return day + NO_DATA_SUFFIX;
            }
            else if (times.Length == 0)
            {
                return day + CLOSED_SUFFIX;
            }

            TypeEnum currentType = TypeEnum.Open;
            StringBuilder sb = new StringBuilder($"{day} {COLON}");
            for (int i = 0; i < times.Length; i++)
            {
                Day time = times[i];
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time.Value).UtcDateTime;
                if (i > 0 && time.Type == TypeEnum.Open)
                {
                    sb.Append(", ");
                }
                if (time.Type == TypeEnum.Close)
                {
                    sb.Append(" - ");
                }

                sb.Append($"{dateTimeOffset: hh:mm tt}");
            }

            return sb.ToString();
        }
    }

    
}
