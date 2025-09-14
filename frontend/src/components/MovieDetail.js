/* eslint-disable no-undef */
/* globals $ */

import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

const MovieDetail = () => {
  const { title } = useParams();
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

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
      if ($.fn.DataTable.isDataTable('#actorsTable')) {
        $('#actorsTable').DataTable().destroy();
      }
      $('#actorsTable').DataTable({
        data: (movie.actors || []).map(actor => [actor]),
        columns: [{ 
          title: 'Actors',
          render: (data, type, row) => {
            return `<a href="#" class="person-link">${data}</a>`;
          } 
        }],
        paging: true,
        searching: true,
        responsive: true,
        createdRow: (row, data) => {
          $(row).on('click', '.person-link', (e) => {
            e.preventDefault();
            navigate(`/person/${encodeURIComponent(data[0])}`);
          });
        }
      });

      if ($.fn.DataTable.isDataTable('#tagsTable')) {
        $('#tagsTable').DataTable().destroy();
      }
      $('#tagsTable').DataTable({
        data: (movie.tags || []).map(tag => [tag]),
        columns: [{ 
          title: 'Tags',
          render: (data, type, row) => {
            return `<a href="#" class="tag-link">${data}</a>`;
          }
        }],
        paging: true,
        searching: true,
        responsive: true,
        createdRow: (row, data) => {
          $(row).on('click', '.tag-link', (e) => {
            e.preventDefault();
            navigate(`/tag/${encodeURIComponent(data[0])}`);
          });
        }
      });
    }
  }, [movie, navigate]);

  const handleDirectorClick = () => {
    if (movie.director) {
      navigate(`/person/${encodeURIComponent(movie.director)}`);
    }
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h1>{movie.title}</h1>
      <p>Rating: {movie.rating || 'N/A'}</p>
      <p>Director: <a href="#" onClick={handleDirectorClick} style={{ cursor: 'pointer', color: 'blue', textDecoration: 'underline' }}>{movie.director || 'N/A'}</a></p>
      <h2>Actors</h2>
      <table id="actorsTable" className="display" style={{ width: '100%' }}></table>
      <h2>Tags</h2>
      <table id="tagsTable" className="display" style={{ width: '100%' }}></table>
    </div>
  );
};

export default MovieDetail;