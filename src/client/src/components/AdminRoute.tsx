import { Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';
import AdminLayout from './layout/AdminLayout';
import { getRoleFromToken } from '../utils/jwt';

const AdminRoute = () => {
  const { token } = useSelector((state: RootState) => state.auth);
  const role = token ? getRoleFromToken(token) : null;
  const isAdmin = role === 'Admin';

  if (!token || !isAdmin) {
    return <Navigate to="/login" replace />;
  }

  return <AdminLayout />;
};

export default AdminRoute; 
