import { Typography, Row, Col, Card } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';

const { Title, Paragraph } = Typography;

const Home = () => {
  const { user } = useSelector((state: RootState) => state.auth);

  return (
    <div>
      <Title level={2}>Ласкаво просимо на платформу курсів</Title>
      <Paragraph>
        {user ? (
          `З поверненням, ${user.username}!`
        ) : (
          'Будь ласка, увійдіть, щоб отримати доступ до своїх курсів.'
        )}
      </Paragraph>

      <Row gutter={[16, 16]} style={{ marginTop: 24 }}>
        <Col xs={24} sm={12} md={12}>
          <Card title="Переглянути курси" hoverable>
            <Paragraph>
              Ознайомтеся з нашим широким асортиментом курсів, розроблених, щоб допомогти вам вчитися та розвиватися.
            </Paragraph>
            <Link to="/courses">Переглянути курси</Link>
          </Card>
        </Col>
        <Col xs={24} sm={12} md={12}>
          <Card title="Відстежуйте прогрес" hoverable>
            <Paragraph>
              Слідкуйте за своїм навчальним прогресом і виконуйте завдання.
            </Paragraph>
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default Home; 