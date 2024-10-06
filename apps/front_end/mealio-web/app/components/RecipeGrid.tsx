import RecipeCard from "./RecipeCard";

async function getRecipes() {
  const res = await fetch("http://gateway:5187/recipes");
  return res.json();
}

export default async function RecipeGrid() {
  const recipes = await getRecipes();

  return (
    <div className="grid grid-cols-4 gap-4">
      {recipes &&
        recipes.map((recipe: any) => {
          return <RecipeCard recipe={recipe} key={recipe.id} />;
        })}
    </div>
  );
}
