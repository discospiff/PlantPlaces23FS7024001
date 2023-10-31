﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using PlantPlacesSpecimens;
//
//    var specimen = Specimen.FromJson(jsonString);

namespace PlantPlacesSpecimens
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Specimen
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("plant_id")]
        public long PlantId { get; set; }

        [JsonProperty("specimen_id")]
        public long SpecimenId { get; set; }

        [JsonProperty("common")]
        public string Common { get; set; }

        [JsonProperty("genus")]
        public string Genus { get; set; }

        [JsonProperty("species")]
        public string Species { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }

    public enum Address { Empty, The3400VineStreetCincinnatiOh45220 };

    public partial class Specimen
    {
        public static List<Specimen> FromJson(string json) => JsonConvert.DeserializeObject<List<Specimen>>(json, PlantPlacesSpecimens.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<Specimen> self) => JsonConvert.SerializeObject(self, PlantPlacesSpecimens.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AddressConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AddressConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Address) || t == typeof(Address?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
                    return Address.The3400VineStreetCincinnatiOh45220;
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Address)untypedValue;
            switch (value)
            {
                case Address.Empty:
                    serializer.Serialize(writer, " ");
                    return;
                case Address.The3400VineStreetCincinnatiOh45220:
                    serializer.Serialize(writer, "3400 Vine Street Cincinnati OH 45220");
                    return;
            }
            throw new Exception("Cannot marshal type Address");
        }

        public static readonly AddressConverter Singleton = new AddressConverter();
    }
}
