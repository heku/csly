﻿using jsonparser;
using jsonparser.JsonModel;
using NFluent;
using sly.parser;
using sly.parser.generator;
using Xunit;

namespace ParserTests.samples
{
    public class JsonGenericTests
    {
        public JsonGenericTests()
        {
            var jsonParser = new EbnfJsonGenericParser();
            var builder = new ParserBuilder<JsonTokenGeneric, JSon>();
            Parser = builder.BuildParser(jsonParser, ParserType.EBNF_LL_RECURSIVE_DESCENT, "root").Result;
        }

        private static Parser<JsonTokenGeneric, JSon> Parser;

      

        [Fact]
        public void TestDoubleValue()
        {
            var r = Parser.Parse("0.1");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsValue);
            Assert.True(((JValue) r.Result).IsDouble);
            Assert.Equal(0.1d, ((JValue) r.Result).GetValue<double>());
        }


        [Fact]
        public void TestEmptyListValue()
        {
            var r = Parser.Parse("[]");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsList);
            Assert.Equal(0, ((JList) r.Result).Count);
        }

        [Fact]
        public void TestEmptyObjectValue()
        {
            var r = Parser.Parse("{}");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsObject);
            Assert.Equal(0, ((JObject) r.Result).Count);
        }

        [Fact]
        public void TestFalseBooleanValue()
        {
            var r = Parser.Parse("false");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsValue);
            var val = (JValue) r.Result;
            Assert.True(val.IsBool);
            Assert.False(val.GetValue<bool>());
        }

        [Fact]
        public void TestIntValue()
        {
            var r = Parser.Parse("1");
            Assert.False(r.IsError);
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsValue);
            Assert.True(((JValue) r.Result).IsInt);
            Assert.Equal(1, ((JValue) r.Result).GetValue<int>());
        }

        [Fact]
        public void TestManyListValue()
        {
            var r = Parser.Parse("[1,2]");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsList);
            var list = (JList) r.Result;
            Assert.Equal(2, list.Count);
            Check.That(list).HasItem(0, 1);
            Check.That(list).HasItem(1, 2);
        }

        [Fact]
        public void TestManyMixedListValue()
        {
            var r = Parser.Parse("[1,null,{},true,42.58]");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.NotNull(r.Result);
            object val = r.Result;
            Assert.True(r.Result.IsList);
            var list = (JList) r.Result;
            Assert.Equal(5, ((JList) r.Result).Count);
            Check.That(list).HasItem(0, 1);
            Assert.True(((JList) r.Result)[1].IsNull);
            Check.That(list).HasObjectItem(2,0);
            Check.That(list).HasItem(3, true);
            Check.That(list).HasItem(4, 42.58d);
        }

        [Fact]
        public void TestManyNestedPropertyObjectValue()
        {
            var json = "{\"p1\":\"v1\",\"p2\":\"v2\",\"p3\":{\"inner1\":1}}";

            var r = Parser.Parse(json);
            Assert.False(r.IsError);
            Assert.NotNull(r);
            Assert.True(r.Result.IsObject);
            var values = (JObject) r.Result;
            Assert.Equal(3, values.Count);
            Check.That(values).HasProperty("p1", "v1");
            Check.That(values).HasProperty("p1", "v1");
            Check.That(values).HasProperty("p2", "v2");

            Assert.True(values.ContainsKey("p3"));
            var inner = values["p3"];
            Assert.True(inner.IsObject);
            var innerObj = (JObject) inner;

            Assert.Equal(1, innerObj.Count);
            Check.That(innerObj).HasProperty("inner1", 1);
        }

        [Fact]
        public void TestManyPropertyObjectValue()
        {
            var json = "{\"p1\":\"v1\",\"p2\":\"v2\"}";
            json = "{\"p1\":\"v1\" , \"p2\":\"v2\" }";
            var r = Parser.Parse(json);
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsObject);
            var values = (JObject) r.Result;
            Assert.Equal(2, values.Count);
            Check.That(values).HasProperty("p1", "v1");
            Check.That(values).HasProperty("p2", "v2");
        }

        [Fact]
        public void TestNullValue()
        {
            var r = Parser.Parse("null");
            Assert.False(r.IsError);
            Assert.True(r.Result.IsNull);
        }

        [Fact]
        public void TestSingleListValue()
        {
            var r = Parser.Parse("[1]");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsList);
            var list = (JList) r.Result;
            Assert.Equal(1, list.Count);
            Check.That(list).HasItem(0, 1);
        }


        [Fact]
        public void TestSinglePropertyObjectValue()
        {
            var r = Parser.Parse("{\"prop\":\"value\"}");
            Check.That(r.IsError).IsFalse();
            Check.That(r.Result).IsNotNull();
            Check.That(r.Result.IsObject).IsTrue();
            var values = (JObject) r.Result;
            Check.That(values).CountIs(1);
            Check.That(values).HasProperty("prop", "value");
        }

        [Fact]
        public void TestStringValue()
        {
            var val = "hello";
            var r = Parser.Parse("\"" + val + "\"");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsValue);
            Assert.True(((JValue) r.Result).IsString);
            Assert.Equal(val, ((JValue) r.Result).GetValue<string>());
        }

        [Fact]
        public void TestTrueBooleanValue()
        {
            var r = Parser.Parse("true");
            Assert.False(r.IsError);
            Assert.NotNull(r.Result);
            Assert.True(r.Result.IsValue);
            var val = (JValue) r.Result;
            Assert.True(val.IsBool);
            Assert.True(val.IsBool);
            Assert.True(val.GetValue<bool>());
        }
    }
}