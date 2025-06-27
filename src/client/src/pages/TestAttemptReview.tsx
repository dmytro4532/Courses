import { CheckCircleOutlined, CloseCircleOutlined } from '@ant-design/icons';
import { Alert, Card, List, Space, Spin, Tag, Typography } from 'antd';
import { useEffect } from 'react';
import { useParams, useSearchParams, Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../hooks/store';
import { getAttemptQuestions } from '../store/slices/attemptQuestionsSlice';
import { fetchQuestions } from '../store/slices/questionsSlice';
import { getTestAttempt } from '../store/slices/testAttemptsSlice';
import { fetchTestById } from '../store/slices/testSlice';
import type { Answer, AttemptQuestion, Question } from '../types';

const { Title, Text } = Typography;

export const TestAttemptReview = () => {
    const { attemptId } = useParams<{ attemptId: string }>();
    const [searchParams] = useSearchParams();
    const courseId = searchParams.get('courseId');
    const dispatch = useAppDispatch();

    const { currentAttempt, isLoading: isAttemptLoading } = useAppSelector(state => state.testAttempts);
    const { questions: attemptQuestions } = useAppSelector(state => state.attemptQuestions);
    const { test, isTestLoading } = useAppSelector(state =>
        ({ test: state.test.tests.find(t => t.id === currentAttempt?.testId), isTestLoading: state.test.loading }));
    const { paged: testQuestions, status: questionsStatus } = useAppSelector(state => state.questions);

    useEffect(() => {
        const loadData = async () => {
            if (attemptId) {
                const attempt = await dispatch(getTestAttempt(attemptId)).unwrap();
                await dispatch(fetchTestById({ testId: attempt.testId })).unwrap();
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
        return <Alert type="error" message="Спробу тесту не знайдено" />;
    }

    return (
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
            {courseId && (
                <Link to={`/courses/${courseId}/topics`} style={{ marginTop: 16, display: 'inline-block' }}>
                    ← Назад до курсу
                </Link>
            )}
            <Card>
                <Space direction="vertical" style={{ width: '100%' }}>
                    <Title level={2}>{test.title} - Огляд</Title>
                    <Text>Оцінка: {currentAttempt.score}%</Text>
                </Space>
            </Card>

            <Card title="Питання">
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
                                                Правильно
                                            </Tag>
                                        ) : (
                                            <Tag icon={<CloseCircleOutlined />} color="error">
                                                Неправильно
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
                                                        {isCorrectAnswer && ' (Правильно)'}
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