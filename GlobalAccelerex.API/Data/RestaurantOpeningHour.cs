using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAccelerex.API.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RestaurantOpeningHour
    {
        [JsonProperty("monday")]
        public Day[] Monday { get; set; }

        [JsonProperty("tuesday")]
        public Day[] Tuesday { get; set; }

        [JsonProperty("wednesday")]
        public Day[] Wednesday { get; set; }

        [JsonProperty("thursday")]
        public Day[] Thursday { get; set; }

        [JsonProperty("friday")]
        public Day[] Friday { get; set; }

        [JsonProperty("saturday")]
        public Day[] Saturday { get; set; }

        [JsonProperty("sunday")]
        public Day[] Sunday { get; set; }
    }

    public partial class Day
    {
        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public enum TypeEnum { Close, Open };

    public partial class RestaurantOpeningHour
    {
        public static RestaurantOpeningHour FromJson(string json) => JsonConvert.DeserializeObject<RestaurantOpeningHour>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RestaurantOpeningHour self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "close":
                    return TypeEnum.Close;
                case "open":
                    return TypeEnum.Open;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Close:
                    serializer.Serialize(writer, "close");
                    return;
                case TypeEnum.Open:
                    serializer.Serialize(writer, "open");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
