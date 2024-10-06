import { Recipe } from "../types";
import RecipeCard from "./RecipeCard";

async function getRecipes(): Promise<Recipe[]> {
  const res = await fetch("http://gateway:5187/recipes");
  return res.json();
}

export default async function RecipeGrid() {
  const recipes = await getRecipes();

  return (
    <div className="grid grid-cols-4 gap-4">
      {recipes &&
        recipes.map((recipe: Recipe) => (
          <RecipeCard recipe={recipe} key={recipe.id} />
        ))}
    </div>
  );
}
