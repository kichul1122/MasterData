using MasterMemory;
using MessagePack;
using System.Collections.Generic;

namespace KC
{
	[MemoryTable("samplemulti"), MessagePackObject(true)]
    public partial class SampleMultiMasterData
    {
        [PrimaryKey, NonUnique] 
        public long Id { get; set; }
        public long SecondId { get; set; }
        public string Name { get; set; }
        public SampleGrades Grade { get; set; }
        public SampleTypes CharacterType { get; set; }
        public int Edition { get; set; }
        public float MoveSpeed { get; set; }
        public string SecondName { get; set; }
        public List<int> Numbers { get; set; }
        public List<string> Descriptions { get; set; }
    }
}