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
   public sealed partial class Sample3MasterDataTable : TableBase<Sample3MasterData>, ITableUniqueValidate
   {
        public Func<Sample3MasterData, long> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Sample3MasterData, long> primaryIndexSelector;


        public Sample3MasterDataTable(Sample3MasterData[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Sample3MasterData FindById(long key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindById(long key, out Sample3MasterData result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public Sample3MasterData FindClosestById(long key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<long>.Default, key, selectLower);
        }

        public RangeView<Sample3MasterData> FindRangeById(long min, long max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<long>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "Id", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(Sample3MasterData), typeof(Sample3MasterDataTable), "sample3",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("Id")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("Name")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("Grade")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("CharacterType")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("Edition")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("MoveSpeed")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("SecondName")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("Numbers")),
                    new MasterMemory.Meta.MetaProperty(typeof(Sample3MasterData).GetProperty("Descriptions")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample3MasterData).GetProperty("Id"),
                    }, true, true, System.Collections.Generic.Comparer<long>.Default),
                });
        }

#endif
    }
}