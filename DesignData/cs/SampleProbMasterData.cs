using MasterMemory;
using MessagePack;
using System.Collections.Generic;

namespace KC
{
	[MemoryTable("sampleprob"), MessagePackObject(true)]
    public partial class SampleProbMasterData
    {
        [PrimaryKey, NonUnique] 
        public long Id { get; set; }
        public int ProbType { get; set; }
        public int Prob { get; set; }
    }
}