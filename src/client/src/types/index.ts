export interface Topic {
  id: string;
  title: string;
  content: string;
  courseId: string;
  mediaUrl?: string;
  testId?: string;
}

export interface Test {
  id: string;
  title: string;
  description: string;
  topicId: string;
  questions: Question[];
  createdAt: string;
  updatedAt?: string;
}

export interface Question {
  id: string;
  content: string;
  order: number;
  testId: string;
  answers: Answer[];
}

export interface Answer {
  id: string;
  value: string;
  isCorrect: boolean;
}

export interface User {
  id: string;
  username: string;
  email: string;
  role: string;
}

export interface CourseResponse {
  id: string;
  title: string;
  description: string;
  imageUrl?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface PagedList<T> {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  items: T[];
}

export interface TestAttempt {
    id: string;
    testId: string;
    userId: string;
    startedAt: string;
    completedAt?: string;
    score?: number;
}

export interface AttemptQuestion {
    id: string;
    testAttemptId: string;
    questionId: string;
    content: string;
    order: number;
    answers: AttemptQuestionAnswer[];
}

export interface AttemptQuestionAnswer {
    id: string;
    isSelected: boolean;
    value: string;
}