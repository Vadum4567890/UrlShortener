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

  const handleDelete = async (id) => {
    try {
      const response = await axios.delete(`https://localhost:7058/api/ShortUrl/${id}`);
      if (response.status === 200) {
        // Успішне видалення, оновлюємо список скорочених URL
        fetchShortUrls();
      } else {
        console.error('Помилка при видаленні скороченого URL:', response);
      }
    } catch (error) {
      console.error('Помилка при видаленні скороченого URL:', error);
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
            <th>Delete</th>
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
              <td>
                <button onClick={() => handleDelete(url.id)}>Delete</button> {/* Кнопка для видалення */}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ShortUrlTable;