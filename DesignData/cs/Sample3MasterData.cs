using MasterMemory;
using MessagePack;
using System.Collections.Generic;

namespace KC
{
	[MemoryTable("sample3"), MessagePackObject(true)]
    public partial class Sample3MasterData
    {
        [PrimaryKey] 
        public long Id { get; set; }
        public string Name { get; set; }
        public SampleGrades Grade { get; set; }
        public SampleType2s CharacterType { get; set; }
        public int Edition { get; set; }
        public float MoveSpeed { get; set; }
        public string SecondName { get; set; }
        public List<int> Numbers { get; set; }
        public List<string> Descriptions { get; set; }
    }
}