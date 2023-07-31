import React, { useState } from 'react';
import axios from 'axios';
import './styles/AddUrlForm.css';

const AddUrlForm = () => {
  const [originalUrl, setOriginalUrl] = useState('');
  const [shortenedUrl, setShortenedUrl] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();

    // Створюємо об'єкт з усіма параметрами моделі ShortUrlViewModel
    const newUrlData = {
      originalUrl: originalUrl,
      shortUrl: '', // Даний параметр буде порожнім, так як сервер генерує скорочені URL
      createdBy: '', // Даний параметр також буде порожнім, так як ми його заповнимо на сервері
      createdDate: '', // Даний параметр також буде порожнім, так як ми його заповнимо на сервері
    };

    // Викликаємо API для створення нового скороченого URL
    axios.post('https://localhost:7058/api/ShortUrl', newUrlData, {withCredentials: true})
      .then(response => {
        // При успішному створенні, отримуємо скорочений URL з відповіді сервера
        setShortenedUrl(response.data.shortUrl);
        window.location.href = '/';
      })
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