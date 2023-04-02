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
   public sealed class MemoryDatabase : MemoryDatabaseBase
   {
        public Sample2MasterDataTable Sample2MasterDataTable { get; private set; }
        public Sample3MasterDataTable Sample3MasterDataTable { get; private set; }
        public SampleMasterDataTable SampleMasterDataTable { get; private set; }
        public SampleMultiMasterDataTable SampleMultiMasterDataTable { get; private set; }
        public SampleProbMasterDataTable SampleProbMasterDataTable { get; private set; }

        public MemoryDatabase(
            Sample2MasterDataTable Sample2MasterDataTable,
            Sample3MasterDataTable Sample3MasterDataTable,
            SampleMasterDataTable SampleMasterDataTable,
            SampleMultiMasterDataTable SampleMultiMasterDataTable,
            SampleProbMasterDataTable SampleProbMasterDataTable
        )
        {
            this.Sample2MasterDataTable = Sample2MasterDataTable;
            this.Sample3MasterDataTable = Sample3MasterDataTable;
            this.SampleMasterDataTable = SampleMasterDataTable;
            this.SampleMultiMasterDataTable = SampleMultiMasterDataTable;
            this.SampleProbMasterDataTable = SampleProbMasterDataTable;
        }

        public MemoryDatabase(byte[] databaseBinary, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1)
            : base(databaseBinary, internString, formatterResolver, maxDegreeOfParallelism)
        {
        }

        protected override void Init(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            if(maxDegreeOfParallelism == 1)
            {
                InitSequential(header, databaseBinary, options, maxDegreeOfParallelism);
            }
            else
            {
                InitParallel(header, databaseBinary, options, maxDegreeOfParallelism);
            }
        }

        void InitSequential(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            this.Sample2MasterDataTable = ExtractTableData<Sample2MasterData, Sample2MasterDataTable>(header, databaseBinary, options, xs => new Sample2MasterDataTable(xs));
            this.Sample3MasterDataTable = ExtractTableData<Sample3MasterData, Sample3MasterDataTable>(header, databaseBinary, options, xs => new Sample3MasterDataTable(xs));
            this.SampleMasterDataTable = ExtractTableData<SampleMasterData, SampleMasterDataTable>(header, databaseBinary, options, xs => new SampleMasterDataTable(xs));
            this.SampleMultiMasterDataTable = ExtractTableData<SampleMultiMasterData, SampleMultiMasterDataTable>(header, databaseBinary, options, xs => new SampleMultiMasterDataTable(xs));
            this.SampleProbMasterDataTable = ExtractTableData<SampleProbMasterData, SampleProbMasterDataTable>(header, databaseBinary, options, xs => new SampleProbMasterDataTable(xs));
        }

        void InitParallel(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            var extracts = new Action[]
            {
                () => this.Sample2MasterDataTable = ExtractTableData<Sample2MasterData, Sample2MasterDataTable>(header, databaseBinary, options, xs => new Sample2MasterDataTable(xs)),
                () => this.Sample3MasterDataTable = ExtractTableData<Sample3MasterData, Sample3MasterDataTable>(header, databaseBinary, options, xs => new Sample3MasterDataTable(xs)),
                () => this.SampleMasterDataTable = ExtractTableData<SampleMasterData, SampleMasterDataTable>(header, databaseBinary, options, xs => new SampleMasterDataTable(xs)),
                () => this.SampleMultiMasterDataTable = ExtractTableData<SampleMultiMasterData, SampleMultiMasterDataTable>(header, databaseBinary, options, xs => new SampleMultiMasterDataTable(xs)),
                () => this.SampleProbMasterDataTable = ExtractTableData<SampleProbMasterData, SampleProbMasterDataTable>(header, databaseBinary, options, xs => new SampleProbMasterDataTable(xs)),
            };
            
            System.Threading.Tasks.Parallel.Invoke(new System.Threading.Tasks.ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            }, extracts);
        }

        public ImmutableBuilder ToImmutableBuilder()
        {
            return new ImmutableBuilder(this);
        }

        public DatabaseBuilder ToDatabaseBuilder()
        {
            var builder = new DatabaseBuilder();
            builder.Append(this.Sample2MasterDataTable.GetRawDataUnsafe());
            builder.Append(this.Sample3MasterDataTable.GetRawDataUnsafe());
            builder.Append(this.SampleMasterDataTable.GetRawDataUnsafe());
            builder.Append(this.SampleMultiMasterDataTable.GetRawDataUnsafe());
            builder.Append(this.SampleProbMasterDataTable.GetRawDataUnsafe());
            return builder;
        }

        public DatabaseBuilder ToDatabaseBuilder(MessagePack.IFormatterResolver resolver)
        {
            var builder = new DatabaseBuilder(resolver);
            builder.Append(this.Sample2MasterDataTable.GetRawDataUnsafe());
            builder.Append(this.Sample3MasterDataTable.GetRawDataUnsafe());
            builder.Append(this.SampleMasterDataTable.GetRawDataUnsafe());
            builder.Append(this.SampleMultiMasterDataTable.GetRawDataUnsafe());
            builder.Append(this.SampleProbMasterDataTable.GetRawDataUnsafe());
            return builder;
        }

