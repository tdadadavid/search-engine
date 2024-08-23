'use client'

import React, { useCallback, useState } from 'react';
import { FaSearch } from "react-icons/fa";
import './search.css'

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
  <form class='form'>
      <div className="relative">
          <input type="search" placeholder='Type Here' class=' input' onChange={(e) => handleSearch(e)}/>
          <button className='search'>
              <FaSearch />
          </button>
      </div>

      {
          activeSearch.length > 0 && (
              <div className="suggest">
                  {
                      activeSearch.map((s, index) => (
                          <span key={index}>{s}</span>
                      ))
                  }
              </div>
          )
      }


      
  </form>
)
}

export default SearchBar;
