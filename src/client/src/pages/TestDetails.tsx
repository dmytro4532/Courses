import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Button, Card, Typography, Space, List, Spin, Alert } from 'antd';
import { useAppDispatch, useAppSelector } from '../hooks/store';
import { getActiveTestAttempt, createTestAttempt } from '../store/slices/testAttemptsSlice';
import { fetchTestById } from '../store/slices/testSlice';
import { fetchQuestions } from '../store/slices/questionsSlice';
import type { Question } from '../types';

const { Title, Text } = Typography;

const TestDetails = () => {
  const { testId } = useParams<{ testId: string }>();
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const { test, loading: isTestLoading, error: testError } = useAppSelector(state => state.test);
  const { activeAttempt, isLoading: isAttemptLoading } = useAppSelector(state => state.testAttempts);
  const { paged: questions, status: questionsStatus } = useAppSelector(state => state.questions);

  useEffect(() => {
    const loadData = async () => {
      if (testId) {
        await dispatch(fetchTestById(testId)).unwrap();
        await dispatch(fetchQuestions({ testId })).unwrap();
        await dispatch(getActiveTestAttempt(testId));
      }
    };
    loadData();
  }, [dispatch, testId]);

  if (isTestLoading || isAttemptLoading || questionsStatus === 'loading') {
    return <Spin size="large" />;
  }

  if (testError) {
    return <Alert type="error" message="Failed to load test details" />;
  }

  if (!test || !questions) {
    return <Alert type="error" message="Test not found" />;
  }

  const handleStartAttempt = async () => {
    try {
      const result = await dispatch(createTestAttempt(testId!)).unwrap();
      navigate(`/test-attempts/${result.id}`);
    } catch (error) {
      console.error('Failed to create test attempt:', error);
    }
  };

  const handleContinueAttempt = () => {
    if (activeAttempt) {
      navigate(`/test-attempts/${activeAttempt.id}`);
    }
  };

  return (
    <Space direction="vertical" size="large" style={{ width: '100%' }}>
      <Card>
        <Space direction="vertical" style={{ width: '100%' }}>
          <Title level={2}>{test.title}</Title>
          <Text>{test.description}</Text>
          <Space>
            {activeAttempt ? (
              <Button type="primary" onClick={handleContinueAttempt}>
                Continue Attempt
              </Button>
            ) : (
              <Button type="primary" onClick={handleStartAttempt} loading={isAttemptLoading}>
                Start Test
              </Button>
            )}
          </Space>
        </Space>
      </Card>

      <Card title="Questions">
        <List
          dataSource={questions.items}
          renderItem={(question: Question, index) => (
            <List.Item>
              <Space direction="vertical" style={{ width: '100%' }}>
                <Text strong>{`${index + 1}. ${question.content}`}</Text>
                <List
                  dataSource={question.answers}
                  renderItem={(answer) => (
                    <List.Item>
                      <Text>{answer.value}</Text>
                    </List.Item>
                  )}
                />
              </Space>
            </List.Item>
          )}
        />
      </Card>
    </Space>
  );
};

export default TestDetails;