import React, { useState, useEffect } from 'react';
import axios from 'axios';
import  { Link } from 'react-router-dom';
import './styles/ShortUrlTable.css';


const ShortUrlTable = () => {
  const [shortUrls, setShortUrls] = useState([]);

  useEffect(() => {
    fetchShortUrls();
  }, []);

  const fetchShortUrls = async () => {
    try {
      const response = await axios.get('https://localhost:7058/api/ShortUrl');
      setShortUrls(response.data);
    } catch (error) {
      console.error('Помилка при отриманні списку скорочених URL:', error);
    }
  };

  return (
    <div>
      <h2>ShortUrlTable</h2>
      <table>
        <thead>
          <tr>
            <th>originalUrl URL</th>
            <th>ShortUrl URL</th>
            <th>Owner</th>
            <th>Details</th>
          </tr>
        </thead>
        <tbody>
          {shortUrls.map((url) => (
            <tr key={url.id}>
              <td>{url.originalUrl}</td>
              <td>{url.shortUrl}</td>
              <td>{url.createdBy}</td>
              <td>
                <Link to={`/shorturl/${url.id}`}>Details</Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ShortUrlTable;