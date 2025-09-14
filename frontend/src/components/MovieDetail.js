/* eslint-disable no-undef */
/* globals $ */

import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const MovieDetail = () => {
  const { title } = useParams();
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`/api/movie/${encodeURIComponent(title)}`);
        if (!res.ok) throw new Error('Movie not found');
        const data = await res.json();
        setMovie(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [title]);

  useEffect(() => {
    if (movie) {
      // Initialize Actors DataTable
      if ($.fn.DataTable.isDataTable('#actorsTable')) {
        $('#actorsTable').DataTable().destroy();
      }
      $('#actorsTable').DataTable({
        data: (movie.actors || []).map(actor => [actor]),
        columns: [{ title: 'Actors' }],
        paging: true,
        searching: true,
        responsive: true
      });

      // Initialize Tags DataTable
      if ($.fn.DataTable.isDataTable('#tagsTable')) {
        $('#tagsTable').DataTable().destroy();
      }
      $('#tagsTable').DataTable({
        data: (movie.tags || []).map(tag => [tag]),
        columns: [{ title: 'Tags' }],
        paging: true,
        searching: true,
        responsive: true
      });
    }
  }, [movie]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h1>{movie.title}</h1>
      <p>Rating: {movie.rating || 'N/A'}</p>
      <p>Director: {movie.director || 'N/A'}</p>
      <h2>Actors</h2>
      <table id="actorsTable" className="display" style={{ width: '100%' }}></table>
      <h2>Tags</h2>
      <table id="tagsTable" className="display" style={{ width: '100%' }}></table>
    </div>
  );
};

export default MovieDetail;