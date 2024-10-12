import { Recipe } from "../types";
import RecipeCard from "./RecipeCard";

async function getRecipes(): Promise<Recipe[]> {
  const res = await fetch("http://gateway:5187/recipes");
  return res.json();
}

export default async function RecipeGrid() {
  const recipes = await getRecipes();

  console.log(recipes);

  return (
    <div className="grid pr-36 pl-36 grid-cols-3 gap-8">
      {recipes &&
        recipes.map((recipe: Recipe) => (
          <RecipeCard recipe={recipe} key={recipe.id} />
        ))}
    </div>
  );
}
