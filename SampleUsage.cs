public class SampleUsage
{
	private static IClient client = SearchClient.Instance;

	public void Test()
	{
	// Tags is list/array of strings indexed
		client.Search<Article>()
		.AndAny(new string[]{"a", "b", "c"}, (v, c) => v.Tags.MatchContained(c));
		
	// Author is ContentReference
		client.Search<Article>()
		 .OrAny(new ContentRefence[]{new ContentRefence(), new ContentRefence()}, (v, c) => v.Author.Match(c))
		 
		// Example of Class Person is indexed with property Name
		client.Search<Recipe>()
		 .OrAny(new Ingredient[]{new Ingredient{Name="Salt"}, new Ingredient{Name ="Pepper"}}, (v, c) => v.Ingredients.MatchContained(s => s.Name, c.Name))
	}
}
