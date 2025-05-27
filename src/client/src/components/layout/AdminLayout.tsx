import { Layout, Menu } from 'antd';
import { Outlet, Link, useLocation } from 'react-router-dom';
import { BookOutlined } from '@ant-design/icons';
import type { ReactNode } from 'react';

const { Content, Sider } = Layout;

interface AdminLayoutProps {
  children?: ReactNode;
}

const AdminLayout: React.FC<AdminLayoutProps> = ({ children }) => {
  const location = useLocation();

  const getCurrentPath = () => {
    return location.pathname.startsWith('/admin/courses') ? '1' : '';
  };

  const menuItems = [
    {
      key: '1',
      icon: <BookOutlined />,
      label: <Link to="/admin/courses">Courses</Link>,
    },
  ];

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider width={200} theme="light">
        <div style={{ padding: '16px', fontWeight: 'bold', fontSize: '18px' }}>
          Admin Panel
        </div>
        <Menu
          mode="inline"
          selectedKeys={[getCurrentPath()]}
          items={menuItems}
          style={{ height: '100%', borderRight: 0 }}
        />
      </Sider>
      <Layout style={{ padding: '24px' }}>
        <Content style={{ padding: 24, margin: 0, background: '#fff' }}>
          {children || <Outlet />}
        </Content>
      </Layout>
    </Layout>
  );
};

export default AdminLayout; 