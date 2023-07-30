import React from 'react';
import { Routes, Route } from 'react-router-dom';

import AddUrlForm from './AddUrlForm'; // Import your components here
import ShortUrlTable from './ShortUrlTable';
import NotFound from './NotFound';

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<ShortUrlTable />} />
      <Route path="/add" element={<AddUrlForm />} />
      {/* Add more routes here */}
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default AppRoutes;