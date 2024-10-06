import Image from "next/image";
import placeholderImage from "@/assets/image-placeholder-500x500.jpg";
import { Recipe } from "../types";
import { formatTime } from "@/lib/utils";

type RecipeCardProps = {
  recipe: Recipe;
};

export default function recipe({ recipe }: RecipeCardProps) {
  return (
    <div className="w-full rounded-lg">
      <Image
        src={placeholderImage}
        alt={recipe.title}
        priority
        className="w-full h-48 object-cover rounded-lg"
      />
      <div className="flex justify-between items-center center mt-4 ml-4 mr-4">
        <h5 className=" text-gray-700">{recipe.title}</h5>
        <h5 className=" text-gray-700">{formatTime(recipe.totalTime)}</h5>
      </div>
    </div>
  );
}
