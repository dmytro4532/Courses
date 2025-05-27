import { Layout, Menu } from 'antd';
import { Outlet, Link, useLocation } from 'react-router-dom';
import { BookOutlined, FileTextOutlined, LinkOutlined, UserOutlined } from '@ant-design/icons';
import type { ReactNode } from 'react';

const { Content, Sider } = Layout;

interface AdminLayoutProps {
  children?: ReactNode;
}

const AdminLayout: React.FC<AdminLayoutProps> = ({ children }) => {
  const location = useLocation();

  const getCurrentPath = () => {
    if (location.pathname.startsWith('/admin/courses')) return '1';
    if (location.pathname.startsWith('/admin/tests')) return '2';
    if (location.pathname.startsWith('/admin/users')) return '3';
    return '';
  };

  const menuItems = [
    {
      key: '1',
      icon: <BookOutlined />,
      label: <Link to="/admin/courses">Courses</Link>,
    },
    {
      key: '2',
      icon: <FileTextOutlined />,
      label: <Link to="/admin/tests">Tests</Link>,
    },
    {
      key: '3',
      icon: <UserOutlined />,
      label: <Link to="/admin/users">Users</Link>,
    },
    {
      key: '4',
      icon: <LinkOutlined />,
      label: <Link to="/" target="_blank" rel="noopener noreferrer">View Website</Link>,
    },
  ];

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider 
        width={200} 
        theme="light"
        style={{
          position: 'fixed',
          left: 0,
          top: 0,
          bottom: 0,
          boxShadow: '0 2px 8px rgba(0,0,0,0.15)'
        }}
      >
        <div style={{ 
          padding: '16px', 
          fontWeight: 'bold', 
          fontSize: '18px',
          borderBottom: '1px solid #f0f0f0',
          color: 'black'
        }}>
          Admin Panel
        </div>
        <Menu
          mode="inline"
          selectedKeys={[getCurrentPath()]}
          items={menuItems}
          style={{ height: 'calc(100vh - 53px)', borderRight: 0 }}
        />
      </Sider>
      <Layout style={{ marginLeft: 200, minHeight: '100vh' }}>
        <Content style={{ 
          padding: 24, 
          margin: 24, 
          background: '#fff',
          borderRadius: '4px',
          boxShadow: '0 1px 3px rgba(0,0,0,0.05)'
        }}>
          {children || <Outlet />}
        </Content>
      </Layout>
    </Layout>
  );
};

export default AdminLayout; 