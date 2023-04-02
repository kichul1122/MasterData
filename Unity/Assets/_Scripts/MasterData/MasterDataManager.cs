
using Cysharp.Threading.Tasks;
using KC.Tables;
using System.Collections.Generic;
using System.Threading;

namespace KC
{
    public partial class MasterDataManager
    {
		public SampleMasterDataTable Sample => DB.SampleMasterDataTable;
		public Sample2MasterDataTable Sample2 => DB.Sample2MasterDataTable;
		public Sample3MasterDataTable Sample3 => DB.Sample3MasterDataTable;
		public SampleConstMasterData SampleConst { get; private set; }
		public SampleConst2MasterData SampleConst2 { get; private set; }
		public SampleConst3MasterData SampleConst3 { get; private set; }
		public SampleMultiMasterDataTable SampleMulti => DB.SampleMultiMasterDataTable;

		public async UniTask LoadAsync(string path, CancellationToken ct)
		{
			var builder = new DatabaseBuilder();

			List<UniTask> tasks = new List<UniTask>(7)
			{
				UniTask.Create(async () => builder.Append(await LoadTableAsync<SampleMasterData>(path, nameof(Sample), ct))),
				UniTask.Create(async () => builder.Append(await LoadTableAsync<Sample2MasterData>(path, nameof(Sample2), ct))),
				UniTask.Create(async () => builder.Append(await LoadTableAsync<Sample3MasterData>(path, nameof(Sample3), ct))),
				UniTask.Create(async () => SampleConst = await LoadConstAsync<SampleConstMasterData>(path, nameof(SampleConst), ct)),
				UniTask.Create(async () => SampleConst2 = await LoadConstAsync<SampleConst2MasterData>(path, nameof(SampleConst2), ct)),
				UniTask.Create(async () => SampleConst3 = await LoadConstAsync<SampleConst3MasterData>(path, nameof(SampleConst3), ct)),
				UniTask.Create(async () => builder.Append(await LoadTableAsync<SampleMultiMasterData>(path, nameof(SampleMulti), ct))),
			};

			await UniTask.WhenAll(tasks);

			DB = new MemoryDatabase(builder.Build());
		}

		public async UniTask LoadSequentialAsync(string path, CancellationToken ct)
		{
			var builder = new DatabaseBuilder();

				builder.Append(await LoadTableAsync<SampleMasterData>(path, nameof(Sample), ct));
				builder.Append(await LoadTableAsync<Sample2MasterData>(path, nameof(Sample2), ct));
				builder.Append(await LoadTableAsync<Sample3MasterData>(path, nameof(Sample3), ct));
				SampleConst = await LoadConstAsync<SampleConstMasterData>(path, nameof(SampleConst), ct);
				SampleConst2 = await LoadConstAsync<SampleConst2MasterData>(path, nameof(SampleConst2), ct);
				SampleConst3 = await LoadConstAsync<SampleConst3MasterData>(path, nameof(SampleConst3), ct);
				builder.Append(await LoadTableAsync<SampleMultiMasterData>(path, nameof(SampleMulti), ct));

			DB = new MemoryDatabase(builder.Build());
		}
    }
}