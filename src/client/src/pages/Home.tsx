import { Typography, Row, Col, Card } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';

const { Title, Paragraph } = Typography;

const Home = () => {
  const { user } = useSelector((state: RootState) => state.auth);

  return (
    <div>
      <Title level={2}>Welcome to Courses Platform</Title>
      <Paragraph>
        {user ? (
          `Welcome back, ${user.username}!`
        ) : (
          'Please login to access your courses.'
        )}
      </Paragraph>

      <Row gutter={[16, 16]} style={{ marginTop: 24 }}>
        <Col xs={24} sm={12} md={12}>
          <Card title="Browse Courses" hoverable>
            <Paragraph>
              Explore our wide range of courses designed to help you learn and grow.
            </Paragraph>
            <Link to="/courses">View Courses</Link>
          </Card>
        </Col>
        <Col xs={24} sm={12} md={12}>
          <Card title="Track Progress" hoverable>
            <Paragraph>
              Monitor your learning progress and complete assessments to earn certificates.
            </Paragraph>
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default Home; 