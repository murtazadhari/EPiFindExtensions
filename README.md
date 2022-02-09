# EPiFindExtensions
This Repository Contains the Utility EPiFind extensions methods list to build OrAny and AndAny conditions

```c#
// Tags is list/array of strings indexed in Article Class
client.Search<Article>()
.For("test")
.AndAny(new string[]{"a", "b", "c"}, (v, c) => v.Tags.MatchContained(c));
		
// Author is ContentReference in Article Class
client.Search<Article>()
.For("test")
.OrAny(new ContentRefence[]{new ContentRefence(), new ContentRefence()}, (v, c) => v.Author.Match(c))
		 
// Example of Class Ingredient which is indexed as list of Ingredient in Recipe Class
client.Search<Recipe>()
.For("test")
.OrAny(new Ingredient[]{new Ingredient{Name="Salt"}, new Ingredient{Name ="Pepper"}}, (v, c) => v.Ingredients.MatchContained(s => s.Name, c.Name))
```
