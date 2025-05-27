import { Card, Image, Typography, Button, Tag, Space, Divider } from 'antd';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../../store';
import type { Topic, TestAttempt } from '../../../types';
import { fetchTestAttemptsByTest } from '../../../store/slices/testAttemptsSlice';
import { fetchTestById } from '../../../store/slices/testSlice';

const { Meta } = Card;
const { Paragraph, Text } = Typography;

interface TopicCardProps {
  topic: Topic;
  isCompleted?: boolean;
  onComplete?: () => void;
  courseStarted?: boolean;
}

const TopicCard = ({ topic, isCompleted, onComplete, courseStarted }: TopicCardProps) => {
  const dispatch = useDispatch<AppDispatch>();
  const { attempts } = useSelector((state: RootState) => state.testAttempts);
  const { test, loading: testLoading } = useSelector((state: RootState) => state.test);

  useEffect(() => {
    if (topic.testId) {
      dispatch(fetchTestById(topic.testId));
      dispatch(fetchTestAttemptsByTest(topic.testId));
    }
  }, [dispatch, topic.testId]);


  const testAttempts = attempts.items.filter((a: TestAttempt) => a.testId === topic.testId);
  const bestScore = testAttempts.reduce((max: number, attempt: TestAttempt) =>
    attempt.completedAt && attempt.score ? Math.max(max, attempt.score) : max, 0);

  const canComplete = () => {
    if (!courseStarted) return false;
    if (isCompleted) return false;

    if (topic.testId) {
      const hasPassingAttempt = bestScore >= 70;
      return hasPassingAttempt;
    }

    return true;
  };

  const renderTestInfo = () => {
    if (!topic.testId) return null;
    if (testLoading || !test) {
      return <Text type="secondary">Loading test information...</Text>;
    }
    const attemptCount = testAttempts.length;
    const hasPassed = bestScore >= 70;
    const hasIncompleteAttempt = testAttempts.some(attempt => !attempt.completedAt);
    const incompleteAttempt = testAttempts.find(attempt => !attempt.completedAt);

    return (
      <>
        <Divider />
        <div>
          <Text strong>{test.title}</Text>
          <br />
          {attemptCount > 0 ? (
            <>
              <Text>Attempts: {attemptCount}</Text>
              <br />
              <Text>Best Score: {bestScore}%</Text>
              <br />
              {hasPassed ? (
                <Tag color="success">Passed</Tag>
              ) : (
                <Tag color="warning">Not Passed</Tag>
              )}
            </>
          ) : (
            <Text type="secondary">No attempts yet</Text>
          )}
        </div>
        <div style={{ marginTop: 8 }}>
          {hasIncompleteAttempt ? (
            <Space direction="vertical">
              <Text type="warning">You have an incomplete attempt.</Text>
              <Link to={`attempt/${incompleteAttempt?.id}`}>
                Continue Attempt
              </Link>
            </Space>
          ) : (
            <Link to={`/tests/${topic.testId}`}>Take Test</Link>
          )}
        </div>
      </>
    );
  };

  return (
    <Card
      key={topic.id}
      hoverable
      styles={{ body: { textAlign: 'left' } }}
    >
      <Meta title={topic.title} />
      {topic.mediaUrl && (
        <div style={{ display: 'flex', justifyContent: 'center', margin: '16px 0' }}>
          <Image src={topic.mediaUrl} alt={topic.title} width={'80%'} preview={false} />
        </div>
      )}
      <Paragraph style={{ margin: '16px 0' }}>{topic.content}</Paragraph>
      <Space>
        {isCompleted && (
          <Tag color="success">Completed</Tag>
        )}
        {canComplete() && !isCompleted && (
          <Button type="primary" onClick={onComplete}>
            Complete
          </Button>
        )}
      </Space>
      {renderTestInfo()}
    </Card>
  );
};

export default TopicCard;