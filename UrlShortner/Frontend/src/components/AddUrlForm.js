import React, { useState } from 'react';
import axios from 'axios';
import './styles/AddUrlForm.css';

localStorage.setItem('authToken', 'тут ваш токен');

const AddUrlForm = () => {
  const [originalUrl, setOriginalUrl] = useState('');
  const [shortenedUrl, setShortenedUrl] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    const authToken = localStorage.getItem('authToken');
    // Виклик API для створення нового скороченого URL
    axios.post('https://localhost:7058/api/ShortUrl', { originalUrl })
      .then(response => setShortenedUrl(response.data.shortUrl))
      .catch(error => console.error('Error adding URL:', error));
  };

  return (
    <div>
      <h1>Додати новий URL</h1>
      <form onSubmit={handleSubmit}>
        <label>
          Оригінальний URL:
          <input type="text" value={originalUrl} onChange={e => setOriginalUrl(e.target.value)} />
        </label>
        <button type="submit">Скоротити</button>
      </form>
      {shortenedUrl && (
        <p>Скорочений URL: {shortenedUrl}</p>
      )}
    </div>
  );
};

export default AddUrlForm;