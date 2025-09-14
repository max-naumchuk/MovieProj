/* eslint-disable no-undef */
/* globals $ */
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const TagDetail = () => {
  const { tag } = useParams();
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`/api/tag/${encodeURIComponent(tag)}`);
        if (!res.ok) throw new Error('Tag not found');
        const data = await res.json();
        setMovies(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [tag]);

  useEffect(() => {
    if (movies.length > 0) {
      if ($.fn.DataTable.isDataTable('#moviesTable')) {
        $('#moviesTable').DataTable().destroy();
      }
      $('#moviesTable').DataTable({
        data: movies.map(m => [m]),
        columns: [{ title: 'Movies' }],
        paging: true,
        searching: true,
        responsive: true
      });
    }
  }, [movies]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h1>Tag: {tag}</h1>
      <h2>Associated Movies</h2>
      <table id="moviesTable" className="display" style={{ width: '100%' }}></table>
    </div>
  );
};

export default TagDetail;