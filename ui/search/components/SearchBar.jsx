'use client'

import React, { useCallback, useState } from 'react';
import { FaSearch } from "react-icons/fa";


const SearchBar = () => {

  const [activeSearch, setActiveSearch] = useState([])

  const handleSearch = (e) => {
      if(e.target.value == ''){
          setActiveSearch([])
          return false
      }
      setActiveSearch(words.filter(w => w.includes(e.target.value)).slice(0,8))
  }

return (
  <form className='w-[500px] relative'>
      <div className="relative">
          <input type="search" placeholder='Type Here' className='w-full p-4 rounded-full bg-slate-800' onChange={(e) => handleSearch(e)}/>
          <button className='absolute right-1 top-1/2 -translate-y-1/2 p-4 bg-slate-600 rounded-full'>
              <FaSearch />
          </button>
      </div>

      {
          activeSearch.length > 0 && (
              <div className="absolute top-20 p-4 bg-slate-800 text-white w-full rounded-xl left-1/2 -translate-x-1/2 flex flex-col gap-2">
                  {
                      activeSearch.map(s => (
                          <span>{s}</span>
                      ))
                  }
              </div>
          )
      }


      
  </form>
)
}

export default SearchBar;
