import React, { useState, useEffect } from 'react';
import axios from 'axios';

const ShortUrlTable = () => {
  const [shortUrls, setShortUrls] = useState([]);

  useEffect(() => {
    // Отримання списку скорочених URL при завантаженні компонента
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
          </tr>
        </thead>
        <tbody>
          {shortUrls.map((url) => (
            <tr key={url.id}>
              <td>{url.originalUrl}</td>
              <td>{url.shortUrl}</td>
              <td>{url.createdBy}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ShortUrlTable;