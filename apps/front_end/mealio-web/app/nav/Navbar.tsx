import React from "react";

export default function Navbar() {
  return (
    <header className="sticky top-0 z-50 flex justify-between bg-slate-100 p-5 items-center text-gray-800 shadow-md">
      <div className="text-2xl font-semibold">
        <div>Mealio</div>
      </div>
      <div>---</div>
      <div>Login?</div>
    </header>
  );
}
