import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const SearchBar = () => {
  const [query, setQuery] = useState('');
  const [suggestions, setSuggestions] = useState([]);
  const navigate = useNavigate();

  const handleChange = async (e) => {
    const value = e.target.value;
    setQuery(value);
    if (value.length >= 3) {
      try {
        const response = await fetch(`/api/search?query=${encodeURIComponent(value)}`);
        if (response.ok) {
          const data = await response.json();
          setSuggestions(data);
        }
      } catch (error) {
        console.error('Search error:', error);
      }
    } else {
      setSuggestions([]);
    }
  };

  const handleSelect = (type, value) => {
    setQuery('');
    setSuggestions([]);
    navigate(`/${type}/${encodeURIComponent(value)}`);
  };

  return (
    <div style={{ position: 'relative' }}>
      <input
        type="text"
        value={query}
        onChange={handleChange}
        placeholder="Search movies, people, or tags (3+ chars)"
        style={{ width: '300px', padding: '10px' }}
      />
      {suggestions.length > 0 && (
        <ul style={{ listStyle: 'none', position: 'absolute', background: 'white', border: '1px solid #ccc', width: '300px', zIndex: 1 }}>
          {suggestions.map((sug, index) => (
            <li key={index} onClick={() => handleSelect(sug.type, sug.value)} style={{ padding: '10px', cursor: 'pointer' }}>
              {sug.value} ({sug.type})
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default SearchBar;