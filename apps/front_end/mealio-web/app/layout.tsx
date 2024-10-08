import { Inter } from "next/font/google";
import "./globals.css";
import Navbar from "./components/nav/Navbar";

const inter = Inter({ subsets: ["latin"] });

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <Navbar />
        <main className="container pt-10 m-auto">{children}</main>
      </body>
    </html>
  );
}
