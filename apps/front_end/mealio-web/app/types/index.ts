export type Recipe = {
  id: string;
  title: string;
  servings: number;
  preparationTime: string;
  totalTime: string;
  ingredients: Ingredient[];
  instructions: Instruction[];
  nutritionPerServing: NutritionPerServing;
};

export type Ingredient = {
  name: string;
  quantity: number;
  unit?: string;
};

export type Instruction = {
  stepNumber: number;
  description: string;
};

export type NutritionPerServing = {
  calories: number;
  protein: number;
  fat: number;
  saturatedFat: number;
  fiber: number;
  carbohydrates: number;
  sugars: number;
  salt: number;
};
