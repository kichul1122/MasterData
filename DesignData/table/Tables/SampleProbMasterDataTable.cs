// <auto-generated />
#pragma warning disable CS0105
using KC;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace KC.Tables
{
   public sealed partial class SampleProbMasterDataTable : TableBase<SampleProbMasterData>, ITableUniqueValidate
   {
        public Func<SampleProbMasterData, long> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<SampleProbMasterData, long> primaryIndexSelector;


        public SampleProbMasterDataTable(SampleProbMasterData[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        public RangeView<SampleProbMasterData> FindById(long key)
        {
            return FindManyCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<long>.Default, key);
        }

        public RangeView<SampleProbMasterData> FindClosestById(long key, bool selectLower = true)
        {
            return FindManyClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<long>.Default, key, selectLower);
        }

        public RangeView<SampleProbMasterData> FindRangeById(long min, long max, bool ascendant = true)
        {
            return FindManyRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<long>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR


#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(SampleProbMasterData), typeof(SampleProbMasterDataTable), "sampleprob",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(SampleProbMasterData).GetProperty("Id")),
                    new MasterMemory.Meta.MetaProperty(typeof(SampleProbMasterData).GetProperty("ProbType")),
                    new MasterMemory.Meta.MetaProperty(typeof(SampleProbMasterData).GetProperty("Prob")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(SampleProbMasterData).GetProperty("Id"),
                    }, true, false, System.Collections.Generic.Comparer<long>.Default),
                });
        }

#endif
    }
}