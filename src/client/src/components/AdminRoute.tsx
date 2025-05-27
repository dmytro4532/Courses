import { Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';
import AdminLayout from './layout/AdminLayout';

const AdminRoute = () => {
  const { user, token } = useSelector((state: RootState) => state.auth);
  const isAdmin = user?.role === 'Admin';

  // if (!token || !isAdmin) {
  //   return <Navigate to="/login" replace />;
  // }

  return <AdminLayout />;
};

export default AdminRoute; 
