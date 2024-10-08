import Image from "next/image";
import placeholderImage from "@/assets/image-placeholder-500x500.jpg";
import { Recipe } from "../types";
import { formatTime } from "@/lib/utils";
import Link from "next/link";

type RecipeCardProps = {
  recipe: Recipe;
};

export default function recipe({ recipe }: RecipeCardProps) {
  return (
    <Link href={`/recipes/${recipe.id}`} passHref className="hover:opacity-75">
      <div className="w-full rounded-lg cursor-pointer border border-gray-200">
        <div className="relative w-full h-48">
          <Image
            src={placeholderImage}
            alt={recipe.title}
            priority
            className="w-full h-full object-cover rounded-t-lg"
          />
          <div className="absolute inset-0 bg-gradient-to-t from-slate-800 via-transparent opacity-45"></div>
        </div>

        <div className="p-4 bg-white dark:bg-gray-800 rounded-b-lg">
          <h5 className="text-lg font-bold text-gray-800 dark:text-gray-200 truncate">
            {recipe.title}
          </h5>
          <div className="flex justify-between items-center mt-2">
            <span className="text-sm text-gray-600 dark:text-gray-400">
              {formatTime(recipe.totalTime)}
            </span>
            {/* Example Icon
            <span className="text-sm text-gray-600 dark:text-gray-400">
              ⭐ 4.5
            </span> */}
          </div>
        </div>
      </div>
    </Link>
  );
}
