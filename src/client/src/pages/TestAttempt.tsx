import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Button, Card, Typography, Space, List, Spin, Alert, Checkbox, Modal } from 'antd';
import { useAppDispatch, useAppSelector } from '../hooks/store';
import { getTestAttempt, completeTestAttempt } from '../store/slices/testAttemptsSlice';
import { getAttemptQuestions, createAttemptQuestion } from '../store/slices/attemptQuestionsSlice';
import { fetchTestById } from '../store/slices/testSlice';
import { fetchQuestions } from '../store/slices/questionsSlice';
import type { Question, AttemptQuestion } from '../types';
import { enqueueSnackbar } from 'notistack';

const { Title, Text } = Typography;

export const TestAttempt = () => {
    const { attemptId } = useParams<{ attemptId: string }>();
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const [selectedAnswers, setSelectedAnswers] = useState<Record<string, string[]>>({});
    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState(false);
    
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

    const handleAnswerChange = async (question: Question, answerIds: string[]) => {
        setSelectedAnswers(prev => ({ ...prev, [question.id]: answerIds }));

        try {
            await dispatch(createAttemptQuestion({
                testAttemptId: attemptId!,
                questionId: question.id,
                selectedAnswerIds: answerIds,
            })).unwrap();
        } catch (error) {
            console.error('Failed to save answer:', error);
        }
    };

    const handleComplete = async () => {
        try {
            await dispatch(completeTestAttempt(attemptId!)).unwrap();
            enqueueSnackbar('Test completed successfully', { variant: 'success', autoHideDuration: 3000 });
            navigate(`/test-attempts/${attemptId}/review`);
        } catch (error) {
            enqueueSnackbar('Failed to complete test', { variant: 'error', autoHideDuration: 3000 });
            console.error('Failed to complete test:', error);
        }
    };

    const getSelectedAnswersForQuestion = (questionId: string): string[] => {
        if (selectedAnswers[questionId]) {
            return selectedAnswers[questionId];
        }

        const attemptQuestion = attemptQuestions.items.find((aq: AttemptQuestion) => aq.questionId === questionId);

        if (attemptQuestion?.answers) {
            return attemptQuestion.answers
                .filter(a => a.isSelected)
                .map(a => a.id);
        }

        return [];
    };

    return (
        <>
            <Space direction="vertical" size="large" style={{ width: '100%' }}>
                <Card>
                    <Space direction="vertical" style={{ width: '100%' }}>
                        <Title level={2}>{test.title}</Title>
                        <Text>{test.description}</Text>
                    </Space>
                </Card>

                <Card title="Questions">
                    <List
                        dataSource={testQuestions.items}
                        renderItem={(question) => {
                            const selectedAnswerIds = getSelectedAnswersForQuestion(question.id);
                            return (
                                <List.Item>
                                    <Space direction="vertical" style={{ width: '100%' }}>
                                        <Text strong>{question.content}</Text>
                                        <Checkbox.Group
                                            value={selectedAnswerIds}
                                            onChange={(checkedValues) => handleAnswerChange(question, checkedValues as string[])}
                                        >
                                            <Space direction="vertical">
                                                {question.answers.map((answer) => (
                                                    <Checkbox key={answer.id} value={answer.id}>
                                                        {answer.value}
                                                    </Checkbox>
                                                ))}
                                            </Space>
                                        </Checkbox.Group>
                                    </Space>
                                </List.Item>
                            );
                        }}
                    />
                </Card>

                <Button type="primary" onClick={() => setIsConfirmModalOpen(true)}>
                    Complete Test
                </Button>
            </Space>

            <Modal
                title="Complete Test"
                open={isConfirmModalOpen}
                onOk={handleComplete}
                onCancel={() => setIsConfirmModalOpen(false)}
                okText="Yes"
                cancelText="No"
            >
                Are you sure you want to complete this test? You won't be able to change your answers after completion.
            </Modal>
        </>
    );
}; 