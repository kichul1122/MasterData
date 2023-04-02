namespace MasterData.Core.GeneratorTemplate
{
	public class Property
	{
		public string Attribute { get; set; }
		public string DataType { get; set; }
		public string Name { get; set; }

		public string LowerName => Name.ToLower();

		public string Value { get; set; }

		public string Comment { get; set; }
	}

	public partial class MasterDataTemplate
	{
		public string Namespace { get; set; }
		public string PrefixClassName { get; set; }

		public Property[] Properties { get; set; }

		public string LowerClassName => PrefixClassName.ToLower();
		public string ClassName => PrefixClassName + "MasterData";
	}

	public partial class ConstTemplate
	{
		public string Namespace { get; set; }
		public string PrefixClassName { get; set; }

		public Property[] Properties { get; set; }

		public string LowerClassName => PrefixClassName.ToLower();
		public string ClassName => PrefixClassName + "MasterData";
	}

	public partial class EnumTemplate
	{
		public string Namespace { get; set; }
		public string PrefixClassName { get; set; }

		public Property[] Properties { get; set; }

		public string LowerClassName => PrefixClassName.ToLower();
		public string ClassName => PrefixClassName;
	}

	public partial class MasterManagerTemplate
	{
		public string Namespace { get; set; }

		public string Name { get; set; }

		public string MasterDataTable => "MasterDataTable";
		public string MasterData => "MasterData";

		public Property[] Properties { get; set; }

	}
}
