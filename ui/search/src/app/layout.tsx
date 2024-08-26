import type { Metadata } from "next";
import Link from "next/link";
import { Inter } from "next/font/google";
import "./globals.css";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "CSC 322 - Search Engine",
  description: "Created by the genuises of unilag!",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <nav className='bg-slate-700 max-w-screen-lg my-1 mx-auto flex justify-center gap-3'>
          <Link className="p-3 hover:bg-slate-500" href="/">Home</Link>
          <Link className="p-3 hover:bg-slate-500" href="/upload">Upload</Link>
        </nav>
        {children}
      </body>
    </html>
  );
}
