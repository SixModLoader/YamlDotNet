//  This file is part of YamlDotNet - A .NET library for YAML.
//  Copyright (c) Antoine Aubry and contributors

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System.IO;
using BenchmarkDotNet.Attributes;
using YamlDotNet.PerformanceTests.Lib.Tests;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.PerformanceTests.vlatest
{
    [MemoryDiagnoser]
    public class ReceiptTest
    {
#if DEBUG
        [GlobalSetup]
        public void Debug()
        {
            System.Diagnostics.Debugger.Launch();
        }
#endif

        [IterationSetup]
        public void Setup()
        {
            _writer = new StringWriter();
            _reader = new StringReader(_yaml);
        }

        private readonly Receipt _receipt = TestData.Graph;
        private readonly string _yaml = @"
receipt: Oz-Ware Purchase Invoice
date: 2007-08-06T00:00:00.0000000
customer:
  given: Dorothy
  family: Gale
items:
- partNo: A4786
  descrip: Water Bucket (Filled)
  price: 1.47
  quantity: 4
- partNo: E1628
  descrip: High Heeled ""Ruby"" Slippers
  price: 100.27
  quantity: 1
billTo: &o0
  street: >-
    123 Tornado Alley
    Suite 16
  city: East Westville
  state: KS
shipTo: *o0
specialDelivery: >-
  Follow the Yellow Brick
  Road to the Emerald City.
  Pay no attention to the
  man behind the curtain.
";

        private StringWriter _writer;
        private StringReader _reader;

        private readonly ISerializer _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        private readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        //[Benchmark(Description = "Serialize vlatest")]
        //public void Serialize()
        //{
        //    _serializer.Serialize(_writer, _receipt);
        //}

        [Benchmark(Description = "Deserialize vlatest")]
        public void Deserialize()
        {
            _deserializer.Deserialize<Receipt>(_reader);
        }
    }
}