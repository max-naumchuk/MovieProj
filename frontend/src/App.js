import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import SearchBar from './components/SearchBar';
import MovieDetail from './components/MovieDetail';
import PersonDetail from './components/PersonDetail';
import TagDetail from './components/TagDetail';

function App() {
  return (
    <Router>
      <div className="App">
        <SearchBar />
        <Routes>
          <Route path="/movie/:title" element={<MovieDetail />} />
          <Route path="/person/:name" element={<PersonDetail />} />
          <Route path="/tag/:tag" element={<TagDetail />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
