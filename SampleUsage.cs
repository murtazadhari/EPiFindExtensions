public class SampleUsage
{
	private static IClient client = SearchClient.Instance;

	public void Test()
	{
	// Tags is list/array of strings indexed
		client.Search<T>()
		.AndAny(new string[]{"a", "b", "c"}, (v, c) => v.Tags.MatchContained(c));
		
	// Topic is ContentReference
		client.Search<T>()
		 .OrAny(new ContentRefence[]{new ContentRefence(), new ContentRefence()}, (v, c) => v.Topic.Match(c))
		 
		// Example of Class Person is indexed with property Name
		client.Search<T>()
		 .OrAny(new Person[]{new Person{Name="Ali"}, new Person{Name ="Murtaza"}}, (v, c) => v.Person.MatchContained(s => s.Name, c.Name))
	}
}