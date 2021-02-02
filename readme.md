## Resuturant Hour Service

API that takes JSON-formatted opening hours of a restaurant as an input and outputs hours in human readable format.

The JSON-format data is structrued into C# class, convert property using Newtonsoft.JSON library and annotate the property conrresponding to the json field
 ```csharp
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
 ```

```csharp
    public partial class Day
    {
        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }
```

```csharp
    public enum TypeEnum { Close, Open };
```

```csharp
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
```

#### Tell us what do you think about the data format. Is the current JSON structure the best  way to store that kind of data or can you come up with a better version? There are no right answers here

The data structure is not in the best formart, Kindly check format below

Data Sturcture Explaination

```
{
    "openingHours": [
        {
            "day": 0,
            "data": [
                {
                    "state": 0,
                    "time": 3600
                },
                {
                    "state": 1,
                    "time": 3600
                }
            ]
        },
        {
            "day": 1,
            "data": [
                {
                    "state": 1,
                    "time": 3600
                },
                {
                    "state": 2,
                    "time": 3600
                }
            ]
        }
    ]
}
```

Day is an enum
```
Monday - 0
Tuesday - 0
Wednesday - 0
```

State Type is an enum
```
Close - 0
Open - 1
```
