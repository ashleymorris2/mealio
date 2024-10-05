import React from "react";

async function getRecipes() {
  const res = await fetch("http://localhost:5187/recipes");
  return res.json();
}

export default async function Recipe() {
  const recipes = await getRecipes();
  return <div>{JSON.stringify(recipes, null, 2)}</div>;
}
