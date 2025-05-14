import { Layout, Menu } from 'antd';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from '../store/slices/authSlice';
import type { RootState } from '../store';
import { UserOutlined } from '@ant-design/icons';

const { Header } = Layout;

const Navbar = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useDispatch<any>();
  const { token } = useSelector((state: RootState) => state.auth);

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  };

  const getCurrentPath = () => {
    const path = location.pathname;
    if (path === '/') return '1';
    if (path === '/courses') return '2';
    if (path === '/profile') return '3';
    if (path === '/login' || path === '/register') return '4';
    return '';
  };

  const authenticatedItems = [
    {
      key: '1',
      label: <Link to="/">Home</Link>,
    },
    {
      key: '2',
      label: <Link to="/courses">Courses</Link>,
    },
    {
      key: '3',
      label: <Link to="/profile">Profile</Link>,
      icon: <UserOutlined />,
    },
    {
      key: '4',
      label: <span onClick={handleLogout}>Logout</span>,
    },
  ];

  const unauthenticatedItems = [
    {
      key: '1',
      label: <Link to="/">Home</Link>,
    },
    {
      key: '2',
      label: <Link to="/courses">Courses</Link>,
    },
    {
      key: '4',
      label: <Link to="/login">Login</Link>,
    },
  ];

  return (
    <Header style={{ position: 'fixed', zIndex: 1, width: '100%', display: 'flex', justifyContent: 'space-between' }}>
      <div className="logo" style={{ color: '#fff', fontWeight: 700, fontSize: 22, letterSpacing: 1 }}>Courses</div>
      <Menu
        theme="dark"
        mode="horizontal"
        selectedKeys={[getCurrentPath()]}
        items={token ? authenticatedItems : unauthenticatedItems}
        style={{ minWidth: '400px', justifyContent: 'flex-end' }}
      />
    </Header>
  );
};

export default Navbar; 