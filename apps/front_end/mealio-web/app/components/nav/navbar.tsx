import Link from "next/link";
import {
  Navbar as FlowbiteNavbar,
  NavbarBrand,
  NavbarCollapse,
  NavbarLink,
  NavbarToggle,
} from "flowbite-react";

export default function Navbar() {
  return (
    <FlowbiteNavbar fluid border className="bg-slate-50">
      <NavbarBrand>
        <img
          src="../../../favicon.ico" //todo: Change this in future
          className="mr-3 h-6 sm:h-6"
          alt="Flowbite React Logo"
        />
        <span className="self-center whitespace-nowrap text-xl font-semibold dark:text-white">
          Mealio
        </span>
      </NavbarBrand>
      <NavbarToggle />
      <NavbarCollapse>
        <NavbarLink href="#" active>
          Home
        </NavbarLink>
        <NavbarLink as={Link} href="#">
          About
        </NavbarLink>
        <NavbarLink href="#">Pricing</NavbarLink>
        <NavbarLink href="#">Login</NavbarLink>
      </NavbarCollapse>
    </FlowbiteNavbar>
  );
}
