import { CheckCircleOutlined, CloseCircleOutlined } from '@ant-design/icons';
import { Alert, Card, List, Space, Spin, Tag, Typography } from 'antd';
import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../hooks/store';
import { getAttemptQuestions } from '../store/slices/attemptQuestionsSlice';
import { fetchQuestions } from '../store/slices/questionsSlice';
import { getTestAttempt } from '../store/slices/testAttemptsSlice';
import { fetchTestById } from '../store/slices/testSlice';
import type { Answer, AttemptQuestion, Question } from '../types';

const { Title, Text } = Typography;

export const TestAttemptReview = () => {
    const { attemptId } = useParams<{ attemptId: string }>();
    const dispatch = useAppDispatch();
    
    const { currentAttempt, isLoading: isAttemptLoading } = useAppSelector(state => state.testAttempts);
    const { questions: attemptQuestions } = useAppSelector(state => state.attemptQuestions);
    const { test, loading: isTestLoading } = useAppSelector(state => state.test);
    const { paged: testQuestions, status: questionsStatus } = useAppSelector(state => state.questions);

    useEffect(() => {
        const loadData = async () => {
            if (attemptId) {
                const attempt = await dispatch(getTestAttempt(attemptId)).unwrap();
                await dispatch(fetchTestById(attempt.testId)).unwrap();
                await dispatch(fetchQuestions({ testId: attempt.testId })).unwrap();
                await dispatch(getAttemptQuestions(attemptId));
            }
        };
        loadData();
    }, [dispatch, attemptId]);

    if (isAttemptLoading || isTestLoading || questionsStatus === 'loading') {
        return <Spin size="large" />;
    }

    if (!currentAttempt || !test || !testQuestions) {
        return <Alert type="error" message="Test attempt not found" />;
    }

    return (
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
            <Card>
                <Space direction="vertical" style={{ width: '100%' }}>
                    <Title level={2}>{test.title} - Review</Title>
                    <Text>Score: {currentAttempt.score}%</Text>
                </Space>
            </Card>

            <Card title="Questions">
                <List
                    dataSource={testQuestions.items}
                    renderItem={(question: Question) => {
                        const attemptQuestion = attemptQuestions.items.find((aq: AttemptQuestion) => aq.questionId === question.id);
                        const selectedAnswers = attemptQuestion?.answers.filter(a => a.isSelected) ?? [];
                        const correctAnswers = question.answers.filter(a => a.isCorrect);
                        const isCorrect = selectedAnswers.every(sa => 
                            correctAnswers.some(ca => ca.id === sa.id)) &&
                            correctAnswers.every(ca => 
                                selectedAnswers.some(sa => sa.id === ca.id));

                        return (
                            <List.Item>
                                <Space direction="vertical" style={{ width: '100%' }}>
                                    <Space>
                                        <Text strong>{question.content}</Text>
                                        {isCorrect ? (
                                            <Tag icon={<CheckCircleOutlined />} color="success">
                                                Correct
                                            </Tag>
                                        ) : (
                                            <Tag icon={<CloseCircleOutlined />} color="error">
                                                Incorrect
                                            </Tag>
                                        )}
                                    </Space>
                                    <List
                                        dataSource={question.answers}
                                        renderItem={(answer: Answer) => {
                                            const isSelected = selectedAnswers.some(sa => sa.id === answer.id);
                                            const isCorrectAnswer = correctAnswers.some(ca => ca.id === answer.id);
                                            let color: string | undefined;
                                            if (isSelected && isCorrectAnswer) {
                                                color = 'success';
                                            } else if (isSelected && !isCorrectAnswer) {
                                                color = 'error';
                                            } else if (!isSelected && isCorrectAnswer) {
                                                color = 'warning';
                                            }

                                            return (
                                                <List.Item>
                                                    <Tag color={color}>
                                                        {answer.value}
                                                        {isCorrectAnswer && ' (Correct)'}
                                                    </Tag>
                                                </List.Item>
                                            );
                                        }}
                                    />
                                </Space>
                            </List.Item>
                        );
                    }}
                />
            </Card>
        </Space>
    );
}; 