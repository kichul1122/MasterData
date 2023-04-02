// <auto-generated />
#pragma warning disable CS0105
using KC;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using KC.Tables;

namespace KC
{
    public class MasterMemoryResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new MasterMemoryResolver();

        MasterMemoryResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = MasterMemoryResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class MasterMemoryResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static MasterMemoryResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(5)
            {
                {typeof(Sample2MasterData[]), 0 },
                {typeof(Sample3MasterData[]), 1 },
                {typeof(SampleMasterData[]), 2 },
                {typeof(SampleMultiMasterData[]), 3 },
                {typeof(SampleProbMasterData[]), 4 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.ArrayFormatter<Sample2MasterData>();
                case 1: return new MessagePack.Formatters.ArrayFormatter<Sample3MasterData>();
                case 2: return new MessagePack.Formatters.ArrayFormatter<SampleMasterData>();
                case 3: return new MessagePack.Formatters.ArrayFormatter<SampleMultiMasterData>();
                case 4: return new MessagePack.Formatters.ArrayFormatter<SampleProbMasterData>();
                default: return null;
            }
        }
    }
}