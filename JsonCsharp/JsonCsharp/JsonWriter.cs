using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace org.vxwo.csharp.json
{
    public class JsonWriter
    {
        public static string Write(JsonValue obj)
        {
            return new JsonSerializer(true, false).ConvertToJSON(obj);
        }
    }

    internal class JsonSerializer
    {
        private readonly StringBuilder _output = new StringBuilder();
        readonly bool serializeNulls = true;
        readonly int _MAX_DEPTH = 10;
        bool _Indent = false;
        int _current_depth = 0;

        internal JsonSerializer(bool SerializeNulls, bool IndentOutput)
        {
            _Indent = IndentOutput;
            this.serializeNulls = SerializeNulls;
        }

        internal string ConvertToJSON(JsonValue obj)
        {
            WriteValue(obj);
            return _output.ToString();
        }

        private void WriteValue(JsonValue obj)
        {
            switch (obj.type)
            {
                case JsonType.None:
                case JsonType.Null:
                    _output.Append("null");
                    break;
                case JsonType.Boolean:
                    _output.Append(((bool)obj.store) ? "true" : "false");
                    break;
                case JsonType.Int:
                    _output.Append(Convert.ToSingle((int)obj.store));
                    break;
                case JsonType.Long:
                    _output.Append(Convert.ToSingle((long)obj.store));
                    break;
                case JsonType.Double:
                    _output.Append(Convert.ToSingle((double)obj.store));
                    break;
                case JsonType.String:
                    WriteString((string)obj.store);
                    break;
                case JsonType.Object:
                    WriteObject(obj);
                    break;
                case JsonType.Array:
                    WriteArray(obj);
                    break;
            }
        }

        private void WriteObject(JsonValue obj)
        {
            Indent();
            _current_depth++;
            if (_current_depth > _MAX_DEPTH)
                throw new Exception("Serializer encountered maximum depth of " + _MAX_DEPTH);
            _output.Append('{');

            bool append = false;
            foreach (KeyValuePair<string, JsonValue> kv in obj.store as Dictionary<string, JsonValue>)
            {
                if (append)
                    _output.Append(',');

                if (kv.Value.type == JsonType.None || (kv.Value.type == JsonType.Null && serializeNulls == false))
                    append = false;
                else
                {
                    WritePair(kv.Key, kv.Value);
                    append = true;
                }
            }

            _current_depth--;
            Indent();
            _output.Append('}');
            _current_depth--;
        }

        private void WriteArray(JsonValue obj)
        {
            Indent();
            _current_depth++;
            if (_current_depth > _MAX_DEPTH)
                throw new Exception("Serializer encountered maximum depth of " + _MAX_DEPTH);
            _output.Append('[');

            bool append = false;
            foreach (JsonValue v in obj.store as List<JsonValue>)
            {
                if (append)
                    _output.Append(',');

                if (v.type == JsonType.None || (v.type == JsonType.Null && serializeNulls == false))
                    append = false;
                else
                {
                    WriteValue(v);
                    append = true;
                }
            }

            _current_depth--;
            Indent();
            _output.Append(']');
            _current_depth--;
        }

        private void Indent()
        {
            Indent(false);
        }

        private void Indent(bool dec)
        {
            if (_Indent)
            {
                _output.Append("\r\n");
                for (int i = 0; i < _current_depth - (dec ? 1 : 0); i++)
                    _output.Append("\t");
            }
        }

        private void WritePairFast(string name, string value)
        {
            if ((value == null) && serializeNulls == false)
                return;
            Indent();
            WriteStringFast(name);

            _output.Append(':');

            WriteStringFast(value);
        }

        private void WritePair(string name, JsonValue value)
        {
            if ((value.type == JsonType.None || value.type == JsonType.Null) && serializeNulls == false)
                return;
            Indent();
            WriteStringFast(name);

            _output.Append(':');

            WriteValue(value);
        }

        private void WriteStringFast(string s)
        {
            _output.Append('\"');
            _output.Append(s);
            _output.Append('\"');
        }

        private void WriteString(string s)
        {
            _output.Append('\"');

            int runIndex = -1;

            for (var index = 0; index < s.Length; ++index)
            {
                var c = s[index];

                if (c >= ' ' && c < 128 && c != '\"' && c != '\\')
                {
                    if (runIndex == -1)
                    {
                        runIndex = index;
                    }

                    continue;
                }

                if (runIndex != -1)
                {
                    _output.Append(s, runIndex, index - runIndex);
                    runIndex = -1;
                }

                switch (c)
                {
                    case '\t': _output.Append("\\t"); break;
                    case '\r': _output.Append("\\r"); break;
                    case '\n': _output.Append("\\n"); break;
                    case '"':
                    case '\\': _output.Append('\\'); _output.Append(c); break;
                    default:
                        _output.Append("\\u");
                        _output.Append(((int)c).ToString("X4", NumberFormatInfo.InvariantInfo));
                        break;
                }
            }

            if (runIndex != -1)
            {
                _output.Append(s, runIndex, s.Length - runIndex);
            }

            _output.Append('\"');
        }
    }
}
