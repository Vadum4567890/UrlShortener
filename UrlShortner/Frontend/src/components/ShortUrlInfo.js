import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, Link } from 'react-router-dom';

const ShortUrlInfo = () => {
  const { id } = useParams();
  const [urlInfo, setUrlInfo] = useState(null);

  useEffect(() => {
    // Отримання інформації про конкретний URL при завантаженні компонента
    fetchShortUrlInfo(id);
  }, [id]);

  const getFormattedDate = (dateTimeString) => {
    const date = new Date(dateTimeString);
    return date.toISOString().split('T')[0];
  };

  const fetchShortUrlInfo = async (id) => {
    try {
      const response = await axios.get(`https://localhost:7058/api/ShortUrl/${id}`);
      setUrlInfo(response.data);
    } catch (error) {
      console.error('Помилка при отриманні інформації про URL:', error);
    }
  };

  if (!urlInfo) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h2>ShortUrl Info</h2>
      <table>
        <thead>
          <tr>
            <th>Parameter</th>
            <th>Value</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Original URL:</td>
            <td>{urlInfo.originalUrl}</td>
          </tr>
          <tr>
            <td>Short URL:</td>
            <td>{urlInfo.shortUrl}</td>
          </tr>
          <tr>
            <td>Owner:</td>
            <td>{urlInfo.createdBy}</td>
          </tr>
          <tr>
            <td>Created Date:</td>
            <td>{getFormattedDate(urlInfo.createdDate)}</td>
          </tr>
        </tbody>
      </table>
      <p>
        <Link to="/">Back to ShortUrlTable</Link>
      </p>
    </div>
  );
};

export default ShortUrlInfo;
