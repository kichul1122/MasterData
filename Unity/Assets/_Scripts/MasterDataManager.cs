using Cysharp.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace KC
{
	public partial class MasterDataManager : MonoBehaviour
	{
		private MemoryDatabase DB;

		private void Awake()
		{
			CreateDataBaseAsync().Forget();
		}

		public async UniTaskVoid CreateDataBaseAsync()
		{
			string path = "MasterData";

			var ct = this.GetCancellationTokenOnDestroy();

			await LoadAsync(path, ct);

			//Test
			var sampleMasterData = Sample.FindById(1);
			var bin = MessagePackSerializer.Serialize(sampleMasterData);
			Debug.Log(MessagePackSerializer.ConvertToJson(bin));

			var sampleConst = SampleConst2;
			var bin2 = MessagePackSerializer.Serialize(sampleConst);
			Debug.Log(MessagePackSerializer.ConvertToJson(bin2));

			MasterMemory.RangeView<SampleMultiMasterData> sampleMulties = SampleMulti.FindById((1));
			foreach (var sampleMulti in sampleMulties)
			{
				var bin3 = MessagePackSerializer.Serialize(sampleMulti);
				Debug.Log(MessagePackSerializer.ConvertToJson(bin3));
			}
		}

		public async UniTask<T> LoadConstAsync<T>(string path, string fileName, CancellationToken ct)
		{
			var json = ((await Resources.LoadAsync<TextAsset>($"{path}/{fileName}")) as TextAsset).text;
			var binary = MessagePackSerializer.ConvertFromJson(json);
			return MessagePackSerializer.Deserialize<T>(binary);
		}

		public async UniTask<IEnumerable<T>> LoadTableAsync<T>(string path, string fileName, CancellationToken ct)
		{
			var json = ((await Resources.LoadAsync<TextAsset>($"{path}/{fileName}")) as TextAsset).text;
			var binary = MessagePackSerializer.ConvertFromJson(json);
			return MessagePackSerializer.Deserialize<IEnumerable<T>>(binary);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
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
}
