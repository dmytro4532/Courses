export interface Topic {
  id: string;
  title: string;
  content: string;
  courseId: string;
  tests: Test[];
}

export interface Test {
  id: string;
  title: string;
  description: string;
  topicId: string;
  questions: Question[];
}

export interface Question {
  id: string;
  text: string;
  options: string[];
  correctOptionIndex: number;
  testId: string;
}

export interface User {
  id: string;
  username: string;
  email: string;
  role: 'student' | 'teacher' | 'admin';
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