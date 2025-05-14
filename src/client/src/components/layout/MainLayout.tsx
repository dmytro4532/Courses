import { Layout } from 'antd';
import { Outlet } from 'react-router-dom';
import Navbar from '../Navbar';

const { Content, Footer } = Layout;

const MainLayout = () => {
  return (
    <Layout className="layout">
      <Navbar />
      <Content style={{ padding: '64px 48px 0 48px' }}>
        <Outlet />
      </Content>
      <Footer style={{ textAlign: 'center' }}>
        Courses Platform ©{new Date().getFullYear()}
      </Footer>
    </Layout>
  );
};

export default MainLayout; 