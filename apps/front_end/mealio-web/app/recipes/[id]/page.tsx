export default function RecipePage({ params }: { params: { id: string } }) {
  return <h1 className="font-extrabold">Hello, Recipe {params.id}</h1>;
}
