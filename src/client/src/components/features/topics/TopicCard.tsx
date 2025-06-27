import { Card, Image, Typography, Button, Tag, Space, Divider } from 'antd';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../../store';
import type { Topic, TestAttempt } from '../../../types';
import { clearAttempts, fetchTestAttemptsByTest } from '../../../store/slices/testAttemptsSlice';
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
  const attempts = useSelector((state: RootState) => state.testAttempts.attempts.items.filter(a => a.testId === topic.testId));
  const { test, testLoading } = useSelector((state: RootState) =>
    ({ test: state.test.tests.find(t => t.id === topic.testId), testLoading: state.test.loading }));

    useEffect(() => {
      dispatch(clearAttempts());
      if (topic.testId) {
        dispatch(fetchTestById({ testId: topic.testId, topicId: topic.id }));
        dispatch(fetchTestAttemptsByTest(topic.testId));
      }
    }, [dispatch, topic.testId]);

  const bestScore = attempts.reduce((max: number, attempt: TestAttempt) =>
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
      return <Text type="secondary">Завантаження інформації про тест...</Text>;
    }
    const attemptCount = attempts.length;
    const hasPassed = bestScore >= 70;
    const hasIncompleteAttempt = attempts.some(attempt => !attempt.completedAt);
    const incompleteAttempt = attempts.find(attempt => !attempt.completedAt);

    return (
      <>
        <Divider />
        <div>
          <Text strong>{test.title}</Text>
          <br />
          {attemptCount > 0 ? (
            <>
              <Text>Спроби: {attemptCount}</Text>
              <br />
              <Text>Найкращий результат: {bestScore}%</Text>
              <br />
              {hasPassed ? (
                <Tag color="success">Пройдено</Tag>
              ) : (
                <Tag color="warning">Не пройдено</Tag>
              )}
            </>
          ) : (
            <Text type="secondary">Спроб ще немає</Text>
          )}
        </div>
        <div style={{ marginTop: 8 }}>
          {hasIncompleteAttempt ? (
            <Space direction="vertical">
              <Text type="warning">У вас є незавершена спроба.</Text>
              <Link to={`/test-attempts/${incompleteAttempt?.id}${`?courseId=${topic.courseId}`}`}>
                Продовжити спробу
              </Link>
            </Space>
          ) : (
            <Link to={`/tests/${topic.testId}${`?courseId=${topic.courseId}`}`}>Пройти тест</Link>
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
          <Tag color="success">Завершено</Tag>
        )}
        {canComplete() && !isCompleted && (
          <Button type="primary" onClick={onComplete}>
            Завершити
          </Button>
        )}
      </Space>
      {renderTestInfo()}
    </Card>
  );
};

export default TopicCard;