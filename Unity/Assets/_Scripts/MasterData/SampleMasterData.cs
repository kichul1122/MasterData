using MasterMemory;
using MessagePack;
using System.Collections.Generic;

namespace KC
{
	[MemoryTable("sample"), MessagePackObject(true)]
    public partial class SampleMasterData
    {
        [PrimaryKey] 
        public long Id { get; set; }
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