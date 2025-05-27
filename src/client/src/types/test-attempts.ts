export interface TestAttempt {
    id: string;
    testId: string;
    userId: string;
    score: number;
    startedAt: string;
    completedAt?: string;
}

export interface AttemptQuestion {
    id: string;
    testAttemptId: string;
    questionId: string;
    answers: AttemptAnswer[];
}

export interface AttemptAnswer {
    id: string;
    value: string;
    isSelected: boolean;
    isCorrect?: boolean;
}

export interface CreateAttemptQuestionRequest {
    testAttemptId: string;
    questionId: string;
    selectedAnswerIds: string[];
} 