#if !DISABLE_MASTERMEMORY_VALIDATOR

        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            var database = new ValidationDatabase(new object[]
            {
                Sample2MasterDataTable,
                Sample3MasterDataTable,
                SampleMasterDataTable,
                SampleMultiMasterDataTable,
                SampleProbMasterDataTable,
            });

            ((ITableUniqueValidate)Sample2MasterDataTable).ValidateUnique(result);
            ValidateTable(Sample2MasterDataTable.All, database, "Id", Sample2MasterDataTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)Sample3MasterDataTable).ValidateUnique(result);
            ValidateTable(Sample3MasterDataTable.All, database, "Id", Sample3MasterDataTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SampleMasterDataTable).ValidateUnique(result);
            ValidateTable(SampleMasterDataTable.All, database, "Id", SampleMasterDataTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SampleMultiMasterDataTable).ValidateUnique(result);
            ValidateTable(SampleMultiMasterDataTable.All, database, "Id", SampleMultiMasterDataTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SampleProbMasterDataTable).ValidateUnique(result);
            ValidateTable(SampleProbMasterDataTable.All, database, "Id", SampleProbMasterDataTable.PrimaryKeySelector, result);

            return result;
        }

#endif

        static MasterMemory.Meta.MetaDatabase metaTable;

        public static object GetTable(MemoryDatabase db, string tableName)
        {
            switch (tableName)
            {
                case "sample2":
                    return db.Sample2MasterDataTable;
                case "sample3":
                    return db.Sample3MasterDataTable;
                case "sample":
                    return db.SampleMasterDataTable;
                case "samplemulti":
                    return db.SampleMultiMasterDataTable;
                case "sampleprob":
                    return db.SampleProbMasterDataTable;
                
                default:
                    return null;
            }
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaDatabase GetMetaDatabase()
        {
            if (metaTable != null) return metaTable;

            var dict = new Dictionary<string, MasterMemory.Meta.MetaTable>();
            dict.Add("sample2", KC.Tables.Sample2MasterDataTable.CreateMetaTable());
            dict.Add("sample3", KC.Tables.Sample3MasterDataTable.CreateMetaTable());
            dict.Add("sample", KC.Tables.SampleMasterDataTable.CreateMetaTable());
            dict.Add("samplemulti", KC.Tables.SampleMultiMasterDataTable.CreateMetaTable());
            dict.Add("sampleprob", KC.Tables.SampleProbMasterDataTable.CreateMetaTable());

            metaTable = new MasterMemory.Meta.MetaDatabase(dict);
            return metaTable;
        }

#endif
    }
}