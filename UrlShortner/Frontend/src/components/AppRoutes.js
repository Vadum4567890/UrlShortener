import React from 'react';
import { Routes, Route } from 'react-router-dom';
import AddUrlForm from './AddUrlForm'; // Import your components here
import ShortUrlTable from './ShortUrlTable';
import NotFound from './NotFound';
import ShortUrlInfo from './ShortUrlInfo';

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<ShortUrlTable />} />
      <Route path="/add" element={<AddUrlForm />} />
      <Route path="/shorturl/:id" element={<ShortUrlInfo />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default AppRoutes;