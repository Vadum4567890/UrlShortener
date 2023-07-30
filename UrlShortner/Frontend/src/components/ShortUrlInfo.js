import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';

const ShortUrlInfo = () => {
  const { id } = useParams();
  const [urlInfo, setUrlInfo] = useState(null);

  useEffect(() => {
    // Отримання інформації про конкретний URL при завантаженні компонента
    fetchShortUrlInfo(id);
  }, [id]);

  const fetchShortUrlInfo = async (id) => {
    try {
      // Виклик API для отримання інформації про конкретний URL з використанням сесій
      const response = await axios.get(`https://localhost:7058/api/ShortUrlInfo/${id}`, { withCredentials: true });
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
      <p>Original URL: {urlInfo.originalUrl}</p>
      <p>Short URL: {urlInfo.shortUrl}</p>
      <p>Owner: {urlInfo.createdBy}</p>
      <p>CreatedDate: {urlInfo.createdDate}</p>
    </div>
  );
};

export default ShortUrlInfo;