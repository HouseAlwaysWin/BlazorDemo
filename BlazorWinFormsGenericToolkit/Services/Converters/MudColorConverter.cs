﻿using Microsoft.VisualBasic;
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BlazorWinFormsGenericToolKit.Services.Converters
{
    /// <summary>
    /// MudColor 反序列化轉換
    /// </summary>
    internal class MudColorConverter : System.Text.Json.Serialization.JsonConverter<MudColor>
    {
        public override MudColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string hexStr = string.Empty;
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var rootElement = document.RootElement;
                if (rootElement.TryGetProperty("Value", out JsonElement valueElement))
                {
                    hexStr = valueElement.GetString() ?? string.Empty;
                }
            }
            return new MudColor(hexStr);
        }

        public override void Write(Utf8JsonWriter writer, MudColor value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Value", value.Value);
            writer.WriteNumber("R", value.R);
            writer.WriteNumber("R", value.R);
            writer.WriteNumber("B", value.B);
            writer.WriteNumber("APercentage", value.APercentage);
            writer.WriteNumber("H", value.H);
            writer.WriteNumber("L", value.L);
            writer.WriteNumber("S", value.S);
            writer.WriteEndObject();
        }
    }
}
