import Image from "next/image";
import SearchBar from '../../components/searchBar'

export default function Home() {
  return (
    <div className="m-8 flex flex-col justify-center max-w-lg">
      <SearchBar />
    </div>
  );
}
