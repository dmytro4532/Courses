import { useParams } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { Typography, Card, Alert, Empty, List } from 'antd';
import type { RootState } from '../store';

const { Title, Paragraph } = Typography;

const TestDetails = () => {
  const { courseId, topicId, testId } = useParams<{ courseId: string; topicId: string; testId: string }>();
  const { items: courses = [] } = useSelector((state: RootState) => state.courses);

  const course = courses.find((c) => c.id === courseId);
  const topic = course?.topics.find((t) => t.id === topicId);
  const test = topic?.tests.find((t) => t.id === testId);

  if (!course || !topic || !test) {
    return <Alert type="error" message="Test not found" style={{ marginTop: 48 }} />;
  }

  return (
    <div style={{ padding: '24px 0' }}>
      <Title level={2}>{test.title}</Title>
      <Paragraph>{test.description}</Paragraph>
      <Title level={3} style={{ marginTop: 24 }}>Questions</Title>
      {test.questions.length === 0 ? (
        <Empty description="No questions for this test" />
      ) : (
        <List
          dataSource={test.questions}
          renderItem={(q, idx) => (
            <List.Item>
              <Card title={`Q${idx + 1}: ${q.text}`} style={{ width: '100%' }}>
                <ol>
                  {q.options.map((opt, i) => (
                    <li key={i}>{opt}</li>
                  ))}
                </ol>
              </Card>
            </List.Item>
          )}
        />
      )}
    </div>
  );
};

export default TestDetails; 