import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import completedTopicsReducer from './slices/completedTopicsSlice';
import courseReducer from './slices/courseSlice';
import coursesReducer from './slices/coursesSlice';
import progressesReducer from './slices/progressesSlice';
import topicsReducer from './slices/topicsSlice';
import testAttemptsReducer from './slices/testAttemptsSlice';
import attemptQuestionsReducer from './slices/attemptQuestionsSlice';
import testReducer from './slices/testSlice';
import questionsReducer from './slices/questionsSlice';
import testsReducer from './slices/testsSlice';

export const store = configureStore({
  reducer: {
    courses: coursesReducer,
    auth: authReducer,
    topics: topicsReducer,
    course: courseReducer,
    progresses: progressesReducer,
    completedTopics: completedTopicsReducer,
    test: testReducer,
    testAttempts: testAttemptsReducer,
    attemptQuestions: attemptQuestionsReducer,
    questions: questionsReducer,
    tests: testsReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware(),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch; 