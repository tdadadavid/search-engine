'use client'

import React, { useCallback, useState } from 'react';
import { FaSearch } from "react-icons/fa";
import './search.css'
import axios from "axios";

const SearchBar = () => {
	const [activeSearch, setActiveSearch] = useState([]);
	const [loading, setLoading] = useState(false); // Optional: add loading state

	const handleSearch = async (e) => {
		const query = e.target.value.trim();
		if (query === "") {
			setActiveSearch([]);
			return;
		}

		setLoading(true); // Start loading
		try {
			const response = await axios.get(
				`http://localhost:5268/api/documents/search?q=${query}`
			);
			setActiveSearch(response.data.result); // Update state with search results
		} catch (error) {
			console.error("Error fetching search results", error);
		} finally {
			setLoading(false); // End loading
		}
	};

	return (
		<form className="form">
			<div className="relative">
				<input
					type="search"
					placeholder="Type Here"
					className="input"
					onChange={handleSearch}
				/>
				<button className="search" type="button">
					<FaSearch />
				</button>
			</div>

			<div id="results">
				{loading && <div>Loading...</div>}
				{activeSearch.length > 0 && (
					<div className="search-results">
						{activeSearch.map((item, index) => (
							<div key={index} className="result-card">
								<div className="result-score">
									Score: {item.rank.toFixed(2)}
								</div>
								<div className="result-content">
									<a
										href={item.link}
										target="_blank"
										rel="noopener noreferrer"
										className="result-link"
									>
										{item.link}
									</a>
									<p className="result-snippet">
										{/* Placeholder snippet text. Replace with actual snippet if available */}
										This is a snippet or description related to the document...
									</p>
								</div>
							</div>
						))}
					</div>
				)}
			</div>
		</form>
	);
};

export default SearchBar;
