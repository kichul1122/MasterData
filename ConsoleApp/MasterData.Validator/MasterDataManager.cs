using MasterMemory;
using MessagePack;
using MessagePack.Resolvers;

namespace KC
{
	public partial class MasterDataManager
	{
		public MemoryDatabase DB { get; set; }

		public async Task<T> LoadConstAsync<T>(string path, string fileName, CancellationToken ct)
		{
			var json = await System.IO.File.ReadAllTextAsync($"{path}/{fileName}.json", ct);
			var binary = MessagePackSerializer.ConvertFromJson(json);
			return MessagePackSerializer.Deserialize<T>(binary);
		}

		public async Task<IEnumerable<T>> LoadTableAsync<T>(string path, string fileName, CancellationToken ct)
		{
			var json = await System.IO.File.ReadAllTextAsync($"{path}/{fileName}.json", ct);
			var binary = MessagePackSerializer.ConvertFromJson(json);
			return MessagePackSerializer.Deserialize<IEnumerable<T>>(binary);
		}

		public static void SetupMessagePackResolver()
		{
			StaticCompositeResolver.Instance.Register(new[]{
				MasterMemoryResolver.Instance,

				MessagePack.Resolvers.BuiltinResolver.Instance,
				MessagePack.Resolvers.AttributeFormatterResolver.Instance,

				// replace enum resolver
				MessagePack.Resolvers.DynamicEnumAsStringResolver.Instance,

				MessagePack.Resolvers.DynamicGenericResolver.Instance,
				MessagePack.Resolvers.DynamicUnionResolver.Instance,
				MessagePack.Resolvers.DynamicObjectResolver.Instance,

				MessagePack.Resolvers.PrimitiveObjectResolver.Instance,

				// final fallback(last priority)
				MessagePack.Resolvers.DynamicContractlessObjectResolver.Instance,
				StandardResolver.Instance
			});

			var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
			MessagePackSerializer.DefaultOptions = options;
		}
	}

	public partial class SampleMasterData : IValidatable<SampleMasterData>
	{
		public void Validate(IValidator<SampleMasterData> validator)
		{
			var items = validator.GetReferenceSet<SampleMasterData>();

			//validator.Validate(x => x.Id <= 1);

			// Custom if logics.
			//if (this.RewardId > 0)
			//{
			//    // RewardId must exists in Item.ItemId
			//    items.Exists(x => x.RewardId, x => x.ItemId);
			//}

			//// Range check, Cost must be 10..20
			//validator.Validate(x => x.Cost >= 10);
			//validator.Validate(x => x.Cost <= 20);

			//// In this region, only called once so enable to validate overall of tables.
			//if (validator.CallOnce())
			//{
			//    var quests = validator.GetTableSet();
			//    // Check unique othe than index property.
			//    quests.Where(x => x.RewardId != 0).Unique(x => x.RewardId);
			//}
		}
	}
}
