import { configureStore } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import completedTopicsReducer from './slices/completedTopicsSlice';
import courseReducer from './slices/courseSlice';
import coursesReducer from './slices/coursesSlice';
import progressesReducer from './slices/progressesSlice';
import topicsReducer from './slices/topicsSlice';

export const store = configureStore({
  reducer: {
    courses: coursesReducer,
    auth: authReducer,
    topics: topicsReducer,
    course: courseReducer,
    progresses: progressesReducer,
    completedTopics: completedTopicsReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch; 