/* eslint-disable no-undef */
/* globals $ */
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const PersonDetail = () => {
  const { name } = useParams();
  const [person, setPerson] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`/api/person/${encodeURIComponent(name)}`);
        if (!res.ok) throw new Error('Person not found');
        const data = await res.json();
        setPerson(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [name]);

  useEffect(() => {
    if (person) {
      const actedMoviesData = (person.acted_movies || []).map(m => [m]);
      const directedMoviesData = (person.directed_movies || []).map(m => [m]);

      if ($.fn.DataTable.isDataTable('#actedTable')) {
        $('#actedTable').DataTable().destroy();
      }
      $('#actedTable').DataTable({
        data: actedMoviesData,
        columns: [{ title: 'Acted Movies' }],
        paging: true,
        searching: true,
        responsive: true
      });

      if ($.fn.DataTable.isDataTable('#directedTable')) {
        $('#directedTable').DataTable().destroy();
      }
      $('#directedTable').DataTable({
        data: directedMoviesData,
        columns: [{ title: 'Directed Movies' }],
        paging: true,
        searching: true,
        responsive: true
      });
    }
  }, [person]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h1>{person.name}</h1>
      <h2>Acted Movies</h2>
      <table id="actedTable" className="display" style={{ width: '100%' }}></table>
      <h2>Directed Movies</h2>
      <table id="directedTable" className="display" style={{ width: '100%' }}></table>
    </div>
  );
};

export default PersonDetail;